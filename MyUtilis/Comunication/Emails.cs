using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Comunication
{
    class Emails
    {

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string UserCredential { get; set; }
        public string PasswordCredential { get; set; }

        public bool DefaultCredentials { get; set; }

        public string Host { get; set; }

        public string From { get; set; }
        public Emails()
        {
            To = "";
            Subject = "";
            Body = "";
        }

        public Emails(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }


        /// <summary>
        /// Execute email method.
        /// </summary>
        /// <param name="host"></param>
        /// <param name="userCredential"></param>
        /// <param name="passCredential"></param>
        /// <param name="from"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="toMails"></param>
        ///  <param name="defaultCredentials"></param>
        public void Send(string host = "", string userCredential = "", string passCredential = "", string from = "", string subject = ""
            , string body = "", string toMails = "",bool defaultCredentials = false)
        {
            if (host == "")
                host = Host;
            if (userCredential == "")
                userCredential = UserCredential;
            if (passCredential == "")
                passCredential = PasswordCredential;
            if (from == "")
                from = From;
            if (subject == "")
                subject = Subject;
            if (body == "")
                body = Body;
            if (toMails == "")
                toMails = To;

            try
            {
                SmtpClient SmtpServer = new SmtpClient();
                MailMessage mail = new MailMessage();
                SmtpServer.Credentials = new System.Net.NetworkCredential(userCredential, passCredential);
                SmtpServer.UseDefaultCredentials = defaultCredentials;
                SmtpServer.Host = host;
                mail = new MailMessage();
                mail.From = new MailAddress(from);
                string[] _cc = toMails.Split(';');
                if (To != null)
                {
                    foreach (string cc in _cc)
                    {
                        mail.To.Add(cc);
                    }
                }
                mail.Subject = subject;
                mail.Body = body;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
