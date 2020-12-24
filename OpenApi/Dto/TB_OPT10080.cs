using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPT10080  /* 5분만 보자.. 그럼 하루에 76개 이다. 컬럼으로 두자.*/
    {
        public String stock_cd; //종목코드
        public String stock_dt; //일자
        public int current_amt; //현재가(종가)
        public int contrast_yesterday_symbol; //전일대비기호
        public int contrast_yesterday; //전일대비
        public float fluctuation_rt; //등락율
        public int trade_qty; //거래량
        public int trade_amt; //거래대금
        public int before_market_trade_qty;// 장전거래량
        public float before_market_trade_rt;	//	장전거래비중
        public int market_trade_qty;	//	장중거래량
        public float market_trade_rt;	//	장중거래비중
        public int after_market_trade_qty;  //	장후거래량
        public float after_market_trade_rt;  //	장후거래비중
        public int between_trade_qty;  //	기간중거래량
        public string prev_next;//0이면 종료 2이면 진행

    }
}