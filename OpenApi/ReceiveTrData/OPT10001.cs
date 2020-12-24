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

    public class OPT10001 : ReceiveTrData
    {
        public OPT10001(){
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            try { 
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                String 종목코드     = "XXXX";
                String keyStockCodeLayout   = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout, e.sRQName, e.sTrCode, e.sScrNo);
                종목코드            = AppLib.getClass1Instance().getStockCode(keyStockCode);
                String keyLayout    = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|stockCode:{3}";
                String key          = String.Format(keyLayout, e.sRQName, e.sTrCode, e.sScrNo, 종목코드);
                spell               = AppLib.getClass1Instance().getSpell(key).ShallowCopy();
                TB_OPT10001 tmp     = new TB_OPT10001();

                String 종목코드1    = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0, "종목코드").Trim();
                if (종목코드1.Equals(""))
                {
                    종목코드1 = 종목코드;//일부 종목코드들은 아래 데이터가 아예 안나온다. ///51A077
                }
                tmp.stock_cd       = 종목코드1;
                String 결산월      = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"결산월").Trim();
                tmp.settlement_mm  = 결산월;

                String str액면가    = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"액면가").Trim();
                int 액면가          = 0;
                int.TryParse(str액면가.Trim(), out 액면가);
                tmp.face_amt        = 액면가;

                String str자본금    = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"자본금").Trim();
                int 자본금          = 0;
                int.TryParse(str자본금.Trim(), out 자본금);
                tmp.capital_amt     = 자본금;

                String str상장주식  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"상장주식").Trim();
                int 상장주식        = 0;
                int.TryParse(str상장주식.Trim(), out 상장주식);
                tmp.stock_cnt       = 상장주식;
                
                String str신용비율  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"신용비율").Trim();
                float 신용비율      = 0;
                float.TryParse(str신용비율.Trim(), out 신용비율);
                tmp.credit_rt       = 신용비율;
                
                String str연중최고  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"연중최고").Trim();
                int 연중최고        = 0;
                int.TryParse(str연중최고.Trim(), out 연중최고);                
                tmp.year_high_amt   = 연중최고;

                String str연중최저  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"연중최저").Trim();
                int 연중최저        = 0;
                int.TryParse(str연중최저.Trim(), out 연중최저);
                tmp.year_low_amt    = 연중최저;
                
                
                String str시가총액  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"시가총액").Trim();
                int 시가총액        = 0;
                int.TryParse(str시가총액.Trim(), out 시가총액);
                tmp.total_mrkt_amt  = 시가총액;
                

                String str시가총액비중 = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"시가총액비중").Trim();
                float 시가총액비중     = 0;
                float.TryParse(str시가총액비중.Trim(), out 시가총액비중);
                tmp.total_mrkt_amt_rt  = 시가총액비중;

                String str외인소진률         = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"외인소진률").Trim();
                float 외인소진률             = 0;
                float.TryParse(str외인소진률.Trim(), out 외인소진률);
                tmp.foreigner_exhaustion_rt  = 외인소진률;

                String str대용가   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"대용가").Trim();
                int 대용가         = 0;
                int.TryParse(str대용가.Trim(), out 대용가);
                tmp.substitute_amt = 대용가;
                       
                String strPER   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"PER").Trim();
                float PER       = 0;
                float.TryParse(strPER.Trim(), out PER);
                tmp.per         = PER;                        

                String strEPS   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"EPS").Trim();
                int EPS         = 0;
                int.TryParse(strEPS.Trim(), out EPS);
                tmp.eps         = EPS;

                String strROE   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"ROE").Trim();
                float ROE       = 0;
                float.TryParse(strROE.Trim(), out ROE);
                tmp.roe         = ROE;

                String strPBR   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"PBR").Trim();
                float PBR       = 0;
                float.TryParse(strPBR.Trim(), out PBR);
                tmp.pbr         = PBR;

                String strEV    = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"EV").Trim();
                float EV        = 0;
                float.TryParse(strEV.Trim(), out EV);
                tmp.ev          = EV;

                String strBPS   = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"BPS").Trim();
                int BPS         = 0;
                int.TryParse(strBPS.Trim(), out BPS);
                tmp.bps         = BPS;

                String str매출액 = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"매출액").Trim();
                int 매출액       = 0;
                int.TryParse(str매출액.Trim(), out 매출액);
                tmp.sales        = 매출액;

                String str영업이익  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"영업이익").Trim();
                int 영업이익        = 0;
                int.TryParse(str영업이익.Trim(), out 영업이익);
                tmp.business_profits= 영업이익;

                String strD250최고  = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최고").Trim();
                int D250최고        = 0;
                int.TryParse(strD250최고.Trim(), out D250최고);
                tmp.d250_high_amt   = D250최고;

                String strD250최저 = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최저").Trim();
                int D250최저       = 0;
                int.TryParse(strD250최저.Trim(), out D250최저);
                tmp.d250_low_amt   = D250최저;

                int 시가      = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"시가").Trim());//[23]
                int 고가      = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"고가").Trim());//[24]
                int 저가      = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"저가").Trim());//[25]
                int 상한가    = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"상한가").Trim());//[26]
                int 하한가    = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"하한가").Trim());//[27]
                int 기준가    = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"기준가").Trim());//[28]
                tmp.start_amt = 시가;
                tmp.high_amt  = 고가;
                tmp.low_amt   = 저가;
                tmp.upper_amt_lmt = 상한가;
                tmp.lower_amt_lmt = 하한가;
                tmp.yesterday_amt = 기준가;

                String str예상체결가         = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"예상체결가").Trim();
                int 예상체결가               = 0;
                int.TryParse(str예상체결가.Trim(), out 예상체결가);
                tmp.expectation_contract_amt = 예상체결가;


                String str예상체결수량         = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"예상체결수량").Trim();
                int 예상체결수량               = 0;
                int.TryParse(str예상체결수량.Trim(), out 예상체결수량);
                tmp.expectation_contract_qty   = 예상체결수량;

                String D250최고가일     = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최고가일").Trim();
                tmp.d250_high_dt        = D250최고가일;

                float D250최고가대비율  = float.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최고가대비율").Trim());
                tmp.d250_high_rt        = D250최고가대비율;


                String D250최저가일     = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최저가일").Trim();
                tmp.d250_low_dt         = D250최저가일;

                float D250최저가대비율  = float.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"250최저가대비율").Trim());
                tmp.d250_low_rt         = D250최저가대비율;

                int 현재가              = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"현재가").Trim());
                tmp.curr_amt            = 현재가;

                int 대비기호            = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"대비기호").Trim());
                tmp.contrast_symbol     = 대비기호;

                int 전일대비            = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"전일대비").Trim());
                tmp.contrast_yesterday  = 전일대비;

                float 등락율            = float.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"등락율").Trim());
                tmp.fluctuation_rt      = 등락율;

                int 거래량              = int.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"거래량").Trim());
                tmp.trade_qty           = 거래량;

                float 거래대비          = float.Parse(axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0,"거래대비").Trim());
                tmp.trade_contrast      = 거래대비;
                                
                tmp.face_amt_unit = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0, "액면가단위").Trim();

                String str유통주식          = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0, "유통주식").Trim();//[31]
                int 유통주식                = 0;
                int.TryParse(str유통주식.Trim(), out 유통주식);
                tmp.distribution_stock      = 유통주식;

                String str유통비율          = axKHOpenAPI.GetCommData(e.sTrCode, e.sRQName, 0, "유통비율").Trim();//[31]
                float 유통비율              = 0;
                float.TryParse(str유통비율.Trim(), out 유통비율);
                tmp.distribution_stock_rt   = 유통비율;
                
                //이것은 연속적이지 않기 때문에 바로 제거 한다.
                AppLib.getClass1Instance().removeSpellDictionary(spell.key);
                int position = spell.key.LastIndexOf("|");
                String key1 = spell.key.Substring(0, position);
                AppLib.getClass1Instance().removeStockCodeDictionary(key1);
                //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
                AppLib.getClass1Instance().setOpt10081(spell.sTrCode);

                DailyData dd = new DailyData();
                dd.insertOpt10001(tmp);
            }
            catch(Exception ex)
            {
                FileLog.PrintF("[ALERT-ReceivedData-OPT10001]Exception ex=" + ex.Message);
            }
        }
     
        
        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
 [ opt10001 : 주식기본정보요청 ]
  1. Open API 조회 함수 입력값을 설정합니다.
	종목코드 = 전문 조회할 종목코드
	SetInputValue("종목코드"	,  "181710;066570");    
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "opt10001"	,  "0"	,  "화면번호"); 
            */
            /*정상동작확인 주석
            FileLog.PrintF("OPT10001:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10001:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10001:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10001:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10001:Run 종목코드=>" + spell.stockCode);
            */

            axKHOpenAPI.SetInputValue("종목코드", spell.stockCode);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}
