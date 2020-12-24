using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;
using System.Threading;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using OpenApi.ReceiveTrData;
using System.IO.Compression;
using OpenApi.Dao;
using OpenApi.Dto;

namespace OpenApi
{

    public class AppLib
    {
        private static Boolean _multiThread = true;

        private List<String> stockCodeList;

        readonly object lockerSpellDictionary = new object();
        readonly object lockerStockDictionary = new object();
        readonly object lockerRunQueue = new object();
        readonly object lockerReceivedQueue = new object();
        readonly object lockerOrderedCodeCount = new object();
        readonly object lockerRanUniqStockCount = new object();
        readonly object lockerCurrentRunSpellOpt = new object();

        readonly object jijs = new object();
        
        readonly object lockerOrderQueue = new object();
        private ConcurrentQueue<OpenApi.Spell.SpellOpt> orderQueue = new ConcurrentQueue<OpenApi.Spell.SpellOpt>();
        private ConcurrentQueue<OpenApi.Spell.SpellOpt> runQueue = new ConcurrentQueue<OpenApi.Spell.SpellOpt>();
        private Dictionary<String, OpenApi.Spell.SpellOpt> spellDictionary = new Dictionary<String, OpenApi.Spell.SpellOpt>();
        private Dictionary<String, String> stockCodeDictionary = new Dictionary<String, String>();
        private int orderedCodeCount = 0;
        private int ranUniqStockCount = 0;
        private OpenApi.Spell.SpellOpt CurrentRunSpellOpt=null;
        
        public int iEOS = 0;
        public string endDateEos = DateTime.Now.ToString("yyyyMMdd");

        //opt10081s 입력을 받고 완료가 되때까지 기다리게 할목적의 거시기 ResetEvent();
        private AutoResetEvent _evtOpt10081 = new AutoResetEvent(true);

        public void ClosedAll()
        {
            FileLog.PrintF("End");
            
            t2.Abort();
            t5.Abort();
            Stop();
            axKHOpenAPI.Dispose();
            
        }
        
        
        Thread t2;
        Thread t5;

        public void Start()
        {
            FileLog.PrintF("Start");
            this.stockCodeList = setStockCodeList();
            
            //이라고 되어있는데  한버에 100종목이고 모두 등록가능하다.
            //SetRegReal005930();
            SetRegRealAll();  //실시간 은 최대 100종목까지만 가능함.
        }

        private void Stop()
        {

            ScreenNumber.getClass1Instance().DisconnectAllAnyTimeData();
            ScreenNumber.getClass1Instance().DisconnectAllEOSData();
            ScreenNumber.getClass1Instance().DisconnectAllRealTimeData();
        }

        private AppLib()
        {
            t2 = new Thread(new ThreadStart(orderReceivedMessage));
            t5 = new Thread(new ThreadStart(CheckTimeCurrentRunSellOpt));
            t2.Start();
            t5.Start();
        }
        public void waitOneOpt10081(String sTrCode)
        {
             FileLog.PrintF("waitOneOpt10081 start  sTrCode=>"+ sTrCode);
             _evtOpt10081.WaitOne();
            FileLog.PrintF("waitOneOpt10081  end sTrCode=>" + sTrCode);
        }
        public void setOpt10081(String sTrCode)
        {
            FileLog.PrintF("setOpt10081 sTrCode=>" + sTrCode);
            _evtOpt10081.Set();
        }



        public OpenApi.Spell.SpellOpt DequeueByRunQueue()
        {
            OpenApi.Spell.SpellOpt tmp;
            if(!runQueue.TryDequeue(out tmp)){
                tmp = null;
            }
            return tmp;
        }
        
        public void EnqueueByRunQueue(OpenApi.Spell.SpellOpt message)
        {
                runQueue.Enqueue(message);
        }



        public  void EnqueueByOrderQueue(OpenApi.Spell.SpellOpt message)
        {
            lock (lockerOrderQueue)
            {
                FileLog.PrintF("EnqueueByOrderQueue key=>" + message.key);
                orderQueue.Enqueue(message);
            }
        }
    
        public void AddSpellDictionary(String key,  OpenApi.Spell.SpellOpt value)
        {
            lock (lockerSpellDictionary)
            {
               // FileLog.PrintF("AddSpellDictionary key=>" + key);
                if (!spellDictionary.ContainsKey(key)) {
                    spellDictionary.Add(key, value);
                }
            }
        }

        public  OpenApi.Spell.SpellOpt getSpell(String key)
        {
            //'System.Collections.Generic.KeyNotFoundException'
            lock(lockerSpellDictionary) {
              // FileLog.PrintF("getSpell key=>" + key);
                OpenApi.Spell.SpellOpt tmp = spellDictionary[key];
                //spellDictionary.Remove(key);
                return tmp;
            }
        }

        public void InRanUniqStockCount()
        {
            lock (lockerRanUniqStockCount)
            {
                this.ranUniqStockCount = this.ranUniqStockCount + 1;
            }
        }

        public int GetRanUniqStockCount()
        {
            lock (lockerRanUniqStockCount)
            {
                return ranUniqStockCount;
            }
        }


        public void InCraeteOrderedCodeCount()
        {
            lock (lockerOrderedCodeCount)
            {
                this.orderedCodeCount = this.orderedCodeCount + 1;
            }
        }

        public int GetCntOrderedCodeCount()
        {
            lock (lockerOrderedCodeCount)
            {
                return orderedCodeCount;
            }
        }


        public void AddStockCodeDictionary(String key, String value)
        {
            lock (lockerStockDictionary)
            {
                
                if(!stockCodeDictionary.ContainsKey(key))
                {
                    //FileLog.PrintF("AddStockCodeDictionary  key:" + key);
                    stockCodeDictionary.Add(key, value);
                }
                
            }
        }

        public void removeStockCodeDictionary(String key)
        {
            lock (lockerStockDictionary)
            {
                stockCodeDictionary.Remove(key);
            }
        }


        public void removeSpellDictionary(String key)
        {
            lock (lockerSpellDictionary)
            {
                spellDictionary.Remove(key);
            }
        }

        public String getStockCode(String key)
        {
            lock(lockerStockDictionary) {
                String tmp = "";
                //      FileLog.PrintF("getStockCode  get key:" + key);
                if (stockCodeDictionary.ContainsKey(key)) { 
                     tmp = stockCodeDictionary[key];
                }
                //stockCodeDictionary.Remove(key);
                return tmp;
            }
        }

