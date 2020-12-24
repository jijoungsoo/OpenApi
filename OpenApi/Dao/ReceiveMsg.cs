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
    class ReceiveMsg
    {
        public void insertReceiveMsg(String scr_no, String rq_name, String tr_code, String msg, String stock_cd)
        {
            using (var conn = new NpgsqlConnection(Config.GetDbConnStr()))
            {
                try
                {
                    conn.Open();
                    String sql = @"insert into tb_receive_msg
(
scr_no
,rq_name
,tr_code
,msg
,stock_cd
,crt_dtm
)
values 
(
 @p_scr_no
,@p_rq_name
,@p_tr_code
,@p_msg
,@p_stock_cd
,now()
)";
                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_scr_no", NpgsqlTypes.NpgsqlDbType.Text, scr_no);
                        cmd.Parameters.AddWithValue("@p_rq_name", NpgsqlTypes.NpgsqlDbType.Text, rq_name);
                        cmd.Parameters.AddWithValue("@p_tr_code", NpgsqlTypes.NpgsqlDbType.Text, tr_code);
                        cmd.Parameters.AddWithValue("@p_msg", NpgsqlTypes.NpgsqlDbType.Text, msg);
                        cmd.Parameters.AddWithValue("@p_stock_cd", NpgsqlTypes.NpgsqlDbType.Text, stock_cd);
                                        


                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    FileLog.PrintF("[insertReceiveMsg]Exception ex=" + ex.Message);
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }
        }
    }
}
