using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSAPI.Models
{
    public class AppConfig
    {
        public string ConnectionString;
        public int DatabaseType;
        public string WorkingDir;
        public string LogFile;
        public string CallLogFile;
    }
}