        /*처음 조회를 요청했을때 큐에 담아있는걸 꺼내다가 다시 호출*/
        private void orderReceivedMessage()   
        {
            while (true)
            {
                lock (lockerOrderQueue)
                {
                    OpenApi.Spell.SpellOpt tmp;
                    if (orderQueue.TryDequeue(out tmp)){
                        FileLog.PrintF("orderReceivedMessage");
                        //래치는 안써되 될것 같다 호출에 1초에 5번 제약이 있는 것인지. 중간중간에 제약이 있는 게 아니다.
                        //균일하게 호출될 것을 보장하려면 써야겠지만 일단 주석
                        waitOneOpt10081(tmp.sTrCode);// 멈추기..    
                                                     //그런데 이거 없으면 에러남 0.2초 딜레이에서 유지해야할듯.

                        /*
                         
- 1초당 5회 조회를 1번 발생시킨 경우 : 17초대기
- 1초당 5회 조회를 5연속 발생시킨 경우 : 90초대기
- 1초당 5회 조회를 10연속 발생시킨 경우 : 3분(180초)대기

1초 5회에 대한 제한이 걸려 있다. 

                        03초 로 늘려서 해당 제한을 피하자. 

안녕하십니까. 키움증권 운영자입니다.
먼저 키움증권에 관심을 가져주시고 이용해주시는 고객님께 감사 인사드립니다.

에러코드 -200 또는 -209, -210은 시세조회제한에 해당하는것이며 지속적인 조회가 원인일수 있습니다.

1. 시세조회 제한
OpenAPI 조회는 초당 5회 조회제한 외에 강화된 조회제한이 2017년 4월 7일부터 적용되었으며 이 기준(조회횟수)은 비공개 운영으로 답변 드리지 못하는 점 양해부탁드립니다.
강화된 조회제한은 실서버와 모의투자서버에 관계없이 OpenAPI를 실행중이면 항상 적용되며
CommRqData(), CommKwRqData()등 조회함수 호출을 합산처리하는것으로 SetRealReg()등 기타 함수호출이나 실시간 데이터 수신과도 관련이 없습니다.

연속조회 역시 조회함수를 이용하므로 합산처리되어 사용중에 조회제한 알림창이 출력될 수 있습니다.
강화된 조회제한에 해당하면 팝업알림창 출력과 (-200)에러코드를 리턴하며 재 로그인 하시면 다시 OpenAPI를 사용하실 수 있습니다.
이 조회제한은 모든 고객분들이 거래데이터를 원할하게 사용하실수 있도록 할 목적이므로 고객님께서 이점 양해해 주시기 바랍니다.
시세조회나 실시간 데이터는 시세과부방지를 위해 지속적으로 모니터링하고 있으며 원할한 데이터 제공을 위해 필요한 경우 예고없이 시세조회 횟수 제한이 적용될수있습니다.

전업종지수요청(OPT20003)조회하면 실시간 타입 "업종지수"와 "업종등락"을 수신하므로 이 데이터를 이용할수 있는지 검토해보세요.
KOA Studio 개발 가이드에는 OpenAPI기능별 간단한 설명과 관련 함수 이벤트를 함께 소개하고 있으니 개발에 참고하세요.

2. 시세조회 강화관련 수정가이드 공지내용을 첨부
[공지]조회횟수 제한 강화에 따른 수정 가이드_내용추가
작성자키움증권등록일12/24 17:46
내용주문에 대한 제한은 기존 초당5초 외에 추가된 제한이 없습니다.(추가내용)
제한기준이 시장상황과 서버상황에따라 유동적인 이유로 이에 차단메세지를 수신하신 고객님(Client)들께 공통적인 가이드를 제시하는데 시간이 지체되었습니다.

서버운영에 문제를 야기시켰던 부분은 1초당 5회라는 횟수보다는 이를 반복하는 것 입니다.
서버의 점유율이 해소되기 위한 idle Time을 확보하기가 어렵습니다.

서버리스크를 회피하면서, OpenAPI 의 모든 고객분들께서 조회 차단을 회피하는 가이드는 아래와 같습니다.
(1초당 5회로 기작업되어 있다는 전제하에 이를 기준으로 한 가이드 입니다.)
- 1초당 5회 조회를 1번 발생시킨 경우 : 17초대기
- 1초당 5회 조회를 5연속 발생시킨 경우 : 90초대기
- 1초당 5회 조회를 10연속 발생시킨 경우 : 3분(180초)대기

위 방법으로 대응이 어려운 성격의 프로그램을 운영중이신 고객분께서는 글 올려주시면 별도의 대안으로 답변드리도록 하겠습니다.
안정적인 서비스 운영이 최우선이니 양해바라겠습니다.

감사합니다.

- 키움증권 리테일전략팀 -*/

                        /*
                         이해가 안갔는데 계속 에러나서 다시 읽어보니까. 
                         ㅋㅋㅋㅋㅋ
                         대기를 걸어달라는거네. 1초에 2번이 아니라.

                        실제로 돌려보니까. 1초에 5번 제한 말고 100회 호출 제한이 걸려 잇는것 같다.

                                                1*5*10번 연속이면 50번 한거고 3분대기를 하면 
                                                3분 10초에 50항목을 조회한것이 되고 6000개를 다조회하려면 120을 곱해야하니까.
                                                360분이고 6시간에 해당하는 내용이다.

                                                18초에 5개로 생각하면 
                                                50개면 180초  == 3분정도 시간이 걸리는거네.. 그러면 똑같네..

                        
                        6분에 100개 60분 1시간 걸린다.  1000개
                        1000*6개     6시간 걸린다.

                        제한이 풀리려면 껐다가 다시 해야한다. 60번을... 이게 현실적이다.
                        코스피만 보면 1300개다.
                        2시간쯤 걸린다고 보면된다.
                        아마 코스닥 보면 3000개 정도 될거다.
                        3시간 정도 생각하면 된다.
                        장마감 4시부터 7시까지 돈다고 생각하면 괜찮다.


                        
                        키움 답변
                        CommKwRqData 를 써라 이거 ;를 구분자로 100개까지 넣을 수 있다.
                        와 함수만 바꾸면 쓸수있다는 내용인지 알았는데 주문도 다르다.

                        OPTKWFID를 사용하라는 내용이었다.
                        복수호출을 조회하는 것들은
                        OPTKWFID  ==> 관심종목정보요청
                        OPTKWINV  ==> 관심종목투자정보
                        OPTKWPRO  ==> 관심종목프로그램정보요청

                        이것도 하고.
                        주식기본정보 요청 이것도 다 돌리자.

                                                 */


                        ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
                        ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(tmp.sTrCode);
                        int nRet = rt.Run(axKHOpenAPI, tmp);
                        FileLog.PrintF("orderReceivedMessage sTrCode , stockCode ,  nRet  =>  " + tmp.sTrCode+","+tmp.stockCode+","+nRet);
                        ///nRet  반환값을 파일에 남겨야한다. 그래야.... 실행이 되었는지 아닌지 알수 있다.

                        tmp.startRunTime = DateTime.Now;
                        setCurrentRunSellOpt(tmp);
                        this.EnqueueByRunQueue(tmp);//실행중인 데이터크기를 대충알기위해서                       
                        this.InRanUniqStockCount();

                       
                    }
                    
                }
                Thread.Sleep(200); //0.2초에 한번씩 확인 --1초에 5번 호출되는 제약사항 때문에 
            }
        }
        
        private void setCurrentRunSellOpt(OpenApi.Spell.SpellOpt value)
        {
            lock (lockerCurrentRunSpellOpt)
            {
                this.CurrentRunSpellOpt = value;
            }
        }

