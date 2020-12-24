using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;
using KiwoomCode;
using WcfServiceLibrary;
using OpenApi.ReceiveTrData;
using System.Threading;
using System.Collections;
using OpenApi.ReceiveTrData;
using OpenApi.ReceiveRealData;
using OpenApi.ReceiveChejanData;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi
{
    public partial class Form1 : Form
    {
        private ServiceHost productsServiceHost;
        public Form1()
        {
            InitializeComponent();
            init();
            AppLib.getClass1Instance().setAxKHOpenAPIInstance(this.axKHOpenAPI);
        }
        private void init()
        {
            Dictionary<String, String> comboSourceGetLoginInfo = new Dictionary<String, String>();
            comboSourceGetLoginInfo.Add("ACCOUNT_CNT", "전체계좌수");
            comboSourceGetLoginInfo.Add("ACCNO", "전체계좌");
            comboSourceGetLoginInfo.Add("USER_ID", "사용자ID");
            comboSourceGetLoginInfo.Add("USER_NAME", "사용자명");
            comboSourceGetLoginInfo.Add("KEY_BSECGB", "키보드보안 해지여부. 0:정상, 1:해지");
            comboSourceGetLoginInfo.Add("FIREW_SECGB", "방화벽 설정 여부. 0:미설정, 1:설정, 2:해지");

            cb_GetLoginInfo.DataSource = new BindingSource(comboSourceGetLoginInfo, null);
            cb_GetLoginInfo.DisplayMember = "Value";
            cb_GetLoginInfo.ValueMember = "Key";

            Dictionary<String, String> comboSourceGetCodeListByMarket = new Dictionary<String, String>();
            comboSourceGetCodeListByMarket.Add("0", "장내");
            comboSourceGetCodeListByMarket.Add("3", "ELW");
            comboSourceGetCodeListByMarket.Add("4", "뮤추얼펀드");
            comboSourceGetCodeListByMarket.Add("5", "신주인수권");
            comboSourceGetCodeListByMarket.Add("6", "리츠");
            comboSourceGetCodeListByMarket.Add("8", "ETF");
            comboSourceGetCodeListByMarket.Add("9", "하이일드펀드");
            comboSourceGetCodeListByMarket.Add("10", "코스닥");
            comboSourceGetCodeListByMarket.Add("30", "제3시장");

            cb_GetCodeListByMarket.DataSource = new BindingSource(comboSourceGetCodeListByMarket, null);
            cb_GetCodeListByMarket.DisplayMember = "Value";
            cb_GetCodeListByMarket.ValueMember = "Key";


            Dictionary<int, String> comboSourceSendOrderNOrderType = new Dictionary<int, String>();
            comboSourceSendOrderNOrderType.Add(1, "(1)신규매수");
            comboSourceSendOrderNOrderType.Add(2, "(2)신규매도");
            comboSourceSendOrderNOrderType.Add(3, "(3)매수취소");
            comboSourceSendOrderNOrderType.Add(4, "(4)매도취소");
            comboSourceSendOrderNOrderType.Add(5, "(5)매수정정");
            comboSourceSendOrderNOrderType.Add(6, "(6)매도정정");

            cb_SendOrderNOrderType.DataSource = new BindingSource(comboSourceSendOrderNOrderType, null);
            cb_SendOrderNOrderType.DisplayMember = "Value";
            cb_SendOrderNOrderType.ValueMember = "Key";


            Dictionary<String, String> comboSourceSendOrderSHogaGb = new Dictionary<String, String>();
            comboSourceSendOrderSHogaGb.Add("00", "(00)지정가");
            comboSourceSendOrderSHogaGb.Add("03", "(03)시장가");
            comboSourceSendOrderSHogaGb.Add("05", "(05)조건부지정가");
            comboSourceSendOrderSHogaGb.Add("06", "(06)최유리지정가");
            comboSourceSendOrderSHogaGb.Add("07", "(07)최우선지정가");
            comboSourceSendOrderSHogaGb.Add("10", "(10)지정가IOC");
            comboSourceSendOrderSHogaGb.Add("13", "(13)시장가IOC");
            comboSourceSendOrderSHogaGb.Add("16", "(16)최유리IOC");
            comboSourceSendOrderSHogaGb.Add("20", "(20)지정가FOK");
            comboSourceSendOrderSHogaGb.Add("23", "(23)시장가FOK");
            comboSourceSendOrderSHogaGb.Add("26", "(26)최유리FOK");
            comboSourceSendOrderSHogaGb.Add("61", "(61)지정가IOC");
            comboSourceSendOrderSHogaGb.Add("81", "(81)지정가IOC");

            cb_SendOrderSHogaGb.DataSource = new BindingSource(comboSourceSendOrderSHogaGb, null);
            cb_SendOrderSHogaGb.DisplayMember = "Value";
            cb_SendOrderSHogaGb.ValueMember = "Key";

        }


        private void 로그인_Click(object sender, EventArgs e)
        {
            if (axKHOpenAPI.CommConnect() == 0)
            {
                Logger(Log.일반, "로그인창 열기 성공");
            }
            else
            {
                Logger(Log.에러, "로그인창 열기 실패");
            }
        }

        private Boolean IsConnected()
        {
            int ret = axKHOpenAPI.GetConnectState();
            if (ret == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        private void axKHOpenAPI_OnReceiveTrData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrData===============================");
            FileLog.PrintF("axKHOpenAPI_OnReceiveTrData e.sRecordName=>" + e.sRecordName);
            FileLog.PrintF("axKHOpenAPI_OnReceiveTrData e.sRQName=>" + e.sRQName);
            FileLog.PrintF("axKHOpenAPI_OnReceiveTrData e.sScrNo=>" + e.sScrNo);
            FileLog.PrintF("axKHOpenAPI_OnReceiveTrData e.sTrCode=>" + e.sTrCode);
            int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
            FileLog.PrintF("axKHOpenAPI   GetRepeatCnt=>" + nCnt);
            FileLog.PrintF(" AppLib.getClass1Instance().GetRanUniqStockCount()=>" + AppLib.getClass1Instance().GetRanUniqStockCount());
            
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveTrData===============================");


            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
            rtf.runReceiveTrData(axKHOpenAPI, e);
        }

        private void axKHOpenAPI_OnEventConnect(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnEventConnectEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnEventConnect===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnEventConnect===============================");
            if (Error.IsError(e.nErrCode))
            {
                Logger(Log.일반, "[로그인 처리결과] " + Error.GetErrorMessage());
                AppLib.getClass1Instance().Start();
                //axKHOpenAPI.GetConditionLoad();
            }
            else
            {
                Logger(Log.에러, "[로그인 처리결과] " + Error.GetErrorMessage());
            }
        }

        private void axKHOpenAPI_OnReceiveChejanData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            /*주문이 들어가면 체결데이터가 이리들어온다고 한다...*/
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveChejanData===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveChejanData===============================");
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData e.sGubun =>" + e.sGubun);

            ReceiveChejanDataFactory rtf = ReceiveChejanDataFactory.getClass1Instance();
            rtf.runReceiveChejanData(axKHOpenAPI, e);

            if (e.sGubun == "0") //0은 주문접수 주문체결
            {
                Logger(Log.실시간, "구분 : 주문체결통보");
                Logger(Log.실시간, "주문/체결시간 : " + axKHOpenAPI.GetChejanData(908));
                Logger(Log.실시간, "종목명 : " + axKHOpenAPI.GetChejanData(302));
                Logger(Log.실시간, "주문수량 : " + axKHOpenAPI.GetChejanData(900));
                Logger(Log.실시간, "주문가격 : " + axKHOpenAPI.GetChejanData(901));
                Logger(Log.실시간, "체결수량 : " + axKHOpenAPI.GetChejanData(911));
                Logger(Log.실시간, "체결가격 : " + axKHOpenAPI.GetChejanData(910));
                Logger(Log.실시간, "=======================================");
            }
            else if (e.sGubun == "1")  //1은 반영된잔고를 보여준다.
            {
                Logger(Log.실시간, "구분 : 잔고통보");

            }
            else if (e.sGubun == "3")
            {
                Logger(Log.실시간, "구분 : 특이신호");
            }

        }

        private void axKHOpenAPI_OnReceiveMsg(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveMsgEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveMsg===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveMsg===============================");
            Logger(Log.조회, "화면번호:{0} | RQName:{1} | TRCode:{2} | 메세지:{3}", e.sScrNo, e.sRQName, e.sTrCode, e.sMsg);
            FileLog.PrintF(String.Format("화면번호:{0} | RQName:{1} | TRCode:{2} | 메세지:{3}", e.sScrNo, e.sRQName, e.sTrCode, e.sMsg));

            String 종목코드 = "XXXX";
            String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
            String keyStockCode = String.Format(keyStockCodeLayout, e.sRQName, e.sTrCode, e.sScrNo);
            종목코드 = AppLib.getClass1Instance().getStockCode(keyStockCode);
            String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";

            ReceiveMsg receiveMsg = new ReceiveMsg();
            receiveMsg.insertReceiveMsg(e.sScrNo, e.sRQName, e.sTrCode, e.sMsg, 종목코드);


        }

        private void axKHOpenAPI_OnReceiveRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            //FileLog.PrintF("====================axKHOpenAPI_OnReceiveRealData===============================");
            //Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveRealData===============================");
            //                FileLog.PrintF(String.Format("종목코드 : {0} | RealType : {1} | RealData : {2}", e.sRealKey, e.sRealType, e.sRealData));

            ReceiveRealDataFactory rtf = ReceiveRealDataFactory.getClass1Instance();
            rtf.runReceiveRealData(axKHOpenAPI, e);
        }

        // 로그를 출력합니다.
        public void Logger(Log type, string format, params Object[] args)
        {
            string message = String.Format(format, args);

            switch (type)
            {
                case Log.조회:
                    if (lst조회.InvokeRequired)
                    {
                        lst조회.BeginInvoke(new Action(() =>
                        {
                            lst조회.Items.Add(message);
                            if (lst조회.Items.Count > 20)
                            {
                                lst조회.Items.Clear();
                            }
                            lst조회.SelectedIndex = lst조회.Items.Count - 1;
                        }));

                    }
                    else
                    {
                        lst조회.Items.Add(message);
                        if (lst조회.Items.Count > 20)
                        {
                            lst조회.Items.Clear();
                        }
                        lst조회.SelectedIndex = lst조회.Items.Count - 1;
                    }
                    break;
                case Log.에러:
                    if (lst에러.InvokeRequired)
                    {
                        lst에러.BeginInvoke(new Action(() =>
                        {
                            lst에러.Items.Add(message);
                            if (lst에러.Items.Count > 20)
                            {
                                lst에러.Items.Clear();
                            }
                            lst에러.SelectedIndex = lst에러.Items.Count - 1;
                        }));

                    }
                    else
                    {
                        lst에러.Items.Add(message);
                        if (lst에러.Items.Count > 20)
                        {
                            lst에러.Items.Clear();
                        }
                        lst에러.SelectedIndex = lst에러.Items.Count - 1;

                    }
                    break;

                case Log.일반:
                    if (lst일반.InvokeRequired)
                    {
                        lst일반.BeginInvoke(new Action(() =>
                        {
                            lst일반.Items.Add(message);
                            if (lst일반.Items.Count > 20)
                            {
                                lst일반.Items.Clear();
                            }
                            lst일반.SelectedIndex = lst일반.Items.Count - 1;
                        }));
                    }
                    else
                    {
                        lst일반.Items.Add(message);
                        if (lst일반.Items.Count > 20)
                        {
                            lst일반.Items.Clear();
                        }
                        lst일반.SelectedIndex = lst일반.Items.Count - 1;
                    }


                    break;
                case Log.실시간:
                    if (lst실시간.InvokeRequired)
                    {
                        lst실시간.BeginInvoke(new Action(() =>
                        {
                            lst실시간.Items.Add(message);
                            if (lst실시간.Items.Count > 20)
                            {
                                lst실시간.Items.Clear();
                            }
                            lst실시간.SelectedIndex = lst실시간.Items.Count - 1;
                        }));
                    }
                    else
                    {
                        lst실시간.Items.Add(message);
                        if (lst실시간.Items.Count > 20)
                        {
                            lst실시간.Items.Clear();
                        }
                        lst실시간.SelectedIndex = lst실시간.Items.Count - 1;
                    }

                    break;
                case Log.디버깅:
                    if (lst디버깅.InvokeRequired)
                    {
                        lst디버깅.BeginInvoke(new Action(() =>
                        {
                            lst디버깅.Items.Add(message);
                            if (lst디버깅.Items.Count > 20)
                            {
                                lst디버깅.Items.Clear();
                            }
                            lst디버깅.SelectedIndex = lst디버깅.Items.Count - 1;
                        }));
                    }
                    else
                    {
                        lst디버깅.Items.Add(message);
                        if (lst디버깅.Items.Count > 20)
                        {
                            lst디버깅.Items.Clear();
                        }
                        lst디버깅.SelectedIndex = lst디버깅.Items.Count - 1;
                    }

                    break;
                default:
                    break;
            }
        }

        private void axKHOpenAPI_OnReceiveRealCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealConditionEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveRealCondition===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveRealCondition===============================");
            Logger(Log.실시간, "========= 조건조회 실시간 편입/이탈 ==========");
            Logger(Log.실시간, "[종목코드] : " + e.sTrCode);
            Logger(Log.실시간, "[실시간타입] : " + e.strType);
            Logger(Log.실시간, "[조건명] : " + e.strConditionName);
            Logger(Log.실시간, "[조건명 인덱스] : " + e.strConditionIndex);
        }

        private void axKHOpenAPI_OnReceiveTrCondition(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrConditionEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveTrCondition===============================");
            FileLog.PrintF("[화면번호] : " + e.sScrNo);
            FileLog.PrintF("[종목리스트] : " + e.strCodeList);
            FileLog.PrintF("[조건명] : " + e.strConditionName);
            FileLog.PrintF("[조건명 인덱스 ] : " + e.nIndex.ToString());
            FileLog.PrintF("[연속조회] : " + e.nNext.ToString());
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveTrCondition===============================");
            Logger(Log.조회, "[화면번호] : " + e.sScrNo);
            Logger(Log.조회, "[종목리스트] : " + e.strCodeList);
            Logger(Log.조회, "[조건명] : " + e.strConditionName);
            Logger(Log.조회, "[조건명 인덱스 ] : " + e.nIndex.ToString());
            Logger(Log.조회, "[연속조회] : " + e.nNext.ToString());
        }

        private void axKHOpenAPI_OnReceiveConditionVer(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveConditionVerEvent e)
        {
            FileLog.PrintF("====================axKHOpenAPI_OnReceiveConditionVer===============================");
            Logger(Log.디버깅, "====================axKHOpenAPI_OnReceiveConditionVer===============================");
            if (e.lRet == 1)
            {
                Logger(Log.일반, "[이벤트] 조건식 저장 성공");
                FileLog.PrintF("[이벤트] 조건식 저장 성공");
                String ret = axKHOpenAPI.GetConditionNameList(); // 인덱스1^조건명1;인덱스2^조건명2;인덱스3^조건명3;…  조건리스트를 불러온다.
            }
            else
            {
                Logger(Log.에러, "[이벤트] 조건식 저장 실패 : " + e.sMsg);
                FileLog.PrintF("[이벤트] 조건식 저장 실패 : " + e.sMsg);
            }

        }

        private void 로그아웃_Click(object sender, EventArgs e)
        {
            ScreenNumber.getClass1Instance().DisconnectAllEOSData();
            ScreenNumber.getClass1Instance().DisconnectAllAnyTimeData();
            Logger(Log.일반, "실시간해제");
        }

        private void 접속상태_Click(object sender, EventArgs e)
        {
            int ret = axKHOpenAPI.GetConnectState();
            Logger(Log.일반, "Open API 연결 : " + ret);
            if (ret == 0)
            {
                Logger(Log.일반, "Open API 연결 : 미연결");
            }
            else
            {
                Logger(Log.일반, "Open API 연결 : 연결중");
            }

        }

        private void debugCommunicationState(CommunicationState ret)
        {
            if (ret == CommunicationState.Created)
            {
                Logger(Log.디버깅, "CommunicationState.Created 상태");
            }
            else if (ret == CommunicationState.Opening)
            {
                Logger(Log.디버깅, "CommunicationState.Opening 상태");
                //     통신 개체에서 전환 중인 것을 나타냅니다는 System.ServiceModel.CommunicationState.Opening 상태에 있는
            }
            else if (ret == CommunicationState.Opened)
            {
                Logger(Log.디버깅, "CommunicationState.Opened 상태");
                //     통신 개체가 현재 열려 있고 사용할 준비가 되었습니다. 임을 나타냅니다.
            }
            else if (ret == CommunicationState.Closing)
            {
                Logger(Log.디버깅, "CommunicationState.Closing 상태");
            }
            else if (ret == CommunicationState.Closed)
            {
                Logger(Log.디버깅, "CommunicationState.Closed 상태");
                //     통신 개체가 닫혀 서 더 이상 사용할 수 없습니다 것을 나타냅니다.
            }
            else if (ret == CommunicationState.Faulted)
            {
                Logger(Log.디버깅, "CommunicationState.Faulted 상태");
                //     장애는 더 이상 사용할 수 없습니다와에서 복구할 수 없는 오류 또는 통신 개체를 발견 했음을 나타냅니다.
            }
        }

        // procedure to run on a new thread
        private void StartService()
        {

            if (productsServiceHost == null)
            {

                if (axKHOpenAPI != null)
                {
                    productsServiceHost = new ServiceHost(typeof(KiwoomOpenApiService));
                    productsServiceHost.Faulted += new EventHandler(wcfFaultHandler);
                    productsServiceHost.UnknownMessageReceived += new EventHandler<UnknownMessageReceivedEventArgs>(wcfUnknownMessageReceived);
                }
                else
                {
                    Logger(Log.디버깅, "WCF_ON : 로그인을 하지 않았습니다. 먼저 로그인을 해주세요");
                }

            }
            else
            {
                Logger(Log.디버깅, "WCF_ON : WCF 객체가 이미 생성되었음.");
            }
            CommunicationState ret = productsServiceHost.State;
            if (ret == CommunicationState.Faulted || ret == CommunicationState.Closed || ret == CommunicationState.Created)
            {
                productsServiceHost.Open();
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN  실행");
            }
            else
            {
                Logger(Log.디버깅, "WCF_ON: WCF 객체.OPEN 열려있는 상태입니다.");
            }
        }
        Thread t;
        private void WCF_ON_Click(object sender, EventArgs e)
        {
            // starting the thread
            if (t == null)
            {
                t = new Thread(StartService);
                t.Start();
            }
        }

        private void WCF_OFF_Click(object sender, EventArgs e)
        {
            if (productsServiceHost != null)
            {
                productsServiceHost.Close();
                Logger(Log.디버깅, "WCF_OFF:호출");
            }
            else
            {
                Logger(Log.디버깅, "WCF_OFF:wcf 객체 null");
            }

        }

        private void WCF상태_Click(object sender, EventArgs e)
        {
            if (productsServiceHost != null)
            {
                CommunicationState ret = productsServiceHost.State;
                debugCommunicationState(ret);
            }
            else
            {
                Logger(Log.디버깅, "WCF상태:wcf 객체 null");
            }

        }

        private void GetLoginInfo_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }

            String sTag = cb_GetLoginInfo.SelectedValue.ToString();
            Logger(Log.디버깅, "cb_GetLoginIn.Value:" + sTag);
            String tTag = cb_GetLoginInfo.Text;
            Logger(Log.디버깅, "cb_GetLoginIn.Text:" + tTag);
            if (sTag.Equals(""))
            {
                MessageBox.Show("사용자정보를 반환할 키값을 선택해주세요");
                return;
            }
            String ret = axKHOpenAPI.GetLoginInfo(sTag);
            Logger(Log.디버깅, "GetLoginIn[" + sTag + "]:" + ret);
            if (sTag.Equals("ACCNO"))
            {
                tx_account.Text = ret;
            }
        }

        private void GetAPIModulePath_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            /*OpenAPI모듈의 경로를 반환한다.*/
            String ret = axKHOpenAPI.GetAPIModulePath();
            Logger(Log.디버깅, "GetAPIModulePath:" + ret);
        }

        private void GetCodeListByMarket_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }

            String sTag = cb_GetCodeListByMarket.SelectedValue.ToString();
            Logger(Log.디버깅, "cb_GetCodeListByMarket.Value:" + sTag);
            String tTag = cb_GetCodeListByMarket.Text;
            Logger(Log.디버깅, "cb_GetCodeListByMarket.Text:" + tTag);
            if (sTag.Equals(""))
            {
                MessageBox.Show("시장구분을 선택해주세요");
                return;
            }
            String ret = axKHOpenAPI.GetCodeListByMarket(sTag);
            Logger(Log.디버깅, "GetCodeListByMarket[" + sTag + "]:" + ret);
        }
        private void wcfFaultHandler(object sender, EventArgs e)
        {

        }

        private void wcfUnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
            FileLog.PrintF(e.Message.ToString());
        }

        private void GetBalance_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String account = tx_account.Text;
            String password = tx_password.Text;

            Logger(Log.디버깅, "tx_account.Text:" + account);
            Logger(Log.디버깅, "tx_password.Text:" + password);
            if (account.Equals(""))
            {
                MessageBox.Show("계좌번호를 입력해주세요.");
                return;
            }

            if (password.Equals(""))
            {
                MessageBox.Show("이 패스워드는 의미 없음 하단에 트레이아이콘에서 설정을 눌러서 비밀번호를 저장해야함.");
                return;
            }

            axKHOpenAPI.SetInputValue("계좌번호", account.Trim());
            axKHOpenAPI.SetInputValue("비밀번호", password.Trim());
            axKHOpenAPI.SetInputValue("상장폐지조회구분", "0");
            axKHOpenAPI.SetInputValue("비밀번호입력매체구분", "00");
            //int nRet = axKHOpenAPI.CommRqData("RQName", "OPW00004", 0, "0200");
            int nRet = axKHOpenAPI.CommRqData("[0346]실시간계좌관리(T)-잔고확인", "OPW00004", 0, "화면번호");
            if (Error.IsError(nRet))
            {
                Logger(Log.일반, "[OPW00004] : " + Error.GetErrorMessage());
            }
            else
            {
                Logger(Log.에러, "[OPW00004] : " + Error.GetErrorMessage());
            }
            Logger(Log.디버깅, "GetBalance_Click::CommRqData::ret:" + nRet);

        }

        private void lst조회_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != lst조회) return;

            if (e.Control && e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in lst조회.SelectedItems)
                builder.AppendLine(item.SubItems[1].Text);

            Clipboard.SetText(builder.ToString());
        }


        private void GetStockOrgan_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            string 매매구분 = null;
            foreach (Control control in this.매매구분.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        매매구분 = radio.Tag.ToString();
                    }
                }
            }

            string 금액수량구분 = null;
            foreach (Control control in this.금액수량구분.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        금액수량구분 = radio.Tag.ToString();
                    }
                }
            }
            String stockCode = this.tx_stockCode_1.Text.ToString();
            String startDate = this.tx_startDate_1.Text.ToString();
            String endDate = this.tx_endDate_1.Text.ToString();            
            AppLib.InsertOpt10059(stockCode, startDate, endDate, 매매구분, 금액수량구분);
        }

        private void btn_sellStockOff_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String stockCode = this.tx_stockCode_2.Text.ToString();
            String startDate = this.tx_startDate_2.Text.ToString();
            String endDate = this.tx_endDate_2.Text.ToString();
            AppLib.InsertOpt10014(stockCode, startDate, endDate);
        }

        private void btn_dailyDetailTrade_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String stockCode = this.tx_stockCode_3.Text.ToString();
            String startDate = this.tx_startDate_3.Text.ToString();
            String endDate = this.tx_endDate_3.Text.ToString();

            //일별거래상세요청
            //OPT10015
            AppLib.InsertOpt10015(stockCode, startDate, endDate);
        }

        private void btn_EOS_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }

            //1.주식기본정보요청 (당일)
            String sRQName = "주식기본정보요청";
            String sTrCode = "OPT10001";
            String startDate = tx_startDate_EOS.Text;
            String endDate = tx_endDate_EOS.Text;

            OpenApi.Spell.SpellOpt spellOpt10001 = new OpenApi.Spell.SpellOpt();
            spellOpt10001.sRQNAME = sRQName;
            spellOpt10001.sTrCode = sTrCode;
            spellOpt10001.endDate = endDate;  //안쓰지만 ZIP파일을 만들때 확인해야하므로 넣는다.
            //spellOpt10060.stockCode = stockCode;
            spellOpt10001.nPrevNext = 0;
            //spellOpt10060.sScreenNo = sScrNo;

            AppLib.threadJob(spellOpt10001);
            FileLog.PrintF("btn_EOS_Click spellOpt10001 threadJob");

            //2.주식분봉차트조회요청 
            String tick = "1";//1분봉만 받아서 나머지는 만들자.
            sRQName = "주식분봉차트조회요청";
            sTrCode = "OPT10080";

            OpenApi.Spell.SpellOpt spellOpt10080 = new OpenApi.Spell.SpellOpt();
            spellOpt10080.sRQNAME = sRQName;
            spellOpt10080.sTrCode = sTrCode;
            //spellOpt10080.stockCode = stockCode;
            spellOpt10080.startDate = startDate; //startDate는 의미가 있다. nPrevNext가 2로 나온다..
            spellOpt10080.endDate = endDate;   //endDate가 실상의미가 없다... 최근것부터 받으므로..
            spellOpt10080.tick = tick;
            spellOpt10080.nPrevNext = 0;
            //spellOpt10080.sScreenNo = sScrNo;
            AppLib.threadJob(spellOpt10080);
            FileLog.PrintF("btn_EOS_Click spellOpt10080 threadJob");

            //3.주식기본정보요청
            sTrCode = "OPT10081";
            sRQName = "주식일봉차트조회";

            OpenApi.Spell.SpellOpt spellOpt10081 = new OpenApi.Spell.SpellOpt();
            spellOpt10081.sRQNAME = sRQName;
            spellOpt10081.sTrCode = sTrCode;
            //spellOpt10081.stockCode = 종목코드;
            spellOpt10081.startDate = startDate;
            spellOpt10081.endDate = endDate;
            spellOpt10081.nPrevNext = 0;
            //spellOpt10081.sScreenNo = sScrNo;

            AppLib.threadJob(spellOpt10081);
            FileLog.PrintF("btn_EOS_Click OPT10081 threadJob");

            //4.일별거래상세요청
            sRQName = "일별거래상세요청";
            sTrCode = "OPT10015";

            OpenApi.Spell.SpellOpt spellOpt10015 = new OpenApi.Spell.SpellOpt();
            spellOpt10015.sRQNAME = sRQName;
            spellOpt10015.sTrCode = sTrCode;
            //spellOpt10015.stockCode = stockCode;
            spellOpt10015.startDate = startDate;
            spellOpt10015.endDate = endDate;
            spellOpt10015.nPrevNext = 0;
            //spellOpt10015.sScreenNo = sScrNo;

            AppLib.threadJob(spellOpt10015);
            FileLog.PrintF("btn_EOS_Click OPT10015 threadJob");

            //5.종목별투자자기관별차트(금액 AND 매수)
            sRQName = "종목별투자자기관별차트요청_1_1";
            sTrCode = "OPT10059";
            OpenApi.Spell.SpellOpt spellOpt10059 = new OpenApi.Spell.SpellOpt();
            spellOpt10059.sRQNAME = sRQName;
            spellOpt10059.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10059.startDate = startDate;
            spellOpt10059.endDate = endDate;
            spellOpt10059.nPrevNext = 0;
            spellOpt10059.priceOrAmount = "1";
            spellOpt10059.buyOrSell = "1";
            //spellOpt10060.sScreenNo = sScrNo;
            AppLib.threadJob(spellOpt10059);
            FileLog.PrintF("btn_EOS_Click OPT10059 금액 AND 매수  threadJob");
            //5.종목별투자자기관별차트(금액 AND 매도)
            sRQName = "종목별투자자기관별차트요청_1_2";
            spellOpt10059 = new OpenApi.Spell.SpellOpt();
            spellOpt10059.sRQNAME = sRQName;
            spellOpt10059.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10059.startDate = startDate;
            spellOpt10059.endDate = endDate;
            spellOpt10059.nPrevNext = 0;
            spellOpt10059.priceOrAmount = "1";
            spellOpt10059.buyOrSell = "2";
            //spellOpt10060.sScreenNo = sScrNo;
            AppLib.threadJob(spellOpt10059);
            FileLog.PrintF("btn_EOS_Click OPT10059 금액 AND 매도  threadJob");
            //3.종목별투자자기관별차트(수량 AND 매수)
            sRQName = "종목별투자자기관별차트요청_2_1";
            spellOpt10059 = new OpenApi.Spell.SpellOpt();
            spellOpt10059.sRQNAME = sRQName;
            spellOpt10059.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10059.startDate = startDate;
            spellOpt10059.endDate = endDate;
            spellOpt10059.nPrevNext = 0;
            spellOpt10059.priceOrAmount = "2";
            spellOpt10059.buyOrSell = "1";
            //spellOpt10060.sScreenNo = sScrNo;
            AppLib.threadJob(spellOpt10059);
            FileLog.PrintF("btn_EOS_Click OPT10059 수량 AND 매수  threadJob");

            //5.종목별투자자기관별차트(수량 AND 매도)
            sRQName = "종목별투자자기관별차트요청_2_2";
            spellOpt10059 = new OpenApi.Spell.SpellOpt();
            spellOpt10059.sRQNAME = sRQName;
            spellOpt10059.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10059.startDate = startDate;
            spellOpt10059.endDate = endDate;
            spellOpt10059.nPrevNext = 0;
            spellOpt10059.priceOrAmount = "2";
            spellOpt10059.buyOrSell = "2";
            //spellOpt10060.sScreenNo = sScrNo;
            AppLib.threadJob(spellOpt10059);
            FileLog.PrintF("btn_EOS_Click OPT10059 수량 AND 매도  threadJob");


            //5.공매도추이요청
            sRQName = "공매도추이요청";
            sTrCode = "OPT10014";

            OpenApi.Spell.SpellOpt spellOpt10014 = new OpenApi.Spell.SpellOpt();
            spellOpt10014.sRQNAME = sRQName;
            spellOpt10014.sTrCode = sTrCode;
            //spellOpt10060.stockCode = stockCode;
            spellOpt10014.startDate = startDate;
            spellOpt10014.endDate = endDate;
            spellOpt10014.nPrevNext = 0;
            //spellOpt10060.sScreenNo = sScrNo;

            AppLib.threadJob(spellOpt10014);
            FileLog.PrintF("btn_EOS_Click OPT10014 threadJob");

            //EOS 가 다돌고 파일을 압축할수있도록
            AppLib.getClass1Instance().iEOS = 1;
            AppLib.getClass1Instance().endDateEos = endDate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppLib.getClass1Instance().endDateEos = this.tx_endDate_EOS.Text.ToString();
            AppLib.getClass1Instance().iEOS = 1;
            StockFile.EOS_CompressZip();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var confirmation = MessageBox.Show("종료되려나?", "Confirm", MessageBoxButtons.YesNo);
            if (confirmation == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true; //Even cancelled, form will not get closed now
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppLib.getClass1Instance().ClosedAll();
            FileLog.PrintF("Form1_FormClosed Form_Closed");
        }

        private void btn_pushFtp_Click(object sender, EventArgs e)
        {
            StockFile.PushFtpZipFiles();
        }

        String strRealType = "0";

        private void btn_setRealData_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            /*
            SetRealReg(LPCTSTR strScreenNo, LPCTSTR strCodeList, LPCTSTR strFidList, LPCTSTR strRealType)
            설명 실시간 등록을 한다.
            입력값
            strScreenNo – 실시간 등록할 화면 번호
            strCodeList – 실시간 등록할 종목코드(복수종목가능 – “종목1;종목2;종목3;….”)
            strFidList – 실시간 등록할 FID(“FID1;FID2;FID3;…..”)
            strRealType – “0”, “1” 타입
            반환값 통신결과
            비고
            strRealType이 “0” 으로 하면 같은화면에서 다른종목 코드로 실시간 등록을 하게 되면
            마지막에 사용한 종목코드만 실시간 등록이 되고 기존에 있던 종목은 실시간이 자동 해
            지됨.
            “1”로 하면 같은화면에서 다른 종목들을 추가하게 되면 기존에 등록한 종목도 함께 실
            시간 시세를 받을 수 있음.
            꼭 같은
            */



            //String strCodeList = "181710;035420;035720";
            String strCodeList = this.tx_stockCodeReal.Text.ToString().Trim();
            String strScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            /*String strFidList = "현재가(10);";*/
            String strFidList = this.tx_fid.Text.ToString().Trim();
            /*112405	-51000	-300	-0.58	-51100	-51000	-6	41414	2121	 51300	+51700	-50900	5	-109963	-5515337950	-27.36	0.21	169	91.79	9978	2	0	000000	000000*/

            //strRealType = "0";  /*설명서에는 0,1로 나와있는데 ... 처음엔 0으로 한번 지나고 나면 1로. */
            /*알아보니
            주식시세하고 주식체결 하고 별반 차이가 없는것으로.. 주식체결은 빈번하고 주식시세는 좀처럼 발생안한다고 한다.
            SetRealReg 함수의 인자로 "주식시세" 타입에 속해있는 FID 들을 입력하셨다면 "주식시세" 타입의 실시간데이터가 OnReceiveRealData로 수신됩니다. 체결없이 현재가가 갱신되는 경우 "주식시세" 타입이 발생하여 체결로서 발생되는 "주식체결" 타입보다는 빈도수가 훨씬 적게 수신되니 참고해주시기 바랍니다. 
            ----------------------------------------------------------------------------------------------
            전종목을 실시간으로 보아도 문제가 없다???
            안녕하세요~? 
            엄격히는 실시간 "주식 시세" 와 실시간 "주식 체결" 과는 조금 다른데요.. 
            아마 질문 하시는 의도가 "주식 체결" 을 뜻하는거 같습니다. 
            최대 1개의 화면번호에서 100개 까지 요청 가능 합니다. 
            더 필요하면 화면번호를 늘리면 되겠습니다.(물론 사용할수 있는 화면번호의 갯수의 제한이 있습니다.) 
            실시간 주식 체결 데이터에서는 매도 체결인지 매수 체결인지 구분 가능합니다. 
            관련 FID를 쭉 찍어 보시면 확인 가능하고 매수체결인지 매도체결인지에 따라 값앞에 +/- 기호가 붙기도 합니다. 
            그럼 좀 급해보이시는거 같아 영자님 출몰하시기 전에 미리 답해드립니다. 
            */
            FileLog.PrintF("btn_setRealData_Click strScreenNo=>" + strScreenNo);
            FileLog.PrintF("btn_setRealData_Click strCodeList=>" + strCodeList);
            FileLog.PrintF("btn_setRealData_Click strFidList=>" + strFidList);
            FileLog.PrintF("btn_setRealData_Click strRealType=>" + strRealType);
            axKHOpenAPI.SetRealReg(strScreenNo, strCodeList, strFidList, strRealType);
            strRealType = "1";

        }

        private void btn_getStockDataToDay_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String stockCode = this.tx_stockCode_5.Text.ToString();
            FileLog.PrintF("btn_getStockDataToDay_Click stockCode=" + stockCode);
            AppLib.InsertOpt10001(stockCode);
        }

        private void btn_getStockDataMinute_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String stockCode = this.tx_stockCode_6.Text.ToString();
            String startDate = this.tx_startDate_6.Text.ToString();
            String tick = this.tx_tick_6.Text.ToString();
            String sRQName = "주식분봉차트조회요청";
            String sTrCode = "OPT10080";

            FileLog.PrintF("btn_getStockDataMinute_Click stockCode=" + stockCode);
            FileLog.PrintF("btn_getStockDataMinute_Click startDate=" + startDate);
            FileLog.PrintF("btn_getStockDataMinute_Click tick=" + tick);

            OpenApi.Spell.SpellOpt spellOpt10080 = new OpenApi.Spell.SpellOpt();
            spellOpt10080.sRQNAME = sRQName;
            spellOpt10080.sTrCode = sTrCode;
            //spellOpt10015.stockCode = stockCode;
            spellOpt10080.startDate = startDate;
            spellOpt10080.tick = tick;
            spellOpt10080.nPrevNext = 0;
            //spellOpt10015.sScreenNo = sScrNo;

            if (stockCode.Equals("ALL"))
            {
                AppLib.threadJob(spellOpt10080);
                FileLog.PrintF("btn_getStockDataMinute_Click threadJob");
            }
            else
            {
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , sRQName
                    , sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , sRQName
                    , sTrCode
                    , sScrNo
                    , stockCode
                );
                FileLog.PrintF("keyStockCode  ==" + keyStockCode);
                FileLog.PrintF("key  ==" + key);

                spellOpt10080.stockCode = stockCode;
                spellOpt10080.key = key;
                spellOpt10080.sScreenNo = sScrNo;
                spellOpt10080.reportGubun = "FILE";

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10080);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10080);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }
        }

        private void btn_removeRealData_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
        }

        private void btn_GetEstimationProperty_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String accountNum = this.tx_accountNum_1.Text.ToString();
            String password = this.tx_password_1.Text.ToString();
            String sRQName = "추정자산조회요청";
            String sTrCode = "OPW00003";

            FileLog.PrintF("btn_GetEstimationProperty_Click accountNum=" + accountNum);
            FileLog.PrintF("btn_GetEstimationProperty_Click password=" + password);

            OpenApi.Spell.SpellOpt spellOpw00003 = new OpenApi.Spell.SpellOpt();
            spellOpw00003.sRQNAME = sRQName;
            spellOpw00003.sTrCode = sTrCode;
            spellOpw00003.accountNum = accountNum;
            spellOpw00003.password = password;


            String sScrNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
            String keyStockCode = String.Format(keyStockCodeLayout
                , sRQName
                , sTrCode
                , sScrNo
            );
            String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|accountNum:{3}";
            String key = String.Format(keyLayout
               , sRQName
                , sTrCode
                , sScrNo
                , accountNum
            );
            spellOpw00003.sScreenNo = sScrNo;
            FileLog.PrintF("keyStockCode  ==" + keyStockCode);

            AppLib.getClass1Instance().AddSpellDictionary(key, spellOpw00003);
            AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, accountNum);
            //QUEUE를 따지 않고 바로 실행되어야 한다.
            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(spellOpw00003.sTrCode);
            int nRet = rt.Run(axKHOpenAPI, spellOpw00003);
            spellOpw00003.startRunTime = DateTime.Now;

        }

        private void btn_GetAccountEarningsRate_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String accountNum = this.tx_accountNum_2.Text.ToString();
            String sRQName = "계좌수익률요청";
            String sTrCode = "OPT10085";

            FileLog.PrintF("btn_GetAccountEarningsRate_Click accountNum=" + accountNum);

            OpenApi.Spell.SpellOpt spellOpt10085 = new OpenApi.Spell.SpellOpt();
            spellOpt10085.sRQNAME = sRQName;
            spellOpt10085.sTrCode = sTrCode;
            spellOpt10085.accountNum = accountNum;

            String sScrNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
            String keyStockCode = String.Format(keyStockCodeLayout
                , sRQName
                , sTrCode
                , sScrNo
            );
            String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|accountNum:{3}";
            String key = String.Format(keyLayout
               , sRQName
                , sTrCode
                , sScrNo
                , accountNum
            );
            spellOpt10085.sScreenNo = sScrNo;
            FileLog.PrintF("keyStockCode  ==" + keyStockCode);

            spellOpt10085.key = key;
            spellOpt10085.sScreenNo = sScrNo;
            spellOpt10085.reportGubun = "FILE";

            AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10085);
            AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10085);
            AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, accountNum);
        }

        private void btn_GetConditionNameList_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            axKHOpenAPI.GetConditionLoad();

        }

        private void btn_sendCondition_Click(object sender, EventArgs e)
        {
            /*
            :BOOL SendCondition(LPCTSTR strScrNo, LPCTSTR strConditionName, int nIndex, int nSearch);
            -반환값 : FALSE(실패), TRUE(성공)
            - 파라메터 설명
             strScrNo : 화면번호
             strConditionName :GetConditionNameList()로 불러온 조건명중 하나의 조건명.
             nIndex : GetCondionNameList()로 불러온 조건인덱스.
             nSearch : 일반조회(0), 실시간조회(1), 연속조회(2)
            nSearch 를 0으로 조회하면 단순 해당 조건명(식)에 맞는 종목리스트를 받아올 수 있습니다. 
            1로 조회하면 해당 조건명(식)에 맞는 종목리스트를 받아 오면서 실시간으로 편입, 이탈하는 종목을 받을 수 있는 조건이 됩니다.
            - 1번으로 조회 할 수 있는 화면 개수는 최대 10개까지 입니다.
            - 2은 OnReceiveTrCondition 이벤트 함수에서 마지막 파라메터인 nNext가 “2”로들어오면 종목이 더 있기 때문에 
            다음 조회를 원할 때 OnReceiveTrCondition 이벤트 함수에서 사용하시면 됩니다.
            인덱스1^조건명1;인덱스2^조건명2;인덱스3^조건명3;…
            */
            String strConditionName = this.tx_conditionName.Text.ToString();
            int nIndex = Int32.Parse(this.tx_index.Text.ToString());
            String sScrNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            FileLog.PrintF("btn_sendCondition_Click strConditionName=>" + strConditionName);
            FileLog.PrintF("btn_sendCondition_Click nIndex=>" + nIndex);
            FileLog.PrintF("btn_sendCondition_Click sScrNo=>" + sScrNo);
            axKHOpenAPI.SendCondition(sScrNo, strConditionName, nIndex, 0);
        }

        private void btn_SendOrder_Click(object sender, EventArgs e)
        {
            /*
            LONG SendOrder(BSTR sRQName,BSTR sScreenNo,BSTR sAccNo,LONG nOrderType,BSTR sCode,LONG nQty,LONG nPrice,BSTR sHogaGb,BSTR sOrgOrderNo)
                설명 주식 주문을 서버로 전송한다.
                입력값
                sRQName - 사용자 구분 요청 명
                sScreenNo - 화면번호[4]
                sAccNo - 계좌번호[10]
                nOrderType - 주문유형(1:신규매수, 2:신규매도, 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정)
                sCode, -주식종목코드
                nQty – 주문수량
                nPrice – 주문단가
                sHogaGb - 거래구분
                sOrgOrderNo – 원주문번호
                반환값 에러코드 < 4.에러코드표 참고 >
                비고
                sHogaGb – 00:지정가, 03:시장가, 05:조건부지정가, 06:최유리지정가, 07:최우선지정가, 10:지정가IOC, 13:시장가IOC, 16:최유리IOC, 20:지정가FOK, 23:시장가FOK, 26:최유리FOK, 61:시간외단일가매매, 81:시간외종가
                ex) 지정가 매수 -openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 1, “000660”, 10, 48500,“0”, “”);
                    시장가 매수 -openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 1, “000660”, 10, 0, “3”,“”);
                    매수 정정 -openApi.SendOrder(“RQ_1”,“0101”, “5015123410”, 5, “000660”, 10, 49500,“0”, “1”);
                    매수 취소 -openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 3, “000660”, 10, “0”, “2”);
            */

            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }


            String accountNumber = this.tx_accountNum_3.Text.ToString().Trim();
            int nOrderType = int.Parse(this.cb_SendOrderNOrderType.SelectedValue.ToString());
            String sCode = this.tx_sCode.Text.ToString().Trim();
            int nQty = int.Parse(this.tx_nQty.Text.ToString().Trim());
            int nPrice = int.Parse(this.tx_nPrice.Text.ToString().Trim());
            String sHogaGb = this.cb_SendOrderSHogaGb.SelectedValue.ToString();
            String sOrgOrderNo = this.tx_sOrgOrderNo.Text.ToString().Trim();
            String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();


            FileLog.PrintF("btn_SendOrder_Click sScreenNo=>" + sScreenNo);
            FileLog.PrintF("btn_SendOrder_Click accountNumber=>" + accountNumber);
            FileLog.PrintF("btn_SendOrder_Click nOrderType=>" + nOrderType);
            FileLog.PrintF("btn_SendOrder_Click sCode=>" + sCode);
            FileLog.PrintF("btn_SendOrder_Click nQty=>" + nQty);
            FileLog.PrintF("btn_SendOrder_Click nPrice=>" + nPrice);
            FileLog.PrintF("btn_SendOrder_Click sHogaGb=>" + sHogaGb);
            FileLog.PrintF("btn_SendOrder_Click sOrgOrderNo=>" + sOrgOrderNo);


            int ret = axKHOpenAPI.SendOrder("주문", sScreenNo, accountNumber, nOrderType, sCode, nQty, nPrice, sHogaGb, sOrgOrderNo);

            FileLog.PrintF("btn_SendOrder_Click ret=>" + ret);

        }



        private void btn_AutoSell(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String accountNumber = this.tx_accountNum_4.Text.ToString().Trim();
            int profit_rate = int.Parse(this.tx_profit_rate.Text.ToString().Trim());
            int loss_rate = int.Parse(this.tx_profit_rate.Text.ToString().Trim());
            Boolean loss_status = false;
            foreach (Control control in this.손절매활성화여부.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        String tmp = radio.Tag.ToString();
                        if (tmp.ToString().Trim().Equals("1"))
                        {
                            loss_status = true;
                        }
                        else
                        {
                            loss_status = false;
                        }
                    }
                }
            }

            Boolean profit_status = false;
            foreach (Control control in this.이익매활성화여부.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        String tmp = radio.Tag.ToString();
                        if (tmp.ToString().Trim().Equals("1"))
                        {
                            profit_status = true;
                        }
                        else
                        {
                            profit_status = false;
                        }
                    }
                }
            }

            if (loss_status == true)
            {
                lb_lossStatus.BackColor = Color.GreenYellow;
                lb_lossStatus.Text = "손절매ON";
            }
            else
            {
                lb_lossStatus.BackColor = Color.Gray;
                lb_lossStatus.Text = "손절매OFF";
            }

            if (profit_status == true)
            {
                lb_profitStatus.BackColor = Color.GreenYellow;
                lb_profitStatus.Text = "이익매ON";
            }
            else
            {
                lb_profitStatus.BackColor = Color.Gray;
                lb_profitStatus.Text = "이익매ON";
            }

            MyStock.getClass1Instance().profit_rate = profit_rate;
            MyStock.getClass1Instance().loss_rate = loss_rate;
            MyStock.getClass1Instance().loss_status = loss_status;
            MyStock.getClass1Instance().profit_status = profit_status;
            MyStock.getClass1Instance().accountNumber = accountNumber;
        }

        private void btn_SearchContractStatus_Click(object sender, EventArgs e)
        {
            String accountNum = this.tx_accountNum_5.Text.ToString().Trim();
            String orderStatus = this.tx_orderStatus.Text.ToString().Trim();  //체결구분 0:전체 ,1:미체결
            String orderGubun = this.tx_orderGubun.Text.ToString().Trim(); //매매구분 0:전체,  1:매도, 2:매수

            String sRQName = "실시간미체결요청";
            String sTrCode = "OPT10075";

            OpenApi.Spell.SpellOpt spellOpt10075 = new OpenApi.Spell.SpellOpt();
            spellOpt10075.sRQNAME = sRQName;
            spellOpt10075.sTrCode = sTrCode;
            spellOpt10075.accountNum = accountNum;
            spellOpt10075.orderStatus = orderStatus;
            spellOpt10075.orderGubun = orderGubun;


            String sScrNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
            String keyStockCode = String.Format(keyStockCodeLayout
                , sRQName
                , sTrCode
                , sScrNo
            );
            String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|accountNum:{3}";
            String key = String.Format(keyLayout
               , sRQName
                , sTrCode
                , sScrNo
                , accountNum
            );
            spellOpt10075.sScreenNo = sScrNo;
            FileLog.PrintF("keyStockCode  ==" + keyStockCode);

            AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10075);
            AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, accountNum);


            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(spellOpt10075.sTrCode);
            int nRet = rt.Run(axKHOpenAPI, spellOpt10075);
            spellOpt10075.startRunTime = DateTime.Now;
        }

        private void btn_GetStockInfo_Click(object sender, EventArgs e)
        {
            String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            String stockCode = this.tx_stockCode_7.Text.ToString().Trim();
            String ret = "";

            if (stockCode.Trim().Equals("ALL"))
            {
                ret = GetStockInfoAll();

            }
            else
            {

                String tmp = "{0};{1};{2};{3};{4};{5};{6};{7}";
                String stockName = axKHOpenAPI.GetMasterCodeName(stockCode).ToString().Trim();//종목이름
                int stockCount = axKHOpenAPI.GetMasterListedStockCnt(stockCode); //상장주식수
                String strYesterdayCurrentPrice = axKHOpenAPI.GetMasterLastPrice(stockCode).ToString().Trim();//전일가(기준가)
                int yesterdayCurrentPrice = int.Parse(strYesterdayCurrentPrice);
                String status = axKHOpenAPI.GetMasterStockState(stockCode).ToString().Trim();//종목상태반환 // 정상, 증거금100%, 거래정지, 관리종목, 감리종목, 투자유의종목, 담보대출, 액면분할, 신용가능
                String construction = axKHOpenAPI.GetMasterConstruction(stockCode).ToString().Trim();//감리구분 //정상, 투자주의, 투자경고, 투자위험, 투자주의환기종목
                String startDate = axKHOpenAPI.GetMasterListedStockDate(stockCode).ToString().Trim();//상장일반환  20160225
                startDate = startDate.TrimEnd(new char[] { '\0' });
                String content = String.Format(tmp
                   , dayTime  //[0]
                    , stockCode  //[1]
                    , stockName  //[2]
                    , stockCount  //[3]
                    , yesterdayCurrentPrice  //[4]
                    , status  //[5]
                    , construction  //[6]
                    , startDate  //[7]
                );
                ret = content;
                FileLog.PrintF("btn_GetStockInfo_Click  yesterdayCurrentPrice=>" + yesterdayCurrentPrice);
                FileLog.PrintF("btn_GetStockInfo_Click  startDate=>" + startDate);
            }
            FileLog.PrintF("btn_GetStockInfo_Click  ret=>" + ret);

        }

        private String GetStockInfoAll()
        {
            String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<String> stockCodeList = AppLib.getClass1Instance().getStockCodeList();
            StringBuilder sb = new StringBuilder();
            String tmp = "{0};{1};{2};{3};{4};{5};{6};{7}";
            foreach (String stockCode in stockCodeList)
            {
                String stockName = axKHOpenAPI.GetMasterCodeName(stockCode);//종목이름
                int stockCount = axKHOpenAPI.GetMasterListedStockCnt(stockCode); //상장주식수
                String strYesterdayCurrentPrice = axKHOpenAPI.GetMasterLastPrice(stockCode).ToString().Trim();//전일가(기준가)
                int yesterdayCurrentPrice = int.Parse(strYesterdayCurrentPrice);
                String status = axKHOpenAPI.GetMasterStockState(stockCode);//종목상태반환 // 정상, 증거금100%, 거래정지, 관리종목, 감리종목, 투자유의종목, 담보대출, 액면분할, 신용가능
                String construction = axKHOpenAPI.GetMasterConstruction(stockCode);//감리구분 //정상, 투자주의, 투자경고, 투자위험, 투자주의환기종목
                String startDate = axKHOpenAPI.GetMasterListedStockDate(stockCode);//상장일반환  20160225
                startDate = startDate.TrimEnd(new char[] { '\0' });
                String content = String.Format(tmp
                   , dayTime  //[0]
                    , stockCode  //[1]
                    , stockName  //[2]
                    , stockCount  //[3]
                    , yesterdayCurrentPrice  //[4]
                    , status  //[5]
                    , construction  //[6]
                    , startDate  //[7]
                );
                sb.AppendLine(content);
            }
            return sb.ToString();
        }

        private void BTN_FST_DATA_SETUP_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            //0:장내, 3:ELW, 4:뮤추얼펀드, 5:신주인수권, 6:리츠, 8:ETF, 9:하이일드펀드, 10:코스닥, 30:K - OTC, 50:코넥스(KONEX)
            String[] arr_market = { "0", "3", "4", "5", "6", "8", "9", "10", "30", "50" };
            List<TB_STOCK> lst = new List<TB_STOCK>();
            for (int i = 0; i < arr_market.Length; i++)
            {
                String market_cd = arr_market[i];
                String ret = axKHOpenAPI.GetCodeListByMarket(market_cd);
                String[] arr_ret = ret.Split(';');

                for (int j = 0; j < arr_ret.Length; j++)
                {
                    TB_STOCK tmp = new TB_STOCK();
                    String stock_cd = arr_ret[j];
                    String stock_nm = axKHOpenAPI.GetMasterCodeName(stock_cd);
                    int cnt = axKHOpenAPI.GetMasterListedStockCnt(stock_cd);
                    String construction = axKHOpenAPI.GetMasterConstruction(stock_cd);
                    String stock_dt = axKHOpenAPI.GetMasterListedStockDate(stock_cd);
                    String last_price = axKHOpenAPI.GetMasterLastPrice(stock_cd);
                    String stock_state = axKHOpenAPI.GetMasterStockState(stock_cd);


                    tmp.MARKET_CD = market_cd;
                    tmp.STOCK_CD = stock_cd;
                    tmp.STOCK_NM = stock_nm;
                    tmp.CNT = cnt;
                    tmp.CONSTRUCTION = construction;
                    tmp.STOCK_DT = stock_dt;
                    int i_last_price = 0;
                    if (int.TryParse(last_price, out i_last_price))
                    {
                        tmp.LAST_PRICE = i_last_price;
                    }
                    else
                    {
                        tmp.LAST_PRICE = 0;
                    }
                    tmp.STOCK_STATE = stock_state;
                    lst.Add(tmp);
                }
            }



            if (lst.Count > 0)
            {
                FirstData fd = new FirstData();
                fd.insertMarketStockData(lst);
            }
            lst.Clear();

            //정렬순서 (0:코드순, 1:테마순)
            //비고 반환값의 코드와 코드명 구분은 ‘|’ 코드의 구분은 ‘;’ Ex) 100|태양광_폴리실리콘;152|합성섬유
            String ret_theme = axKHOpenAPI.GetThemeGroupList(0);
            String[] arr_theme = ret_theme.Split(';');
            List<TB_THEME> lst_theme = new List<TB_THEME>();
            for (int i = 0; i < arr_theme.Length; i++)
            {
                String[] tmp2 = arr_theme[i].Split('|');
                TB_THEME theme = new TB_THEME();
                theme.THEME_CD = tmp2[0];
                theme.THEME_NM = tmp2[1];
                lst_theme.Add(theme);
                String ret = axKHOpenAPI.GetThemeGroupCode(tmp2[0]);
                String[] arr_ret = ret.Split(';');

                for (int j = 0; j < arr_ret.Length; j++)
                {
                    TB_STOCK tmp_stock = new TB_STOCK();
                    String stock_cd = arr_ret[j];
                    String stock_nm = axKHOpenAPI.GetMasterCodeName(stock_cd);
                    int cnt = axKHOpenAPI.GetMasterListedStockCnt(stock_cd);
                    String construction = axKHOpenAPI.GetMasterConstruction(stock_cd);
                    String stock_dt = axKHOpenAPI.GetMasterListedStockDate(stock_cd);
                    String last_price = axKHOpenAPI.GetMasterLastPrice(stock_cd);
                    String stock_state = axKHOpenAPI.GetMasterStockState(stock_cd);


                    tmp_stock.THEME_CD = theme.THEME_CD;
                    tmp_stock.STOCK_CD = stock_cd;
                    tmp_stock.STOCK_NM = stock_nm;
                    tmp_stock.CNT = cnt;
                    tmp_stock.CONSTRUCTION = construction;
                    tmp_stock.STOCK_DT = stock_dt;
                    int i_last_price = 0;
                    if (int.TryParse(last_price, out i_last_price))
                    {
                        tmp_stock.LAST_PRICE = i_last_price;
                    }
                    else
                    {
                        tmp_stock.LAST_PRICE = 0;
                    }
                    tmp_stock.STOCK_STATE = stock_state;
                    lst.Add(tmp_stock);
                }

            }

            if (lst_theme.Count > 0)
            {
                FirstData fd = new FirstData();
                fd.insertThemeData(lst_theme);
            }

            if (lst.Count > 0)
            {
                FirstData fd = new FirstData();
                fd.insertThemeStockData(lst);


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void axKHOpenAPI_OnReceiveInvestRealData(object sender, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveInvestRealDataEvent e)
        {

        }

        private void bnt_optkwfid_Click(object sender, EventArgs e)
        {
            if (IsConnected() == false)
            {
                MessageBox.Show("로그인을해주세요");
                return;
            }
            String stockCode = this.tx_stockCode_optkwfid.Text.ToString();
            FileLog.PrintF("bnt_optkwfid_Click stockCode=" + stockCode);
            AppLib.InsertOptkwfid(stockCode);
        }
    }
}
