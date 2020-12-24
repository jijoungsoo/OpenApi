using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_REALTIME_CONTRACT  /* 주식체결.*/
    {
        public String stock_cd; //종목코드
        public String real_name; /*주식체결*/
        public String stock_dt; //일자
        public String contract_time; //현재시간 HHMMDD
        public int curr_amt; //현재가(종가)
        public int contrast_yesterday_symbol; //전일대비기호
        public int contrast_yesterday; //전일대비
        public float fluctuation_rt; //등락율
        public int offered_amt; //매도호가
        public int bid_amt; //매수호가
        public int trade_qty;// 거래량
        public int accumulated_trade_qty;	//	누적거래량
        public int accumulated_trade_amt;	//	누적거래대금
        public int start_amt;	//	시가
        public int high_amt;  //	고가
        public int low_amt;  //	저가
        public int between_trade_qty;  //	기간중거래량
        public int yesterday_contrast_trade_qty;//전일거래량대비(계약,주)

        public float trade_amount_variation;  //	거래대금증감  (숫치가 커서 int가 안먹음)
        public float yesterday_contrast_trade_rt;  //	전일거래량대비(비율)
        public float trade_turnover_ratio;  //	거래회전율
        public int trade_cost;  //	거래비용
        public float contract_strength;  //	체결강도
        public int total_market_amt;  //	시가총액(억)

        public int market_gubun;  //	장구분
        public int ko_accessibility_rt;  //	KO접근도
        public String upper_amt_limit_time;  //	상한가발생시간  hhmmdd
        public String lower_amt_limit_time;  //	하한가발생시간  hhmmdd

    }
}