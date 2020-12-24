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

namespace OpenApi.ReceiveRealData
{
    /// <summary>
    ///  [ REAL10002 : 주식체결 ]
    ///</summary>
    public class REAL10002 : ReceiveRealData
    {
        public REAL10002(){
          //  FileLog.PrintF("REAL10002");
        }
        public override void ReceivedData(AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI, AxKHOpenAPILib._DKHOpenAPIEvents_OnReceiveRealDataEvent e)
        {
            /*
            
                [20] = 체결시간            //(0)
            	[10] = 현재가              //(1)
	            [11] = 전일대비            //(2)
	            [12] = 등락율              //(3)
	            [27] = (최우선)매도호가    //(4)
	            [28] = (최우선)매수호가    //(5)
                [15] = 거래량              //(6)
	            [13] = 누적거래량          //(7)
	            [14] = 누적거래대금        //(8)    
	            [16] = 시가                //(9)
	            [17] = 고가                //(10)
	            [18] = 저가                //(11)
	            [25] = 전일대비기호        //(12)
	            [26] = 전일거래량대비(계약,주)   //(13)
	            [29] = 거래대금증감              //(14)
	            [30] = 전일거래량대비(비율)      //(15)
	            [31] = 거래회전율                //(16)
	            [32] = 거래비용                  //(17)
                [228] = 체결강도                 //(18)
	            [311] = 시가총액(억)             //(19)
                [290] = 장구분                   //(20)
                [691] = KO접근도                 //(21)
	            [567] = 상한가발생시간           //(22) 
	            [568] = 하한가발생시간           //(23)
//[2020-04-02 10:18:30]종목코드 : 005930 | RealType : 주식체결 | 
RealData : 101829	-45600	-200	-0.44	-45600	-45550	+240	7648579	351335	+46200	+46250	-45350	5	-19610953	-931147601650	-28.06	0.13	129	78.06	2722221	2	0	-27.39	000000	000000	10825	090018	090526	090159	4001191	3123474	-43.84	10171	23587	+10944	 0	+240	+240	4560	3648	45935	252                
[2020-04-02 10:40:28]체결시간=> 104027 
[2020-04-02 10:40:28]현재가=> 45800 
[2020-04-02 10:40:28]전일대비=> 0 
[2020-04-02 10:40:28]등락율=> 0.00 
[2020-04-02 10:40:28](최우선)매도호가=> 45800 
[2020-04-02 10:40:28](최우선)매수호가=> -45750 
[2020-04-02 10:40:28]거래량=> +1 
[2020-04-02 10:40:28]누적거래량=> 8718082 
[2020-04-02 10:40:28]누적거래대금=> 400224 
[2020-04-02 10:40:28]시가=> +46200 
[2020-04-02 10:40:28]고가=> +46250 
[2020-04-02 10:40:28]저가=>-45350 
[2020-04-02 10:40:28]전일대비기호=> 3 
[2020-04-02 10:40:28]전일거래량대비_계약_주=> -18541450 
[2020-04-02 10:40:28]거래대금증감=> -882258926600 
[2020-04-02 10:40:28]전일거래량대비_비율=> -31.98 
[2020-04-02 10:40:28]거래회전율=> 0.15 
[2020-04-02 10:40:28]거래비용=> 129 
[2020-04-02 10:40:28]체결강도=> 82.85 
[2020-04-02 10:40:28]시가총액_억=> 2734160 
[2020-04-02 10:40:28]장구분=> 2 
[2020-04-02 10:40:28]KO접근도=> 0 
[2020-04-02 10:40:28]상한가발생시간=> 000000 
[2020-04-02 10:40:28]하한가발생시간=> 000000 
[2020-04-02 10:40:28]종목코드=> 005930 
[2020-04-02 10:40:28]RealName=> 주식체결 
            */
            /*
            FileLog.PrintF(String.Format("체결시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 20).Trim()));   //[0]
            FileLog.PrintF(String.Format("현재가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim()));     //[1]
            FileLog.PrintF(String.Format("전일대비=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim()));   //[2]
            FileLog.PrintF(String.Format("등락율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim()));     //[3]
            FileLog.PrintF(String.Format("(최우선)매도호가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim()));   //[4]
            FileLog.PrintF(String.Format("(최우선)매수호가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim()));   //[5]
            FileLog.PrintF(String.Format("거래량=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 15).Trim()));      //[6]
            FileLog.PrintF(String.Format("누적거래량=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim()));   //[7]
            FileLog.PrintF(String.Format("누적거래대금=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim())); //[8]
            FileLog.PrintF(String.Format("시가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim()));          //[9]
            FileLog.PrintF(String.Format("고가=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim()));          //[10]
            FileLog.PrintF(String.Format("저가=>{0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim()));         //[11]
            FileLog.PrintF(String.Format("전일대비기호=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim()));  //[12]
            FileLog.PrintF(String.Format("전일거래량대비_계약_주=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim()));  //[13]
            FileLog.PrintF(String.Format("거래대금증감=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim()));             //[14]
            FileLog.PrintF(String.Format("전일거래량대비_비율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim()));      //[15]
            FileLog.PrintF(String.Format("거래회전율=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim()));               //[16]
            FileLog.PrintF(String.Format("거래비용=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim()));               //[17]
            FileLog.PrintF(String.Format("체결강도=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 228).Trim()));               //[18]
            FileLog.PrintF(String.Format("시가총액_억=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim()));               //[19]
            FileLog.PrintF(String.Format("장구분=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 290).Trim()));               //[20]
            FileLog.PrintF(String.Format("KO접근도=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 691).Trim()));               //[21]
            FileLog.PrintF(String.Format("상한가발생시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim()));         //[22]
            FileLog.PrintF(String.Format("하한가발생시간=> {0} ", axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim()));         //[23]
            FileLog.PrintF(String.Format("종목코드=> {0} ", e.sRealKey.ToString().Trim()));
            FileLog.PrintF(String.Format("RealName=> {0} ", e.sRealType.ToString().Trim()));
            FileLog.PrintF(String.Format("sRealData=> {0} ", e.sRealData.ToString().Trim()));
            */
            try
            {
                String 현재일자 = DateTime.Now.ToString("yyyyMMdd");
                String 체결시간TMP = axKHOpenAPI.GetCommRealData(e.sRealType, 20).Trim();           //[0]
                                                                                                //체결시간이 6자리이므로 HHMMSS ==> HH:MM:SS로 바꿔야한다.
                TB_REALTIME_CONTRACT real10002_data = new TB_REALTIME_CONTRACT();
                //String 현재시간 = DateTime.Now.ToString("yyyyMMdd HH:mm:ss:fff");
                real10002_data.real_name = e.sRealType.ToString().Trim();
                real10002_data.stock_cd = e.sRealKey.ToString().Trim();
                real10002_data.stock_dt = 현재일자;
                real10002_data.contract_time = 체결시간TMP;
                real10002_data.curr_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 10).Trim());         //[1]
                real10002_data.contrast_yesterday = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 11).Trim());         //[2]
                real10002_data.fluctuation_rt = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 12).Trim());         //[3]
                real10002_data.offered_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 27).Trim());         //[4]
                real10002_data.bid_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 28).Trim());         //[5]
                real10002_data.trade_qty = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 15).Trim());         //[6]
                real10002_data.accumulated_trade_qty = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 13).Trim());  //[7]
                real10002_data.accumulated_trade_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 14).Trim());  //[8]
                real10002_data.start_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 16).Trim());  //[9]
                real10002_data.high_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 17).Trim());  //[10]
                real10002_data.low_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 18).Trim());  //[11]
                real10002_data.contrast_yesterday_symbol = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 25).Trim());  //[12]
                real10002_data.yesterday_contrast_trade_qty = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 26).Trim()); //[13]
                real10002_data.trade_amount_variation = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 29).Trim());  //[14]
                real10002_data.yesterday_contrast_trade_rt = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 30).Trim());  //[15]
                real10002_data.trade_turnover_ratio = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 31).Trim());   //[16]
                real10002_data.trade_cost = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 32).Trim());   //[17]
                real10002_data.contract_strength = float.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 228).Trim());   //[18]
                real10002_data.total_market_amt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 311).Trim()); //[19]
                real10002_data.market_gubun = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 290).Trim()); //[20]
                real10002_data.ko_accessibility_rt = Int32.Parse(axKHOpenAPI.GetCommRealData(e.sRealType, 691).Trim()); //[21]
                real10002_data.upper_amt_limit_time = axKHOpenAPI.GetCommRealData(e.sRealType, 567).Trim(); //[22]
                real10002_data.lower_amt_limit_time = axKHOpenAPI.GetCommRealData(e.sRealType, 568).Trim(); //[23]


                //MyStock.getClass1Instance().UpdateStockList(real10002_data);

                RealTimeData realTimeData = new RealTimeData();
                realTimeData.insertRealtimeContract(real10002_data);
            } catch(Exception ex)
            {
                FileLog.PrintF("[REAL10002]Exception ex=" + ex.Message);
                //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                //		Message	"42601: 구문 오류, 입력 끝부분"	string
            }

        }
    }
}
