using Admin.BaseModels.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace Admin.CustomCode
{
    class SendMail
    {
        string subject, body;

        List<string> to = new List<string>();

        public List<string> To
        {
            get { return to; }
            set { to = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public void Send()
        {
            BaseConfiguration configuration = new BaseConfiguration();
            MailMessage mail = new MailMessage();
            if (!configuration.email.Contains("$NotConfigured$"))
            {
                //mail.ReplyToList.Add(new MailAddress(company.smtpReplyUsername, company.smtpReplyName));
                mail.From = new MailAddress(configuration.email, configuration.email);
                foreach (string item in to)
                {
                    if (!string.IsNullOrEmpty(item))
                        mail.To.Add(new MailAddress(item));
                }
                SmtpClient client = new SmtpClient();
                client.Port = configuration.port;
                client.EnableSsl = configuration.smtpSSL;
                if (configuration.smtpSSL)
                {
                    client.UseDefaultCredentials = true;
                    client.Credentials = new NetworkCredential(configuration.email, configuration.password);
                }
                else
                {
                    client.UseDefaultCredentials = false;

                }
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Timeout = int.MaxValue;
                client.Host = configuration.server;
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
        } 

        public void Send(string fromEmail, string password)
        {
            BaseConfiguration configuration = new BaseConfiguration();

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            foreach (string item in to)
            {
                if (!string.IsNullOrEmpty(item))
                    mail.To.Add(new MailAddress(item));
            }

            SmtpClient client = new SmtpClient();
            client.Port = configuration.port;
            client.UseDefaultCredentials = false;
            NetworkCredential cred = new NetworkCredential(fromEmail, password);
            client.Credentials = cred;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Timeout = int.MaxValue;
            client.Host = configuration.server;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            client.Send(mail);
        }
    }
}