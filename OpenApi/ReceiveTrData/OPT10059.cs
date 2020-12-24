using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi.ReceiveTrData
{

    public class OPT10059 : ReceiveTrData
    {

        public OPT10059() {
            FileLog.PrintF("ReceivedData OPT10059");
        }


        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10059");
            try
            {
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                List<TB_OPT10059> lst = new List<TB_OPT10059>();
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
            
                spell = AppLib.getClass1Instance().getSpell(key);
                String startDate = spell.startDate;

                String lastStockDate = "";
                int startDate일자 = 0;
                if (!int.TryParse(startDate, out startDate일자))
                {
                    startDate일자 = 0;
                }

                if (nCnt > 0)
                {
                    for (int i = 0; i < nCnt; i++)
                    {                   
                        int 일자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[0]
                        lastStockDate = 일자.ToString();
                        if (startDate != null)
                        {
                            if (!(일자 >= startDate일자))
                            {
                                //입력받은 20200301 을 정수로 바꾼 strartDAte일자
                                //데이터로 받을 "일자" 가 이것보다 큰것만 가져온다.
                                //명령을 넣을때 실제로 시작일자는 ui상에서 종료일자가 들어간다.\
                                // api는 시작일자를 기준으로 내림차순으로 하기 때문이다.
                                //내림차순으로 받은거에서 끊는 역할을 해준다.
                                break;
                            }
                        }

                        int 개인투자자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "개인투자자").Trim());//[6]
                        int 외국인투자자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "외국인투자자").Trim());//[7]
                        int 기관계 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기관계").Trim());//[8]
                        int 금융투자 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "금융투자").Trim());//[9]
                        int 보험 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "보험").Trim());//[10]
                        int 투신 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "투신").Trim());//[11]
                        int 기타금융 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기타금융").Trim());//[12]
                        int 은행 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "은행").Trim());//[13]
                        int 연기금등 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "연기금등").Trim());//[14]
                        int 사모펀드 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "사모펀드").Trim());//[15]
                        int 국가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "국가").Trim());//[16]
                        int 기타법인 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "기타법인").Trim());//[17]
                        int 내외국인 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "내외국인").Trim());//[18]


                        TB_OPT10059 tmp = new TB_OPT10059();
                        tmp.stock_cd = 종목코드;
                        tmp.stock_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim();
                        tmp.buy_sell = spell.buyOrSell;
                        tmp.amt_amount = spell.priceOrAmount;
                        tmp.domestic_investor = 개인투자자;
                        tmp.foreign_investor = 외국인투자자;
                        tmp.institution = 기관계;
                        tmp.financial_investment = 금융투자;
                        tmp.insurance = 보험;
                        tmp.investment_trust = 투신;
                        tmp.etc_financial = 기타금융;
                        tmp.bank = 은행;
                        tmp.pension_fund = 연기금등;
                        tmp.private_equity_fund = 사모펀드;
                        tmp.nation = 국가;
                        tmp.etc_corporation = 기타법인;
                        tmp.foregin_investment_in_korea = 내외국인;
                        tmp.prev_next = e.sPrevNext;
                        lst.Add(tmp);
                    }
                }
                else
                {
                    종목코드 = "00000";
                }

                if (lst.Count() > 0)
                {
                    DailyData dd = new DailyData();
                    dd.insertOpt10059(lst);
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

                if (startDate.Equals("ZERO") || (startDate.Length == 8 && startDate.CompareTo(lastStockDate) >= 0) || prevNext == 0)
                //ZERO면 한번만 호출이다. 또는 시작일과 마지막 리턴일이 같다면 종료되어야한다.
                {
                    AppLib.getClass1Instance().removeSpellDictionary(spell.key);
                    int position = spell.key.LastIndexOf("|");
                    String key1 = spell.key.Substring(0, position);
                    //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
                }
                else if (prevNext > 0)
                {
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
                FileLog.PrintF("[ALERT-ReceivedData-OPT10059]Exception ex=" + ex.Message);
            }
        }

        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell)
        {
            /*
             [ OPT10059 : 종목별투자자기관별요청 ]
              1. Open API 조회 함수 입력값을 설정합니다.
	            일자 = YYYYMMDD (20160101 연도4자리, 월 2자리, 일 2자리 형식)
	            SetInputValue("일자"	,  "입력값 1");
                종목코드 = 전문 조회할 종목코드
	            SetInputValue("종목코드"	,  "입력값 2");
                금액수량구분 = 1:금액, 2:수량
	            SetInputValue("금액수량구분"	,  "입력값 3");
                매매구분 = 0:순매수, 1:매수, 2:매도
	            SetInputValue("매매구분"	,  "입력값 4");
                단위구분 = 1000:천주, 1:단주
	            SetInputValue("단위구분"	,  "입력값 5");
            2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	            CommRqData( "RQName"	,  "OPT10059"	,  "0"	,  "화면번호"); 
            */
            
            /* 정상동작확인 주석
            FileLog.PrintF("OPT10059:Run sRQNAME=> " + spell.sRQNAME);
            FileLog.PrintF("OPT10059:Run sTrCode=> " + spell.sTrCode);
            FileLog.PrintF("OPT10059:Run nPrevNext=> " + spell.nPrevNext);
            FileLog.PrintF("OPT10059:Run lastStockDate=> " + spell.lastStockDate);
            FileLog.PrintF("OPT10059:Run sScreenNo=> " + spell.sScreenNo);
            FileLog.PrintF("OPT10059:Run 일자=> " + spell.endDate);
            FileLog.PrintF("OPT10059:Run 종목코드=> " + spell.stockCode);
            FileLog.PrintF("OPT10059:Run 금액수량구분=> " + spell.priceOrAmount);
            FileLog.PrintF("OPT10059:Run 매매구분=> " + spell.buyOrSell);
            FileLog.PrintF("OPT10059:Run 단위구분=> 1");
            */
            axKHOpenAPI.SetInputValue("일자", spell.endDate);
            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);           
            axKHOpenAPI.SetInputValue("금액수량구분", spell.priceOrAmount);   
            axKHOpenAPI.SetInputValue("매매구분", spell.buyOrSell); 
            axKHOpenAPI.SetInputValue("단위구분", "1"); /*단위구분 = 1000:천주, 1:단주   --수량단위구분 모조건 1   -금액은 무조건 백만원단위 */
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }

    }
}
