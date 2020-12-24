using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using OpenApi.Spell;
using AxKHOpenAPILib;
using MySql.Data.MySqlClient;
using OpenApi.Dto;
using OpenApi.Dao;

namespace OpenApi.ReceiveTrData
{

    public class OPT10085 : ReceiveTrData
    {
        public OPT10085(){
            FileLog.PrintF("OPT10085");
        }

        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveTrDataEvent e)
        {
            FileLog.PrintF("ReceivedData OPT10085");
            //try { 
                /*
                sScrNo – 화면번호
                sRQName – 사용자구분 명
                sTrCode – Tran 명
                sRecordName – Record 명
                sPreNext – 연속조회 유무
                */
                String 계좌번호 = "XXXXXXXXXX";
                int nCnt = axKHOpenAPI.GetRepeatCnt(e.sTrCode, e.sRQName);
                String keyStockCodeLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}";
                String keyStockCode = String.Format(keyStockCodeLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                );
                계좌번호 = AppLib.getClass1Instance().getStockCode(keyStockCode);

                String keyLayout = "sRQName:{0}|sTrCode:{1}|sScreenNo:{2}|accountNum:{3}";
                String key = String.Format(keyLayout
                    , e.sRQName
                    , e.sTrCode
                    , e.sScrNo
                    , 계좌번호
                );
            spell = AppLib.getClass1Instance().getSpell(key).ShallowCopy();

            List < TB_OPT10085 >  opt10085_DataList = new List<TB_OPT10085>();


