using Dapper;
using DBL.Models;
using DBL.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Repositories
{
    public class SecurityRepository:BaseRepository,ISecurityRepository
    {
        public SecurityRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<GenericModel> AuthorizeApp(string appid, string appkey)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AppId", appid);
                parameters.Add("@AppKey", appkey);

                return (await connection.QueryAsync<GenericModel>("sp_AuthorizeApp", parameters, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }

        public async Task<AppResponse> CreateApp(App model)
        {
            string token = AppUtil.GenerateAppSecret();
            string appkey = AppUtil.GenerateAppKey();

            var resp = new AppResponse();
            try
            {
                using (var connection = new SqlConnection(_connString))
                {
                    connection.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@AppName", model.AppName);
                    parameters.Add("@AppKey", appkey);
                    parameters.Add("@AppSecret", token);

                    resp = connection.Query<AppResponse>("sp_CreateSmsApp", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                resp.RespStatus = 3;
                resp.RespMessage = ex.Message;
            }

            return resp;
        }
    }
}
