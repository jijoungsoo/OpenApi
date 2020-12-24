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
    class Order : ReceiveChejanData
    {
        public Order()
        {
            FileLog.PrintF("Order");
        }

        public override void ReceivedData(AxKHOpenAPI axKHOpenAPI, _DKHOpenAPIEvents_OnReceiveChejanDataEvent e)
        {
            /*주문체결
9201 계좌번호
9203 주문번호
9205 관리자사번
9001 종목코드, 업종코드
912 주문업무분류(JJ: 주식주문, FJ: 선물옵션, JG: 주식잔고, FG: 선물옵션잔고)
913 주문상태(10:원주문, 11:정정주문, 12:취소주문, 20:주문확인, 21:정정확인, 22:취소확인, 90 - 92:주문거부)
302 종목명
900 주문수량
901 주문가격
902 미체결수량
903 체결누계금액
904 원주문번호
905 주문구분(+현금내수, -현금매도…)
906 매매구분(보통, 시장가…)
907 매도수구분(1:매도, 2:매수)
908 주문 / 체결시간(HHMMSSMS)
909 체결번호
910 체결가
911 체결량
10 현재가, 체결가, 실시간종가
27(최우선)매도호가
28(최우선)매수호가
914 단위체결가
915 단위체결량
938 당일매매 수수료
939 당일매매세금
*/
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 구분 : 주문접수--통보");
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문/체결시간=>" + axKHOpenAPI.GetChejanData(908));   //[0]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 계좌번호=>" + axKHOpenAPI.GetChejanData(9201));   //[1]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문번호=>" + axKHOpenAPI.GetChejanData(9203));   //[2]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 관리자사번=>" + axKHOpenAPI.GetChejanData(9205));   //[3]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목코드, 업종코드=>" + axKHOpenAPI.GetChejanData(9001));   //[4]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문업무분류=>" + axKHOpenAPI.GetChejanData(912));   //[5]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문상태=>" + axKHOpenAPI.GetChejanData(913));   //[6]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 종목명=>" + axKHOpenAPI.GetChejanData(302));   //[7]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문수량=>" + axKHOpenAPI.GetChejanData(900));   //[8]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문가격=>" + axKHOpenAPI.GetChejanData(901));   //[9]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 미체결수량=>" + axKHOpenAPI.GetChejanData(902));   //[10]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결누계금액=>" + axKHOpenAPI.GetChejanData(903));   //[11]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 원주문번호=>" + axKHOpenAPI.GetChejanData(904));   //[12]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문구분(+현금내수, -현금매도…)=>" + axKHOpenAPI.GetChejanData(905));   //[13]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매매구분(보통, 시장가…)=>" + axKHOpenAPI.GetChejanData(906));   //[14]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매도수구분(1:매도, 2:매수)" + axKHOpenAPI.GetChejanData(907));   //[15]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결번호" + axKHOpenAPI.GetChejanData(909));   //[16]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결가=>" + axKHOpenAPI.GetChejanData(910));   //[17]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 체결량=>" + axKHOpenAPI.GetChejanData(911));   //[18]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 현재가, 체결가, 실시간종가=>" + axKHOpenAPI.GetChejanData(10));   //[19]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매도호가=>" + axKHOpenAPI.GetChejanData(27));   //[20]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData (최우선)매수호가=>" + axKHOpenAPI.GetChejanData(28));   //[21]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 단위체결가=>" + axKHOpenAPI.GetChejanData(914));   //[22]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 단위체결량=>" + axKHOpenAPI.GetChejanData(915));   //[23]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일매매 수수료=>" + axKHOpenAPI.GetChejanData(938));   //[24]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 당일매매세금=>" + axKHOpenAPI.GetChejanData(939));   //[25]

            /*카페 정보아래는 확인이 필요*/
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 화면번호=>" + axKHOpenAPI.GetChejanData(920));   //[26]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 터미널번호=>" + axKHOpenAPI.GetChejanData(921));   //[27]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 신용구분=>" + axKHOpenAPI.GetChejanData(922));   //[28]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 대출일=>" + axKHOpenAPI.GetChejanData(923));   //[29]


            /*추가적인 기록--주문에서 아래 데이터가 나오나 ??*/
            /* 안나온다.
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 거부사유=>" + axKHOpenAPI.GetChejanData(919));   //[29]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 보유수량=>" + axKHOpenAPI.GetChejanData(930));   //[29]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 매입단가=>" + axKHOpenAPI.GetChejanData(931));   //[29]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 총매입가=>" + axKHOpenAPI.GetChejanData(932));   //[29]
            FileLog.PrintF("axKHOpenAPI_OnReceiveChejanData 주문가능수량=>" + axKHOpenAPI.GetChejanData(933));   //[29]
            */



            TB_CHEJAN_ORDER order_Data = new TB_CHEJAN_ORDER();
            order_Data.curr_time = axKHOpenAPI.GetChejanData(908).ToString().Trim();
            order_Data.acct_num = axKHOpenAPI.GetChejanData(9201).ToString().Trim(); //[1]
            order_Data.order_num = axKHOpenAPI.GetChejanData(9203).ToString().Trim();  //[2]
            order_Data.stock_cd = axKHOpenAPI.GetChejanData(9001).ToString().Trim();  //[4]
            order_Data.order_business_classification = axKHOpenAPI.GetChejanData(912).ToString().Trim();   //[5]
            order_Data.order_status = axKHOpenAPI.GetChejanData(913).ToString().Trim();  //[6]
            order_Data.order_qty = int.Parse(axKHOpenAPI.GetChejanData(900).ToString().Trim());  //[8]
            order_Data.order_amt = int.Parse(axKHOpenAPI.GetChejanData(901).ToString().Trim());  //[9]
            order_Data.not_contract_qty = int.Parse(axKHOpenAPI.GetChejanData(902).ToString().Trim());   //[10]
            order_Data.contract_tot_amt = int.Parse(axKHOpenAPI.GetChejanData(903).ToString().Trim());  //[11]
            order_Data.ongn_order_num = axKHOpenAPI.GetChejanData(904).ToString().Trim();   //[12]
            order_Data.order_gubun = axKHOpenAPI.GetChejanData(905).ToString().Trim();  //[13]
            order_Data.trade_gubun = axKHOpenAPI.GetChejanData(906).ToString().Trim();   //[14]
            order_Data.order_type = int.Parse(axKHOpenAPI.GetChejanData(907).ToString().Trim());   //[15]
            order_Data.contract_num = axKHOpenAPI.GetChejanData(909).ToString().Trim();  //[16]
            String str체결가 = axKHOpenAPI.GetChejanData(910).ToString().Trim();  //[17]
            order_Data.contract_amt = 0;
            if (!str체결가.Equals(""))
            {
                order_Data.contract_amt = int.Parse(str체결가);
            }
            String str체결량 = axKHOpenAPI.GetChejanData(911).ToString().Trim();//[18]
            order_Data.contract_qty = 0;
            if (!str체결량.Equals(""))
            {
                order_Data.contract_qty = int.Parse(str체결량);
            }
            order_Data.curr_amt = int.Parse(axKHOpenAPI.GetChejanData(10).ToString().Trim());   //[19]
            order_Data.offered_amt = int.Parse(axKHOpenAPI.GetChejanData(27).ToString().Trim());   //[20]
            order_Data.bid_amt = int.Parse(axKHOpenAPI.GetChejanData(28).ToString().Trim());   //[21]

            String str단위체결가 = axKHOpenAPI.GetChejanData(914).ToString().Trim(); //[22]
            order_Data.contract_amt_unit = 0;
            if (!str단위체결가.Equals(""))
            {
                order_Data.contract_amt_unit = int.Parse(str단위체결가);
            }
            String str단위체결량 = axKHOpenAPI.GetChejanData(915).ToString().Trim(); //[23]
            order_Data.contract_amt_qty = 0;
            if (!str단위체결량.Equals(""))
            {
                order_Data.contract_amt_qty = int.Parse(str단위체결량);
            }
            order_Data.today_commission = int.Parse(axKHOpenAPI.GetChejanData(938).ToString().Trim());   //[24]
            order_Data.today_tax = int.Parse(axKHOpenAPI.GetChejanData(939).ToString().Trim());    //[25]

            order_Data.screen_num = axKHOpenAPI.GetChejanData(920).ToString().Trim();    //[25]
            order_Data.terminal_num = axKHOpenAPI.GetChejanData(921).ToString().Trim();    //[25]
            order_Data.credit_gubun = axKHOpenAPI.GetChejanData(922).ToString().Trim();    //[25]
            order_Data.loan_dt = axKHOpenAPI.GetChejanData(923).ToString().Trim();    //[25]

            ChejanData chejanData = new ChejanData();
            chejanData.insertChejanOrder(order_Data);
        }
    }
}