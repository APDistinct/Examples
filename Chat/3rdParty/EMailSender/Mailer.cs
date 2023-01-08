using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace EMailSender
{
    public class Mailer
    {

        public string Subject { get; set; }
        public string Body { get; set; }
        public string Destination { get; set; }
        private string Login { get; }
        private string Password { get; }
        private string Alias { get; set; }
        private string Host{ get; set; }
        private int Port { get; set; }
        private bool UseSSL { get; set; }

        public Mailer(string login, string password, string host, int port, bool smtpUseSSL = true)
        {
            Host = host;
            Port = port;
            Login = login;
            Password = password;
            Alias = login;
            UseSSL = smtpUseSSL;
        }

        public void SetAlias(string alias)
        {
            Alias = alias;
        }
        public async Task SendMessageAsync()
        {
            var client = GetSmtpClient();

            var mail = new MailMessage
            {
                From = new MailAddress(Login, Alias),
                Subject = Subject,
                Body = Body,
                IsBodyHtml = false //true
            };
            mail.To.Add(Destination);

            try
            {
                await client.SendMailAsync(mail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SendMessage()
        {
            var client = GetSmtpClient();

            var mail = new MailMessage
            {
                From = new MailAddress(Login, Alias),
                Subject = Subject,
                Body = Body,
                IsBodyHtml = false //true
            };
            mail.To.Add(Destination);

            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient
            {
                EnableSsl = UseSSL, //true,
                Host = Host,
                Port = Port,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(Login/*.Split('@')[0]*/, Password),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 1000*60*5,
        };
            client.SendCompleted += (s, e) => { client.Dispose(); };

            return client;
        }

        private string GetBody()
        {
            var body = new StringBuilder();
            body.AppendLine("Здравствуйте");
            body.AppendLine("<br><br>");
            body.AppendLine("Мы обработали вашу заявку №2017");
            body.AppendLine("<br><br>");
            body.AppendLine("Спасибо что пользуетесь нашеми услугами.");
            body.AppendLine("<br><br>");
            body.AppendLine("Желаем успешной работы!");

            return body.ToString();
        }
    }

}