        public static void InsertOpt10001( String stockCode)
        {
            OpenApi.Spell.SpellOpt spellOpt10001 = new OpenApi.Spell.SpellOpt();
            spellOpt10001.sRQNAME = "주식기본정보요청";
            spellOpt10001.sTrCode = "OPT10001";
            spellOpt10001.nPrevNext = 0;

            if (stockCode.Equals("ALL"))
            {
                
                AppLib.threadJob(spellOpt10001);
            }
            else
            {
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , spellOpt10001.sRQNAME
                    , spellOpt10001.sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                  , spellOpt10001.sRQNAME
                    , spellOpt10001.sTrCode
                    , sScrNo
                    , stockCode
                );
                FileLog.PrintF("keyStockCode  ==" + keyStockCode);
                FileLog.PrintF("key  ==" + key);

                spellOpt10001.stockCode = stockCode;
                spellOpt10001.key = key;
                spellOpt10001.sScreenNo = sScrNo;

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10001);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10001);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }

        }


        public static void InsertOptkwfid(String stockCode)
        {
            OpenApi.Spell.SpellOpt spellOptkwfid = new OpenApi.Spell.SpellOpt();
            spellOptkwfid.sRQNAME = "관심종목정보요청";
            spellOptkwfid.sTrCode = "OPTKWFID";
            spellOptkwfid.nPrevNext = 0;

            if (stockCode.Equals("ALL"))
            {

                AppLib.threadJobOptkwfid(spellOptkwfid);
            }
            else
            {
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , spellOptkwfid.sRQNAME
                    , spellOptkwfid.sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                  , spellOptkwfid.sRQNAME
                    , spellOptkwfid.sTrCode
                    , sScrNo
                    , stockCode
                );
                FileLog.PrintF("keyStockCode  ==" + keyStockCode);
                FileLog.PrintF("key  ==" + key);

                spellOptkwfid.stockCode = stockCode;
                spellOptkwfid.key = key;
                spellOptkwfid.sScreenNo = sScrNo;

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOptkwfid);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOptkwfid);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }

        }

        public static void InsertOpt10015(String stockCode,String startDate,String endDate)
        {
            String sRQName = "일별거래상세요청";
            String sTrCode = "OPT10015";

            OpenApi.Spell.SpellOpt spellOpt10015 = new OpenApi.Spell.SpellOpt();
            spellOpt10015.sRQNAME = sRQName;
            spellOpt10015.sTrCode = sTrCode;
            spellOpt10015.startDate = startDate;
            spellOpt10015.endDate = endDate;
            spellOpt10015.nPrevNext = 0;
            if (stockCode.Equals("ALL"))
            {
                AppLib.threadJob(spellOpt10015);
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
                spellOpt10015.stockCode = stockCode;
                spellOpt10015.key = key;
                spellOpt10015.sScreenNo = sScrNo;
                spellOpt10015.reportGubun = "FILE";

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10015);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10015);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }

        }



        public static void InsertOpt10014(String stockCode, String startDate, String endDate)
        {

            String sRQName = "공매도추이요청";
            String sTrCode = "OPT10014";

            FileLog.PrintF("btn_sellStockOff_Click stockCode=" + stockCode);
            FileLog.PrintF("btn_sellStockOff_Click startDate=" + startDate);
            FileLog.PrintF("btn_sellStockOff_Click endDate=" + endDate);

            OpenApi.Spell.SpellOpt spellOpt10014 = new OpenApi.Spell.SpellOpt();
            spellOpt10014.sRQNAME = sRQName;
            spellOpt10014.sTrCode = sTrCode;
            spellOpt10014.startDate = startDate;
            spellOpt10014.endDate = endDate;
            spellOpt10014.nPrevNext = 0;
            if (stockCode.Equals("ALL"))
            {
                AppLib.threadJob(spellOpt10014);
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

                spellOpt10014.stockCode = stockCode;
                spellOpt10014.key = key;
                spellOpt10014.sScreenNo = sScrNo;
                spellOpt10014.reportGubun = "FILE";

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10014);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10014);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }
        }

        public static void InsertOpt10059(String stockCode, String startDate, String endDate, String buyOrSell, String priceOrAmount)
        {


            String sRQName = "종목별투자자기관별차트요청";
            String sTrCode = "OPT10059";

            FileLog.PrintF("GetStockOrgan_Click stockCode=" + stockCode);
            FileLog.PrintF("GetStockOrgan_Click startDate=" + startDate);
            FileLog.PrintF("GetStockOrgan_Click endDate=" + endDate);
            FileLog.PrintF("GetStockOrgan_Click 매매구분=" + buyOrSell);
            FileLog.PrintF("GetStockOrgan_Click 금액수량구분=" + priceOrAmount);

            OpenApi.Spell.SpellOpt spellOpt10059 = new OpenApi.Spell.SpellOpt();
            spellOpt10059.sRQNAME = sRQName;
            spellOpt10059.sTrCode = sTrCode;
            spellOpt10059.startDate = startDate;
            spellOpt10059.endDate = endDate;
            spellOpt10059.nPrevNext = 0;
            spellOpt10059.buyOrSell = buyOrSell;
            spellOpt10059.priceOrAmount = priceOrAmount;

            if (stockCode.Equals("ALL"))
            {
                AppLib.threadJob(spellOpt10059);
                FileLog.PrintF("GetStockOrgan_Click threadJob");
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

                spellOpt10059.stockCode = stockCode;
                spellOpt10059.key = key;
                spellOpt10059.sScreenNo = sScrNo;

                AppLib.getClass1Instance().EnqueueByOrderQueue(spellOpt10059);
                AppLib.getClass1Instance().AddSpellDictionary(key, spellOpt10059);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
            }
        }

    public static void threadJob(OpenApi.Spell.SpellOpt spellOpt)
        {
            List<String> stockCodeList = AppLib.getClass1Instance().getStockCodeList();
            for (int i = 0; i < stockCodeList.Count; i++)
            {
                OpenApi.Spell.SpellOpt tmp = spellOpt.ShallowCopy();
                String stockCode = stockCodeList[i];
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , tmp.sRQNAME
                    , tmp.sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , tmp.sRQNAME
                    , tmp.sTrCode
                    , sScrNo
                    , stockCode
                );
                tmp.sScreenNo = sScrNo;
                tmp.stockCode = stockCode;
                tmp.key = key;
             
                AppLib.getClass1Instance().EnqueueByOrderQueue(tmp);
                AppLib.getClass1Instance().AddSpellDictionary(key, tmp);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, stockCode);
                AppLib.getClass1Instance().InCraeteOrderedCodeCount();
            }
        }

        public static void threadJobOptkwfid(OpenApi.Spell.SpellOpt spellOpt)
        {
            List<String> stockCodeList = AppLib.getClass1Instance().getStockCodeList();
            FileLog.PrintF("threadJobOptkwfid.count=>" + stockCodeList.Count());
            List<String> strCodeList = new List<string>();
            int cnt = stockCodeList.Count();

            double tmp = cnt / 100f;
            double tmpCnt = Math.Ceiling(tmp);

            for (int i = 1; i <= tmpCnt; i++)
            {
                int a = (i * 100) - 100;
                int b = (i * 100);
                if (b > cnt)
                {
                    b = cnt % 100 - 1;
                }
                else
                {
                    b = 100;
                }
                //FileLog.PrintF(String.Format("SetRegRealAll a{0}~갯수{1}", a, b));
                List<String> tmpList = stockCodeList.GetRange(a, b);
                strCodeList.Add(string.Join(";", tmpList));
            }

            //67개  6700개
            //로그상 13개 등록하는데서 멈췄다.
            //52EE86 이런게 찍혔는데..
            //정황상... 주식종목코드가 맞지 않아서 인것 같다.
        

            for (int i = 0; i < strCodeList.Count; i++)
            {
                OpenApi.Spell.SpellOpt tmp2 = spellOpt.ShallowCopy();
                String strCode = strCodeList[i];
                String sScrNo = ScreenNumber.getClass1Instance().GetEosScrNum();
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , tmp2.sRQNAME
                    , tmp2.sTrCode
                    , sScrNo
                );
                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                   , tmp2.sRQNAME
                    , tmp2.sTrCode
                    , sScrNo
                    , strCode
                );
                tmp2.sScreenNo = sScrNo;
                tmp2.stockCode = strCode;
                tmp2.key = key;

                AppLib.getClass1Instance().EnqueueByOrderQueue(tmp2);
                AppLib.getClass1Instance().AddSpellDictionary(key, tmp2);
                AppLib.getClass1Instance().AddStockCodeDictionary(keyStockCode, strCode);
                AppLib.getClass1Instance().InCraeteOrderedCodeCount();
            }

        }

        private void CheckTimeCurrentRunSellOpt()
        {
            while (true)
            {
                lock (lockerCurrentRunSpellOpt)
                {
                    if (this.CurrentRunSpellOpt != null)
                    {
                        DateTime startRunTime = this.CurrentRunSpellOpt.startRunTime;
                        DateTime checkRunTime = DateTime.Now;
                        TimeSpan gap = checkRunTime - startRunTime;
                        int iGap = gap.Seconds;
                        String key = "[ALERT]"+this.CurrentRunSpellOpt.sTrCode + "::" + this.CurrentRunSpellOpt.stockCode + "::" + this.CurrentRunSpellOpt.endDate + "::iGap=> " + iGap;
                        if (iGap > 30) { 
                            FileLog.PrintF(key);
                            OpenApi.Spell.SpellOpt tmp = this.CurrentRunSpellOpt.ShallowCopy();
                            ReceiveTrDataFactory rtf = ReceiveTrDataFactory.getClass1Instance();
                            ReceiveTrData.ReceiveTrData rt = rtf.getReceiveTrData(tmp.sTrCode);
                            int nRet = rt.Run(axKHOpenAPI, tmp);  //연속조회시
                            tmp.startRunTime = DateTime.Now;
                            setCurrentRunSellOpt(tmp);
                            this.DequeueByRunQueue();//안에 들어있는 걸빼고 
                            this.EnqueueByRunQueue(tmp);//지금 생성한것을 넣음
                            /*장애가 났다는것을 알릴수있는 어떤 함수가 필요하다고 생각함*/
                        }
                    } else
                    {
                        String key = "[ALERT2] 진행중인 데이터가 없다.";
                        FileLog.PrintF(key);
                    }
                }
                Thread.Sleep(300000); //5분에 한번씩 돔
            }

        }
        
        private void printRunTime(OpenApi.Spell.SpellOpt value)
        {
            DateTime startRunTime = value.startRunTime;
            DateTime checkRunTime = DateTime.Now;
            TimeSpan gap = checkRunTime - startRunTime;
            int iGap = gap.Milliseconds;
            String key ="[FINISH]"+ value.sTrCode + "::" + value.stockCode + "::" + value.endDate + "::iGap[Milliseconds]=> " + iGap;
            FileLog.PrintF(key);
        }

       
        public AxKHOpenAPI getAxKHOpenAPIInstance()
        {
            return axKHOpenAPI;
        }

        public void setAxKHOpenAPIInstance(AxKHOpenAPI axKHOpenAPI)
        {
            this.axKHOpenAPI = axKHOpenAPI;
        }

        private   AxKHOpenAPI axKHOpenAPI;

        private static AppLib _class1 = null;
        private static Object _object1  = new Object();
        public static AppLib getClass1Instance() {
            if (_multiThread == false) {
                if (_class1 == null) {
                    _class1 = new AppLib();
                }               
            }else{
                if (_class1 == null) {
                    lock (_object1) {
                        _class1 = new AppLib();
                    }
                }                
            }
            return _class1;
        }


        public List<String> getStockCodeList() {
            return stockCodeList;
        }

        private List<String> setStockCodeList()
        {
            List<String> rtn = new  List<String>();
            //0:장내, 3:ELW, 4:뮤추얼펀드, 5:신주인수권, 6:리츠, 8:ETF, 9:하이일드펀드, 10:코스닥, 30:K - OTC, 50:코넥스(KONEX)
            //String[] arr_market = { "0", "3", "4", "5", "6", "8", "9", "10", "30", "50" };

            //52F170 이런코드들이 들어왔는데... 
            //다른것들이 들어와서 그런것 같다. 0: 장내 만 뽑아보자.
            //2008 개
            //그런데 이렇게 해도 A053660 이런것들이 들어왔다. 네이버에서 검색했을때 안나온다.
            //데이터 뽑아올때보니까 테마주이다.
            //테마를 빼니까 1306개이다..
            //혹시나 해서 0번만 해보니까. 되고
            //3번만 해보니까 되고
            //확인한 내용 실제로 다 된다.
            //그런데 중간에 공백이 들어가 있는 것들이 있어서 멈췄었다.
            //stock_cd가 비어있는게 있는지 확인하는 내용을 넣으니 해결되었다.
            //나는 코스피만 보면 되는데 코스피만 보도록 "0" 넣었다.
            //2020년04월19일 A 이렇게 붙는거 의미가 있다고 한다.
            //알파벳 A는 장내주식, J는 ELW종목, Q는 ETN종목을 의미합니다.
            //이게 진실이다. ==>  그런데 중간에 공백이 들어가 있는 것들이 있어서 멈췄었다.

            //String[] arr_market = { "0"};
            String[] arr_market = { "0", "3", "4", "5", "6", "8", "9", "10", "30", "50" };
            List<TB_STOCK> lst = new List<TB_STOCK>();
            for (int i = 0; i < arr_market.Length; i++)
            {
                String market_cd = arr_market[i];
                String ret = axKHOpenAPI.GetCodeListByMarket(market_cd);
                String[] arr_ret = ret.Split(';');
                for (int j = 0; j < arr_ret.Length; j++)
                {
                    String stock_cd = arr_ret[j].Trim();
                    if(!stock_cd.Equals(""))
                    {
                        rtn.Add(stock_cd);
                        TB_STOCK tmp = new TB_STOCK();
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
            }



            if (lst.Count > 0)
            {
                FirstData fd = new FirstData();
                fd.insertMarketStockData(lst);
            }
            lst.Clear();

            //정렬순서 (0:코드순, 1:테마순)
            //비고 반환값의 코드와 코드명 구분은 ‘|’ 코드의 구분은 ‘;’ Ex) 100|태양광_폴리실리콘;152|합성섬유
            //테마주는 다 A가 들어가는데 사실 A로 시작되는 종목은 없는것 같다.

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
                    String stock_cd = arr_ret[j].Trim();
                    if(!stock_cd.Equals(""))
                    {
                        TB_STOCK tmp_stock = new TB_STOCK();
                        //rtn.Add(stock_cd);  테마는 실종목이 아니므로 실시간에 넣지 않는다.
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

            rtn = rtn.Distinct().ToList();

            rtn.Remove("069500");           //KODEX200
            rtn.Remove("069660");           //KOSEF200
            rtn.Remove("091160");           //KODEX반도체
            rtn.Remove("091170");           //KODEX은행
            rtn.Remove("091180");           //KODEX자동차
            rtn.Remove("099140");           //KODEXChinaH
            rtn.Remove("100910");           //KOSEFKRX100
            rtn.Remove("101280");           //KODEXJapan
            rtn.Remove("102780");           //KODEX삼성그룹
            rtn.Remove("102960");           //KODEX조선
            rtn.Remove("102970");           //KODEX증권
            rtn.Remove("104520");           //KOSEF블루칩
            rtn.Remove("104530");           //KOSEF고배당
            rtn.Remove("114260");           //KODEX국고채
            rtn.Remove("114470");           //KOSEF국고채
            rtn.Remove("114800");           //KODEX인버스
            rtn.Remove("117460");           //KODEX에너지화학
            rtn.Remove("117680");           //KODEX철강
            rtn.Remove("117700");           //KODEX건설
            rtn.Remove("122260");           //KOSEF통안채
            rtn.Remove("122630");           //KODEX레버리지
            rtn.Remove("130730");           //KOSEF단기자금
            rtn.Remove("132030");           //KODEX골드선물(H)
            rtn.Remove("136280");           //KODEX소비재
            rtn.Remove("138230");           //KOSEF달러선물
            rtn.Remove("138910");           //KODEX구리선물(H)
            rtn.Remove("138920");           //KODEX콩선물(H)
            rtn.Remove("139660");           //KOSEF달러인버스선물
            rtn.Remove("140700");           //KODEX보험
            rtn.Remove("140710");           //KODEX운송
            rtn.Remove("144600");           //KODEX은선물(H)
            rtn.Remove("148070");           //KOSEF10년국고채
            rtn.Remove("152280");           //KOSEF200선물
            rtn.Remove("152380");           //KODEX10년국채선물
            rtn.Remove("153130");           //KODEX단기채권
            rtn.Remove("153270");           //KOSEF100
            rtn.Remove("156080");           //KODEXMSCIKOREA
            rtn.Remove("167860");           //KOSEF10년국고채레버리지
            rtn.Remove("169950");           //KODEX중국본토A50
            rtn.Remove("176950");           //KODEX인버스국채선물10년
            rtn.Remove("185680");           //KODEX미국바이오(합성)
            rtn.Remove("200020");           //KODEX미국IT(합성)
            rtn.Remove("200030");           //KODEX미국산업재(합성)
            rtn.Remove("200040");           //KODEX미국금융(합성)
            rtn.Remove("200050");           //KODEXMSCI독일(합성)
            rtn.Remove("200250");           //KOSEF인디아(합성)
            rtn.Remove("204450");           //KODEXChinaH레버리지(H)
            rtn.Remove("211900");           //KODEX배당성장
            rtn.Remove("213610");           //KODEX삼성그룹밸류
            rtn.Remove("214980");           //KODEX단기채권PLUS
            rtn.Remove("218420");           //KODEX미국에너지(합성)
            rtn.Remove("219480");           //KODEXS&P500선물(H)
            rtn.Remove("223190");           //KODEX200내재가치
            rtn.Remove("225800");           //KOSEF미국달러선물레버리지(합성)
            rtn.Remove("226490");           //KODEX코스피
            rtn.Remove("226980");           //KODEX200중소형
            rtn.Remove("229200");           //KODEX코스닥150
            rtn.Remove("229720");           //KODEXKTOP30
            rtn.Remove("230480");           //KOSEF미국달러선물인버스2X(합성)
            rtn.Remove("233740");           //KODEX코스닥150레버리지
            rtn.Remove("237350");           //KODEX200대형
            rtn.Remove("237370");           //KODEX배당성장채권혼합
            rtn.Remove("083350");           //동북아10호
            rtn.Remove("083360");           //동북아11호
            rtn.Remove("083370");           //동북아12호
            rtn.Remove("083380");           //동북아13호
            rtn.Remove("083390");           //동북아14호
            rtn.Remove("083570");           //아시아10호
            rtn.Remove("083580");           //아시아11호
            rtn.Remove("083590");           //아시아12호
            rtn.Remove("083600");           //아시아13호
            rtn.Remove("083610");           //아시아14호
            rtn.Remove("083620");           //아시아15호
            rtn.Remove("088980");           //맥쿼리인플라
            rtn.Remove("090970");           //코리아01호
            rtn.Remove("090980");           //코리아02호
            rtn.Remove("090990");           //코리아03호
            rtn.Remove("091000");           //코리아04호
            rtn.Remove("091210");   //TIGERKRX100
            rtn.Remove("091220");   //TIGER은행
            rtn.Remove("091230");   //TIGER반도체
            rtn.Remove("092630");   //바다로3호
            rtn.Remove("094800");   //맵스리얼티1
            rtn.Remove("096300");   //베트남개발1
            rtn.Remove("097750");   //TREX중소형가치
            rtn.Remove("098560");   //TIGER미디어통신
            rtn.Remove("099340");   //하나니켈1호
            rtn.Remove("099350");   //하나니켈2호
            rtn.Remove("102110");   //TIGER200
            rtn.Remove("105010");   //TIGER라틴
            rtn.Remove("105190");   //KINDEX200
            rtn.Remove("105270");   //KINDEX성장대형F15
            rtn.Remove("105780");   //KStar5대그룹주
            rtn.Remove("107560");   //GIANT현대차그룹
            rtn.Remove("108440");   //KINDEX코스닥스타
            rtn.Remove("108450");   //KINDEX삼성그룹SW
            rtn.Remove("108590");   //TREX200
            rtn.Remove("108630");   //FIRST스타우량
            rtn.Remove("114100");   //KStar국고채
            rtn.Remove("114460");   //KINDEX국고채
            rtn.Remove("114820");   //TIGER국채3
            rtn.Remove("117690");   //TIGER차이나
            rtn.Remove("122090");   //ARIRANGKOSPI50
            rtn.Remove("122390");   //TIGER코스닥프리미어
            rtn.Remove("123310");   //TIGER인버스
            rtn.Remove("123320");   //TIGER레버리지
            rtn.Remove("123760");   //KStar레버리지
            rtn.Remove("130680");   //TIGER원유선물(H)
            rtn.Remove("131890");   //KINDEX삼성그룹EW
            rtn.Remove("133690");   //TIGER나스닥100
            rtn.Remove("136340");   //KStar우량회사채
            rtn.Remove("137610");   //TIGER농산물선물(H)
            rtn.Remove("137930");   //마이다스커버드콜
            rtn.Remove("138520");   //TIGER삼성그룹
            rtn.Remove("138530");   //TIGERLG그룹+
            rtn.Remove("138540");   //TIGER현대차그룹+
            rtn.Remove("139200");   //하이골드2호
            rtn.Remove("139220");   //TIGER200건설
            rtn.Remove("139230");   //TIGER200중공업
            rtn.Remove("139240");   //TIGER200철강소재
            rtn.Remove("139250");   //TIGER200에너지화학
            rtn.Remove("139260");   //TIGER200IT
            rtn.Remove("139270");   //TIGER200금융
            rtn.Remove("139280");   //TIGER경기방어
            rtn.Remove("139290");   //TIGER200경기소비재
            rtn.Remove("139310");   //TIGER금속선물(H)
            rtn.Remove("139320");   //TIGER금은선물(H)
            rtn.Remove("140570");   //KStar수출주
            rtn.Remove("140580");   //KStar우량업종
            rtn.Remove("140890");   //트러스제7호
            rtn.Remove("140910");   //광희리츠
            rtn.Remove("140950");   //파워K100
            rtn.Remove("141240");   //ARIRANGK100EW
            rtn.Remove("143460");   //KINDEX밸류대형
            rtn.Remove("143850");   //TIGERS&P500선물(H)
            rtn.Remove("143860");   //TIGER헬스케어
            rtn.Remove("145270");   //케이탑리츠
            rtn.Remove("145670");   //KINDEX인버스
            rtn.Remove("145850");   //TREX펀더멘탈200
            rtn.Remove("147970");   //TIGER모멘텀
            rtn.Remove("148020");   //KStar200
            rtn.Remove("148040");   //PIONEERSRI
            rtn.Remove("150460");   //TIGER중국소비테마
            rtn.Remove("152100");   //ARIRANG200
            rtn.Remove("152180");   //TIGER생활필수품
            rtn.Remove("152500");   //KINDEX레버리지
            rtn.Remove("152550");   //한국ANKOR유전
            rtn.Remove("152870");   //파워K200
            rtn.Remove("153360");   //하이골드3호
            rtn.Remove("155900");   //바다로19호
            rtn.Remove("157450");   //TIGER유동자금
            rtn.Remove("157490");   //TIGER소프트웨어
            rtn.Remove("157500");   //TIGER증권
            rtn.Remove("157510");   //TIGER자동차
            rtn.Remove("157520");   //TIGER화학
            rtn.Remove("159650");   //하이골드8호
            rtn.Remove("159800");   //마이티K100
            rtn.Remove("160580");   //TIGER구리실물
            rtn.Remove("161490");   //ARIRANG방어주
            rtn.Remove("161500");   //ARIRANG주도주
            rtn.Remove("161510");   //ARIRANG고배당주
            rtn.Remove("166400");   //TIGER커버드C200
            rtn.Remove("168300");   //KTOP50
            rtn.Remove("168490");   //한국패러랠
            rtn.Remove("168580");   //KINDEX중국본토CSI300
            rtn.Remove("170350");   //TIGER베타플러스
            rtn.Remove("172580");   //하이골드12호
            rtn.Remove("174350");   //TIGER로우볼
            rtn.Remove("174360");   //KStar중국본토대형주CSI100
            rtn.Remove("176710");   //파워국고채
            rtn.Remove("181450");   //KINDEX선진국하이일드(합성H)
            rtn.Remove("181480");   //KINDEX미국리츠부동산(합성H)
            rtn.Remove("182480");   //TIGERUS리츠(합성H)
            rtn.Remove("182490");   //TIGER단기선진하이일드(합성H)
            rtn.Remove("183700");   //KStar채권혼합
            rtn.Remove("183710");   //KStar주식혼합
            rtn.Remove("189400");   //ARIRANGAC월드(합성H)
            rtn.Remove("190150");   //ARIRANG바벨채권
            rtn.Remove("190160");   //ARIRANG단기유동성
            rtn.Remove("190620");   //KINDEX단기자금
            rtn.Remove("192090");   //TIGER차이나A300
            rtn.Remove("192720");   //파워고배당저변동성
            rtn.Remove("195920");   //TIGER일본(합성H)
            rtn.Remove("195930");   //TIGER유로스탁스50(합성H)
            rtn.Remove("195970");   //ARIRANG선진국(합성H)
            rtn.Remove("195980");   //ARIRANG신흥국(합성H)
            rtn.Remove("196030");   //KINDEX일본레버리지(H)
            rtn.Remove("196220");   //KStar일본레버리지(H)
            rtn.Remove("196230");   //KStar단기통안채
            rtn.Remove("203780");   //TIGER나스닥바이오
            rtn.Remove("204420");   //ARIRANG차이나H레버리지(합성H)
            rtn.Remove("204480");   //TIGER차이나A레버리지(합성)
            rtn.Remove("205720");   //KINDEX일본인버스(합성H)
            rtn.Remove("208470");   //SMARTMSCI선진국(합성H)
            rtn.Remove("210780");   //TIGER코스피고배당
            rtn.Remove("211210");   //마이티코스피고배당
            rtn.Remove("211260");   //KINDEX배당성장
            rtn.Remove("211560");   //TIGER배당성장
            rtn.Remove("213630");   //ARIRANG미국고배당주(합성H)
            rtn.Remove("215620");   //흥국S&P로우볼
            rtn.Remove("217770");   //TIGER원유인버스선물(H)
            rtn.Remove("217780");   //TIGER차이나A인버스(합성)
            rtn.Remove("217790");   //TIGER가격조정
            rtn.Remove("219390");   //KStar미국원유생산기업(합성H)
            rtn.Remove("219900");   //KINDEX중국본토레버리지(합성)
            rtn.Remove("220130");   //SMART중국본토중소형CSI500(합성H)
            rtn.Remove("222170");   //ARIRANGS&P배당성장
            rtn.Remove("222180");   //ARIRANG스마트베타Value
            rtn.Remove("222190");   //ARIRANG스마트베타Momentum
            rtn.Remove("222200");   //ARIRANG스마트베타Quality
            rtn.Remove("225030");   //TIGERS&P500인버스선물(H)
            rtn.Remove("225040");   //TIGERS&P500레버리지(합성H)
            rtn.Remove("225050");   //TIGER유로스탁스레버리지(합성H)
            rtn.Remove("225060");   //TIGER이머징마켓레버리지(합성H)
            rtn.Remove("225130");   //KINDEX골드선물레버리지(합성H)
            rtn.Remove("226380");   //KINDEX한류
            rtn.Remove("226810");   //파워단기채
            rtn.Remove("227540");   //TIGER200건강관리
            rtn.Remove("227550");   //TIGER200산업재
            rtn.Remove("227560");   //TIGER200생활소비재
            rtn.Remove("227570");   //TIGER우량가치
            rtn.Remove("227830");   //ARIRANG코스피
            rtn.Remove("227930");   //KINDEX코스닥150
            rtn.Remove("228790");   //TIGER화장품
            rtn.Remove("228800");   //TIGER여행레저
            rtn.Remove("228810");   //TIGER미디어컨텐츠
            rtn.Remove("228820");   //TIGERKTOP30
            rtn.Remove("232080");   //TIGER코스닥150
            rtn.Remove("232590");   //KINDEX골드선물인버스2X(합성H)
            rtn.Remove("233160");   //TIGER코스닥150레버리지
            rtn.Remove("234310");   //KStarV&S셀렉트밸류
            rtn.Remove("234790");   //KINDEX코스닥150레버리지
            rtn.Remove("236460");   //ARIRANG스마트베타LowVOL
            rtn.Remove("237440");   //TIGER경기방어채권혼합
            rtn.Remove("238670");   //ARIRANG스마트베타Quality채권혼합
            rtn.Remove("500007");   //신한인버스은선물ETN(H)
            rtn.Remove("500001");   //신한K200USD선물바이셀ETN
            rtn.Remove("500002");   //신한USDK200선물바이셀ETN
            rtn.Remove("500003");   //신한인버스WTI원유선물ETN(H)
            rtn.Remove("500004");   //신한브렌트원유선물ETN(H)
            rtn.Remove("500005");   //신한인버스브렌트원유선물ETN(H)
            rtn.Remove("500006");   //신한인버스금선물ETN(H)
            rtn.Remove("500008");   //신한인버스구리선물ETN(H)
            rtn.Remove("500009");   //신한다우존스지수선물ETN(H)
            rtn.Remove("500010");   //신한인버스다우존스지수선물ETN(H)
            rtn.Remove("500011");   //신한달러인덱스선물ETN(H)
            rtn.Remove("500012");   //신한인버스달러인덱스선물ETN(H)
            rtn.Remove("500013");   //신한옥수수선물ETN(H)
            rtn.Remove("500014");   //신한인버스옥수수선물ETN(H)
            rtn.Remove("500015");   //신한WTI원유선물ETN(H)
            rtn.Remove("500016");   //신한금선물ETN(H)
            rtn.Remove("500017");   //신한은선물ETN(H)
            rtn.Remove("500018");   //신한구리선물ETN(H)
            rtn.Remove("500019");   //신한레버리지WTI원유선물ETN(H)
            rtn.Remove("520001");   //대우로우볼ETN
            rtn.Remove("520004");   //대우전기전자Core5ETN
            rtn.Remove("520005");   //대우인버스전기전자Core5ETN
            rtn.Remove("520006");   //대우에너지화학Core5ETN
            rtn.Remove("520007");   //대우인버스에너지화학Core5ETN
            rtn.Remove("520002");   //대우차이나대표주15ETN(H)
            rtn.Remove("520003");   //대우원자재선물ETN(H)
            rtn.Remove("530003");   //삼성모멘텀탑픽ETN
            rtn.Remove("530004");   //삼성화장품테마주ETN
            rtn.Remove("530005");   //삼성바이오테마주ETN
            rtn.Remove("530006");   //삼성음식료테마주ETN
            rtn.Remove("530007");   //삼성레저테마주ETN
            rtn.Remove("530008");   //삼성미디어테마주ETN
            rtn.Remove("530009");   //삼성증권테마주ETN
            rtn.Remove("530010");   //삼성건축자재테마주ETN
            rtn.Remove("530011");   //삼성온라인쇼핑테마주ETN
            rtn.Remove("530012");   //삼성화학테마주ETN
            rtn.Remove("530013");   //삼성KTOP30ETN
            rtn.Remove("530015");   //삼성미국대형성장주ETN(H)
            rtn.Remove("530016");   //삼성미국대형가치주ETN(H)
            rtn.Remove("530017");   //삼성미국중소형성장주ETN(H)
            rtn.Remove("530018");   //삼성미국중소형가치주ETN(H)
            rtn.Remove("530019");   //삼성미국대형성장주ETN
            rtn.Remove("530020");   //삼성미국대형가치주ETN
            rtn.Remove("530021");   //삼성미국중소형성장주ETN
            rtn.Remove("530022");   //삼성미국중소형가치주ETN
            rtn.Remove("530001");   //삼성유럽고배당주식ETN(H)
            rtn.Remove("530002");   //삼성인버스ChinaA50선물ETN(H)
            rtn.Remove("530014");   //삼성ChinaA50선물ETN(H)
            rtn.Remove("550001");   //QVBigVolETN
            rtn.Remove("550002");   //QVWISE배당ETN
            rtn.Remove("550003");   //QV스마트리밸런싱250/3ETN
            rtn.Remove("550004");   //QV롱숏K150매수로우볼매도ETN
            rtn.Remove("550005");   //QV에너지TOP5ETN
            rtn.Remove("550006");   //QV내수소비TOP5ETN
            rtn.Remove("550007");   //QV조선TOP5ETN
            rtn.Remove("550008");   //QV소프트웨어TOP5ETN
            rtn.Remove("550009");   //QV하드웨어TOP5ETN
            rtn.Remove("550010");   //QV운송TOP5ETN
            rtn.Remove("550011");   //QV자동차TOP5ETN
            rtn.Remove("550012");   //QV의료TOP5ETN
            rtn.Remove("550013");   //QV화학TOP5ETN
            rtn.Remove("550014");   //QV바이오TOP5ETN
            rtn.Remove("550015");   //QV제약TOP5ETN
            rtn.Remove("550016");   //QV건설TOP5ETN
            rtn.Remove("550018");   //QVCHINEXTETN(H)
            rtn.Remove("570001");   //TRUE코스피선물매수콜매도ETN
            rtn.Remove("570002");   //TRUE코스피선물매도풋매도ETN
            rtn.Remove("570003");   //TRUE빅5동일가중ETN
            rtn.Remove("570005");   //TRUE목표변동성20코스피선물ETN
            rtn.Remove("570008");   //TRUE섹터탑픽ETN
            rtn.Remove("570009");   //TRUE코리아프리미어ETN
            rtn.Remove("570004");   //TRUE인버스유로스탁스50ETN(H)
            rtn.Remove("570007");   //TRUE위안화중국5년국채ETN
            rtn.Remove("570006");   //TRUE인버스차이나HETN(H)
            rtn.Remove("580001");   //able코스피200선물플러스ETN
            rtn.Remove("580002");   //ableQuant비중조절ETN
            rtn.Remove("580003");   //ableMonthlyBest11ETN
            rtn.Remove("580004");   //ableKQMonthlyBest11ETN
            rtn.Remove("580005");   //able우량주MonthlyBest11ETN
            rtn.Remove("590001");   //미래에셋미국바이백ETN(H)
            rtn.Remove("590002");	//미래에셋일본바이백ETN(H)

            FileLog.PrintF("setStockCodeList222 rtn.count=>"+ rtn.Count());
            return rtn;
        }

        private void SetRegReal005930()
        {
            //FileLog.PrintF("SetRegReal005930 stockCodeList.count=>" + stockCodeList.Count());
            String tmpStrFidList = "10";
            String strRealType = "1";  //1이어야한다. 0은 마지막에 등록하것만 실시간 받는거고 1은 등록한 모들걸 받는것이다.
            String strCode = "005930";


            FileLog.PrintF(String.Format("SetRegRealAll strCode=>{0}", strCode));
            String tmpStrScreenNo = ScreenNumber.getClass1Instance().GetRealTimeScrNum();
            int rtn = axKHOpenAPI.SetRealReg(tmpStrScreenNo, strCode, tmpStrFidList, strRealType);
            FileLog.PrintF(String.Format("SetRegRealAll 11  strCode=>{0} rtn={1}", strCode, rtn));
            
        }

        private void SetRegRealAll()
        {
            /*
             3.SetRealReg()함수
SetRealReg()함수는 조회하지 않아도 실시간 시세데이터를 수신할 수 있도록 제공된 함수입니다.
(이 함수는 실시간 시세데이터는 조회하는 것이 아니며 수신할 실시간 시세데이터를 등록하는 것입니다.)

상세한 함수설명과 사용예는 KOA StudioSA 개발목록과 자료실 예제에 있으니 개발에 꼭 참고하시기 바랍니다.
이 함수는 최대 100종목 실시간시세 수신이 가능하며 3번째 인자값을 조절해서 종목을 추가해서 수신할 것인지도 결정할 수 있습니다.

필요에 따라 여러종목의 실시간 시세데이터를 추가해서 수신하려면 SetRealReg()함수 마지막 인자값을 1로 등록해서 사용합니다.
그리고 0으로 등록하면 SetRealReg()함수 호출에 사용한 종목만 실시간 시세를 수신하게 됩니다.

실시간 시세 종목을 추가등록하는 경우 - "1"
SetRealReg(sScreenNum, A, sFidList, 1); //A종목 실시간 시세수신
SetRealReg(sScreenNum, B, sFidList, 1); //A,B종목 실시간 시세수신
SetRealReg(sScreenNum, C, sFidList, 1); //A,B, C종목 실시간 시세수신

한 종목만 실시간 시세 종목등록하는 경우 - "0"
SetRealReg(sScreenNum, A, sFidList, 0); //A 종목 실시간 시세수신
SetRealReg(sScreenNum, B, sFidList, 0); //B종목 실시간 시세수신
SetRealReg(sScreenNum, C, sFidList, 0); //C종목 실시간 시세수신

등록한 실시간시세 해지는 SetRealRemove()함수나 DisconnectRealData()함수를 이용할 수 있습니다.
DisconnectRealData()함수는 화면번호에 등록한 모든 실시간 시세를 해지하며 이는 SetRealRemove("화면번호", "ALL")과 같습니다.

참고
"종목프로그램매매" 실시간 시세 데이터는 SetRealReg()함수의 FID 인자값을 "131"번을 사용하시면 됩니다.

4. 실시간 데이터 수신시간
정규시장 시간(09:00 ~ 15:30)이후에도 장후 시간외, 시간외 단일가 거래에 따른 ECN주식체결, ECN주식호가잔량 실시간 데이터가 18:00까지 전달됩니다.
이 데이터는 OpenAPI에서 제공하지 않기 때문에 이용하실 수 없습니다.

5. 예외사항
다음 실시간 타입은 OpenAPI 내부적으로 사용하거나 제공하지않는 실시간 타입니다.
(실시간 시세등록 지원안하는 데이터입니다.)
1. 임의연장정보
2. 시간외종목정보
3. 주식거래원
4. 순간체결량
5. 선물옵션합계
6. 투자자별매매

그리고 "주문체결", "잔고", "파생잔고"는 실시간 시세데이터가 아니며 주문을 통해서만 OnReceiveChejanData이벤트로 전달됩니다.
실시간 시세 "주식호가잔량"에 포함된 FID 201도 이와같은 예외처리부분입니다.



감사합니다.
             */

            FileLog.PrintF("SetRegRealAll stockCodeList.count=>" + stockCodeList.Count());
            String tmpStrFidList = "10";
            String strRealType = "0";  //0 이어야한다 설명은 위에
            List<String> strCodeList = new List<string>();
            int cnt = stockCodeList.Count();

            double tmp = cnt / 100f;
            double tmpCnt = Math.Ceiling(tmp);

            FileLog.PrintF("SetRegRealAll cnt=>" + cnt);
            FileLog.PrintF("SetRegRealAll tmp=>" + tmp);
            FileLog.PrintF("SetRegRealAll tmpCnt=>" + tmpCnt);

            for(int i = 1; i <=tmpCnt; i++)
            {
                int a = (i * 100)-100;
                int b = (i * 100);
                if (b > cnt)
                {
                    b = cnt%100-1;
                } else
                {
                    b = 100;
                }
                //FileLog.PrintF(String.Format("SetRegRealAll a{0}~갯수{1}", a, b));
                List<String> tmpList = stockCodeList.GetRange(a, b);
                strCodeList.Add(string.Join(";", tmpList));
            }

            //67개  6700개
            //로그상 13개 등록하는데서 멈췄다.
            //52EE86 이런게 찍혔는데..
            //정황상... 주식종목코드가 맞지 않아서 인것 같다.
            for (int i=0;i<strCodeList.Count;i++)
            {
                FileLog.PrintF(String.Format("SetRegRealAll i=>{0}", i));
                String strCode = strCodeList[i];
                FileLog.PrintF(String.Format("SetRegRealAll strCode=>{0}", strCode));
                String tmpStrScreenNo = ScreenNumber.getClass1Instance().GetRealTimeScrNum();
                int rtn = axKHOpenAPI.SetRealReg(tmpStrScreenNo, strCode, tmpStrFidList, strRealType);
                FileLog.PrintF(String.Format("SetRegRealAll strRealType={0}", strRealType));
                FileLog.PrintF(String.Format("SetRegRealAll tmpStrFidList={0}", tmpStrFidList));
                FileLog.PrintF(String.Format("SetRegRealAll tmpStrScreenNo={0}", tmpStrScreenNo));
                FileLog.PrintF(String.Format("SetRegRealAll rtn={0}",  rtn));
            }
            
        }

        private List<String> setStockCodeListDb() {
            //디비에 있는걸 읽어오니.. 약간 문제가 있었다. elw가 가끔 키움에서 안가져오는것같다.
            List<String> tt = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr())) {
                //string sql = "SELECT STOCK_CODE FROM stocks WHERE ";
                String sql = "select distinct stock_code from stocks ta, market_stocks tb where ta.stock_code = tb.stock_code_id and market_code_id in (0,10) order by stock_code asc";
                //장내와 코스닥만 조회하도록; 기존에 ELW같은거 같이 조회했더니 키움에서 약간 문제가 있는듯하다.
                conn.Open();
                //ExecuteReader를 이용하여
                //연결 모드로 데이타 가져오기
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ///Console.WriteLine("{0}: {1}", rdr["Id"], rdr["STOCK_CODE"]);
                    String tmp = rdr["STOCK_CODE"].ToString();
                    tt.Add(tmp);
                    //FileLog.PrintF("setStockCodeList tmp = " + tmp);
                }
                rdr.Close();
            }
            return tt;
        }
    }
}
