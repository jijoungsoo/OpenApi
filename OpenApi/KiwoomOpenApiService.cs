using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OpenApi;
using AxKHOpenAPILib;
using KiwoomCode;
using System.Threading;
using OpenApi.ReceiveTrData;

namespace WcfServiceLibrary
{
    // 참고: "리팩터링" 메뉴에서 "이름 바꾸기" 명령을 사용하여 코드 및 config 파일에서 클래스 이름 "Service1"을 변경할 수 있습니다.
    public class KiwoomOpenApiService : IKiwoomOpenApi
    {
        private AppLib class2 = AppLib.getClass1Instance();
        private AxKHOpenAPI axKHOpenAPI= null;

        public KiwoomOpenApiService(){
            axKHOpenAPI = class2.getAxKHOpenAPIInstance();
        }

        public string GetData(String value)
        {
            return string.Format("You entered: {0}", value);
        }
        
        public string  GetCurrentStock(String 종목코드= "035420")
        {
            //035420   <--네이버종목번호\
            this.axKHOpenAPI.SetInputValue("종목코드", 종목코드);
            int nRet = axKHOpenAPI.CommRqData("주식기본정보", "OPT10001", 0, ScreenNumber.getClass1Instance().GetEosScrNum());

            return "sdfsdf:";


        }
        public String test()
        {
            Console.WriteLine("test");
            return "asdffasdfasdf";
        }

        #region 메소드
        /// <summary>
        /// [1]설명 로그인 윈도우를 실행한다.
        ///입력값 없음
        ///반환값 0 - 성공, 음수값은 실패
        ///비고
        ///로그인이 성공하거나 실패하는 경우 OnEventConnect 이벤트가 발생하고 이벤트의 인자값으로 로그인 성공 여부를 알 수 있다.
        ///웹서비스로는 실제 실행할일이 없음.. 엑티브엑스로 화면창이 뜨게 되어있음.
        /// </summary>
        public int CommConnect()
        {
            int ret = axKHOpenAPI.CommConnect();
            FileLog.PrintF("CommConnect():"+ ret);
            return ret;
        }

        /// <summary>
        /// [2]설명 OpenAPI의 서버 접속을 해제한다.
        ///비고 통신 연결 상태는 GetConnectState 메소드로 알 수 있다.
        /// </summary>
        public void CommTerminate()
        {
            axKHOpenAPI.CommTerminate();
            FileLog.PrintF("CommTerminate()");
        }
        
        /// <summary>
        /// [3]설명 Tran을 서버로 송신한다.
        ///입력값 
        ///sRQName – 사용자구분 명
        ///sTrCode - Tran명 입력
        ///nPrevNext - 0:조회, 2:연속
        ///sScreenNo - 4자리의 화면번호
        ///반환값
        ///OP_ERR_SISE_OVERFLOW – 과도한 시세조회로 인한 통신불가
        ///OP_ERR_RQ_STRUCT_FAIL – 입력 구조체 생성 실패
        ///OP_ERR_RQ_STRING_FAIL – 요청전문 작성 실패
        ///OP_ERR_NONE – 정상처리
        ///비고
        ///Ex) openApi.CommRqData( “RQ_1”, “OPT00001”, 0, “0101”);
        /// </summary>
        public int CommRqData(String sRQName, String sTrCode, int nPrevNext, String sScreenNo)
        {
            int ret = axKHOpenAPI.CommRqData(sRQName,sTrCode,nPrevNext,sScreenNo);
            FileLog.PrintF("CommRqData("+sRQName+","+sTrCode+","+nPrevNext+","+sScreenNo+"):" + ret);
            return ret;
        }
        

        /// <summary>
        /// [4]로그인한 사용자 정보를 반환한다.
        /// ACCOUNT_CNT – 전체 계좌 개수를 반환한다.
        ///ACCNO – 전체 계좌를 반환한다.계좌별 구분은 ‘;’이다.
        ///USER_ID - 사용자 ID를 반환한다.
        ///USER_NAME – 사용자명을 반환한다.
        ///KEY_BSECGB – 키보드보안 해지여부. 0:정상, 1:해지
        ///FIREW_SECGB – 방화벽 설정 여부. 0:미설정, 1:설정, 2:해지
        ///Ex) openApi.GetLoginInfo(“ACCOUNT_CNT”);
        /// </summary>
        public string GetLoginInfo(String sTag = "ACCOUNT_CNT")
        {
            String ret = axKHOpenAPI.GetLoginInfo(sTag);
            FileLog.PrintF("GetLoginInfo(" + sTag + ")" + ret);
            return ret;
        }

        



        /// <summary>
        /// [5]설명 주식 주문을 서버로 전송한다.
        ///입력값
        ///sRQName - 사용자 구분 요청 명
        ///sScreenNo - 화면번호[4]
        ///sAccNo - 계좌번호[10]
        ///nOrderType - 주문유형(1:신규매수, 2:신규매도, 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정)
        ///sCode, - 주식종목코드
        ///nQty – 주문수량
        ///nPrice – 주문단가
        ///sHogaGb - 거래구분
        ///sOrgOrderNo – 원주문번호
        ///반환값 에러코드[4.에러코드표 참고]
        ///비고
        ///sHogaGb – 00:지정가, 03:시장가, 05:조건부지정가, 06:최유리지정가, 07:최우선지정가, 10:
        ///지정가IOC, 13:시장가IOC, 16:최유리IOC, 20:지정가FOK, 23:시장가FOK, 26:최유리FOK, 61:시간외단일가매매, 81:시간외종가
        ///ex)
        ///지정가 매수 - openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 1, “000660”, 10, 48500,“0”, “”);
        ///시장가 매수 - openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 1, “000660”, 10, 0, “3”,“”);
        ///매수 정정 - openApi.SendOrder(“RQ_1”,“0101”, “5015123410”, 5, “000660”, 10, 49500,“0”, “1”);
        ///매수 취소 - openApi.SendOrder(“RQ_1”, “0101”, “5015123410”, 3, “000660”, 10, “0”, “2”);
        /// </summary>
        /// 
        public int SendOrder(String rq_name,String acct_num, int order_type, String stock_cd, int qty,int amt, String hoga_gubun, String orgn_order_no)
        {
            String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            int ret = axKHOpenAPI.SendOrder(rq_name,  sScreenNo, acct_num, order_type, stock_cd, qty, amt, hoga_gubun, orgn_order_no);
            FileLog.PrintF("SendOrder("+ rq_name + ","+sScreenNo+","+ acct_num + ","+ order_type + ","+ stock_cd + ","+ qty + ","+ amt + ","+ hoga_gubun + ","+ orgn_order_no + "):" + ret);
            return ret;
        }



