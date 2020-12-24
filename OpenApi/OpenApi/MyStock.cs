using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using KiwoomCode;
using System.Collections.Concurrent;
using System.IO;
using OpenApi.Dto;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace OpenApi
{
    class MyStock
    {
        private MyStock()
        {

        }
        private static Object _lockStockList = new Object();

        public int profit_rate = 0;
        public int loss_rate = 0;
        public Boolean profit_status = false;
        public Boolean loss_status = false;
        public String accountNumber = "";

        private static Boolean _multiThread = true;
        private static MyStock _class1 = null;
        private static Object _object1 = new Object();
        public static MyStock getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_class1 == null)
                {
                    _class1 = new MyStock();
                }
            }
            else {
                if (_class1 == null)
                {
                    lock (_object1)
                    {
                        _class1 = new MyStock();
                    }
                }
            }
            return _class1;
        }

        private List<TB_OPT10085> stockList = new List<TB_OPT10085>();  //계좌를 하나만 사용할거라고 가정한다.
        public void UpdateStockList(TB_REALTIME_CONTRACT real10002_data)
        {
           // FileLog.PrintF("MyStock UpdateStockList real10002_data.종목코드=>"+ real10002_data.종목코드);
            //FileLog.PrintF("MyStock UpdateStockList stockList.Count()=>" + stockList.Count());
            lock (_lockStockList)
            {
                foreach (TB_OPT10085 stock in stockList)
                {
                   // FileLog.PrintF("MyStock UpdateStockList stock.종목코드=>" + stock.종목코드);
                    if (stock.stock_cd.Trim().Equals(real10002_data.stock_cd))
                    {
                       // FileLog.PrintF("MyStock UpdateStockList change");
                        int 현재가 = Math.Abs(real10002_data.curr_amt);
                        /*디비서 조회*/
                        int 보유수량 = stock.possession_qty;
                        int 매입금액 = stock.tot_purchase_amt;

                        int 평가금액 = 현재가 * 보유수량;
                        int 매입수수료 = Commission.GetKiwoomCommissionBuy(매입금액);
                        int 매도수수료 = Commission.GetKiwoomCommissionSell(평가금액);
                        int 수수료 = 매입수수료 + 매도수수료;
                        int 매도세금 = Commission.GetTaxSell(평가금액);
                        

                        int 손익분기매입가 = 0;
                        if (보유수량 != 0)//이게 0일경우가 있다 매도를 한상태일경우 보유수량이 0으로 리스트에 계속 존재한다.
                        {
                            손익분기매입가 = (매입수수료 + 매도수수료 + 매도세금 + 매입금액) / 보유수량;  // 무조건오림
                        }
                        int 평가손익 = 평가금액 - (매입금액 + 수수료 + 매도세금);
                        // float 수익률 = (평가손익 / 매입금액) * 100;   //int끼리 나눠서... 소수점을 버리는구나.. 이런..
                        float 수익률 = 0;
                        if (매입금액 != 0)
                        {
                            수익률 = ((float)평가손익 / (float)매입금액) * 100;
                        }
                        int 손익금액 = (평가금액 - 매입금액);

                        float 손익율 = 0;
                        if (매입금액 != 0)
                        {
                            손익율 = ((float)손익금액 / (float)매입금액) * 100;
                        }

                        stock.curr_amt = 현재가;
                        stock.evaluated_amt = 평가금액;
                        stock.buying_commission = 매도수수료;
                        stock.commission = 수수료;
                        stock.selling_tax = 매도세금;
                        stock.will_profit_amt = 손익분기매입가;
                        stock.valuation_profit_loss = 평가손익;
                        stock.earnings_rt = 수익률;
                        stock.not_commission_profit_loss = 손익금액;
                        stock.profit_loss_rt = 손익율;
                        dbUpdate(stock);
                        autoSale(stock);
                    }
                }
            }
        }
        private void dbUpdate(TB_OPT10085 opt10085_data)
        {
            FileLog.PrintF("MyStock dbUpdate");
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string sql = @"UPDATE opt10085s SET
 current_price=@현재가
,evaluated_price=@평가금액
,selling_commission=@매도수수료
,commission=@수수료
,selling_tax=@매도세금
,will_profit_price=@손익분기매입가
,valuation_profit_and_loss=@평가손익
,earnings_rate=@수익률
,not_commission_profit_and_loss=@손익금액
,not_commission_profit_and_loss_rate=@손익율
,updated_at=@업데이트날짜
WHERE stock_code=@종목코드
;
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@현재가", opt10085_data.curr_amt);
                cmd.Parameters.AddWithValue("@평가금액", opt10085_data.evaluated_amt);
                cmd.Parameters.AddWithValue("@매도수수료", opt10085_data.buying_commission);
                cmd.Parameters.AddWithValue("@수수료", opt10085_data.commission);
                cmd.Parameters.AddWithValue("@매도세금", opt10085_data.selling_tax);
                cmd.Parameters.AddWithValue("@손익분기매입가", opt10085_data.will_profit_amt);
                cmd.Parameters.AddWithValue("@평가손익", opt10085_data.valuation_profit_loss);
                cmd.Parameters.AddWithValue("@수익률", opt10085_data.earnings_rt);
                cmd.Parameters.AddWithValue("@손익금액", opt10085_data.not_commission_profit_loss);
                cmd.Parameters.AddWithValue("@손익율", opt10085_data.profit_loss_rt);
                cmd.Parameters.AddWithValue("@업데이트날짜", dayTime);
                cmd.Parameters.AddWithValue("@종목코드", opt10085_data.stock_cd);                
                cmd.ExecuteNonQuery();
            }
        }
        private void dbUpdateOrderStatus(TB_OPT10085 opt10085_data)
        {
            FileLog.PrintF("MyStock dbUpdateOrderStatus");
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                string sql = @"UPDATE opt10085s SET
 order_status=@주문상태
WHERE stock_code=@종목코드
AND account_number=@계좌번호
;
";
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@주문상태", opt10085_data.order_status );
                cmd.Parameters.AddWithValue("@종목코드", opt10085_data.stock_cd);
                cmd.Parameters.AddWithValue("@계좌번호", opt10085_data.acct_num);
                cmd.ExecuteNonQuery();
            }
        }
        public void reLoad(List<TB_OPT10085> opt10085_DataList)
        {
            SendDirectFile(opt10085_DataList);
            SendDirectDb(opt10085_DataList);
            String strCodeList = "";
            for (int i=0;i< opt10085_DataList.Count();i++)
            {
                strCodeList= strCodeList+ opt10085_DataList[i].stock_cd;
                if(i!= opt10085_DataList.Count())
                {
                    strCodeList = strCodeList + ";";
                }

            }
            if (!strCodeList.Equals(""))
            {
                SetRealReg(strCodeList);
            }
            
        }

        public List<TB_OPT10085> getStockList()
        {
            lock (_lockStockList)
            {
                return stockList;
            }
        }

        private void SetRealReg(String strCodeList)
        {
            FileLog.PrintF("MyStock SetRealReg strCodeList=>" + strCodeList);
            String strScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
            String strFidList = "10"; //현재가만 있으면됨.
            String strRealType = "0"; //같은 화면에 실시간을 더하는지 유무 0 더하지 않는다(지우고새로) , 1 새로받는다.
            FileLog.PrintF("SetRealReg strScreenNo=>" + strScreenNo);
            FileLog.PrintF("SetRealReg strCodeList=>" + strCodeList);
            FileLog.PrintF("SetRealReg strFidList=>" + strFidList);
            FileLog.PrintF("SetRealReg strRealType=>" + strRealType);
            //Class1.getClass1Instance().getAxKHOpenAPIInstance().SetRealReg(strScreenNo, strCodeList, strFidList, strRealType);
        }

        private void autoSale(TB_OPT10085 opt10085_data)
        {
            FileLog.PrintF("autoSale loss_status=>"+ loss_status+ ",loss_rate=>"+ loss_rate+ ",종목코드=>" + opt10085_data.stock_cd + ",손익율=>" + opt10085_data.profit_loss_rt + ",주문상태=>" + opt10085_data.order_status );
            //주식체결 정보가 들어와서 주식 현재가가 변동이 있을때 자동 판매로직이 실행됨
            /*손절매*/
            if (loss_status == true)
            {
                if (opt10085_data.profit_loss_rt <= this.loss_rate && opt10085_data.order_status ==1)//주문상태가 1 즉 보여상태이어야한다.
                {
                    if (MyOrder.getClass1Instance().ExistsOrder(opt10085_data.stock_cd) == false)
                    {
                        //매도주문
                        
                        int nOrderType = 2;//신규메도
                        String sCode = opt10085_data.stock_cd;
                        String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();
                        int nQty = opt10085_data.possession_qty;
                        int nPrice = 0;//일단 시장가매도하자.
                        String sHogaGb = "03"; //시장가 매도
                        String sOrgOrderNo = "";//원주문번호는 공백
                        FileLog.PrintF("autoSale sScreenNo=>" + sScreenNo);
                        FileLog.PrintF("autoSale accountNumber=>" + accountNumber);
                        FileLog.PrintF("autoSale nOrderType=>" + nOrderType);
                        FileLog.PrintF("autoSale sCode=>" + sCode);
                        FileLog.PrintF("autoSale nQty=>" + nQty);
                        FileLog.PrintF("autoSale nPrice=>" + nPrice);
                        FileLog.PrintF("autoSale sHogaGb=>" + sHogaGb);
                        FileLog.PrintF("autoSale sOrgOrderNo=>" + sOrgOrderNo);

                        //아 이거 1초에 5번 즉 0.2초 제한이 여기도 있다. ㅠㅠ이렇게 바로 보내면 안된다...
                        int ret = AppLib.getClass1Instance().getAxKHOpenAPIInstance().SendOrder("손절매_매도주문", sScreenNo, accountNumber, nOrderType, sCode, nQty, nPrice, sHogaGb, sOrgOrderNo);
                        FileLog.PrintF("MyStock AutoSale ret=>" + ret);
                        //상태가 하나더 있어야겠다 주문접수를 한상태인거는 다시 매도를 시도하면 안되므로
                        opt10085_data.order_status  = 2;
                        dbUpdateOrderStatus(opt10085_data);
                    }
                }

            }
            /*이익실현*/
            if (profit_status == true)
            {
                if (opt10085_data.profit_loss_rt >= this.profit_rate && opt10085_data.order_status  == 1)//주문상태가 1 즉 보여상태이어야한다.
                {
                    //매도주문
                    if (MyOrder.getClass1Instance().ExistsOrder(opt10085_data.stock_cd) == false)
                    {
                        
                        int nOrderType = 2;//신규메도
                        String sCode = opt10085_data.stock_cd;
                        int nQty = opt10085_data.possession_qty;
                        int nPrice = 0;//일단 시장가매도하자.
                        String sHogaGb = "03"; //시장가 매도
                        String sOrgOrderNo = "";//원주문번호는 공백

                        String sScreenNo = ScreenNumber.getClass1Instance().GetAnyTimeScrNum();


                        FileLog.PrintF("autoSale sScreenNo=>" + sScreenNo);
                        FileLog.PrintF("autoSale accountNumber=>" + accountNumber);
                        FileLog.PrintF("autoSale nOrderType=>" + nOrderType);
                        FileLog.PrintF("autoSale sCode=>" + sCode);
                        FileLog.PrintF("autoSale nQty=>" + nQty);
                        FileLog.PrintF("autoSale nPrice=>" + nPrice);
                        FileLog.PrintF("autoSale sHogaGb=>" + sHogaGb);
                        FileLog.PrintF("autoSale sOrgOrderNo=>" + sOrgOrderNo);
                        int ret = AppLib.getClass1Instance().getAxKHOpenAPIInstance().SendOrder("이익매_매도주문", sScreenNo, accountNumber, nOrderType, sCode, nQty, nPrice, sHogaGb, sOrgOrderNo);
                        FileLog.PrintF("MyStock AutoSale ret=>" + ret);
                        opt10085_data.order_status  = 2;
                        dbUpdateOrderStatus(opt10085_data);
                    }
                }
            }
        }

        private void SendDirectFile(List<TB_OPT10085> opt10085_DataList)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbAll = new StringBuilder();
            /*일자,계좌번호,종목코드,종목명,현재가,매입가,매입금액,보유수량,당일매도손익,당일매매수수료,당일매매세금,대출일,결제잔고,청산가능수량,신용금액,신용이자
            ,만기일,평가손익,수익률,평가금액,수수료,매입수수료,매도수수료,매도세금,손익분기매입가,손익금액,손익율*/
            sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}");
            String tmp = sb.ToString();
            foreach (TB_OPT10085 opt10085_Data in opt10085_DataList)
            {
                String tmp1 = String.Format(tmp,
                    opt10085_Data.purchase_dt,  //[0]
                    opt10085_Data.acct_num,  //[1]
                    opt10085_Data.stock_cd,  //[2]
                    opt10085_Data.curr_amt,  //[4]
                    opt10085_Data.purchase_amt,  //[5]
                    opt10085_Data.tot_purchase_amt,  //[6]
                    opt10085_Data.possession_qty,  //[7]
                    opt10085_Data.today_sell_profit_loss,  //[8]
                    opt10085_Data.today_commission,  //[9]
                    opt10085_Data.today_tax,  //[10]
                    opt10085_Data.credit_gubun,  //[11]
                    opt10085_Data.loan_dt,  //[12]
                    opt10085_Data.payment_balance,  //[13]
                    opt10085_Data.sellable_qty,  //[14]
                    opt10085_Data.credit_amt,  //[15]
                    opt10085_Data.credit_interest,  //[16]
                    opt10085_Data.expiry_dt,  //[17]
                    opt10085_Data.valuation_profit_loss,  //[18]
                    opt10085_Data.earnings_rt,  //[19]
                    opt10085_Data.evaluated_amt,  //[20]
                    opt10085_Data.commission,  //[21]
                    opt10085_Data.selling_commission,  //[22]
                    opt10085_Data.buying_commission,  //[23]
                    opt10085_Data.selling_tax,  //[24]
                    opt10085_Data.will_profit_amt,  //[25]
                    opt10085_Data.not_commission_profit_loss,  //[26]
                    opt10085_Data.profit_loss_rt  //[27]
                );
                sbAll.AppendLine(tmp1);
            }

            String ret = sbAll.ToString();

            System.IO.StreamWriter file = new System.IO.StreamWriter(Config.GetPath() + "TEST_OPT10085.txt", true);
            file.WriteLine(ret.ToString());
            file.Close();
        }

        private void SendDirectDb(List<TB_OPT10085> opt10085_DataList)
        {
            lock (_lockStockList)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
                    {
                         String sql1= "DELETE FROM opt10085s;";
                        conn.Open();
                        MySqlTransaction tr = conn.BeginTransaction();
                        String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        
                        try
                        {
                            MySqlCommand cmd = new MySqlCommand(sql1, conn, tr);
                            cmd.ExecuteNonQuery();
                            string sql2 = @"INSERT INTO opt10085s (
date_of_purchase  
,account_number
,stock_code
,stock_name
,current_price  
,purchase_price  
,total_amount_of_purchase 
,possession_quantity 
,today_sell_profit_and_loss 
,today_commission 
,today_tax 
,credit_gubun
,loan_date
,payment_balance 
,sellable_quantity 
,credit_amount 
,credit_interest 
,expiration_date
,valuation_profit_and_loss 
,earnings_rate
,evaluated_price 
,commission 
,buying_commission 
,selling_commission 
,selling_tax 
,will_profit_price 
,not_commission_profit_and_loss 
,not_commission_profit_and_loss_rate
,order_status
,created_at
,updated_at
)
VALUES";
String sql2_1= @"
(
@구매일자{0}
,@계좌번호{0}
,@종목코드{0}
,@종목명{0}
,@현재가{0}
,@매입가{0}
,@매입금액{0}
,@보유수량{0}
,@당일매도손익{0}
,@당일매매수수료{0}
,@당일매매세금{0}
,@신용구분{0}
,@대출일{0}
,@결제잔고{0}
,@청산가능수량{0}
,@신용금액{0}
,@신용이자{0}
,@만기일{0}
,@평가손익{0}
,@수익률{0}
,@평가금액{0}
,@수수료{0}
,@매입수수료{0}
,@매도수수료{0}
,@매도세금{0}
,@손익분기매입가{0}
,@손익금액{0}
,@손익율{0}
,@주문상태{0}
,@등록날짜{0}
,@업데이트날짜{0}
),";
                            StringBuilder queryBuilder = new StringBuilder(sql2);
                            for (int i = 0;  i< opt10085_DataList.Count(); i++)
                            {
                                queryBuilder.AppendFormat(sql2_1, i);
                                //once we're done looping we remove the last ',' and replace it with a ';'
                                if (i == opt10085_DataList.Count()-1)
                                {
                                    queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1);
                                }
                            }
                            String sql2_2 = queryBuilder.ToString();
                            FileLog.PrintF("SendDirectDb2 sql2_2:" + sql2_2.ToString());
                            cmd.CommandText = sql2_2;
                            for (int i = 0; i < opt10085_DataList.Count(); i++)
                            {
                                cmd.Parameters.AddWithValue("@구매일자" + i, opt10085_DataList[i].purchase_dt);
                                cmd.Parameters.AddWithValue("@계좌번호" + i, opt10085_DataList[i].acct_num);
                                cmd.Parameters.AddWithValue("@종목코드" + i, opt10085_DataList[i].stock_cd);
                                cmd.Parameters.AddWithValue("@현재가" + i, opt10085_DataList[i].curr_amt);
                                cmd.Parameters.AddWithValue("@매입가" + i, opt10085_DataList[i].purchase_amt);
                                cmd.Parameters.AddWithValue("@매입금액" + i, opt10085_DataList[i].tot_purchase_amt);
                                cmd.Parameters.AddWithValue("@보유수량" + i, opt10085_DataList[i].possession_qty);
                                cmd.Parameters.AddWithValue("@당일매도손익" + i, opt10085_DataList[i].today_sell_profit_loss);
                                cmd.Parameters.AddWithValue("@당일매매수수료" + i, opt10085_DataList[i].today_commission);
                                cmd.Parameters.AddWithValue("@당일매매세금" + i, opt10085_DataList[i].today_tax);
                                cmd.Parameters.AddWithValue("@신용구분" + i, opt10085_DataList[i].credit_gubun);
                                cmd.Parameters.AddWithValue("@대출일" + i, opt10085_DataList[i].loan_dt);
                                cmd.Parameters.AddWithValue("@결제잔고" + i, opt10085_DataList[i].payment_balance);
                                cmd.Parameters.AddWithValue("@청산가능수량" + i, opt10085_DataList[i].sellable_qty);
                                cmd.Parameters.AddWithValue("@신용금액" + i, opt10085_DataList[i].credit_amt);
                                cmd.Parameters.AddWithValue("@신용이자" + i, opt10085_DataList[i].credit_interest);
                                cmd.Parameters.AddWithValue("@만기일" + i, opt10085_DataList[i].expiry_dt);
                                cmd.Parameters.AddWithValue("@평가손익" + i, opt10085_DataList[i].valuation_profit_loss);
                                cmd.Parameters.AddWithValue("@수익률" + i, opt10085_DataList[i].earnings_rt);
                                cmd.Parameters.AddWithValue("@평가금액" + i, opt10085_DataList[i].evaluated_amt);
                                cmd.Parameters.AddWithValue("@수수료" + i, opt10085_DataList[i].commission);
                                cmd.Parameters.AddWithValue("@매입수수료" + i, opt10085_DataList[i].selling_commission);
                                cmd.Parameters.AddWithValue("@매도수수료" + i, opt10085_DataList[i].buying_commission);
                                cmd.Parameters.AddWithValue("@매도세금" + i, opt10085_DataList[i].selling_tax);
                                cmd.Parameters.AddWithValue("@손익분기매입가" + i, opt10085_DataList[i].will_profit_amt);
                                cmd.Parameters.AddWithValue("@손익금액" + i, opt10085_DataList[i].not_commission_profit_loss);
                                cmd.Parameters.AddWithValue("@손익율" + i, opt10085_DataList[i].profit_loss_rt);
                                cmd.Parameters.AddWithValue("@주문상태" + i, 1);//1 은 보유를 의미
                                cmd.Parameters.AddWithValue("@업데이트날짜" + i, dayTime);
                                cmd.Parameters.AddWithValue("@등록날짜" + i, dayTime);
                            }
                            cmd.ExecuteNonQuery();
                            String sql3 = @"SELECT
DATE_OF_PURCHASE  
,ACCOUNT_NUMBER
,STOCK_CODE
,STOCK_NAME
,CURRENT_PRICE  
,PURCHASE_PRICE  
,TOTAL_AMOUNT_OF_PURCHASE 
,POSSESSION_QUANTITY 
,TODAY_SELL_PROFIT_AND_LOSS 
,TODAY_COMMISSION 
,TODAY_TAX 
,CREDIT_GUBUN
,LOAN_DATE
,PAYMENT_BALANCE 
,SELLABLE_QUANTITY 
,CREDIT_AMOUNT 
,CREDIT_INTEREST 
,EXPIRATION_DATE
,VALUATION_PROFIT_AND_LOSS 
,EARNINGS_RATE
,EVALUATED_PRICE 
,COMMISSION 
,BUYING_COMMISSION 
,SELLING_COMMISSION 
,SELLING_TAX 
,WILL_PROFIT_PRICE 
,NOT_COMMISSION_PROFIT_AND_LOSS 
,NOT_COMMISSION_PROFIT_AND_LOSS_RATE
,ORDER_STATUS
FROM opt10085s
ORDER BY date_of_purchase DESC";
                            cmd.CommandText = sql3;
                            MySqlDataReader rdr=cmd.ExecuteReader();
                            // 다음 레코드 계속 가져와서 루핑
                            while (rdr.Read())
                            {
                                // C# 인덱서를 사용하여
                                // 필드 데이타 엑세스
                                TB_OPT10085 tmp = new TB_OPT10085();
                                tmp.purchase_dt = rdr["DATE_OF_PURCHASE"].ToString().Trim();
                                tmp.acct_num = rdr["ACCOUNT_NUMBER"].ToString().Trim();
                                tmp.stock_cd = rdr["STOCK_CODE"].ToString().Trim();
                                tmp.curr_amt = int.Parse(rdr["CURRENT_PRICE"].ToString().Trim());
                                tmp.purchase_amt = int.Parse(rdr["PURCHASE_PRICE"].ToString().Trim());
                                tmp.tot_purchase_amt = int.Parse(rdr["TOTAL_AMOUNT_OF_PURCHASE"].ToString().Trim());
                                tmp.possession_qty = int.Parse(rdr["POSSESSION_QUANTITY"].ToString().Trim());
                                tmp.today_sell_profit_loss = int.Parse(rdr["TODAY_SELL_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.today_commission = int.Parse(rdr["TODAY_COMMISSION"].ToString().Trim());
                                tmp.today_tax = int.Parse(rdr["TODAY_TAX"].ToString().Trim());
                                tmp.credit_gubun = rdr["CREDIT_GUBUN"].ToString().Trim();
                                tmp.loan_dt = rdr["LOAN_DATE"].ToString().Trim();
                                tmp.payment_balance = int.Parse(rdr["PAYMENT_BALANCE"].ToString().Trim());
                                tmp.sellable_qty = int.Parse(rdr["SELLABLE_QUANTITY"].ToString().Trim());
                                tmp.credit_amt = int.Parse(rdr["CREDIT_AMOUNT"].ToString().Trim());
                                tmp.credit_interest = int.Parse(rdr["CREDIT_INTEREST"].ToString().Trim());
                                tmp.expiry_dt = rdr["EXPIRATION_DATE"].ToString().Trim();
                                tmp.valuation_profit_loss = int.Parse(rdr["VALUATION_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.earnings_rt = float.Parse(rdr["EARNINGS_RATE"].ToString().Trim());
                                tmp.evaluated_amt = int.Parse(rdr["EVALUATED_PRICE"].ToString().Trim());
                                tmp.commission = int.Parse(rdr["COMMISSION"].ToString().Trim());
                                tmp.selling_commission = int.Parse(rdr["BUYING_COMMISSION"].ToString().Trim());
                                tmp.buying_commission = int.Parse(rdr["SELLING_COMMISSION"].ToString().Trim());
                                tmp.selling_tax = int.Parse(rdr["SELLING_TAX"].ToString().Trim());
                                tmp.will_profit_amt = int.Parse(rdr["WILL_PROFIT_PRICE"].ToString().Trim());
                                tmp.not_commission_profit_loss = int.Parse(rdr["NOT_COMMISSION_PROFIT_AND_LOSS"].ToString().Trim());
                                tmp.profit_loss_rt = float.Parse(rdr["NOT_COMMISSION_PROFIT_AND_LOSS_RATE"].ToString().Trim());
                                tmp.order_status  = int.Parse(rdr["ORDER_STATUS"].ToString().Trim());
                                stockList.Add(tmp);
                            }
                            // 사용후 닫음
                            rdr.Close();
                            tr.Commit();
                            
                        } catch (MySqlException ex2) {
                            try
                            {
                                tr.Rollback();

                            }
                            catch (MySqlException ex1)
                            {
                                FileLog.PrintF("SendDirectDb1 Error:" + ex1.ToString());
                            }
                            FileLog.PrintF("SendDirectDb2 Error:" + ex2.ToString());

                        }
                    }
                }
                       catch (MySqlException ex3)
                        {
                            FileLog.PrintF("SendDirectDb3 Error:" + ex3.ToString());
                        }
            }
        }
    }
}
