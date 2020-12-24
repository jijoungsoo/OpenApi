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
    class RealTimeData
    {
        /*주식체결*/
        public void insertRealtimeContract(TB_REALTIME_CONTRACT tmp)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"CALL insert_tb_realtime_contract(
@p_stock_dt,
@p_stock_cd,
@p_contract_time,
@p_curr_amt,
@p_fluctuation_rt,
@p_offered_amt,
@p_bid_amt,
@p_trade_qty,
@p_accumulated_trade_qty,
@p_accumulated_trade_amt,
@p_start_amt,
@p_high_amt,
@p_low_amt,
@p_contrast_yesterday_symbol,
@p_contrast_yesterday,
@p_yesterday_contrast_trade_qty,
@p_trade_amount_variation,
@p_yesterday_contrast_trade_rt,
@p_trade_turnover_ratio,
@p_trade_cost,
@p_contract_strength,
@p_total_market_amt,
@p_market_gubun,
@p_ko_accessibility_rt,
@p_upper_amt_limit_time,
@p_lower_amt_limit_time,
@p_real_name
)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_stock_dt"                  , NpgsqlTypes.NpgsqlDbType.Text,     tmp.stock_dt);
                            cmd.Parameters.AddWithValue("@p_stock_cd"                  , NpgsqlTypes.NpgsqlDbType.Text,     tmp.stock_cd);
                            cmd.Parameters.AddWithValue("@p_contract_time", NpgsqlTypes.NpgsqlDbType.Text,     tmp.contract_time);
                            cmd.Parameters.AddWithValue("@p_curr_amt"                  , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.curr_amt);
                            cmd.Parameters.AddWithValue("@p_fluctuation_rt"            , NpgsqlTypes.NpgsqlDbType.Real,     tmp.fluctuation_rt);

                            cmd.Parameters.AddWithValue("@p_offered_amt"               , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.offered_amt);
                            cmd.Parameters.AddWithValue("@p_bid_amt"                   , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.bid_amt);
                            cmd.Parameters.AddWithValue("@p_trade_qty"                 , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.trade_qty);
                            cmd.Parameters.AddWithValue("@p_accumulated_trade_qty"     , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.accumulated_trade_qty);
                            cmd.Parameters.AddWithValue("@p_accumulated_trade_amt"     , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.accumulated_trade_amt);
                        
                            cmd.Parameters.AddWithValue("@p_start_amt"                 , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.start_amt);
                            cmd.Parameters.AddWithValue("@p_high_amt"                  , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.high_amt);
                            cmd.Parameters.AddWithValue("@p_low_amt"                   , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.low_amt);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday_symbol" , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.contrast_yesterday_symbol);
                            cmd.Parameters.AddWithValue("@p_contrast_yesterday"        , NpgsqlTypes.NpgsqlDbType.Integer,  tmp.contrast_yesterday);


                            cmd.Parameters.AddWithValue("@p_yesterday_contrast_trade_qty", NpgsqlTypes.NpgsqlDbType.Integer,tmp.yesterday_contrast_trade_qty);

                            cmd.Parameters.AddWithValue("@p_trade_amount_variation"     , NpgsqlTypes.NpgsqlDbType.Real,    tmp.trade_amount_variation);
                            cmd.Parameters.AddWithValue("@p_yesterday_contrast_trade_rt", NpgsqlTypes.NpgsqlDbType.Real,    tmp.yesterday_contrast_trade_rt);
                            cmd.Parameters.AddWithValue("@p_trade_turnover_ratio"       , NpgsqlTypes.NpgsqlDbType.Real,    tmp.trade_turnover_ratio);
                            cmd.Parameters.AddWithValue("@p_trade_cost"                 , NpgsqlTypes.NpgsqlDbType.Integer, tmp.trade_cost);
                            cmd.Parameters.AddWithValue("@p_contract_strength"          , NpgsqlTypes.NpgsqlDbType.Real,    tmp.contract_strength);
                            cmd.Parameters.AddWithValue("@p_total_market_amt"           , NpgsqlTypes.NpgsqlDbType.Integer, tmp.total_market_amt);
                            cmd.Parameters.AddWithValue("@p_market_gubun"               , NpgsqlTypes.NpgsqlDbType.Integer, tmp.market_gubun);
                            cmd.Parameters.AddWithValue("@p_ko_accessibility_rt"        , NpgsqlTypes.NpgsqlDbType.Integer, tmp.ko_accessibility_rt);
                            cmd.Parameters.AddWithValue("@p_upper_amt_limit_time"       , NpgsqlTypes.NpgsqlDbType.Text,    tmp.upper_amt_limit_time);
                            cmd.Parameters.AddWithValue("@p_lower_amt_limit_time"       , NpgsqlTypes.NpgsqlDbType.Text,    tmp.lower_amt_limit_time);
                            cmd.Parameters.AddWithValue("@p_real_name"                  , NpgsqlTypes.NpgsqlDbType.Text,    tmp.real_name);

                        
                            cmd.ExecuteNonQuery();
                        }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertRealtimeContract]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }
        }
     
    }
}