        /// <summary>
        /// [6]설명 신용주식 주문을 서버로 전송한다.
        ///입력값
        ///sRQName - 사용자 구분 요청 명
        ///sScreenNo - 화면번호[4]
        ///sAccNo - 계좌번호[10]
        //nOrderType - 주문유형(1:신규매수, 2:신규매도, 3:매수취소, 4:매도취소, 5:매수정정, 6:매도정정)
        ///sCode, - 주식종목코드
        ///nQty – 주문수량
        ///nPrice – 주문단가
        ///sHogaGb – 거래구분
        ///sCreditGb – 신용구분
        ///sLoanDate – 대출일
        ///sOrgOrderNo – 원주문번호
        ///반환값 에러코드[4.에러코드표 참고]
        ///비고
        ///sCreditGb – 신용구분 (신용매수:03, 신용매도 융자상환:33, 신용매도 융자합:99)
        ///신용매수 주문- 신용구분값 “03”, 대출일은 “공백”
        ///신용매도 융자상환 주문
        ///- 신용구분값 “33”, 대출일은 종목별 대출일 입력
        ///- OPW00005/OPW00004 TR조회로 대출일 조회
        ///신용매도 융자합 주문시
        ///- 신용구분값 “99”, 대출일은 “99991231”
        ///- 단 신용잔고 5개까지만 융자합 주문가능
        ///나머지 입력값은 SendOrder()함수 설명참고
        /// </summary>
        public int SendOrderCredit(String sRQName, String sScreenNo, String sAccNo, int nOrderType, String sCode, int nQty, int nPrice, String sHogaGb, String sCreditGb, String sLoanDate,String sOrgOrderNo)
        {
            int ret = axKHOpenAPI.SendOrderCredit(sRQName, sScreenNo, sAccNo, nOrderType, sCode, nQty, nPrice, sHogaGb, sCreditGb, sLoanDate, sOrgOrderNo);
            FileLog.PrintF("SendOrderCredtit(" + sRQName + "," + sScreenNo + "," + sAccNo + "," + nOrderType + "," + sCode + "," + nQty + "," + nPrice + "," + sHogaGb + ","+sCreditGb+","+sLoanDate+"," + sOrgOrderNo + "):" + ret);
            return ret;
        }




        /// <summary>
        /// [7]원형 void SetInputValue(BSTR sID, BSTR sValue)
        ////설명 Tran 입력 값을 서버통신 전에 입력한다.
        ///입력값
        ///sID – 아이템명
        ///sValue – 입력 값
        //비고 통신 Tran 매뉴얼 참고
        ///Ex) openApi.SetInputValue(“종목코드”, “000660”);
        ///openApi.SetInputValue(“계좌번호”, “5015123401”);
        /// </summary>
        public void SetInputValue(String sID, String sValue)
        {
            axKHOpenAPI.SetInputValue(sID, sValue);
            FileLog.PrintF("SetInputValue(" + sID + "," + sValue+")");
        }


        /// <summary>
        /// [8]설명 1.0.0.1 버전 이후 사용하지 않음.
        ///입력값
        ///반환값
        ///비고
        /// </summary>
        public void SetOutputFID(String sID)
        {
            axKHOpenAPI.SetOutputFID(sID);
            FileLog.PrintF("SetOutputFID(" + sID + ")");
        }


        /// <summary>
        /// [9]설명 Tran 데이터, 실시간 데이터, 체결잔고 데이터를 반환한다.
        ///입력값<비고>
        ///반환값 요청 데이터
        ///비고
        ///○1 Tran 데이터
        ///sJongmokCode : Tran명
        ///sRealType : 사용안함
        ///sFieldName : 레코드명
        ///nIndex : 반복인덱스
        ///sInnerFieldName: 아이템명
        ///○1 실시간 데이터
        ///sJongmokCode : Key Code
        ///sRealType : Real Type
        ///sFieldName : Item Index
        ///nIndex : 사용안함
        ///sInnerFieldName:사용안함
        ///○1 체결 데이터
        ///sJongmokCode : 체결구분
        ///sRealType : “-1”
        ///sFieldName : 사용안함
        ///nIndex : ItemIndex
        ///sInnerFieldName:사용안함
        ///Ex)
        ///TR정보 요청 - openApi.CommGetData(“OPT00001”, “”, “주식기본정보”, 0, “현재가”);
        ///실시간정보 요청 - openApi.CommGetData(“000660”, “A”, 0);
        ///체결정보 요청 - openApi.CommGetData(“000660”, “-1”, 1);
        /// </summary>
        public String CommGetData(String sJongmokCode, String sRealType, String sFieldName,int nIndex, String sInnerFieldName)
        {
            String ret =axKHOpenAPI.CommGetData( sJongmokCode,  sRealType,  sFieldName,  nIndex,  sInnerFieldName);
            FileLog.PrintF("CommGetData("+sJongmokCode+","+sRealType+","+sFieldName+","+nIndex+","+sInnerFieldName+"):"+ret);
            return ret;
        }

        /// <summary>
        /// [10]화면 내 모든 리얼데이터 요청을 제거한다.
        /// sScnNo – 화면번호[4]
        /// </summary>
        public void DisconnectRealData(String sScnNo)
        {
            FileLog.PrintF("DisconnectRealData(" + sScnNo+")");
            axKHOpenAPI.DisconnectRealData(sScnNo);
        }


