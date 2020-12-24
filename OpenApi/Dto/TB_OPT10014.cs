using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPT10014
    {
        public String stock_cd; //종목코드
        public String stock_dt; //일자
        /* OPT10015하고 중복된다.
        public int current_amt; //현재가(종가)
        public int contrast_yesterday_symbol; //전일대비기호
        public int contrast_yesterday; //전일대비
        public float fluctuation_rt; //등락율
        public int trade_qty; //거래량
        */
        
        public int short_selling_qty;// 공매도량
        public float trade_rt;	//	매매비중
        public int short_selling_trade_amt;	//	공매도거래대금
        public int short_selling_average_amt;	//	공매도평균가
        public string prev_next;//0이면 종료 2이면 진행
    }
}