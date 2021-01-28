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
    [Route("api/v2/sms/[Action]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private Bl bl;
        private string logFile;
        public SmsController(IOptions<AppConfig> appSett)
        {
            bl = new Bl(appSett.Value.ConnectionString);
            bl.LogFile = appSett.Value.LogFile;
            logFile = appSett.Value.LogFile;
        }

        [HttpPost]
        [ActionName("send")]
        public async Task<SmsRes> Operation([FromBody] SmsReq requestData)
        {
            try
            {
                return await bl.SendSms(requestData);
            }
            catch (Exception ex)
            {
                AppUtil.Log.Error(logFile, "Api.Sms.Operation", ex);
                return new SmsRes { Sent = false, message = ex.Message,Balance=0 };
            }
        }
    }
}
