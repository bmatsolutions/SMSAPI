using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class SmsRepository : BaseRepository, ISmsRepository
    {
        public SmsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<DataTable> GetInfo(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                // SqlConnection scon;
                using (SqlConnection scon = new SqlConnection(_connString))
                {
                    scon.Open();
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(sql, scon);
                        da.Fill(dt);
                        scon.Close();
                        scon.Dispose();
                        da.Dispose();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    return dt;
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<string> GetTransactionRef()
        {
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();

                    DynamicParameters parameters = new DynamicParameters();

                    return (await connection.QueryAsync<string>("Sp_GenerateTransactionRef", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
                }
                //string TrnRef = "";
                //string sSql = "";
                //var conn = new SqlConnection(Myconn);
                //var tcmd = new SqlCommand(sSql, conn);
                //tcmd.Connection.Open();
                //var retval = tcmd.ExecuteScalar();
                //if (retval != null)
                //    TrnRef = retval.ToString();
                //conn.Close();
                //tcmd.Dispose();
                //return TrnRef;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public async Task<string> UpdateLog(string sql)
        {
            using (var connection = new SqlConnection(_connString))
            {
                return (await connection.QueryAsync<string>(sql)).FirstOrDefault();
            }
        }
    }
}
