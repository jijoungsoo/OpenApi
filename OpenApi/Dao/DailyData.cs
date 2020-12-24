using Npgsql;
using OpenApi.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenApi.Dao
{
    class DailyData
    {
        public void insertOpt10001(TB_OPT10001 tmp)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_opt10001(
@stock_cd
,@face_amt
,@capital_amt
,@stock_cnt
,@credit_rt
,@settlement_mm
,@year_high_amt
,@year_low_amt
,@total_mrkt_amt
,@total_mrkt_amt_rt
,@foreigner_exhaustion_rt
,@substitute_amt
,@per
,@eps
,@roe
,@pbr
,@ev
,@bps
,@sales
,@business_profits
,@d250_high_amt
,@d250_low_amt
,@start_amt
,@high_amt
,@low_amt
,@upper_amt_lmt
,@lower_amt_lmt
,@yesterday_amt
,@expectation_contract_amt
,@expectation_contract_qty
,@d250_high_dt
,@d250_high_rt
,@d250_low_dt
,@d250_low_rt
,@curr_amt
,@contrast_symbol
,@contrast_yesterday
,@fluctuation_rt
,@trade_qty
,@trade_contrast
,@face_amt_unit
)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@stock_cd", tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@face_amt", tmp.face_amt);
                            cmd.Parameters.AddWithValue("@capital_amt", tmp.capital_amt);
                            cmd.Parameters.AddWithValue("@stock_cnt", tmp.stock_cnt);
                            cmd.Parameters.AddWithValue("@credit_rt", tmp.credit_rt);
                            cmd.Parameters.AddWithValue("@settlement_mm", tmp.settlement_mm);
                            cmd.Parameters.AddWithValue("@year_high_amt", tmp.year_high_amt);
                            cmd.Parameters.AddWithValue("@year_low_amt", tmp.year_low_amt);
                            cmd.Parameters.AddWithValue("@total_mrkt_amt", tmp.total_mrkt_amt);
                            cmd.Parameters.AddWithValue("@total_mrkt_amt_rt", tmp.total_mrkt_amt_rt);
                            cmd.Parameters.AddWithValue("@foreigner_exhaustion_rt", tmp.foreigner_exhaustion_rt);
                            cmd.Parameters.AddWithValue("@substitute_amt", tmp.substitute_amt);
                            cmd.Parameters.AddWithValue("@per", tmp.per);
                            cmd.Parameters.AddWithValue("@eps", tmp.eps);
                            cmd.Parameters.AddWithValue("@roe", tmp.roe);
                            cmd.Parameters.AddWithValue("@pbr", tmp.pbr);
                            cmd.Parameters.AddWithValue("@ev", tmp.ev);
                            cmd.Parameters.AddWithValue("@bps", tmp.bps);
                            cmd.Parameters.AddWithValue("@sales", tmp.sales);
                            cmd.Parameters.AddWithValue("@business_profits", tmp.business_profits);
                            cmd.Parameters.AddWithValue("@d250_high_amt", tmp.d250_high_amt);
                            cmd.Parameters.AddWithValue("@d250_low_amt", tmp.d250_low_amt);
                            cmd.Parameters.AddWithValue("@start_amt", tmp.start_amt);
                            cmd.Parameters.AddWithValue("@high_amt", tmp.high_amt);
                            cmd.Parameters.AddWithValue("@low_amt", tmp.low_amt);
                            cmd.Parameters.AddWithValue("@upper_amt_lmt", tmp.upper_amt_lmt);
                            cmd.Parameters.AddWithValue("@lower_amt_lmt", tmp.lower_amt_lmt);
                            cmd.Parameters.AddWithValue("@yesterday_amt", tmp.yesterday_amt);
                            cmd.Parameters.AddWithValue("@expectation_contract_amt", tmp.expectation_contract_amt);
                            cmd.Parameters.AddWithValue("@expectation_contract_qty", tmp.expectation_contract_qty);
                            cmd.Parameters.AddWithValue("@d250_high_dt", tmp.d250_high_dt);
                            cmd.Parameters.AddWithValue("@d250_high_rt", tmp.d250_high_rt);
                            cmd.Parameters.AddWithValue("@d250_low_dt", tmp.d250_low_dt);
                            cmd.Parameters.AddWithValue("@d250_low_rt", tmp.d250_low_rt);
                            cmd.Parameters.AddWithValue("@curr_amt", tmp.curr_amt);
                            cmd.Parameters.AddWithValue("@contrast_symbol", tmp.contrast_symbol);
                            cmd.Parameters.AddWithValue("@contrast_yesterday", tmp.contrast_yesterday);
                            cmd.Parameters.AddWithValue("@fluctuation_rt", tmp.fluctuation_rt);
                            cmd.Parameters.AddWithValue("@trade_qty", tmp.trade_qty);
                            cmd.Parameters.AddWithValue("@trade_contrast", tmp.trade_contrast);
                            cmd.Parameters.AddWithValue("@face_amt_unit", tmp.face_amt_unit);

                            cmd.ExecuteNonQuery();
                        }

                    


                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOpt10001]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }


        public void insertOpt10015(List<TB_OPT10015> lst)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_opt10015(
@p_stock_dt
,@p_stock_cd
,@p_current_amt
,@p_contrast_yesterday_symbol
,@p_contrast_yesterday
,@p_fluctuation_rt
,@p_trade_qty
,@p_trade_amt
,@p_before_market_trade_qty
,@p_before_market_trade_rt
,@p_market_trade_qty
,@p_market_trade_rt
,@p_after_market_trade_qty
,@p_after_market_trade_rt
,@p_between_trade_qty
,@p_prev_next
)";
                 
                        
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            TB_OPT10015 tmp = lst[i];
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_stock_dt", tmp.stock_dt);
                            cmd.Parameters.AddWithValue("@p_stock_cd", tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@p_current_amt", tmp.curr_amt);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday_symbol", tmp.contrast_yesterday_symbol);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday", tmp.contrast_yesterday);
                            cmd.Parameters.AddWithValue("@p_fluctuation_rt", tmp.fluctuation_rt);
                            cmd.Parameters.AddWithValue("@p_trade_qty", tmp.trade_qty);
                            cmd.Parameters.AddWithValue("@p_trade_amt", tmp.trade_amt);
                            cmd.Parameters.AddWithValue("@p_before_market_trade_qty", tmp.before_market_trade_qty);
                            cmd.Parameters.AddWithValue("@p_before_market_trade_rt", tmp.before_market_trade_rt);
                            cmd.Parameters.AddWithValue("@p_market_trade_qty", tmp.market_trade_qty);
                            cmd.Parameters.AddWithValue("@p_market_trade_rt", tmp.market_trade_rt);
                            cmd.Parameters.AddWithValue("@p_after_market_trade_qty", tmp.after_market_trade_qty);
                            cmd.Parameters.AddWithValue("@p_after_market_trade_rt", tmp.after_market_trade_rt);
                            cmd.Parameters.AddWithValue("@p_between_trade_qty", tmp.between_trade_qty);
                            cmd.Parameters.AddWithValue("@p_prev_next", tmp.prev_next);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                        }
                    }
                    




                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOpt10015]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

        public void insertOpt10014(List<TB_OPT10014> lst)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_opt10014(
@p_stock_dt
,@p_stock_cd
,@p_short_selling_qty
,@p_trade_rt
,@p_short_selling_trade_amt
,@p_short_selling_average_amt
,@p_prev_next
)";


                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            TB_OPT10014 tmp = lst[i];
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_stock_dt", tmp.stock_dt);
                            cmd.Parameters.AddWithValue("@p_stock_cd", tmp.stock_cd);                           
                            cmd.Parameters.AddWithValue("@p_short_selling_qty", tmp.short_selling_qty);
                            cmd.Parameters.AddWithValue("@p_trade_rt", tmp.trade_rt);
                            cmd.Parameters.AddWithValue("@p_short_selling_trade_amt", tmp.short_selling_trade_amt);
                            cmd.Parameters.AddWithValue("@p_short_selling_average_amt", tmp.short_selling_average_amt);
                            cmd.Parameters.AddWithValue("@p_prev_next", tmp.prev_next);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOpt10015]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

        public void insertOpt10059(List<TB_OPT10059> lst)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_opt10059(
@p_stock_dt
,@p_stock_cd
,@p_buy_sell
,@p_amt_amount
,@p_domestic_investor
,@p_foreign_investor
,@p_institution
,@p_financial_investment
,@p_insurance
,@p_investment_trust
,@p_etc_financial
,@p_bank
,@p_pension_fund
,@p_private_equity_fund
,@p_nation
,@p_etc_corporation
,@p_foregin_investment_in_korea
,@p_prev_next
)";


                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            TB_OPT10059 tmp = lst[i];
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_stock_dt", tmp.stock_dt);
                            cmd.Parameters.AddWithValue("@p_stock_cd", tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@p_buy_sell", tmp.buy_sell);
                            cmd.Parameters.AddWithValue("@p_amt_amount", tmp.amt_amount);
                            cmd.Parameters.AddWithValue("@p_domestic_investor", tmp.domestic_investor);
                            cmd.Parameters.AddWithValue("@p_foreign_investor", tmp.foreign_investor);
                            cmd.Parameters.AddWithValue("@p_institution", tmp.institution);
                            cmd.Parameters.AddWithValue("@p_financial_investment", tmp.financial_investment);
                            cmd.Parameters.AddWithValue("@p_insurance", tmp.insurance);
                            cmd.Parameters.AddWithValue("@p_investment_trust", tmp.investment_trust);
                            cmd.Parameters.AddWithValue("@p_etc_financial", tmp.etc_financial);
                            cmd.Parameters.AddWithValue("@p_bank", tmp.bank);
                            cmd.Parameters.AddWithValue("@p_pension_fund", tmp.pension_fund);
                            cmd.Parameters.AddWithValue("@p_private_equity_fund", tmp.private_equity_fund);
                            cmd.Parameters.AddWithValue("@p_nation", tmp.nation);
                            cmd.Parameters.AddWithValue("@p_etc_corporation", tmp.etc_corporation);
                            cmd.Parameters.AddWithValue("@p_foregin_investment_in_korea", tmp.foregin_investment_in_korea);
                            cmd.Parameters.AddWithValue("@p_prev_next", tmp.prev_next);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOpt10015]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

        public void insertOpt10085(List<TB_OPT10085> lst)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @" insert into tb_opt10085
  		(
		    purchase_dt ,
			acct_num ,
			stock_cd ,
			curr_amt ,
			purchase_amt ,
			tot_purchase_amt ,
			possession_qty ,
			today_sell_profit_loss ,
			today_commission ,
			today_tax ,
			credit_gubun ,
			loan_dt ,
			payment_balance ,
			sellable_qty ,
			credit_amt ,
			credit_interest ,
			expiry_dt ,
			valuation_profit_loss ,
			earnings_rt ,
			evaluated_amt ,
			commission ,
			selling_commission ,
			buying_commission ,
			selling_tax ,
			will_profit_amt ,
			not_commission_profit_loss ,
			profit_loss_rt ,
			order_status,
			crt_dtm
        ) 
  values ( 
	        @p_purchase_dt ,
			@p_acct_num ,
			@p_stock_cd ,
			@p_curr_amt ,
			@p_purchase_amt ,
			@p_tot_purchase_amt ,
			@p_possession_qty ,
			@p_today_sell_profit_loss ,
			@p_today_commission ,
			@p_today_tax ,
			@p_credit_gubun ,
			@p_loan_dt ,
			@p_payment_balance ,
			@p_sellable_qty ,
			@p_credit_amt ,
			@p_credit_interest ,
			@p_expiry_dt ,
			@p_valuation_profit_loss ,
			@p_earnings_rt ,
			@p_evaluated_amt ,
			@p_commission ,
			@p_selling_commission ,
			@p_buying_commission ,
			@p_selling_tax ,
			@p_will_profit_amt ,
			@p_not_commission_profit_loss ,
			@p_profit_loss_rt ,
			@p_order_status,
	        now() 
	)";


                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            TB_OPT10085 tmp = lst[i];
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_purchase_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.purchase_dt);
                            cmd.Parameters.AddWithValue("@p_acct_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.acct_num);
                            cmd.Parameters.AddWithValue("@p_stock_cd", NpgsqlTypes.NpgsqlDbType.Text, tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@p_curr_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.curr_amt);
                            cmd.Parameters.AddWithValue("@p_purchase_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.purchase_amt);
                            cmd.Parameters.AddWithValue("@p_tot_purchase_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_purchase_amt);
                            cmd.Parameters.AddWithValue("@p_possession_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.possession_qty);
                            cmd.Parameters.AddWithValue("@p_today_sell_profit_loss", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_sell_profit_loss);
                            cmd.Parameters.AddWithValue("@p_today_commission", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_commission);
                            cmd.Parameters.AddWithValue("@p_today_tax", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_tax);
                            cmd.Parameters.AddWithValue("@p_credit_gubun", NpgsqlTypes.NpgsqlDbType.Text, tmp.credit_gubun);
                            cmd.Parameters.AddWithValue("@p_loan_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.loan_dt);
                            cmd.Parameters.AddWithValue("@p_payment_balance", NpgsqlTypes.NpgsqlDbType.Integer, tmp.payment_balance);
                            cmd.Parameters.AddWithValue("@p_sellable_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.sellable_qty);
                            cmd.Parameters.AddWithValue("@p_credit_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.credit_amt);
                            cmd.Parameters.AddWithValue("@p_credit_interest", NpgsqlTypes.NpgsqlDbType.Integer, tmp.credit_interest);
                            cmd.Parameters.AddWithValue("@p_expiry_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.expiry_dt);
                            cmd.Parameters.AddWithValue("@p_valuation_profit_loss", NpgsqlTypes.NpgsqlDbType.Integer, tmp.valuation_profit_loss);

                            cmd.Parameters.AddWithValue("@p_earnings_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.earnings_rt);
                            cmd.Parameters.AddWithValue("@p_evaluated_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.evaluated_amt);
                            cmd.Parameters.AddWithValue("@p_commission", NpgsqlTypes.NpgsqlDbType.Integer, tmp.commission);
                            cmd.Parameters.AddWithValue("@p_selling_commission", NpgsqlTypes.NpgsqlDbType.Integer, tmp.selling_commission);
                            cmd.Parameters.AddWithValue("@p_buying_commission", NpgsqlTypes.NpgsqlDbType.Integer, tmp.buying_commission);
                            cmd.Parameters.AddWithValue("@p_selling_tax", NpgsqlTypes.NpgsqlDbType.Integer, tmp.selling_tax);
                            cmd.Parameters.AddWithValue("@p_will_profit_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.will_profit_amt);
                            cmd.Parameters.AddWithValue("@p_not_commission_profit_loss", NpgsqlTypes.NpgsqlDbType.Integer, tmp.not_commission_profit_loss);
                            cmd.Parameters.AddWithValue("@p_profit_loss_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.profit_loss_rt);
                            cmd.Parameters.AddWithValue("@p_order_status", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_status);
                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOpt10085]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

        public void insertOptkwfid(List<TB_OPTKWFID> lst)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_optkwfid (
@p_stock_cd
,@p_curr_amt
,@p_yesterday_amt
,@p_contrast_yesterday
,@p_contrast_yesterday_symbol
,@p_fluctuation_rt
,@p_trade_qty
,@p_trade_amt
,@p_contract_qty
,@p_contract_strength
,@p_yesterday_contrast_trade_rt
,@p_offered_amt
,@p_bid_amt
,@p_offered_amt_one
,@p_offered_amt_two
,@p_offered_amt_three
,@p_offered_amt_four
,@p_offered_amt_five
,@p_bid_amt_one
,@p_bid_amt_two
,@p_bid_amt_three
,@p_bid_amt_four
,@p_bid_amt_five
,@p_upper_amt_lmt
,@p_lower_amt_lmt
,@p_start_amt
,@p_high_amt
,@p_low_amt
,@p_clsg_amt
,@p_contract_time
,@p_expectation_contract_amt
,@p_expectation_contract_qty
,@p_capital_amt
,@p_face_amt
,@p_total_mrkt_amt
,@p_stock_cnt
,@p_hoga_time
,@p_stock_dt
,@p_fst_offered_balance
,@p_fst_bid_balance
,@p_fst_offered_qty
,@p_fst_bid_qty
,@p_tot_offered_balance
,@p_tot_bid_balance
,@p_tot_offered_qty
,@p_tot_bid_qty
,@p_parity_rt
,@p_gearing
,@p_break_even_point
,@p_elw_strike_amt
,@p_conversion_rt
,@p_elw_expiry_dt
,@p_open_interest
,@p_contrast_open_interest
,@p_theorist_amt
,@p_implied_volatility
,@p_delta
,@p_gamma
,@p_theta
,@p_vega
,@p_lo
)";


                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        for (int i = 0; i < lst.Count(); i++)
                        {

                            TB_OPTKWFID tmp = lst[i];
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_stock_cd", NpgsqlTypes.NpgsqlDbType.Text, tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@p_curr_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.curr_amt);
                            cmd.Parameters.AddWithValue("@p_yesterday_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.yesterday_amt);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contrast_yesterday);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday_symbol", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contrast_yesterday_symbol);
                            cmd.Parameters.AddWithValue("@p_fluctuation_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.fluctuation_rt);
                            cmd.Parameters.AddWithValue("@p_trade_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.trade_qty);
                            cmd.Parameters.AddWithValue("@p_trade_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.trade_amt);
                            cmd.Parameters.AddWithValue("@p_contract_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_qty);
                            cmd.Parameters.AddWithValue("@p_contract_strength", NpgsqlTypes.NpgsqlDbType.Real, tmp.contract_strength);
                            cmd.Parameters.AddWithValue("@p_yesterday_contrast_trade_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.yesterday_contrast_trade_rt);
                            cmd.Parameters.AddWithValue("@p_offered_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt);
                            cmd.Parameters.AddWithValue("@p_bid_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt);
                            cmd.Parameters.AddWithValue("@p_offered_amt_one", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt_one);
                            cmd.Parameters.AddWithValue("@p_offered_amt_two", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt_two);
                            cmd.Parameters.AddWithValue("@p_offered_amt_three", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt_three);
                            cmd.Parameters.AddWithValue("@p_offered_amt_four", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt_four);
                            cmd.Parameters.AddWithValue("@p_offered_amt_five", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt_five);
                            cmd.Parameters.AddWithValue("@p_bid_amt_one", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt_one);
                            cmd.Parameters.AddWithValue("@p_bid_amt_two", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt_two);
                            cmd.Parameters.AddWithValue("@p_bid_amt_three", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt_three);
                            cmd.Parameters.AddWithValue("@p_bid_amt_four", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt_four);
                            cmd.Parameters.AddWithValue("@p_bid_amt_five", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt_five);
                            cmd.Parameters.AddWithValue("@p_upper_amt_lmt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.upper_amt_lmt);
                            cmd.Parameters.AddWithValue("@p_lower_amt_lmt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.lower_amt_lmt);
                            cmd.Parameters.AddWithValue("@p_start_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.start_amt);
                            cmd.Parameters.AddWithValue("@p_high_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.high_amt);
                            cmd.Parameters.AddWithValue("@p_low_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.low_amt);
                            cmd.Parameters.AddWithValue("@p_clsg_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.clsg_amt);
                            cmd.Parameters.AddWithValue("@p_contract_time", NpgsqlTypes.NpgsqlDbType.Text, tmp.contract_time);
                            cmd.Parameters.AddWithValue("@p_expectation_contract_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.expectation_contract_amt);
                            cmd.Parameters.AddWithValue("@p_expectation_contract_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.expectation_contract_qty);
                            cmd.Parameters.AddWithValue("@p_capital_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.capital_amt);
                            cmd.Parameters.AddWithValue("@p_face_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.face_amt);
                            cmd.Parameters.AddWithValue("@p_total_mrkt_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.total_mrkt_amt);
                            cmd.Parameters.AddWithValue("@p_stock_cnt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.stock_cnt);
                            cmd.Parameters.AddWithValue("@p_hoga_time", NpgsqlTypes.NpgsqlDbType.Text, tmp.hoga_time);
                            cmd.Parameters.AddWithValue("@p_stock_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.stock_dt);
                            cmd.Parameters.AddWithValue("@p_fst_offered_balance", NpgsqlTypes.NpgsqlDbType.Integer, tmp.fst_offered_balance);
                            cmd.Parameters.AddWithValue("@p_fst_bid_balance", NpgsqlTypes.NpgsqlDbType.Integer, tmp.fst_bid_balance);
                            cmd.Parameters.AddWithValue("@p_fst_offered_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.fst_offered_qty);
                            cmd.Parameters.AddWithValue("@p_fst_bid_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.fst_bid_qty);
                            cmd.Parameters.AddWithValue("@p_tot_offered_balance", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_offered_balance);
                            cmd.Parameters.AddWithValue("@p_tot_bid_balance", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_bid_balance);
                            cmd.Parameters.AddWithValue("@p_tot_offered_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_offered_qty);
                            cmd.Parameters.AddWithValue("@p_tot_bid_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_bid_qty);
                            cmd.Parameters.AddWithValue("@p_parity_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.parity_rt);
                            cmd.Parameters.AddWithValue("@p_gearing", NpgsqlTypes.NpgsqlDbType.Real, tmp.gearing);
                            cmd.Parameters.AddWithValue("@p_break_even_point", NpgsqlTypes.NpgsqlDbType.Real, tmp.break_even_point);
                            cmd.Parameters.AddWithValue("@p_elw_strike_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.elw_strike_amt);
                            cmd.Parameters.AddWithValue("@p_conversion_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.conversion_rt);
                            cmd.Parameters.AddWithValue("@p_elw_expiry_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.elw_expiry_dt);
                            cmd.Parameters.AddWithValue("@p_open_interest", NpgsqlTypes.NpgsqlDbType.Integer, tmp.open_interest);
                            cmd.Parameters.AddWithValue("@p_contrast_open_interest", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contrast_open_interest);
                            cmd.Parameters.AddWithValue("@p_theorist_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.theorist_amt);
                            cmd.Parameters.AddWithValue("@p_implied_volatility", NpgsqlTypes.NpgsqlDbType.Integer, tmp.implied_volatility);
                            cmd.Parameters.AddWithValue("@p_delta", NpgsqlTypes.NpgsqlDbType.Integer, tmp.delta);
                            cmd.Parameters.AddWithValue("@p_gamma", NpgsqlTypes.NpgsqlDbType.Integer, tmp.gamma);
                            cmd.Parameters.AddWithValue("@p_theta", NpgsqlTypes.NpgsqlDbType.Integer, tmp.theta);
                            cmd.Parameters.AddWithValue("@p_vega", NpgsqlTypes.NpgsqlDbType.Integer, tmp.vega);
                            cmd.Parameters.AddWithValue("@p_lo", NpgsqlTypes.NpgsqlDbType.Integer, tmp.lo);

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertOptkwfid]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

    }
}