                if (nCnt > 0) {
                    for (int i = 0; i < nCnt; i++) {

                    FileLog.PrintF("OPT10085 ReceivedData 구매일자 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim());//[0]
                    FileLog.PrintF("OPT10085 ReceivedData 종목코드 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim());//[1]
                    FileLog.PrintF("OPT10085 ReceivedData 종목명 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목명").Trim());//[2]
                    FileLog.PrintF("OPT10085 ReceivedData 현재가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim());//[3]
                    FileLog.PrintF("OPT10085 ReceivedData 매입가 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매입가").Trim());//[4]
                    FileLog.PrintF("OPT10085 ReceivedData 매입금액 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매입금액").Trim());//[5]
                    FileLog.PrintF("OPT10085 ReceivedData 보유수량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "보유수량").Trim());//[6]
                    FileLog.PrintF("OPT10085 ReceivedData 당일매도손익 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매도손익").Trim());//[7]
                    FileLog.PrintF("OPT10085 ReceivedData 당일매매수수료 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매수수료").Trim());//[8]
                    FileLog.PrintF("OPT10085 ReceivedData 당일매매세금 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매세금").Trim());//[9]
                    FileLog.PrintF("OPT10085 ReceivedData 신용구분 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용구분").Trim());//[10]
                    FileLog.PrintF("OPT10085 ReceivedData 대출일 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대출일").Trim());//[11]
                    FileLog.PrintF("OPT10085 ReceivedData 결제잔고 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "결제잔고").Trim());//[12]
                    FileLog.PrintF("OPT10085 ReceivedData 청산가능수량 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "청산가능수량").Trim());//[13]
                    FileLog.PrintF("OPT10085 ReceivedData 신용금액 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용금액").Trim());//[14]
                    FileLog.PrintF("OPT10085 ReceivedData 신용이자 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용이자").Trim());//[15]
                    FileLog.PrintF("OPT10085 ReceivedData 만기일 =>" + axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "만기일").Trim());//[16]



                    TB_OPT10085 opt10085_Data = new TB_OPT10085();
                    opt10085_Data.purchase_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "일자").Trim();//[0]
                    opt10085_Data.stock_cd = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "종목코드").Trim();//[1]
                    int 현재가 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "현재가").Trim()); //[3]
                    opt10085_Data.curr_amt = 현재가;
                    opt10085_Data.purchase_amt = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매입가").Trim()); //[4]
                    int 매입금액 = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "매입금액").Trim()); //[5]
                    opt10085_Data.tot_purchase_amt = 매입금액;
                    int 보유수량= Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "보유수량").Trim()); //[6]
                    opt10085_Data.possession_qty = 보유수량;
                    opt10085_Data.today_sell_profit_loss = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매도손익").Trim());  //[7]
                        String str당일매매수수료 = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매수수료");
                    opt10085_Data.today_commission = 0; 
                        if (isNotNull(str당일매매수수료) == true)
                        {
                        opt10085_Data.today_commission = Int32.Parse(str당일매매수수료.Trim());//[8]
                        }
                    opt10085_Data.today_tax = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "당일매매세금").Trim()); //[9]
                    opt10085_Data.credit_gubun = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용구분").Trim();//[10]
                    opt10085_Data.loan_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "대출일").Trim();//[11]
                    opt10085_Data.payment_balance = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "결제잔고").Trim()); //[12]
                    opt10085_Data.sellable_qty = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "청산가능수량").Trim()); //[13]
                    opt10085_Data.credit_amt = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용금액").Trim()); //[14]
                    opt10085_Data.credit_interest = Int32.Parse(axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "신용이자").Trim()); //[15]
                    opt10085_Data.expiry_dt = axKHOpenAPI.CommGetData(e.sTrCode, "", e.sRQName, i, "만기일").Trim();//[16]

                        /*내가 만들 데이터*/
                        /*매입가에는 수수료가 포함안되어있다. 매입당시 수수료가 통장에서 빠져나가지만 내가 얼마나 수익이 났는지 확인하기 위해 
                        매입수수료와 매도 수수료를 합쳐서 수수료로 표시해준다.
                        수수료는 10원 미만 절삭 86원이면 80원임
                        */
                        //System.Math.Truncate()
                        int 평가금액 = Math.Abs(현재가) * 보유수량;
                        int 매입수수료 = Commission.GetKiwoomCommissionBuy(매입금액);
                        int 매도수수료 = Commission.GetKiwoomCommissionSell(평가금액);
                        int 수수료 = 매입수수료 + 매도수수료;
                        int 매도세금 =Commission.GetTaxSell(평가금액);
                    int 손익분기매입가 = 0;
                    if (보유수량 != 0)//이게 0일경우가 있다 매도를 한상태일경우 보유수량이 0으로 리스트에 계속 존재한다.
                    {
                        손익분기매입가 = (매입수수료 + 매도수수료 + 매도세금 + 매입금액) / 보유수량;  // 무조건오림
                    }
                    int 평가손익 = 평가금액 - (매입금액+수수료+매도세금);
                    // float 수익률 = (평가손익 / 매입금액) * 100;   //int끼리 나눠서... 소수점을 버리는구나.. 이런..
                    float 수익률 = 0;
                    if (매입금액 != 0)
                    {
                        수익률 = ((float)평가손익 / (float)매입금액) * 100;
                    }
                    int 손익금액 = (평가금액-매입금액);

                    float 손익율 = 0;
                    if (매입금액 != 0)
                    {
                        손익율 = ((float)손익금액 / (float)매입금액) * 100;
                    }

                    

                    opt10085_Data.evaluated_amt=평가금액;
                    opt10085_Data.selling_commission = 매입수수료;
                    opt10085_Data.buying_commission = 매도수수료;
                    opt10085_Data.commission = 수수료;
                    opt10085_Data.selling_tax = 매도세금;
                    opt10085_Data.will_profit_amt = 손익분기매입가;
                    opt10085_Data.valuation_profit_loss = 평가손익;
                    opt10085_Data.earnings_rt = 수익률;
                    opt10085_Data.not_commission_profit_loss = 손익금액;
                    opt10085_Data.profit_loss_rt = 손익율;
                    opt10085_Data.acct_num = 계좌번호;
                    

                    FileLog.PrintF("OPT10085 ReceivedData 평가손익=>" + 평가손익);
                    FileLog.PrintF("OPT10085 ReceivedData 손익금액=>" + 손익금액);
                    FileLog.PrintF("OPT10085 ReceivedData 매입금액=>" + 매입금액);
                    FileLog.PrintF("OPT10085 ReceivedData 평가손익=>" + 평가손익);
                    FileLog.PrintF("OPT10085 ReceivedData 수익률=>" + 수익률);
                    FileLog.PrintF("OPT10085 ReceivedData 손익율=>" + 손익율);


                    //(8025-8089)/8089*100
                    //24005=23775+160+70
                    //24005-24575=-570
                    //24805
                    opt10085_DataList.Add(opt10085_Data);
                    }
                }

            //이것은 연속적이지 않기 때문에 바로 제거 한다.
            AppLib.getClass1Instance().removeSpellDictionary(spell.key);
            int position = spell.key.LastIndexOf("|");
            String key1 = spell.key.Substring(0, position);
            AppLib.getClass1Instance().removeStockCodeDictionary(key1);
            //래치를 호출해서 잠김을 제거한다.--래치 일단 제거 호출하는데도 제거 했다. 1초에 5번 호출 규칙만 적용해보자.
            AppLib.getClass1Instance().setOpt10081(spell.sTrCode);

            DailyData dd = new DailyData();
            dd.insertOpt10085(opt10085_DataList);
        }


        public override int Run(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, SpellOpt spell) {
            /*
 [ opt10085 : 계좌수익률요청 ]
 1. Open API 조회 함수 입력값을 설정합니다.
	계좌번호 = 전문 조회할 보유계좌번호
	SetInputValue("계좌번호"	,  "입력값 1");
 2. Open API 조회 함수를 호출해서 전문을 서버로 전송합니다.
	CommRqData( "RQName"	,  "opt10085"	,  "0"	,  "화면번호"); 
            */
            FileLog.PrintF("OPT10001:Run sRQNAME=>" + spell.sRQNAME);
            FileLog.PrintF("OPT10001:Run sTrCode=>" + spell.sTrCode);
            FileLog.PrintF("OPT10001:Run nPrevNext=>" + spell.nPrevNext);
            FileLog.PrintF("OPT10001:Run sScreenNo=>" + spell.sScreenNo);
            FileLog.PrintF("OPT10001:Run 계좌번호=>" + spell.accountNum);

            axKHOpenAPI.SetInputValue("계좌번호", spell.accountNum);
            int ret = axKHOpenAPI.CommRqData(spell.sRQNAME, spell.sTrCode, spell.nPrevNext, spell.sScreenNo);
            return ret;
        }
    }
}
