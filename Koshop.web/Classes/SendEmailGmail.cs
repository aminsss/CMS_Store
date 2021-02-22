using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Koshop.web;
using Koshop.DomainClasses;
using Koshop.DataLayer;

namespace Koshop.web
{
    public class SendEmailSMS
    {
        AppDbContext db = new AppDbContext();
        static DateTime EPOCH = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
        static Dictionary<string, Options> WP_Options = new Dictionary<string, Options>();

        public SendEmailSMS()
        {
            List<Options> options = db.Options.ToList(); // store records into a list
            WP_Options.Clear();
            foreach (Options option in options)
            {
                WP_Options.Add(option.Name, option); // Store to dictionary
            }
        }


        public static double ConvertDatetimeToUnixTimeStamp(DateTime date, int Time_Zone = 0)
        {
            TimeSpan The_Date = (date - EPOCH);
            return Math.Floor(The_Date.TotalSeconds) - (Time_Zone * 3600);
        }


        public void SendEmail(IList<string> To,string Subject,string Body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(WP_Options["SmtpClient"].Value);
            mail.From = new MailAddress(WP_Options["OwnerEmail"].Value, WP_Options["SiteName"].Value);
            foreach (var item in To)
            {
                mail.To.Add(item);
            }
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;

            //System.Net.Mail.Attachment attachment;
            // attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            // mail.Attachments.Add(attachment);

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(WP_Options["OwnerEmail"].Value, WP_Options["EmailPassword"].Value);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public void SendSMS(IList<string> reciver,string body)
        {
            DateTime date = DateTime.Now;
            string[] senderNumbers = new string[reciver.Count];
            string[] messageBodies = new string[reciver.Count];
            string[] senddate = new string[reciver.Count];

            string username = WP_Options["SMSUserName"].Value;
            string password = WP_Options["SMSPasword"].Value;
            string[] recipientNumbers = reciver.ToArray();
            for (int i = 0; i < reciver.Count; i++)
            {
                senderNumbers[i] =  WP_Options["SMSNumber"].Value ;
                messageBodies[i] = body ;
                senddate[i] =   Convert.ToString(ConvertDatetimeToUnixTimeStamp(date)) ; 
            }
            int[] messageClasses = { };
            irSample.v2 ws = new irSample.v2();
            var result = ws.SendSMS(username, password, senderNumbers, recipientNumbers, messageBodies, senddate, null, null);
        }

    }
}