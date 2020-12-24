using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_CHEJAN_BALANCE
    {
        public String acct_num; // => "계좌번호"
        public String stock_cd; // => "종목코드, 업종코드"
        public int curr_amt; // => "현재가, 체결가, 실시간종가"
        public int possession_qty; // => "보유수량"
        public int purchase_amt; // => "매입단가"
        public int tot_purchase_amt; // => "총매입가"
        public int order_possible_qty; // => "주문가능수량"
        public int today_net_buy_qty; // => "당일순매수량"
        public int order_type; // => "매도 / 매수구분"
        public int today_sell_profit_loss; // => "당일 총 매도 손익"
        public int deposit; // => "예수금"
        public int offered_amt; // => "(최우선)매도호가"
        public int bid_amt; // => "(최우선)매수호가"
        public int yesterday_amt; //  => "기준가(어제종가)"
        public float profit_loss_rt; //  => "손익율"

        public int credit_amt; // => "신용금액"
        public float credit_interest; // => "신용이자"
        public String expiry_dt; // => "만기일"
        public int today_profit_loss_amt; // => "당일실현손익(유가)"
        public float today_profit_loss_rt; // => "당일실현손익률(유가) "
        public int credit_today_profit_loss_amt; // => "당일실현손익(신용)"
        public float credit_today_profit_loss_rt; // => "당일실현손익률(신용)"
        public int loan_qty; // => "담보대출수량"
        public String loan_dt; // => "대출일
        public String credit_gubun; // => "신용구분"


    }
}
