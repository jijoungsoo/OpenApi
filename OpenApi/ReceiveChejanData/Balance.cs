using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AxKHOpenAPILib;
using MySql.Data.MySqlClient;
using OpenApi.Dao;
using OpenApi.Dto;

namespace OpenApi.ReceiveChejanData
{
    class Balance : ReceiveChejanData
    {
        public Balance()
        {
            FileLog.PrintF("Balance");
        }

        public override void ReceivedData(AxKHOpenAPI axKHOpenAPI, _DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 구분 : 잔고통보");
            //FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문/기록시간=>" + axKHOpenAPI.GetChejanData(908));
            //시간이 없음
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 계좌번호=>" + axKHOpenAPI.GetChejanData(9201));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목코드, 업종코드=>" + axKHOpenAPI.GetChejanData(9001));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 현재가, 체결가, 실시간종가=>" + axKHOpenAPI.GetChejanData(10));

            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 신용구분=>" + axKHOpenAPI.GetChejanData(917));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 대출일=>" + axKHOpenAPI.GetChejanData(916));


            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 보유수량=>" + axKHOpenAPI.GetChejanData(930));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매입단가=>" + axKHOpenAPI.GetChejanData(931));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 총매입가=>" + axKHOpenAPI.GetChejanData(932));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문가능수량=>" + axKHOpenAPI.GetChejanData(933));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일순매수량=>" + axKHOpenAPI.GetChejanData(945));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매도 / 매수구분=>" + axKHOpenAPI.GetChejanData(946));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일 총 매도 손익=>" + axKHOpenAPI.GetChejanData(950));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 예수금=>" + axKHOpenAPI.GetChejanData(951));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매도호가=>" + axKHOpenAPI.GetChejanData(27));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매수호가=>" + axKHOpenAPI.GetChejanData(28));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 기준가=>" + axKHOpenAPI.GetChejanData(307));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 손익율=>" + axKHOpenAPI.GetChejanData(8019));
            

            /*추가-살아있다.*/
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 신용금액=>" + axKHOpenAPI.GetChejanData(957));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 신용이자=>" + axKHOpenAPI.GetChejanData(958));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 만기일=>" + axKHOpenAPI.GetChejanData(918));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일실현손익(유가)=>" + axKHOpenAPI.GetChejanData(990));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일실현손익률(유가) =>" + axKHOpenAPI.GetChejanData(991));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일실현손익(신용)  =>" + axKHOpenAPI.GetChejanData(992));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일실현손익률(신용)  =>" + axKHOpenAPI.GetChejanData(993));
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 담보대출수량  =>" + axKHOpenAPI.GetChejanData(959));

          

            TB_CHEJAN_BALANCE balance_Data = new TB_CHEJAN_BALANCE();
            String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            balance_Data.acct_num = axKHOpenAPI.GetChejanData(9201).ToString().Trim(); //[1]
            balance_Data.stock_cd = axKHOpenAPI.GetChejanData(9001).ToString().Trim();  //[2]
            balance_Data.curr_amt = int.Parse(axKHOpenAPI.GetChejanData(10).ToString().Trim());  //[3]
            balance_Data.loan_dt = axKHOpenAPI.GetChejanData(916).ToString().Trim();  //[3]
            balance_Data.credit_gubun = axKHOpenAPI.GetChejanData(917).ToString().Trim();  //[3]
            balance_Data.possession_qty = int.Parse(axKHOpenAPI.GetChejanData(930).ToString().Trim());  //[4]
            balance_Data.purchase_amt = int.Parse(axKHOpenAPI.GetChejanData(931).ToString().Trim());   //[5]
            balance_Data.tot_purchase_amt = int.Parse(axKHOpenAPI.GetChejanData(932).ToString().Trim());  //[6]
            balance_Data.order_possible_qty = int.Parse(axKHOpenAPI.GetChejanData(933).ToString().Trim());   //[7]
            balance_Data.today_net_buy_qty = int.Parse(axKHOpenAPI.GetChejanData(945).ToString().Trim());  //[8]
            balance_Data.order_type = int.Parse(axKHOpenAPI.GetChejanData(946).ToString().Trim());  //[9]
            balance_Data.today_sell_profit_loss = int.Parse(axKHOpenAPI.GetChejanData(950).ToString().Trim());   //[10]
            balance_Data.deposit = int.Parse(axKHOpenAPI.GetChejanData(951).ToString().Trim());  //[11]
            balance_Data.offered_amt = int.Parse(axKHOpenAPI.GetChejanData(27).ToString().Trim());  //[12]
            balance_Data.bid_amt = int.Parse(axKHOpenAPI.GetChejanData(28).ToString().Trim()); //[13]
            balance_Data.yesterday_amt = int.Parse(axKHOpenAPI.GetChejanData(307).ToString().Trim());   //[14]
            balance_Data.profit_loss_rt = float.Parse(axKHOpenAPI.GetChejanData(8019).ToString().Trim());   //[15]

            balance_Data.profit_loss_rt = float.Parse(axKHOpenAPI.GetChejanData(8019).ToString().Trim());   //[15]
            balance_Data.credit_gubun = axKHOpenAPI.GetChejanData(917);   //[15]
            balance_Data.loan_dt = axKHOpenAPI.GetChejanData(916);   //[15]
            balance_Data.loan_qty = int.Parse(axKHOpenAPI.GetChejanData(959));   //[15]
            balance_Data.credit_amt = int.Parse(axKHOpenAPI.GetChejanData(957));   //[15]
            balance_Data.credit_interest = float.Parse(axKHOpenAPI.GetChejanData(958));   //[15]
            balance_Data.expiry_dt = axKHOpenAPI.GetChejanData(918);   //[15]
            balance_Data.today_profit_loss_amt = int.Parse(axKHOpenAPI.GetChejanData(990));   //[15]
            balance_Data.today_profit_loss_rt = float.Parse(axKHOpenAPI.GetChejanData(991));   //[15]
            balance_Data.credit_today_profit_loss_amt = int.Parse(axKHOpenAPI.GetChejanData(992));   //[15]
            balance_Data.credit_today_profit_loss_rt = float.Parse(axKHOpenAPI.GetChejanData(993));   //[15]

            ChejanData chejanData = new ChejanData();
            chejanData.insertChejanBalance(balance_Data);
        }
    }
}