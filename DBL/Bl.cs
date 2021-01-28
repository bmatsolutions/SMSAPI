using DBL.Models;
using DBL.UOW;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DBL
{
    public class Bl
    {
        private string connString;
        private UnitOfWork db;
        public string LogFile { get; set; }

        public Bl(string dbConnString)
        {
            this.connString = dbConnString;
            if (!string.IsNullOrEmpty(dbConnString))
                db = new UnitOfWork(connString);
        }

        #region Middleware/Security
        public async Task<ApiAuthResult> AuthorizeApiAsync(string authHeaderText)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");

            //--- Get raw header
            string authHeader = encoding.GetString(Convert.FromBase64String(authHeaderText));

            string[] data = authHeader.Split(':');
            if (data.Length != 2)
            {
                return new ApiAuthResult
                {
                    Successful = false,
                    Message = "Invalid authorization header!"
                };
            }

            var auth =await db.SecurityRepository.AuthorizeApp(data[0], data[1]);

            if (auth.RespStatus != 0)
            {
                return new ApiAuthResult
                {
                    Successful = false,
                    ErrorCode = "21",
                    Message = auth.RespMessage
                };
            }

            return new ApiAuthResult
            {
                Successful = true
            };
        }

        public async Task<AppResponse> CreateApp(App model)
        {
            return await db.SecurityRepository.CreateApp(model);
        }
        #endregion

        #region Sms
        public async Task<SmsRes> SendSms(SmsReq Sendreq)
        {
            SmsRes Resp = new SmsRes();
            try
            {
                //clsdb db = new clsdb();
                //  string Conn = System.Configuration.ConfigurationManager.ConnectionStrings["Connstring"].ToString();
                Random rnd = new Random();
                string smsref = await db.SmsRepository.GetTransactionRef();// Convert.ToInt64(DateTime.Now.ToString("ddmmyyss")) ;// + "" + rnd.Next(999));
                //db = new clsdb();
                DataTable dt = new DataTable();
                string sql = "";

                if (Sendreq != null)
                {
                    sql = "select custid from SmsCustomers where Username='" + Sendreq.Username + "'";
                    dt = await db.SmsRepository.GetInfo(sql);
                    if (dt.Rows.Count > 0)
                    {
                        sql = "Insert into smslog (senderid,Destphone,sendsms,sent,smsmessage,smsdate,Addtext,smsref)values('" + dt.Rows[0][0].ToString() + "','" + Sendreq.Receiver.Replace("+", "") + "',0,0,'" + Sendreq.Message.Replace("'", "").Replace(",", "") + "',getdate(),'','" + smsref + "')";
                        //db = new clsdb();
                        //  System.IO.File.AppendAllText(@"c:\Bmat\Sms_Errors.txt", sql + Environment.NewLine);
                    }

                    await db.SmsRepository.GetInfo(sql);
                    if (Sendreq.Message == "")
                    {
                        Resp.Balance = 0;
                        Resp.message = "Your Message cannot be empty";
                        Resp.Sent = false;
                        sql = "update smslog set sent=0,sendsms=7,addtext='Message is blank' where smsref='" + smsref + "'";
                        await db.SmsRepository.UpdateLog(sql);
                        return Resp;
                    }

                    DataTable dts = new DataTable();
                    //db = new clsdb();
                    sql = "Select SmsBalance from CustomerAccount where CustId='" + dt.Rows[0][0].ToString() + "'";
                    dts = await db.SmsRepository.GetInfo(sql);
                    if (dts.Rows.Count < 1)
                    {
                        Resp.Balance = 0;
                        Resp.message = "Your Account is not Activated";
                        Resp.Sent = false;
                        sql = "update smslog set sent=0,sendsms=7,addtext='Account not activated' where smsref='" + smsref + "'";
                        await db.SmsRepository.UpdateLog(sql);
                        return Resp;
                    }
                    if (Convert.ToInt32(dts.Rows[0][0]) < 1)
                    {
                        Resp.Balance = 0;
                        Resp.message = "Your Account balance is 0";
                        Resp.Sent = false;
                        sql = "update smslog set sent=0,sendsms=7,addtext='Your Account balance is 0' where smsref='" + smsref + "'";
                        await db.SmsRepository.UpdateLog(sql);
                        return Resp;
                    }
                    sql = "UPDATE SMSLOG  SET SMSLOG.MNOID = MNOS.MNOID FROM MNOS  WHERE SUBSTRING(SMSLOG.DESTPHONE, 1, 5)= MNOS.PREFIX and smsref='" + smsref + "'";
                    await db.SmsRepository.UpdateLog(sql);
                    sql = "UPDATE SMSLOG  SET SMSLOG.sendingmno = MNOS.sendingmno FROM MNOS  WHERE SUBSTRING(SMSLOG.DESTPHONE, 1, 5)= MNOS.PREFIX and SMSLOG.smsref='" + smsref + "'";
                    await db.SmsRepository.UpdateLog(sql);
                    sql = "update smslog set sendsms=1 where smsref='" + smsref + "'";
                    await db.SmsRepository.UpdateLog(sql);
                    Resp.Balance = Convert.ToInt32(dts.Rows[0][0]);
                    Resp.message = "Message sent";
                    Resp.Sent = true;
                    return Resp;
                }
                else
                {
                    Resp.Balance = 0;
                    Resp.message = "Invalid Details";
                    Resp.Sent = false;
                }
                return Resp;
            }
            catch (Exception ex)
            {
                Resp.Balance = 0;
                Resp.message = "system error " + ex.Message;
                Resp.Sent = false;
                System.IO.File.AppendAllText(@"c:\Bmat\Sms_Errors.txt", ex.Message + Environment.NewLine);
                System.IO.File.AppendAllText(@"c:\Bmat\Mobisms.txt", ex + Environment.NewLine);
            }
            return Resp;
        }
        #endregion

    }
}
