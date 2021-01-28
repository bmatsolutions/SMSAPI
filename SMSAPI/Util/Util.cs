using Bmat.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SMSAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SMSAPI.Util
{
    public class Util
    {
        public static AppConfig GetAppConfig(IConfiguration config, IWebHostEnvironment env)
        {
            var appConfig = new AppConfig();
            appConfig.DatabaseType = 0;// Convert.ToInt32(config.GetSection("DbConnData:DbType").Value);
            if (appConfig.DatabaseType == 0)
            {
                string connId = config.GetSection("DbConnData:Id").Value;
                string connKey = config.GetSection("DbConnData:Key").Value;
                string connData = config.GetSection("DbConnData:Data").Value;

                ConnectionManager conMan = new ConnectionManager();
                string connString = conMan.GetConnectionString(connData, connId, connKey);

                appConfig.ConnectionString = connString;
            }

            appConfig.LogFile = GetErrorLogFile(env);
            appConfig.CallLogFile = GetCallLogFile(env);

            return appConfig;
        }

        public static string GetErrorLogFile(IWebHostEnvironment env)
        {
            try
            {
                string logDir = Path.Combine(env.ContentRootPath, "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                return Path.Combine(logDir, "ErrorLog.log");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetCallLogFile(IWebHostEnvironment env)
        {
            try
            {
                string logDir = Path.Combine(env.ContentRootPath, "logs");

                //---- Create Directory if it does not exist              
                if (!Directory.Exists(logDir))
                {
                    Directory.CreateDirectory(logDir);
                }
                return Path.Combine(logDir, "CallLog.log");
            }
            catch (Exception)
            {
                return "";
            }
        }
    }

}
