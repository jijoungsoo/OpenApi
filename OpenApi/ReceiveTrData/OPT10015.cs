using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenApi.Spell;
using AxKHOpenAPILib;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi.ReceiveTrData
{
    public class OPT10015 : ReceiveTrData
    {

        public OPT10015() {
            FileLog.PrintF("OPT10015");
        }

        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
        private static Dictionary<string, int> clearFlag = new Dictionary<string, int>();

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10015");
            try
            {
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                List<TB_OPT10015> lst = new List<TB_OPT10015>();
                String 종목코드 = "XXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                종목코드 = AppLib.getClass1Instance().getStockCode(keyStockCode);

                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 종목코드
                );

                spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();
                String startDate = spell.startDate;
                String lastStockDate = "";
                int startDate일자 = 0;
                if (!int.TryParse(startDate, out startDate일자))
                {
                    startDate일자 = 0;
                }

                if (nCnt > 0)
                {//시작일자에 내림차순으로 들어온다.
                    for (int i = 0; i < nCnt; i++)
                    {
                        int 일자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[0]
                        lastStockDate = 일자.ToString();
                        if (startDate != null)
                        { 
                            if (!(  일자 >= startDate일자   ))
                            {
                                    //입력받은 20200301 을 정수로 바꾼 strartDAte일자
                                    //데이터로 받을 "일자" 가 이것보다 큰것만 가져온다.
                                    //명령을 넣을때 실제로 시작일자는 ui상에서 종료일자가 들어간다.\
                                    // api는 시작일자를 기준으로 내림차순으로 하기 때문이다.
                                    //내림차순으로 받은거에서 끊는 역할을 해준다.
                                    break;
                            } 
                        }
                        
                        int 종가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종가").Trim());//[1]
                        int 전일대비기호 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비기호").Trim());//[2]
                        int 전일대비 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "전일대비").Trim());//[3]
                        float 등락율 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "등락율").Trim());//[4]
                        int 거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래량").Trim());//[5]
                        int 거래대금 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "거래대금").Trim());//[6]
                        int 장전거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장전거래량").Trim());//[7]
                        float 장전거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장전거래비중").Trim());//[8]
                        int 장중거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장중거래량").Trim());//[9]
                        float 장중거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장중거래비중").Trim());//[10]
                        int 장후거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장후거래량").Trim());//[11]
                        float 장후거래비중 = float.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "장후거래비중").Trim());//[12]
                        int 기간중거래량 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기간중거래량").Trim());//[14]

                        TB_OPT10015 tmp = new TB_OPT10015();
                        tmp.stock_cd = 종목코드;
                        tmp.stock_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim();
                         
                        tmp.curr_amt = 종가;
                        tmp.contrast_yesterday_symbol = 전일대비기호;
                        tmp.contrast_yesterday = 전일대비;
                        tmp.fluctuation_rt = 등락율;
                        tmp.trade_qty = 거래량;
                        tmp.trade_amt = 거래대금;
                        tmp.before_market_trade_qty = 장전거래량;
                        tmp.before_market_trade_rt = 장전거래비중;
                        tmp.market_trade_qty = 장중거래량;
                        tmp.market_trade_rt = 장중거래비중;
                        tmp.after_market_trade_qty = 장후거래량;
                        tmp.after_market_trade_rt = 장후거래비중;
                        tmp.between_trade_qty = 기간중거래량;
                        tmp.prev_next         = e.sPrevNext;

                        lst.Add(tmp);
                    }
                }
                else {
                    종목코드 = "00000";
                }

                if (lst.Count() > 0)
                {
                    DailyData dd = new DailyData();
                    dd.insertOpt10015(lst);
                }

                int prevNext = 0;
                int.TryParse(e.sPrevNext, out prevNext);                
                //ScreenNumber.getClass1Instance().DisconnectRealData(e.sScrNo);
                //ScreenNumber.getClass1Instance().SetRealRemove("ALL", "ALL");
                //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
                AppLib.getClass1Instance().setOpt10081(spell.sTrCode);
                //2020년03월29일 확인 내용
                //nprevNext가 0이면 종료고 2이면 진행으로 알고 있었다.
                //그런데 어느시점이 지나가면 그게 아닌것 같다.
                //이걸 확인하려면 디비에 이걸 넣어야한다.
                //2012년까지 정상동작하고 이전데이터는 무조건 prev_next를 0을 리턴한다.
                //이건 수동으로 봐야겠다. 끝.

                if (startDate.Equals("ZERO") || (  startDate.Length==8 && startDate.CompareTo(lastStockDate)>=0 ) || prevNext==0) 
                    //ZERO면 한번만 호출이다. 또는 시작일과 마지막 리턴일이 같다면 종료되어야한다.
                {
                    AppLib.getClass1Instance().removeSpellDictionary(spell.key);
                    int position = spell.key.LastIndexOf("|");
                    String key1 = spell.key.Substring(0, position);
                    //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
                } else  if(prevNext>0) {
                    //putReceivedQueueAndsetNextSpell(key, prevNext, lastStockDate);
                    OpenApi.Spell.SpellOpt tmp = spell.ShallowCopy();
                    tmp.nPrevNext = prevNext;
                    tmp.lastStockDate = lastStockDate;
                    AppLib.getClass1Instance().removeSpellDictionary(key);
                    AppLib.getClass1Instance().AddSpellDictionary(key, tmp);
                    AppLib.getClass1Instance().EnqueueByOrderQueue(tmp);  //주문을 다시 넣는다.
                }
            }
            catch (Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10015]Exception ex=" + ex.Message);
            }

        }


        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
            /*
                [OPT10015: 일별거래상세요청]
                1. Open API 조회 함수 입력값을 설정합니다.
                종목코드 = 전문 조회할 종목코드
                SetInputValue("종목코드"	,  "입력값 1");
                시작일자 = YYYYMMDD(20160101 연도4자리, 월 2자리, 일 2자리 형식)
                SetInputValue("시작일자"	,  "입력값 2");
                2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
                CommRqData( "RQName"	,  "OPT10015"	,  "0"	,  "화면번호");
            */
            /* 정상동작확인 주석
            FileLog.PrintF("OPT10015:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10015:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10015:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10015:Run lastStockDate=> " + spell.lastStockDate);
            FileLog.PrintF("OPT10015:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10015:Run 시작일자=> " + spell.endDate);
            FileLog.PrintF("OPT10015:Run 종목코드=> " + spell.stockCode);
            */
            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            axKHOpenAPI.SetInputValue("시작일자", spell.endDate);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}