        /// <summary>
        ///[11]설명 레코드 반복횟수를 반환한다.
        ///입력값 sTrCode – Tran 명
        ///입력값 sRecordName – 레코드 명
        ///반환값 레코드의 반복횟수
        ///비고 Ex) openApi.GetRepeatCnt(“OPT00001”, “주식기본정보”);
        /// </summary>
        public int GetRepeatCnt(String sTrCode,String sRecordName)
        {
            int ret =axKHOpenAPI.GetRepeatCnt(sTrCode, sRecordName);
            FileLog.PrintF("GetRepeatCnt(" + sTrCode + "," + sRecordName + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[12]설명 복수종목조회 Tran을 서버로 송신한다.
        ///입력값
        ///sArrCode – 종목리스트
        ///bNext – 연속조회요청
        ///nCodeCount – 종목개수
        ///nTypeFlag – 조회구분
        ///sRQName – 사용자구분 명
        ///sScreenNo – 화면번호[4]
        ///반환값
        ///OP_ERR_RQ_STRING – 요청 전문 작성 실패
        ///OP_ERR_NONE - 정상처리
        ///비고
        ///sArrCode – 종목간 구분은 ‘;’이다.
        ///nTypeFlag – 0:주식관심종목정보, 3:선물옵션관심종목정보
        ///ex) openApi.CommKwRqData(“000660;005930”, 0, 2, 0, “RQ_1”, “0101”);
        /// </summary>
        public int CommKwRqData(String sArrCode, int bNext, int nCodeCount, int nTypeFlag, String sRQName, String sScreenNo)
        {
            int ret = axKHOpenAPI.CommKwRqData( sArrCode,  bNext,  nCodeCount,  nTypeFlag,  sRQName,  sScreenNo);
            FileLog.PrintF(".CommKwRqData("+sArrCode+ ","+ bNext +"," + nCodeCount + ","+ nTypeFlag + "," + sRQName + ","+ sScreenNo + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[13] 설명 OpenAPI모듈의 경로를 반환한다.
        ///반환값 경로
        /// </summary>
        public String GetAPIModulePath()
        {
            String ret = axKHOpenAPI.GetAPIModulePath();
            FileLog.PrintF(".GetAPIModulePath():" + ret);
            return ret;
        }


        /// <summary>
        ///[14] 설명 시장구분에 따른 종목코드를 반환한다.
        ///입력값 sMarket – 시장구분
        ///반환값 종목코드 리스트, 종목간 구분은 ’;’이다.
        ///비고 sMarket – 0:장내, 3:ELW, 4:뮤추얼펀드, 5:신주인수권, 6:리츠, 8:ETF, 9:하이일드펀드, 10:코스닥, 30:제3시장
        /// </summary>
        public String GetCodeListByMarket(String sMarket)
        {
            String ret = axKHOpenAPI.GetCodeListByMarket(sMarket);
            FileLog.PrintF(".GetCodeListByMarket("+ sMarket + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[15] 현재접속상태를 반환한다.
        /// 0:미연결, 1:연결완료
        /// </summary>
        public int GetConnectState()
        {
            int ret = axKHOpenAPI.GetConnectState();
            FileLog.PrintF("GetConnectState:" + ret);
            return ret;

        }

        /// <summary>
        ///[16]설명 종목코드의 한글명을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 종목한글명
        /// 비고 장내외, 지수선옵, 주식선옵 검색 가능.
        /// </summary>
        public String GetMasterCodeName(String strCode)
        {
            String ret = axKHOpenAPI.GetMasterCodeName(strCode);
            FileLog.PrintF("GetMasterCodeName(" + strCode + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[17]설명 종목코드의 상장주식수를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 상장주식수
        /// </summary>
        public int GetMasterListedStockCnt(String strCode)
        {
            int ret = axKHOpenAPI.GetMasterListedStockCnt(strCode);
            FileLog.PrintF("GetMasterListedStockCnt(" + strCode + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[18]설명 종목코드의 감리구분을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 감리구분
        ///비고 감리구분 – 정상, 투자주의, 투자경고, 투자위험, 투자주의환기종목
        /// </summary>
        public String GetMasterConstruction(String strCode)
        {
            String ret = axKHOpenAPI.GetMasterConstruction(strCode);
            FileLog.PrintF("GetMasterConstruction(" + strCode + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[19]설명 종목코드의 상장일을 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 상장일
        ///비고 상장일 포멧 – xxxxxxxx[8]
        /// </summary>
        public String GetMasterListedStockDate(String strCode)
        {
            String ret = axKHOpenAPI.GetMasterListedStockDate(strCode);

            //&#x0; 널캐릭터문제
            /*
            if (ret.Length >= 8){
                ret = ret.Substring(0, 8);
            }
            */
            ret = ret.Trim('\0');
            FileLog.PrintF("GetMasterListedStockDate(" + strCode + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[20]설명 종목코드의 전일가를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 전일가
        /// </summary>
        public String GetMasterLastPrice(String strCode)
        {
            String ret = axKHOpenAPI.GetMasterLastPrice(strCode);
            //&#x0; 널캐릭터문제
            ret = ret.Trim('\0');
            FileLog.PrintF("GetMasterLastPrice(" + strCode + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[21]설명 종목코드의 종목상태를 반환한다.
        ///입력값 strCode – 종목코드
        ///반환값 종목상태
        ///비고 종목상태 – 정상, 증거금100%, 거래정지, 관리종목, 감리종목, 투자유의종목, 담보대출, 액면분할, 신용가능
        /// </summary>
        public String GetMasterStockState(String strCode)
        {
            String ret = axKHOpenAPI.GetMasterStockState(strCode);
            FileLog.PrintF("GetMasterStockState(" + strCode + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[22]설명 레코드의 반복개수를 반환한다.
        ///입력값 strRecordName – 레코드명
        ///반환값 레코드 반복개수
        ///비고 Ex) openApi.GetDataCount(“주식기본정보”);
        /// </summary>
        public int GetDataCount(String strRecordName)
        {
            int ret = axKHOpenAPI.GetDataCount(strRecordName);
            FileLog.PrintF("GetDataCount(" + strRecordName + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[23]설명 레코드의 반복순서와 아이템의 출력순서에 따라 수신데이터를 반환한다.
        ///nRepeatIdx – 반복순서
        ///nItemIdx – 아이템 순서
        ///반환값 수신 데이터
        ///비고 Ex) 현재가출력 - openApi.GetOutputValue(“주식기본정보”, 0, 36)
        /// </summary>
        public String GetOutputValue(String strRecordName, int nRepeatIdx, int nItemIdx)
        {
            String ret = axKHOpenAPI.GetOutputValue(strRecordName,nRepeatIdx,nItemIdx);
            FileLog.PrintF("GetOutputValue(" + strRecordName + ","+ nRepeatIdx + ","+ nItemIdx + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[24]설명 수신 데이터를 반환한다.
        ///입력값
        ///strTrCode – Tran 코드
        ///strRecordName – 레코드명
        ///nIndex – 복수데이터 인덱스
        ///strItemName – 아이템명
        ///반환값 수신 데이터
        ///비고 Ex)현재가출력 - openApi.GetCommData(“OPT00001”, “주식기본정보”, 0, “현재가”);
        /// </summary>
        public String GetCommData(String strTrCode, String strRecordName, int nIndex, String strItemName)
        {
            String ret = axKHOpenAPI.GetCommData(strTrCode, strRecordName, nIndex, strItemName);
            FileLog.PrintF("GetCommData(" + strTrCode + "," + strRecordName + "," + nIndex + ","+strItemName+"):" + ret);
            return ret;
        }


        /// <summary>
        ///[25]설명 실시간데이터를 반환한다.
        ///입력값
        ///strRealType – 실시간 구분
        ///nFid – 실시간 아이템
        ///반환값 수신 데이터
        ///비고 Ex) 현재가출력 - openApi.GetCommRealData(“주식시세”, 10);/// <summary>
        /// 
        ///참고)실시간 현재가는 주식시세, 주식체결 등 다른 실시간타입(RealType)으로도 수신가능
        /// </summary>
        public String GetCommRealData(String strRealType, int nFid)
        {
            String ret = axKHOpenAPI.GetCommRealData(strRealType, nFid);
            FileLog.PrintF("GetCommRealData(" + strRealType + "," +nFid + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[26]설명 체결잔고 데이터를 반환한다.
        ///입력값 nFid – 체결잔고 아이템
        ///반환값 수신 데이터
        ///비고 Ex) 현재가출력 – openApi.GetChejanData(10);
        /// </summary>
        public String GetChejanData(int nFid)
        {
            String ret = axKHOpenAPI.GetChejanData(nFid);
            FileLog.PrintF("GetChejanData(" + nFid + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[27] 설명 테마코드와 테마명을 반환한다.
        ///입력값 nType – 정렬순서 (0:코드순, 1:테마순)
        ///반환값 코드와 코드명 리스트
        ///비고 반환값의 코드와 코드명 구분은 ‘|’ 코드의 구분은 ‘;’
        ///Ex) 100|태양광_폴리실리콘;152|합성섬유
        /// </summary>
        public String GetThemeGroupList(int nType)
        {
            String ret = axKHOpenAPI.GetThemeGroupList(nType);
            FileLog.PrintF("GetThemeGroupList(" + nType + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[28] 설명 테마코드에 소속된 종목코드를 반환한다.
        ///입력값 strThemeCode – 테마코드
        ///반환값 종목코드 리스트
        ///비고 반환값의 종목코드간 구분은 ‘;’
        ///Ex) A000660;A005930
        /// </summary>
        public String GetThemeGroupCode(String strThemeCode)
        {
            String ret = axKHOpenAPI.GetThemeGroupCode(strThemeCode);
            FileLog.PrintF("GetThemeGroupCode(" + strThemeCode + "):" + ret);
            return ret;
        }



        /// <summary>
        ///[29] 설명 지수선물 리스트를 반환한다.
        ///반환값 종목코드 리스트
        ///비고 반환값의 종목코드간 구분은 ‘;’
        ///Ex) 101J9000;101JC000
        /// </summary>
        public String GetFutureList()
        {
            String ret = axKHOpenAPI.GetFutureList();
            FileLog.PrintF("GetFutureList():" + ret);
            return ret;
        }


        /// <summary>
        ///[30] 설명 지수선물 코드를 반환한다.
        ///입력값 nIndex – 0~3 지수선물코드, 4~7 지수스프레드
        ///반환값 종목코드
        ///비고 Ex) 최근월선물 - openApi.GetFutureCodeByInex(0);
        ///최근월스프레드 - openApi.GetFutureCodeByInex(4);
        /// </summary>
        public String GetFutureCodeByIndex(int nIndex)
        {
            String ret = axKHOpenAPI.GetFutureCodeByIndex(nIndex);
            FileLog.PrintF("GetFutureCodeByIndex("+nIndex+ "):" + ret);
            return ret;
        }

        /// <summary>
        ///[31] 설명 지수옵션 행사가 리스트를 반환한다.
        ///반환값 행사가
        ///비고 반환값의 행사가간 구분은 ‘;’
        ///Ex) 265.00;262.50;260.00
        /// </summary>
        public String GetActPriceList()
        {
            String ret = axKHOpenAPI.GetActPriceList();
            FileLog.PrintF("GetActPriceList():" + ret);
            return ret;
        }


        /// <summary>
        ///[32] 설명 지수옵션 월물 리스트를 반환한다.
        ///반환값 월물
        ///비고
        ///반환값의 월물간 구분은 ‘;’
        ///Ex) 201412;201409;201408;201407;201407;201408;201409;201412
        /// </summary>
        public String GetMonthList()
        {
            String ret = axKHOpenAPI.GetMonthList();
            FileLog.PrintF("GetMonthList():" + ret);
            return ret;
        }

        /// <summary>
        ///[33] 설명 행사가와 월물 콜풋으로 종목코드를 구한다.
        ///입력값
        ///strActPrice – 행사가(소수점포함)
        ///nCp – 콜풋구분 2:콜, 3:풋
        ///strMonth – 월물(6자리)
        ///반환값 종목코드
        ///비고 Ex) openApi.GetOptionCode(“260.00”, 2, “201407”);
        /// </summary>
        public String GetOptionCode(String strActPrice, int nCp, String strMonth)
        {
            String ret = axKHOpenAPI.GetOptionCode( strActPrice,  nCp,  strMonth);
            FileLog.PrintF("GetOptionCode("+ strActPrice + ","+ nCp + ","+ strMonth + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[34] 설명 입력된 종목코드와 동일한 행사가의 코드중 입력한 월물의 코드를 구한다.
        ///입력값
        ///strCode – 종목코드
        ///nCp – 콜풋구분 2:콜, 3:풋
        ///strMonth – 월물(6자리)
        ///반환값 종목코드
        ///비고 Ex) openApi.GetOptionCodeByMonth(“201J7260”, 2, “201412”);
        ///결과값 = 201JC260
        /// </summary>
        public String GetOptionCodeByMonth(String strCode, int nCp, String strMonth)
        {
            String ret = axKHOpenAPI.GetOptionCodeByMonth( strCode,  nCp,  strMonth);
            FileLog.PrintF("GetOptionCodeByMonth(" + strCode + "," + nCp + "," + strMonth + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[35] 설명 입력된 종목코드와 동일한 월물의 코드중 입력한 틱만큼 벌어진 코드를 구한다.
        ///입력값
        ///strCode – 종목코드
        ///nCp – 콜풋구분 2:콜, 3:풋
        ///nTick – 행사가 틱
        ///반환값 종목코드
        ///비고 Ex) openApi.GetOptionCodeByActPrice(“201J7260”, 2, -1);
        ///결과값 = 201J7262
        /// </summary>
        public String GetOptionCodeByActPrice(String strCode, int nCp, int Tick)
        {
            String ret = axKHOpenAPI.GetOptionCodeByActPrice(strCode, nCp, Tick);
            FileLog.PrintF("GetOptionCodeByActPrice(" + strCode + "," + nCp + "," + Tick + "):" + ret);
            return ret;
        }




        /// <summary>
        ///[36] 설명 주식선물 코드 리스트를 반환한다.
        ///입력값 strBaseAssetCode – 기초자산코드
        ///반환값 종목코드 리스트
        ///비고 출력값의 코드간 구분은 ‘;’이다.
        /// </summary>
        public String GetSFutureList(String strBaseAssetCode)
        {
            String ret = axKHOpenAPI.GetSFutureList(strBaseAssetCode);
            FileLog.PrintF("GetSFutureList(" + strBaseAssetCode + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[37] 설명 주식선물 코드를 반환한다.
        ///입력값 
        ///strBaseAssetCode – 기초자산코드
        ///nIndex – 0~3 지수선물코드, 4~7 지수스프레드, 8~11 스타 선물, 12~스타 스프레드
        ///반환값 종목코드
        ///비고 Ex) openApi.GetSFutureCodeByIndex(“11”, 0);
        /// </summary>
        public String GetSFutureCodeByIndex(String strBaseAssetCode, int nIndex)
        {
            String ret = axKHOpenAPI.GetSFutureCodeByIndex(strBaseAssetCode, nIndex);
            FileLog.PrintF("GetSFutureCodeByIndex(" + strBaseAssetCode + ","+ nIndex + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[38] 설명 주식옵션 행사가 리스트를 반환한다.
        ///입력값 strBaseAssetGb – 기초자산코드구분
        ///반환값 행사가 리스트, 행사가간 구분은 ‘;’
        ///비고 Ex) openApi.GetSActPriceList(“11”);
        /// </summary>
        public String GetSActPriceList(String strBaseAssetGb)
        {
            String ret = axKHOpenAPI.GetSActPriceList(strBaseAssetGb);
            FileLog.PrintF("GetSActPriceList(" + strBaseAssetGb + "):" + ret);
            return ret;
        }




        /// <summary>
        ///[39] 설명 주식옵션 월물 리스트를 반환한다.
        ///입력값 strBaseAssetGb – 기초자산코드구분
        ///반환값 월물 리스트, 월물간 구분은 ‘;’
        ///비고 Ex) openApi.GetSActPriceList(“11”);
        /// </summary>
        public String GetSMonthList(String strBaseAssetGb)
        {
            String ret = axKHOpenAPI.GetSMonthList(strBaseAssetGb);
            FileLog.PrintF("GetSMonthList(" + strBaseAssetGb + "):" + ret);
            return ret;
        }



        /// <summary>
        ///[40] 설명 주식옵션 코드를 반환한다.
        ///입력값
        ///strBaseAssetGb – 기초자산코드구분
        ///strActPrice – 행사가
        ///nCp – 콜풋구분
        ///strMonth – 월물
        ///반환값 주식옵션 코드
        ///비고 Ex) openApi.GetSOptionCode(“11”, “1300000”, 2, “1412”);
        /// </summary>
        public String GetSOptionCode(String strBaseAssetGb, String strActPrice, int nCp, String strMonth)
        {
            String ret = axKHOpenAPI.GetSOptionCode(strBaseAssetGb, strActPrice, nCp, strMonth);
            FileLog.PrintF("GetSOptionCode("+ strBaseAssetGb+", " + strActPrice + ", "+ nCp + ", "+ strMonth + "):" + ret);
            return ret;
        }


        /// <summary>
        ///[41] 설명 입력한 주식옵션 코드에서 월물만 변경하여 반환한다.
        ///입력값
        ///strBaseAssetGb – 기초자산코드구분
        ///strCode – 종목코드
        ///nCp – 콜풋구분
        ///strMonth – 월물
        ///반환값 주식옵션 코드
        ///비고 Ex) openApi.GetSOptionCodeByMonth(“11”, “211J8045”, 2, “1412”);
        /// </summary>
        public String GetSOptionCodeByMonth(String strBaseAssetGb, String strCode, int nCp, String strMonth)
        {
            String ret = axKHOpenAPI.GetSOptionCodeByMonth(strBaseAssetGb, strCode, nCp, strMonth);
            FileLog.PrintF("GetSOptionCodeByMonth(" + strBaseAssetGb + ", " + strCode + ", " + nCp + ", " + strMonth + "):" + ret);
            return ret;
        }



        /// <summary>
        ///[42] 설명 입력한 주식옵션 코드에서 행사가만 변경하여 반환한다.
        ///입력값
        ///strBaseAssetGb – 기초자산코드구분
        ///strCode – 종목코드
        ///nCp – 콜풋구분
        ///nTick– 월물
        ///반환값 주식옵션 코드
        ///비고 Ex) openApi.GetSOptionCodeByActPrice(“11”, “211J8045”, 2, 4);
        /// </summary>
        public String GetSOptionCodeByActPrice(String strBaseAssetGb, String strCode, int nCp, int nTick)
        {
            String ret = axKHOpenAPI.GetSOptionCodeByActPrice(strBaseAssetGb, strCode, nCp, nTick);
            FileLog.PrintF("GetSOptionCodeByActPrice(" + strBaseAssetGb + ", " + strCode + ", " + nCp + ", " + nTick + "):" + ret);
            return ret;
        }



        /// <summary>
        ///[43] 설명 주식선옵 기초자산코드/종목명을 반환한다.
        ///반환값 기초자산코드/종목명, 코드와 종목명 구분은 ‘|’ 코드간 구분은’;’
        ////Ex) 211J8045|삼성전자 C 201408;212J8009|SK텔레콤 C 201408
        ///비고 Ex) openApi.GetSFOBasisAssetList();
        /// </summary>
        public String GetSFOBasisAssetList()
        {
            String ret = axKHOpenAPI.GetSFOBasisAssetList();
            FileLog.PrintF("GetSFOBasisAssetList():" + ret);
            return ret;
        }



        /// <summary>
        ///[44] 설명 지수옵션 ATM을 반환한다.
        ///반환값 ATM
        ///비고 Ex) openApi.GetOptionATM();
        /// </summary>
        public String GetOptionATM()
        {
            String ret = axKHOpenAPI.GetOptionATM();
            FileLog.PrintF("GetOptionATM():" + ret);
            return ret;
        }



        /// <summary>
        ///[45] 설명 주식옵션 ATM을 반환한다.
        ///입력값
        ///반환값 ATM
        ///비고 Ex) openApi.GetSOptionATM(“11”);
        /// </summary>
        public String GetSOptionATM(String strBaseAssetGb)
        {
            String ret = axKHOpenAPI.GetSOptionATM(strBaseAssetGb);
            FileLog.PrintF("GetSOptionATM("+ strBaseAssetGb + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[46] 설명 회원사 코드와 이름을 반환합니다.
        ///반환값 회원사코드|회원사명;회원사코드|회원사명;…
        ///비고 Ex) openApi.GetBranchCodeName();
        /// </summary>
        public String GetBranchCodeName()
        {
            String ret = axKHOpenAPI.GetBranchCodeName();
            FileLog.PrintF("GetBranchCodeName():" + ret);
            return ret;
        }


        /// <summary>
        ///[47] 설명 투자자 정보조회를 요청한다.
        ///입력값
        ///sMarketGb – 시장구분 001:코스피, 002:코스닥, 003:선물, 004:콜옵션, 005:풋옵션, 006:스타선물
        ///007:주식선물, 008:3년국채, 009:5년국채, 010:10년국채, 011:달러선물, 012:엔선물
        ///013:유로선물, 014:미니금선물, 015:금선물, 016:돈육선물, 017:달러콜옵션, 018:달러풋옵션
        ///sRQName – 사용자구분값
        ///sScreenNo – 화면번호
        ///반환값 통신결과
        ///비고 Ex) openApi.CommInvestRqData(“T00108;T00109”, 0, 2, “RQ_1”, “0101”);
        /// </summary>
        public int CommInvestRqData(String sMarketGb, String sRQName, String sScreenNo)
        {
            int ret = axKHOpenAPI.CommInvestRqData( sMarketGb,  sRQName,  sScreenNo);
            FileLog.PrintF("CommInvestRqData(" + sMarketGb+",  "+sRQName+ ",  "+sScreenNo+"):" + ret);
            return ret;
        }


        /// <summary>
        ///[48] 설명 주식옵션 ATM을 반환한다.
        ///입력값 sInfoData – 아이디
        ///반환값 통신결과
        ///비고 Ex) openApi.SetInfoData(“UserID”);
        /// </summary>
        public int SetInfoData(String sInfoData)
        {
            int ret = axKHOpenAPI.SetInfoData(sInfoData);
            FileLog.PrintF("SetInfoData(" + sInfoData + "):" + ret);
            return ret;
        }

        /// <summary>
        ///[49] 설명 실시간 등록을 한다.
        ///입력값
        ///strScreenNo – 실시간 등록할 화면 번호
        ///strCodeList – 실시간 등록할 종목코드(복수종목가능 – “종목1; 종목2;종목3;….”)
        ///strFidList – 실시간 등록할 FID(“FID1; FID2;FID3;…..”)
        ///strRealType – “0”, “1” 타입
        ///반환값 통신결과
        ///비고
        ///strRealType이 “0” 으로 하면 같은화면에서 다른종목 코드로 실시간 등록을 하게 되면
        ///마지막에 사용한 종목코드만 실시간 등록이 되고 기존에 있던 종목은 실시간이 자동 해지됨.
        ///“1”로 하면 같은화면에서 다른 종목들을 추가하게 되면 기존에 등록한 종목도 함께 실시간 시세를 받을 수 있음.
        ///꼭 같은 화면이여야 하고 최초 실시간 등록은 “0”으로 하고 이후부터 “1”로 등록해야함.
        /// </summary>
        public int SetRealReg(String strScreenNo, String strCodeList, String strFidList, String strRealType)
        {
            int ret = axKHOpenAPI.SetRealReg( strScreenNo,  strCodeList,  strFidList,  strRealType);
            FileLog.PrintF("SetRealReg("+strScreenNo+","+strCodeList+","+strFidList+","+strRealType+"):" + ret);
            return ret;
        }


        /// <summary>
        ///[50] 설명 종목별 실시간 해제.
        /// 입력값
        ///strScrNo – 실시간 해제할 화면 번호
        ///strDelCode – 실시간 해제할 종목.
        ///반환값 통신결과
        ///비고 SetRealReg() 함수로 실시간 등록한 종목만 실시간 해제 할 수 있다.
        /// </summary>
        public void SetRealRemove(String strScreenNo, String strDelCode)
        {
            axKHOpenAPI.SetRealRemove(strScreenNo, strDelCode);
            FileLog.PrintF("SetRealRemove(" + strScreenNo + "," + strDelCode + ")");
        }




        /// <summary>
        ///[51] 설명 서버에 저장된 사용자 조건식을 조회해서 임시로 파일에 저장.
        ///반환값 사용자 조건식을 파일로 임시 저장.
        ///비고 System 폴더에 아이디_NewSaveIndex.dat파일로 저장된다.Ocx가 종료되면 삭제시킨다.
        ///조건검색 사용시 이함수를 최소 한번은 호출해야 조건검색을 할 수 있다.
        ///영웅문에서 사용자 조건을 수정 및 추가하였을 경우에도 최신의 사용자 조건을 받고 싶으면 다시 조회해야한다.
        /// </summary>
        public int GetConditionLoad()
        {
            int ret = axKHOpenAPI.GetConditionLoad();
            FileLog.PrintF("GetConditionLoad():"+ret);
            return ret;
        }



        /// <summary>
        ///[52] 설명 조건검색 조건명 리스트를 받아온다.
        ///반환값 조건명 리스트(조건명^인덱스)
        ///비고 조건명 리스트를 구분(“;”)하여 받아온다.Ex) 인덱스1^조건명1;인덱스2^조건명2;인덱스3^조건명3;…
        /// </summary>
        public String GetConditionNameList()
        {
            String ret = axKHOpenAPI.GetConditionNameList();
            FileLog.PrintF("GetConditionNameList():" + ret);
            return ret;
        }



        /// <summary>
        ///[53] 설명 조건검색 종목조회TR송신한다.
        ///입력값
        ///strScrNo : 화면번호
        ///strConditionName : 조건명
        ///nIndex : 조건명인덱스
        ///nSearch : 조회구분(0:일반조회, 1:실시간조회, 2:연속조회)
        ///반환값
        ///비고
        ///단순 조건식에 맞는 종목을 조회하기 위해서는 조회구분을 0으로 하고,
        ///실시간 조건검색을 하기 위해서는 조회구분을 1로 한다.
        ////OnReceiveTrCondition으로 결과값이 온다.
        ///연속조회가 필요한 경우에는 응답받는 곳에서 연속조회 여부에 따라 연속조회를 송신하면된다.
        /// </summary>
        public int SendCondition(String strScrNo, String strConditionName, int nIndex, int nSearch)
        {
            int ret = axKHOpenAPI.SendCondition(strScrNo, strConditionName, nIndex, nSearch);
            FileLog.PrintF("SendCondition(" + strScrNo + "," + strConditionName + "," + nIndex + "," + nSearch + "):"+ret);
            return ret;
        }



        /// <summary>
        ///[54] 설명 조건검색 실시간 중지TR을 송신한다.
        ///입력값
        ///strScrNo : 화면번호
        ///strConditionName : 조건명
        ///nIndex : 조건명인덱스
        ///반환값
        ///비고
        ///해당 조건명의 실시간 조건검색을 중지하거나, 
        ///다른 조건명으로 바꿀 때 이전 조건명으로 실시간 조건검색을 반드시 중지해야한다.
        ///화면 종료시에도 실시간 조건검색을 한 조건명으로 전부 중지해줘야 한다.
        /// </summary>
        public void SendConditionStop(String strScrNo, String strConditionName, int nIndex)
        {
            axKHOpenAPI.SendConditionStop(strScrNo, strConditionName, nIndex);
            FileLog.PrintF("SendConditionStop(" + strScrNo + "," + strConditionName + "," + nIndex + ")");
        }

        /// <summary>
        ///[55] 설명 차트 조회 데이터를 배열로 받아온다.
        ///입력값
        ///strTrCode : 조회한TR코드
        ///strRecordName: 조회한 TR명
        ////반환값  object인데.. 잘모르겠음..
        ///비고
        ///조회 데이터가 많은 차트 경우 GetCommData()로 항목당 하나씩 받아오는 것 보다
        ///한번에 데이터 전부를 받아서 사용자가 처리할 수 있도록 배열로 받는다.
        /// </summary>
        public Object GetCommDataEx(String strTrCode, String strRecordName)
        {
            Object ret = axKHOpenAPI.GetCommDataEx(strTrCode, strRecordName);
            FileLog.PrintF("GetCommDataEx(" + strTrCode + "," + strRecordName +"):"+ ret);
            return ret;
        }
        #endregion


        /// <summary>
        ///[101] 설명 주식 일일 데이터를 주식이 생성된 일부터 받아온다.
        ///입력값
        ///strCode : 종목코드
        ///stockDate: YYYYMMDD  문자열 ex)20160122
        ////반환값  
        /// 성공유무 데이터는 onReceived쪽에서 soap로 쏴줌
        /// </summary>
        /// 
        /// spell  은 내가 만든 속성  api가 너무 과거거 까지 다가져오므로
        /// START_STOCK_DATE   로 넘어오면 START_STOCK_DATE 부터 stockDate 까지 가져오기 형식은 'YYYYMMDD' 
        /// ZERO 으로 나오면 한번 받아올수있는 제한까지만 받아오기.
        /// TWO로 넘어오면 처음 부터 지정한 stockDate까지 nPreNext를 받는쪽에서 2로 지정해서 다받아올수있게 하기.
        public String GetOpt10081(String strCode, String startDate,String endDate)
        {
            //String stockDate = DateTime.Today.ToString("yyyyMMdd");
            //음 nPreNext 를 처음 할때는 0으로 놓고 했는데..
            // 그럼 과거 몇개랑 현재까지 보이고..
            // 받아온것을 nPreNext 2로 하면 이어서 받아오게 되는데..
            // qna 게시판 일어보니 1로 넣으면 그날만 가져온다는 것 같다.
            //해보자.
            /*
                1. 고객님께서 작성하신 로직상 CommRqData가 전송횟수제한 오류 발생없이 모두 전송되고 있다면 무리가 없습니다. 
                2. 2값은 조회하신 기준으로 그 이전의 값을 의미하고, 1값은 조회하신 기준으로 그 이후 연속데이터를 의미합니다. 1 값은 현재 시점 기준이 아닌 특정 일자를 입력하여 조회하는 성격의 TR서비스에서 사용됩니다. 
            */
            //그런데 1로 해도 0이랑했을때랑 차이가 없다. ㅠㅠ
            //결국엔 당일것만 가져오고 싶으면 내가 적절히 짤라 줘야한다..
            //그러기 위해서  명령어를 담아두었다가 receive가 되었을때 비교 해서 적절하게 짤라줄 명령어를 담는 저장공간이  필요하겠다.
            //HashTable이 괜찮겠네...
            //전역으로 읽을 수 있어야 하니까 공간은 Class1
            //내가 마음대로 조절할수있는것은.  sRQName, sScreenNo 두개네...
            /*
            이런 난 수정주가 구분이 1로 고정인지 알았는데 qna를 보니 그게 아닌것 같다..
            
            문의 내용중이런것이 있다));;
                OPT10081 에서 
                수정주가구분 = 0 or 1 
                이것을 변경해도 동일한 값이 나옵니다. 

                테스트 조건 
                SetInputValue("종목코드" , "002990"); 
                SetInputValue("기준일자" , "20121228"); 
                SetInputValue("수정주가구분" , "0"); 

                SetInputValue("종목코드" , "002990"); 
                SetInputValue("기준일자" , "20121228"); 
                SetInputValue("수정주가구분" , "1"); 

                두개 모두 2012년 12월 28일 현재가가 1555원 표시됩니다. 

                해당 종목은 감자가 있었기 때문에 
                차트에서 
                수정주가 적용시 : 10892원 
                수정주가 미적용시 : 1555원 
                이렇게 표시됩니다. 

            기타 다른 답변으로는 
            차트조회시 수정주가 구분값(0:권리락 적용안됨.1:권리락 적용됨)을 넣어서 조회하시면 됩니다. 
            권리락이라는게 뭘까??

                권리락 중 "권리" 에 의미는 유상증장 및 무상증자와 배당에 참여할 수 있는 자격 또는 권리를 의미하며,
                곧 주식 권리락이란 위에 해당하는 권리를 받을 수 있는 자격이 없어진다는 이야기입니다.
                다시 말하면  증자를 통하여 얻게 되는 이익에 비례하여 주가를 떨어뜨립니다.

 
                유상증자에서의 권리락
                말그대로 기존 주주들에게 유상으로(돈을 지불하여) 현 시점의 주가보다 저렴하게 주식을 살 수 있게 권리를 주게 되며,
                이 때문에 생기는 시세차이로 인하여 권리락이 발생하게 됩니다.
                무상증자
                주주들이 지불 없이 더 많은 주식을 받게 됩니다. 회사 입장에서는 주주들에게 지불받는 금액이 없으나 주식수는 늘어나게 됩니다.
                무상증자가 일어나면 주주는 더 많은 주식을 갖게 되지만 주가는 그에 비례하여 하락을 하게 되기 때문에
                결론적으로는 값이 동일해집니다.
                하지만, 대부분 무상증자 시 주가는 기존 값으로 회귀를 하는 경우가 많고, 시장에서 호재로 여겨지는 부분임.
            그냥 1로 표시하자... 골치 아프다..
            */

            /*
            큐를 받아두었다가 처리하는건.. 약간 위험한듯..
            그것보다 여기에서 지연이 생길수있도록 하는게 맞는것같다.
            시스템 성능 만큼 여기서 지연이 일어나야한다..
            이미  HashTable를 사용해서 여기 넣은 명령어가 received이벤트가 발생할 때 읽을 수있도록 했다..
            그리고 그걸 큐에 담아서 soap으로 전송할때 사용했었다..
            큐하고 hashtable 두개를 쓰던걸 hashtable 하나로 바꾸고
            실제로 soap 요청이 완료되면 hashtable에서 그 키값을 바꾸도록 해야겠다.
            그런데 soap 전송에서 key값을 어떻게 알수 있을까???
            음 hashtable에 순서가 있어서 처음들어온것을 빠질수있게 하면 되겠다..
            그런데 hashtable에는 순서가 없네...
            큐에 Dinary를 담자..
            ㅋㅋ 그러면 되겠다...
            아 dictionary를 담으려고 보니까.. 너무 낭비인것 같다.
            클래스를 하나만들자..
            아 생각해보니까. 하나 만들어둔게 있었다..
            ReceiveTrData.class
            아 젠장 큐가 recevie에서 만드니까.. 여기 서 막을 방법이 없네..
            여기서 막으면 행이 걸리는데.. 행이걸리는게 맞긴 한것 같은데..
            행이 오래걸리면 타임아웃이 걸리텐데...
            그럼 다시 원점인데.. 여기다 가 큐를 만들어야하는데..


            여기 쓰레드 큐가 하나 있어야 하고 
            그 쓰레드는 이벤트에 큐가 쌓였을때 
            하나씩 실행되어야 해.. 그래야 맞는것 같아..
            하나는 신문을 넣고 하나는 출판을 하는 그 걸 써야 하는게 맞는것 같은데..
            https://msdn.microsoft.com/ko-kr/library/yy12yx1f%28v=vs.90%29.aspx

            오호 이게 맞는것 같은데..
            http://www.csharpstudy.com//Threads/autoresetevent.aspx
            이아저씨는 천제인가..
            그런데 이건 한번 다 돌고 그다음 다른거 돌고 그런식이네...
            이렇게 구현하면 명령어가 행이 걸릴수도 있겠는데..
            한쪽에서 계속 받아서 쌓이면... 
            그리고 나중에 실행되고 그런식 아닐까...

            음 입력이 불특정으로 일어나니까 쌓이는것도 불특정으로 쌓여서 흐름이 되겠다.
            나쁘지 않다...
            쓰레드 돌리는걸 생각해보았는데.. 생각보다 위험하다..
            kiwoom api가 객체를 여러개 생성하는게 아니라 하나로 쓰는거랑
            변수가 바뀌지 않을꺼라는 보장이 없다..

            델레케이트 풀이 가능 할까???




            이게 맞는것 같은데 이해를 못하고 있으니...
            음 이건 큐가 하나네.. 그런데 내꺼는 큐하나로 못하는데..
            쓰레드 큐가 있어야 하네...
            큐를 넣으면 반환값이 없어지네.. 큐하면 안되겠다 앞에 링크중 
            앞에 그냥 1초 걸자.. 아 일이 진행이 안되네...

            그래서 멈추는걸 로 해서 아래처럼 했느데.. 쓸데없이 뒤에서 보내고 다시 조회하고 그러는데 시간이 딜레이가 걸린다...
            큐에서 3개정도 있고 recevie가 동작을 했으면 좋겠다... 
            그렇게 하려면 
            큐 크기를 조사해서 3보다 작으면 웨이트가 걸리고 3보다 크면 웹이트가 풀리고.. OK??

            */
            //Class1.getClass1Instance().waitOneOpt10081();// 멈추기..
            
            String sScreenNo = ScreenNumber.getClass1Instance().GetEosScrNum();
            String sRQName = "주식일봉차트조회";
            String sTrCode = "OPT10081";
            int nPrevNext = 0;

            String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
            String keyStockCode = String.Format(keyStockCodeLayout
                , sRQName
                , sTrCode
                , sScreenNo
            );

            String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
            String key = String.Format(keyLayout
                , sRQName
                , sTrCode
                , sScreenNo
                , strCode
            );
            FileLog.PrintF("keyStockCode  ==" + keyStockCode);
            FileLog.PrintF("key  ==" + key);

            OpenApi.Spell.SpellOpt spellOpt10081 = new OpenApi.Spell.SpellOpt();
            spellOpt10081.sRQNAME = sRQName;
            spellOpt10081.sTrCode = sTrCode;
            spellOpt10081.stockCode = strCode;
            spellOpt10081.startDate = startDate;
            spellOpt10081.endDate = endDate;
            spellOpt10081.nPrevNext = nPrevNext;
            spellOpt10081.sScreenNo = sScreenNo;


            //음 받는쪽에서 일자하고 nPrevNext를 알수가 없어서..키로 쓸수가 없다...
            //종목코드로도 찾을 수가 없네 종목코드도 지우자.
            //spell=""
            AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, strCode);
            AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10081);


            axKHOpenAPI.SetInputValue("종목코드", strCode);
            axKHOpenAPI.SetInputValue("기준일자", endDate);
            axKHOpenAPI.SetInputValue("수정주가구분", "1");
            int nRet = axKHOpenAPI.CommRqData(sRQName, sTrCode, nPrevNext, sScreenNo);

            if (Error.IsError(nRet))
            {
                return "[OPT10081][OK]:" + Error.GetErrorMessage();
            }
            else
            {
                return "[OPT10081][NOK]:" + Error.GetErrorMessage();
            }
      }


        /// <summary>
        ///[102] 설명 추정자산조회요청
        ///입력값
        ///accountNum : 계좌번호10자리
        ///password : 계좌비밀번호 4 자리모의투자는 0000
        ////반환값  
        /// 성공유무 데이터는 onReceived쪽에서 soap로 쏴줌
        /// </summary>
        public String GetOpw00003(String accountNum,String password)
        {
            String sRQName = "추정자산조회요청WCF";
            String sTrCode = "OPW00003";

            FileLog.PrintF("WCF GetOpt00003 accountNum=" + accountNum);
            FileLog.PrintF("WCF GetOpt00003 password=" + password);

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
            ReceiveTrData rt = rtf.getReceiveTrData(spellOpw00003.sTrCode);
            int nRet = rt.Run(axKHOpenAPI, spellOpw00003);
            spellOpw00003.startRunTime = DateTime.Now;

            if (Error.IsError(nRet))
            {
                return "[WCF GetOpw00003][OK]:" + Error.GetErrorMessage();
            }
            else
            {
                return "[WCF GetOpw00003][NOK]:" + Error.GetErrorMessage();
            }
        }


        /// <summary>
        ///[102] 설명 계좌수익률요청
        ///입력값
        ///accountNum : 계좌번호10자리
        ////반환값  
        /// 성공유무 데이터는 onReceived쪽에서 soap로 쏴줌
        /// </summary>
        public String GetOpt10085(String accountNum)
        {
            String sRQName = "계좌수익률요청WCF";
            String sTrCode = "OPT10085";

            FileLog.PrintF("WCF GetOpt10085 accountNum=" + accountNum);

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

            AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10085);
            AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, accountNum);
            //QUEUE를 따지 않고 바로 실행되어야 한다.
            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
            ReceiveTrData rt = rtf.getReceiveTrData(spellOpt10085.sTrCode);
            int nRet = rt.Run(axKHOpenAPI, spellOpt10085);
            spellOpt10085.startRunTime = DateTime.Now;

            if (Error.IsError(nRet))
            {
                return "[WCF GetOpt10085][OK]:" + Error.GetErrorMessage();
            }
            else
            {
                return "[WCF GetOpt10085][NOK]:" + Error.GetErrorMessage();
            }
        }
    }
}
