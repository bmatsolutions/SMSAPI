using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class ApiAuthResult
    {
        public bool Successful { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public bool CallLog { get; set; }
    }

    public class App
    {
        public string AppName { get; set; }
    }

    public class AppResponse
    {
        public int RespStatus { get; set; }
        public string RespMessage { get; set; }
        public string AppKey { get; set; }
        public string AppId { get; set; }
    }
}
