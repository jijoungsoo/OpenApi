using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_CHEJAN_ORDER
    {
      public String curr_time;  // => "주문/체결시간"
      public String acct_num;  // => "계좌번호"
      public String order_num;  // => "주문번호"      
      public String stock_cd;  // => "종목코드, 업종코드"
      public String order_business_classification;  // => "주문업무분류"
      public String order_status;  // => "주문상태"
      public int order_qty;  // => "주문수량"
      public int order_amt;  // => "주문가격"
      public int not_contract_qty;  // => "미체결수량"
      public int contract_tot_amt;  // => "체결누계금액"
      public String ongn_order_num;  // => "원주문번호"
      public String order_gubun;  // => "주문구분(+현금내수, -현금매도…)"
      public String trade_gubun;  // => "매매구분(보통, 시장가…)"
      public int order_type;  // => "매도수구분(1:매도, 2:매수)"
      public String contract_num;  // => "체결번호"
      public int contract_amt;  // => "체결가"
      public int contract_qty;  // => "체결가"
      public int curr_amt;  // => "현재가, 체결가, 실시간종가"
      public int offered_amt;  // => "(최우선)매도호가"
      public int bid_amt;  // => "(최우선)매수호가"
      public int contract_amt_unit;  // => "단위체결가"
      public int contract_amt_qty;  // => "단위체결량"
      public int today_commission;  // => "당일매매수수료"
      public int today_tax;  // => "당일매매세금"

        public String screen_num;  // => "화면번호"
        public String terminal_num;  // => "터미널번호"
        public String credit_gubun;  // => "신용구분"
        public String loan_dt;  // => "대출일"
    }
}
