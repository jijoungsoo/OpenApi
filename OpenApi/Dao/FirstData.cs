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
    class FirstData
    {
        public void insertMarketStockData(List<TB_STOCK> lst)
        {
            using (var conn = new NpgsqlConnection("host=localhost;username=postgres;password=pwd;database=postgres"))
            {
                try
                {
                    conn.Open();

                    for (int i = 0; i < lst.Count; i++)
                    {
                        TB_STOCK tmp = lst[i];
                        /*postgre sql에서 아래와 같이 호출하면 function 을 호출한다. 
                         https://www.npgsql.org/doc/basic-usage.html
                         */
                        /*
                       using (var cmd = new NpgsqlCommand("insert_tb_stock", conn))
                       {
                           cmd.CommandType = CommandType.StoredProcedure;
                           cmd.Parameters.AddWithValue("p_market_cd", tmp.MARKET_CD);
                           cmd.Parameters.AddWithValue("p_stock_cd", tmp.STOCK_CD);
                           cmd.Parameters.AddWithValue("p_stock_nm", tmp.STOCK_NM);
                           cmd.Parameters.AddWithValue("p_stock_dt", tmp.STOCK_DT);
                           cmd.Parameters.AddWithValue("p_cnt", tmp.CNT);
                           cmd.Parameters.AddWithValue("p_last_price", tmp.LAST_PRICE);
                           cmd.Parameters.AddWithValue("p_stock_state", tmp.STOCK_STATE);
                           cmd.Parameters.AddWithValue("p_construction", tmp.CONSTRUCTION);
                           cmd.ExecuteNonQuery();
                       }*/
                        /*postgre sql에서 procedure는 11 버전부터 지원해서 아직 위 처럼 호출하는 방법은 없다.
                         storedProcedure 부분을 지우고 call을 명시적으로 해준다.

                        https://www.npgsql.org/doc/basic-usage.html
                         */
                        String sql = @"CALL insert_tb_market_stock(
@p_market_cd
,@p_stock_cd
,@p_stock_nm
,@p_stock_dt
,@p_stock_cnt
,@p_last_price
,@p_stock_state
,@p_construction
)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_market_cd", tmp.MARKET_CD);
                            cmd.Parameters.AddWithValue("@p_stock_cd", tmp.STOCK_CD);
                            cmd.Parameters.AddWithValue("@p_stock_nm", tmp.STOCK_NM);
                            cmd.Parameters.AddWithValue("@p_stock_dt", tmp.STOCK_DT);
                            cmd.Parameters.AddWithValue("@p_stock_cnt", tmp.CNT);
                            cmd.Parameters.AddWithValue("@p_last_price", tmp.LAST_PRICE);
                            cmd.Parameters.AddWithValue("@p_stock_state", tmp.STOCK_STATE);
                            cmd.Parameters.AddWithValue("@p_construction", tmp.CONSTRUCTION);
                            cmd.ExecuteNonQuery();
                        }

                    }


                }
                catch (Exception ex)
                {
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string
                    Console.WriteLine(ex.Message);

                }
            }

        }



        public void insertThemeData(List<TB_THEME> lst)
        {
            using (var conn = new NpgsqlConnection("host=localhost;username=postgres;password=pwd;database=postgres"))
            {
                try
                {
                    conn.Open();

                    for (int i = 0; i < lst.Count; i++)
                    {
                        TB_THEME tmp = lst[i];
                        String sql = @"CALL insert_tb_theme(
@p_theme_cd
,@p_theme_nm
)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_theme_cd", tmp.THEME_CD);
                            cmd.Parameters.AddWithValue("@p_theme_nm", tmp.THEME_NM);
                            cmd.ExecuteNonQuery();
                        }

                    }


                }
                catch (Exception ex)
                {
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }

        public void insertThemeStockData(List<TB_STOCK> lst)
        {
            using (var conn = new NpgsqlConnection("host=localhost;username=postgres;password=pwd;database=postgres"))
            {
                try
                {
                    conn.Open();

                    for (int i = 0; i < lst.Count; i++)
                    {
                        TB_STOCK tmp = lst[i];                      
                        String sql = @"CALL insert_tb_theme_stock(
@p_theme_cd
,@p_stock_cd
)";
                        using (var cmd = new NpgsqlCommand(sql, conn))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@p_theme_cd", tmp.THEME_CD);
                            cmd.Parameters.AddWithValue("@p_stock_cd", tmp.STOCK_CD);
                            cmd.ExecuteNonQuery();
                        }

                    }


                }
                catch (Exception ex)
                {
                    //{ "42883: insert_tb_stock(p_market_cd => text, p_stock_cd => text, p_stock_nm => text, p_stock_dt => text, p_cnt => integer, p_last_price => text, p_stock_state => text, p_construction => text) 이름의 함수가 없음"}
                    //		Message	"42601: 구문 오류, 입력 끝부분"	string

                }
            }

        }
    }
}
