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

namespace OpenApi.ReceiveRealData
{

    /// <summary>
    ///  [ REAL10001 : 주식시세 ]
    ///</summary>
    public class REAL10001 : ReceiveRealData
    {
        public REAL10001(){
            FileLog.PrintF("REAL10001");           
        }
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /*
            
            	[10] = 현재가              //(0)
	            [11] = 전일대비            //(1)
	            [12] = 등락율              //(2)
	            [27] = (최우선)매도호가    //(3)
	            [28] = (최우선)매수호가    //(4)
	            [13] = 누적거래량          //(5)
	            [14] = 누적거래대금        //(6)    
	            [16] = 시가                //(7)
	            [17] = 고가                //(8)
	            [18] = 저가                //(9)
	            [25] = 전일대비기호        //(10)
	            [26] = 전일거래량대비(계약,주)   //(11)
	            [29] = 거래대금증감              //(12)
	            [30] = 전일거래량대비(비율)      //(13)
	            [31] = 거래회전율                //(14)
	            [32] = 거래비용                  //(15)
	            [311] = 시가총액(억)             //(16)
	            [567] = 상한가발생시간           //(17) 
	            [568] = 하한가발생시간           //(18)
                
            */
            
            FileLog.PrintF(String.Format("현재가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim()));
            FileLog.PrintF(String.Format("전일대비 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim()));
            FileLog.PrintF(String.Format("등락율 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim()));
            FileLog.PrintF(String.Format("매도호가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim()));
            FileLog.PrintF(String.Format("매수호가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim()));
            FileLog.PrintF(String.Format("누적거래량 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim()));
            FileLog.PrintF(String.Format("누적거래대금 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim()));
            FileLog.PrintF(String.Format("시가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim()));
            FileLog.PrintF(String.Format("고가 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim()));
            FileLog.PrintF(String.Format("저가 : {0}  ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim()));
            FileLog.PrintF(String.Format("전일대비기호 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim()));
            FileLog.PrintF(String.Format("전일거래량대비_계약_주 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim()));
            FileLog.PrintF(String.Format("거래대금증감 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim()));
            FileLog.PrintF(String.Format("전일거래량대비_비율 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim()));
            FileLog.PrintF(String.Format("거래회전율 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim()));
            FileLog.PrintF(String.Format("거래비용 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim()));
            FileLog.PrintF(String.Format("시가총액_억 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim()));
            FileLog.PrintF(String.Format("상한가발생시간 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim()));
            FileLog.PrintF(String.Format("하한가발생시간 : {0} ==>", axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim()));
            FileLog.PrintF(String.Format("종목코드 : {0} ==>", e.sRealKey.ToString().Trim()));
            FileLog.PrintF(String.Format("RealName : {0} ==>", e.sRealType.ToString().Trim()));
            FileLog.PrintF(String.Format("sRealData : {0} ==>", e.sRealData.ToString().Trim()));
            
            TB_REAL10001 real10001_data = new TB_REAL10001();
            //String 현재시간 = DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff");
            real10001_data.현재시간 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            real10001_data.현재가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim());
            real10001_data.전일대비 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim());
            real10001_data.등락율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim());
            real10001_data.매도호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim());
            real10001_data.매수호가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim());
            real10001_data.누적거래량 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim());
            real10001_data.누적거래대금 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim());
            real10001_data.시가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim());
            real10001_data.고가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim());
            real10001_data.저가 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim());
            real10001_data.전일대비기호 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim());
            real10001_data.전일거래량대비_계약_주 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim());
            real10001_data.거래대금증감 = decimal.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim());
            real10001_data.전일거래량대비_비율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim());
            real10001_data.거래회전율 = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim());
            real10001_data.거래비용 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim());
            real10001_data.시가총액_억 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim());
            real10001_data.상한가발생시간 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim());
            real10001_data.하한가발생시간 = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim());
            real10001_data.종목코드 = e.sRealKey.ToString().Trim();
            real10001_data.RealName = e.sRealType.ToString().Trim();
            
        //        SendDirectFile(real10001_data);
                //SendDirectDb(real10001_data);
        }
        
        private void SendDirectDb(TB_REAL10001 real10001_data)
        {
            using (MySqlConnection conn = new MySqlConnection(Config.GetDbConnStr()))
            {
                String dayTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                
                string sql = @"INSERT into  realtime_prices (
stock_date
,stock_code
,current_price
,contrast_yesterday
,fluctuation_rate
,offered_price
,bid_price
,accumulated_trade_quantity
,accumulated_trade_price
,start_price
,high_price
,low_price
,contrast_yesterday_symbol
,yesterday_contrast_trade_quantity
,trade_amount_variation
,yesterday_contrast_trade_rate
,trade_turnover_ratio
,trade_cost
,total_market_price
,upper_price_limit_time
,lower_price_limit_time
,created_at
,updated_at
)
VALUES
(
@현재시간
,@종목코드
,@현재가
,@전일대비
,@등락율
,@매도호가
,@매수호가
,@누적거래량
,@누적거래대금
,@시가
,@고가
,@저가
,@전일대비기호
,@전일거래량대비_계약_주
,@거래대금증감
,@전일거래량대비_비율
,@거래회전율
,@거래비용
,@시가총액_억
,@상한가발생시간
,@하한가발생시간
,current_timestamp
,current_timestamp
);
";               
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@현재시간", real10001_data.현재시간);
                cmd.Parameters.AddWithValue("@종목코드", real10001_data.종목코드);
                cmd.Parameters.AddWithValue("@현재가", real10001_data.현재가);
                cmd.Parameters.AddWithValue("@전일대비", real10001_data.전일대비);
                cmd.Parameters.AddWithValue("@등락율", real10001_data.등락율);
                cmd.Parameters.AddWithValue("@매도호가", real10001_data.매도호가);
                cmd.Parameters.AddWithValue("@매수호가", real10001_data.매수호가);
                cmd.Parameters.AddWithValue("@누적거래량", real10001_data.누적거래량);
                cmd.Parameters.AddWithValue("@누적거래대금", real10001_data.누적거래대금);
                cmd.Parameters.AddWithValue("@시가", real10001_data.시가);
                cmd.Parameters.AddWithValue("@고가", real10001_data.고가);
                cmd.Parameters.AddWithValue("@저가", real10001_data.저가);
                cmd.Parameters.AddWithValue("@전일대비기호", real10001_data.전일대비기호);
                cmd.Parameters.AddWithValue("@전일거래량대비_계약_주", real10001_data.전일거래량대비_계약_주);
                cmd.Parameters.AddWithValue("@거래대금증감", real10001_data.거래대금증감);
                cmd.Parameters.AddWithValue("@전일거래량대비_비율", real10001_data.전일거래량대비_비율);
                cmd.Parameters.AddWithValue("@거래회전율", real10001_data.거래회전율);
                cmd.Parameters.AddWithValue("@거래비용", real10001_data.거래비용);
                cmd.Parameters.AddWithValue("@시가총액_억", real10001_data.시가총액_억);
                cmd.Parameters.AddWithValue("@상한가발생시간", real10001_data.상한가발생시간);
                cmd.Parameters.AddWithValue("@하한가발생시간", real10001_data.하한가발생시간);             
                cmd.ExecuteNonQuery();
            }
        }
    }
}
