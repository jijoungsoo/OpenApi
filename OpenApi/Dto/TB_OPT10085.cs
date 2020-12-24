using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dto
{
    class TB_OPT10085
    {
        public String purchase_dt;
        public String acct_num;
        public String stock_cd;
        public int curr_amt;
        public int purchase_amt;
        public int tot_purchase_amt;
        public int possession_qty;
        public int today_sell_profit_loss;
        public int today_commission;
        public int today_tax;
        public String credit_gubun;
        public String loan_dt;
        public int payment_balance;
        public int sellable_qty;
        public int credit_amt;
        public int credit_interest;
        public String expiry_dt;
        public int valuation_profit_loss;
        public float earnings_rt;
        public int evaluated_amt;
        public int commission;
        public int selling_commission;
        public int buying_commission;
        public int selling_tax;
        public int will_profit_amt;
        public int not_commission_profit_loss;
        public float profit_loss_rt;
        public int order_status =1;  /*1<==보유,2<==주문접수,<==주문체결*/
    }
}
