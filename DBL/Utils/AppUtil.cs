using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Utils
{
    public class AppUtil
    {
        public static class Log
        {
            public static void Infor(string logFile, string functioName, string message)
            {
                WriteLog(logFile, functioName, new Exception(message), false);
            }

            public static void Error(string logFile, string functioName, Exception ex)
            {
                WriteLog(logFile, functioName, ex);
            }

            public static void Error(string logFile, string functioName, string message)
            {
                WriteLog(logFile, functioName, new Exception(message));
            }

            private static void WriteLog(string logFile, string functioName, Exception ex, bool isError = true)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        //--- Delete log if it more than 500Kb
                        if (File.Exists(logFile))
                        {
                            FileInfo fi = new FileInfo(logFile);
                            if ((fi.Length / 1000) > 100)
                                fi.Delete();
                        }
                        //--- Create stream writter
                        StreamWriter stream = new StreamWriter(logFile, true);
                        await stream.WriteLineAsync(string.Format("{0}|{1:dd-MMM-yyyy HH:mm:ss}|{2}|{3}",
                            isError ? "ERROR" : "INFOR",
                            DateTime.Now,
                            functioName,
                            isError ? ex.ToString() : ex.Message));
                        stream.Close();
                    }
                    catch (Exception) { }
                });
            }
        }

        public static string GenerateAppSecret()
        {
            var rdm = new Random();
            string myPass = rdm.Next(100000, 999999).ToString();
            string alpha = "0abcdefghijklmnopqrstuvwxyz";


            int idx = rdm.Next(1, 26);
            string char1 = alpha.Substring(idx, 1);
            idx = rdm.Next(1, 26);
            string char2 = alpha.Substring(idx, 1);

            myPass = myPass.Insert(4, char1);
            myPass = myPass.Insert(2, char2);

            return myPass;
        }

        public static string GenerateAppKey()
        {
            var rdm = new Random();
            string myKey = rdm.Next(100000, 999999).ToString();
            string myPass = rdm.Next(100000, 999999).ToString();
            string alpha = "0abcdefghijklmnopqrstuvwxyz";


            int idx = rdm.Next(1, 26);
            string char1 = alpha.Substring(idx, 1);
            idx = rdm.Next(1, 26);
            string char2 = alpha.Substring(idx, 1);

            myPass = myPass.Insert(3, char1);
            myPass = myPass.Insert(2, char2);

            myKey = myKey.Insert(3, char1);
            myKey = myKey.Insert(2, char2);

            return myPass + myKey;
        }
    }
}
