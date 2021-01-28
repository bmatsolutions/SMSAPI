using DBL;
using DBL.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SMSAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Middlewares
{
    public class ApiAppAuth
    {
        private readonly RequestDelegate _next;
        private Bl bl;
        private string logFile;
        private string callLogFile;
        string errorCode = "20";

        public ApiAppAuth(RequestDelegate next, IOptions<AppConfig> appConfig)
        {
            _next = next;
            bl = new Bl(appConfig.Value.ConnectionString);
            logFile = appConfig.Value.LogFile;
            callLogFile = appConfig.Value.CallLogFile;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authMessage = "Authorization failed!";
            try
            {
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");

                //---- Get auth header
                string authHeader = httpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
                {
                    //---- Get header details
                    string authHeaderText = authHeader.Substring("Basic ".Length).Trim();

                    //---- Read request data
                    httpContext.Request.EnableBuffering();
                    string jsonData = await new StreamReader(httpContext.Request.Body).ReadToEndAsync();
                    if (string.IsNullOrEmpty(jsonData))
                    {
                        var respData = JsonConvert.SerializeObject(new { err_msg = "Request stream empty!", err_code = "22" });
                        httpContext.Response.StatusCode = 400; //Bad request
                        await httpContext.Response.WriteAsync(respData);

                        return;
                    }

                  
                    var url = httpContext.Request.Path.Value;
                   // string serviceId = url.Substring(url.LastIndexOf("/") + 1);

                    //---- Authorize app header
                    var authResult = await bl.AuthorizeApiAsync(authHeaderText);
                    if (authResult.Successful)
                    {
                        if (authResult.CallLog)
                            AppUtil.Log.Infor(callLogFile, "ApiAppAuth", jsonData);

                        
                        //jsonData = JsonConvert.SerializeObject(reqData);

                        //--- Write data
                        var myData = encoding.GetBytes(jsonData);
                        var stream = new MemoryStream(myData);
                        httpContext.Request.Body = stream;

                        await _next(httpContext);
                        return;
                    }
                    else
                    {
                        errorCode = authResult.ErrorCode;
                        authMessage = authResult.Message;
                        AppUtil.Log.Error(logFile, "ApiAppAuth", new Exception(authMessage));
                    }
                }
                else
                {
                    authMessage = "Authorization details not found!";
                    errorCode = "21";
                    AppUtil.Log.Error(logFile, "ApiAppAuth", new Exception(authMessage));
                }
            }
            catch (Exception ex)
            {
                AppUtil.Log.Error(logFile, "ApiAppAuth", ex);
                authMessage = "Failed to authorize request due to an error!";
            }

            var authData = JsonConvert.SerializeObject(new { err_msg = authMessage, err_code = errorCode });
            httpContext.Response.StatusCode = 401; //Unauthorized
            await httpContext.Response.WriteAsync(authData);
        }
    }

    public static class ApiAppAuthExtensions
    {
        public static IApplicationBuilder UseApiAppAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiAppAuth>();
        }
    }
}
