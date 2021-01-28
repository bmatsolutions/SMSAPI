using DBL;
using DBL.Models;
using DBL.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SMSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSAPI.Controllers
{
    [Route("api/app/[Action]")]
    [ApiController]
    public class AppController : ControllerBase
    {
        private Bl bl;
        private string logFile;
        public AppController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            bl.LogFile = appSett.Value.LogFile;
            logFile = appSett.Value.LogFile;
        }

        [HttpPost]
        [ActionName("reg")]
        public async Task<AppResponse> App([FromBody] App requestData)
        {
            try
            {
                return await bl.CreateApp(requestData);
            }
            catch (Exception ex)
            {
                AppUtil.Log.Error(logFile, "Api.Sms.App", ex);
                return new AppResponse { RespStatus = 1, RespMessage = ex.Message };
            }
        }
    }
}
