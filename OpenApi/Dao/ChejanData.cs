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
    class ChejanData
    {
       
        public void insertChejanBalance(TB_CHEJAN_BALANCE tmp)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @" insert into tb_chejan_balance
  		(
			acct_num ,
			stock_cd ,
			curr_amt ,
			possession_qty ,
			purchase_amt ,
			tot_purchase_amt ,
			order_possible_qty ,
			today_net_buy_qty ,
			order_type ,
			today_sell_profit_loss ,
			deposit ,
			offered_amt ,
			bid_amt ,
			yesterday_amt ,
			profit_loss_rt ,
			credit_amt ,
			credit_interest ,
			today_profit_loss_amt ,
			today_profit_loss_rt ,
			credit_today_profit_loss_amt ,
			credit_today_profit_loss_rt ,
			loan_qty ,
			loan_dt ,
			credit_gubun ,
			crt_dtm
        ) 
  values ( 
	@p_acct_num ,
    @p_stock_cd ,
    @p_curr_amt ,
    @p_possession_qty ,
    @p_purchase_amt ,
    @p_tot_purchase_amt ,
    @p_order_possible_qty ,
    @p_today_net_buy_qty ,
    @p_order_type ,
    @p_today_sell_profit_loss ,
    @p_deposit ,
    @p_offered_amt ,
    @p_bid_amt ,
    @p_yesterday_amt ,
    @p_profit_loss_rt ,
    @p_credit_amt ,
    @p_credit_interest ,
    @p_today_profit_loss_amt ,
    @p_today_profit_loss_rt ,
    @p_credit_today_profit_loss_amt ,
    @p_credit_today_profit_loss_rt ,
    @p_loan_qty ,
    @p_loan_dt ,
    @p_credit_gubun ,
	now() 
	)"; 
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_acct_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.acct_num);
                        cmd.Parameters.AddWithValue("@p_stock_cd", NpgsqlTypes.NpgsqlDbType.Text, tmp.stock_cd);
                        cmd.Parameters.AddWithValue("@p_curr_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.curr_amt);
                        cmd.Parameters.AddWithValue("@p_possession_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.curr_amt);
                        cmd.Parameters.AddWithValue("@p_purchase_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.purchase_amt);

                        cmd.Parameters.AddWithValue("@p_tot_purchase_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.tot_purchase_amt);
                        cmd.Parameters.AddWithValue("@p_order_possible_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_possible_qty);
                        cmd.Parameters.AddWithValue("@p_today_net_buy_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_net_buy_qty);
                        cmd.Parameters.AddWithValue("@p_order_type", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_type);
                        cmd.Parameters.AddWithValue("@p_today_sell_profit_loss", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_sell_profit_loss);

                        cmd.Parameters.AddWithValue("@p_deposit", NpgsqlTypes.NpgsqlDbType.Integer, tmp.deposit);
                        cmd.Parameters.AddWithValue("@p_offered_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt);
                        cmd.Parameters.AddWithValue("@p_bid_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt);
                        cmd.Parameters.AddWithValue("@p_yesterday_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.yesterday_amt);
                        cmd.Parameters.AddWithValue("@p_profit_loss_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.profit_loss_rt);


                        cmd.Parameters.AddWithValue("@p_credit_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.credit_amt);
                        cmd.Parameters.AddWithValue("@p_credit_interest", NpgsqlTypes.NpgsqlDbType.Real, tmp.credit_interest);
                        cmd.Parameters.AddWithValue("@p_today_profit_loss_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_profit_loss_amt);
                        cmd.Parameters.AddWithValue("@p_today_profit_loss_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.today_profit_loss_rt);
                        cmd.Parameters.AddWithValue("@p_credit_today_profit_loss_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.credit_today_profit_loss_amt);
                        cmd.Parameters.AddWithValue("@p_credit_today_profit_loss_rt", NpgsqlTypes.NpgsqlDbType.Real, tmp.credit_today_profit_loss_rt);
                        cmd.Parameters.AddWithValue("@p_loan_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.loan_qty);
                        cmd.Parameters.AddWithValue("@p_loan_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.loan_dt);
                        cmd.Parameters.AddWithValue("@p_credit_gubun", NpgsqlTypes.NpgsqlDbType.Text, tmp.credit_gubun);
                        


                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertChejanBalance]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }
        }

        public void insertChejanOrder(TB_CHEJAN_ORDER tmp)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"insert into tb_chejan_order
  		(
			curr_time ,
			order_num ,
			acct_num ,
			stock_cd ,
			order_business_classification ,
			order_status ,
			order_qty ,
			order_amt ,
			not_contract_qty ,
			contract_tot_amt ,
			ongn_order_num ,
			order_gubun ,
			trade_gubun ,
			order_type ,
			contract_num ,
			contract_amt ,
			contract_qty ,
			curr_amt ,
			offered_amt ,
			bid_amt ,
			contract_amt_unit ,
			contract_amt_qty ,
			today_commission ,
			screen_num ,
			terminal_num ,
			credit_gubun ,
			loan_dt ,
			crt_dtm
        ) 
  values ( 
	@p_curr_time ,
    @p_order_num ,
    @p_acct_num ,
    @p_stock_cd ,
    @p_order_business_classification ,
    @p_order_status ,
    @p_order_qty ,
    @p_order_amt ,
    @p_not_contract_qty ,
    @p_contract_tot_amt ,
    @p_ongn_order_num ,
    @p_order_gubun ,
    @p_trade_gubun ,
    @p_order_type ,
    @p_contract_num ,
    @p_contract_amt ,
    @p_contract_qty ,
    @p_curr_amt ,
    @p_offered_amt ,
    @p_bid_amt ,
    @p_contract_amt_unit ,
    @p_contract_amt_qty ,
    @p_today_commission ,
    @p_screen_num ,
    @p_terminal_num ,
    @p_credit_gubun ,
    @p_loan_dt ,
	now() 
	)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_curr_time", NpgsqlTypes.NpgsqlDbType.Text, tmp.curr_time);
                        cmd.Parameters.AddWithValue("@p_order_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.order_num);
                        cmd.Parameters.AddWithValue("@p_acct_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.acct_num);
                        cmd.Parameters.AddWithValue("@p_stock_cd", NpgsqlTypes.NpgsqlDbType.Text, tmp.stock_cd);
                        cmd.Parameters.AddWithValue("@p_order_business_classification", NpgsqlTypes.NpgsqlDbType.Text, tmp.order_business_classification);

                        cmd.Parameters.AddWithValue("@p_order_status", NpgsqlTypes.NpgsqlDbType.Text, tmp.order_status);
                        cmd.Parameters.AddWithValue("@p_order_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_qty);
                        cmd.Parameters.AddWithValue("@p_order_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_amt);
                        cmd.Parameters.AddWithValue("@p_not_contract_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.not_contract_qty);
                        cmd.Parameters.AddWithValue("@p_contract_tot_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_tot_amt);

                        cmd.Parameters.AddWithValue("@p_ongn_order_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.ongn_order_num);
                        cmd.Parameters.AddWithValue("@p_order_gubun", NpgsqlTypes.NpgsqlDbType.Text, tmp.order_gubun);
                        cmd.Parameters.AddWithValue("@p_trade_gubun", NpgsqlTypes.NpgsqlDbType.Text, tmp.trade_gubun);
                        cmd.Parameters.AddWithValue("@p_order_type", NpgsqlTypes.NpgsqlDbType.Integer, tmp.order_type);
                        cmd.Parameters.AddWithValue("@p_contract_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.contract_num);


                        cmd.Parameters.AddWithValue("@p_contract_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_amt);
                        cmd.Parameters.AddWithValue("@p_contract_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_qty);
                        cmd.Parameters.AddWithValue("@p_curr_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.curr_amt);
                        cmd.Parameters.AddWithValue("@p_offered_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.offered_amt);
                        cmd.Parameters.AddWithValue("@p_bid_amt", NpgsqlTypes.NpgsqlDbType.Integer, tmp.bid_amt);
                        cmd.Parameters.AddWithValue("@p_contract_amt_unit", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_amt_unit);
                        cmd.Parameters.AddWithValue("@p_contract_amt_qty", NpgsqlTypes.NpgsqlDbType.Integer, tmp.contract_amt_qty);
                        cmd.Parameters.AddWithValue("@p_today_commission", NpgsqlTypes.NpgsqlDbType.Integer, tmp.today_commission);

                        cmd.Parameters.AddWithValue("@p_screen_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.screen_num);
                        cmd.Parameters.AddWithValue("@p_terminal_num", NpgsqlTypes.NpgsqlDbType.Text, tmp.terminal_num);
                        cmd.Parameters.AddWithValue("@p_credit_gubun", NpgsqlTypes.NpgsqlDbType.Text, tmp.credit_gubun);
                        cmd.Parameters.AddWithValue("@p_loan_dt", NpgsqlTypes.NpgsqlDbType.Text, tmp.loan_dt);



                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertChejanOrder]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }
        }
    }
}
