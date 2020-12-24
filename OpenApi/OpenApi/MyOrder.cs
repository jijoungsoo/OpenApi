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
    class MyOrder
    {
        private MyOrder()
        {

        }
        private static Boolean _multiThread = true;
        private static MyOrder _class1 = null;
        private static Object _object1 = new Object();
        public static MyOrder getClass1Instance()
        {
            if (_multiThread == false)
            {
                if (_class1 == null)
                {
                    _class1 = new MyOrder();
                }
            }
            else {
                if (_class1 == null)
                {
                    lock (_object1)
                    {
                        _class1 = new MyOrder();
                    }
                }
            }
            return _class1;
        }

        public Boolean ExistsOrder(String stockCode)
        {
            return false;
        }

        public void reLoad(List<OPT10075_Data> opt10075_DataList)
        {
            SendDirectFile(opt10075_DataList);
            SendDirectDb(opt10075_DataList);
            //실시간은 걸필요 없음 MyStock에서 걸었음.
        }


        private void SendDirectFile(List<OPT10075_Data> opt10075_DataList)
        {
            StringBuilder sb = new StringBuilder();
            StringBuilder sbAll = new StringBuilder();
            sb.Append("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}");
            String tmp = sb.ToString();
            foreach (OPT10075_Data opt10075_Data in opt10075_DataList)
            {
                String tmp1 = String.Format(tmp,
                    opt10075_Data.체결시간,  //[0]
                   opt10075_Data.계좌번호,  //[1]
                    opt10075_Data.주문번호,  //[2]
                    opt10075_Data.관리사번,  //[3]
                    opt10075_Data.종목코드,  //[4]
                    opt10075_Data.업무구분,  //[5]
                    opt10075_Data.주문상태,  //[6]
                    opt10075_Data.종목명,  //[7]
                    opt10075_Data.주문수량,  //[8]
                    opt10075_Data.주문가격,  //[9]
                    opt10075_Data.미체결수량,  //[10]
                    opt10075_Data.체결누계금액,  //[11]
                    opt10075_Data.원주문번호,  //[12]
                    opt10075_Data.주문구분,  //[13]
                    opt10075_Data.매매구분,  //[14]
                    opt10075_Data.체결번호,  //[15]
                    opt10075_Data.체결가,  //[16]
                    opt10075_Data.체결량,  //[17]
                    opt10075_Data.현재가,  //[18]
                    opt10075_Data.매도호가,  //[19]
                    opt10075_Data.매수호가,  //[20]
                    opt10075_Data.단위체결가,  //[21]
                    opt10075_Data.단위체결량,  //[22]
                    opt10075_Data.당일매매수수료,  //[23]
                    opt10075_Data.당일매매세금,  //[24]
                    opt10075_Data.개인투자자  //[25]
                );
                sbAll.AppendLine(tmp1);
            }
            String ret = sbAll.ToString();
            System.IO.StreamWriter file = new System.IO.StreamWriter(Config.GetPath() + "TEST_OPT10075.txt", true);
            file.WriteLine(ret.ToString());
            file.Close();
        }

        private void SendDirectDb(List<OPT10075_Data> opt10075_DataList)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
                {
                    String sql1 = "DELETE FROM opt10075s;";
                    conn.Open();
                    MySqlTransaction tr = conn.BeginTransaction();
                    String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    try
                    {
                        MySqlCommand cmd = new MySqlCommand(sql1, conn, tr);
                        cmd.ExecuteNonQuery();
                        string sql2 = @"INSERT INTO opt10075s (
contract_time  
,account_number
,order_number
,manager_number
,stock_code
,business_gubun  
,order_status  
,stock_name 
,order_quantity 
,order_price 
,not_contract_quantity 
,contract_total_amount 
,last_order_number
,order_gubun
,trade_gubun 
,contract_number 
,contract_price 
,contract_quantity 
,current_price
,offered_price 
,bid_price
,contract_price_unit 
,contract_price_quantity 
,today_commission 
,today_tax 
,domestic_investor 
,created_at
,updated_at
)
VALUES";
                        String sql2_1 = @"
(
@체결시간{0}
,@계좌번호{0}
,@주문번호{0}
,@관리사번{0}
,@종목코드{0}
,@업무구분{0}
,@주문상태{0}
,@종목명{0}
,@주문수량{0}
,@주문가격{0}
,@미체결수량{0}
,@체결누계금액{0}
,@원주문번호{0}
,@주문구분{0}
,@매매구분{0}
,@체결번호{0}
,@체결가{0}
,@체결량{0}
,@현재가{0}
,@매도호가{0}
,@매수호가{0}
,@단위체결가{0}
,@단위체결량{0}
,@당일매매수수료{0}
,@당일매매세금{0}
,@개인투자자{0}
,@등록날짜{0}
,@업데이트날짜{0}
),";
                        StringBuilder queryBuilder = new StringBuilder(sql2);
                        for (int i = 0; i < opt10075_DataList.Count(); i++)
                        {
                            queryBuilder.AppendFormat(sql2_1, i);
                            //once we're done looping we remove the last ',' and replace it with a ';'
                            if (i == opt10075_DataList.Count() - 1)
                            {
                                queryBuilder.Replace(',', ';', queryBuilder.Length - 1, 1);
                            }
                        }
                        String sql2_2 = queryBuilder.ToString();
                        FileLog.PrintF("SendDirectDb2 sql2_2:" + sql2_2.ToString());
                        cmd.CommandText = sql2_2;
                        for (int i = 0; i < opt10075_DataList.Count(); i++)
                        {
                            cmd.Parameters.AddWithValue("@체결시간" + i, opt10075_DataList[i].체결시간); //[0]
                            cmd.Parameters.AddWithValue("@계좌번호" + i, opt10075_DataList[i].계좌번호); //[1]
                            cmd.Parameters.AddWithValue("@주문번호" + i, opt10075_DataList[i].주문번호); //[2]
                            cmd.Parameters.AddWithValue("@관리사번" + i, opt10075_DataList[i].관리사번); //[3]
                            cmd.Parameters.AddWithValue("@종목코드" + i, opt10075_DataList[i].종목코드); //[4]
                            cmd.Parameters.AddWithValue("@업무구분" + i, opt10075_DataList[i].업무구분); //[5]
                            cmd.Parameters.AddWithValue("@주문상태" + i, opt10075_DataList[i].주문상태); //[6]
                            cmd.Parameters.AddWithValue("@종목명" + i, opt10075_DataList[i].종목명); //[7]
                            cmd.Parameters.AddWithValue("@주문수량" + i, opt10075_DataList[i].주문수량); //[8]
                            cmd.Parameters.AddWithValue("@주문가격" + i, opt10075_DataList[i].주문가격); //[9]
                            cmd.Parameters.AddWithValue("@미체결수량" + i, opt10075_DataList[i].미체결수량); //[10]
                            cmd.Parameters.AddWithValue("@체결누계금액" + i, opt10075_DataList[i].체결누계금액); //[11]
                            cmd.Parameters.AddWithValue("@원주문번호" + i, opt10075_DataList[i].원주문번호); //[12]
                            cmd.Parameters.AddWithValue("@주문구분" + i, opt10075_DataList[i].주문구분); //[13]
                            cmd.Parameters.AddWithValue("@매매구분" + i, opt10075_DataList[i].매매구분); //[14]
                            cmd.Parameters.AddWithValue("@체결번호" + i, opt10075_DataList[i].체결번호); //[15]
                            cmd.Parameters.AddWithValue("@체결가" + i, opt10075_DataList[i].체결가); //[16]
                            cmd.Parameters.AddWithValue("@체결량" + i, opt10075_DataList[i].체결량); //[17]
                            cmd.Parameters.AddWithValue("@현재가" + i, opt10075_DataList[i].현재가); //[18]
                            cmd.Parameters.AddWithValue("@매도호가" + i, opt10075_DataList[i].매도호가); //[19]
                            cmd.Parameters.AddWithValue("@매수호가" + i, opt10075_DataList[i].매수호가); //[20]
                            cmd.Parameters.AddWithValue("@단위체결가" + i, opt10075_DataList[i].단위체결가); //[21]
                            cmd.Parameters.AddWithValue("@단위체결량" + i, opt10075_DataList[i].단위체결량); //[22]
                            cmd.Parameters.AddWithValue("@당일매매수수료" + i, opt10075_DataList[i].당일매매수수료); //[23]
                            cmd.Parameters.AddWithValue("@당일매매세금" + i, opt10075_DataList[i].당일매매세금); //[24]
                            cmd.Parameters.AddWithValue("@개인투자자" + i, opt10075_DataList[i].개인투자자); //[25]
                            cmd.Parameters.AddWithValue("@업데이트날짜" + i, dayTime);
                            cmd.Parameters.AddWithValue("@등록날짜" + i, dayTime);
                        }
                        cmd.ExecuteNonQuery();
                        tr.Commit();

                    }
                    catch (MySqlException ex2)
                    {
                        try
                        {
                            tr.Rollback();

                        }
                        catch (MySqlException ex1)
                        {
                            FileLog.PrintF("MyOrder SendDirectDb1 Error:" + ex1.ToString());
                        }
                        FileLog.PrintF("MyOrder SendDirectDb2 Error:" + ex2.ToString());

                    }
                }
            }
            catch (MySqlException ex3)
            {
                FileLog.PrintF("MyOrder SendDirectDb3 Error:" + ex3.ToString());
            }
        }
    }
}
