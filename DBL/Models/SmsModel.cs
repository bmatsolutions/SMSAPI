using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DBL.Models
{
    public class SmsRes
    {
        [JsonProperty("msg")]
        public string message { get; set; }
        [JsonProperty("bal")]
        public int Balance { get; set; }
        [JsonProperty("sent")]
        public Boolean Sent { get; set; }
    }

    public class SmsReq
    {
        [JsonProperty("uname")]
        public string Username { get; set; }
        [JsonProperty("gwpass")]
        public string gwpass { get; set; }
        [JsonProperty("sender")]
        public string Sender { get; set; }
        [JsonProperty("receiver")]
        public string Receiver { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("mno")]
        public string Mno { get; set; }
    }
}
