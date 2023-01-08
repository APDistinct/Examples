using EMailSender;
using FLChat.Core;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FLChat.Transport
{
    public class MailSender : ITextSender, IMessageSender
    {
        private readonly String email;            // Ваш email адрес для отправки
        private readonly String smtpLogin;        // Логин для авторизации на SMTP-сервере.
        private readonly String smtpPassword;     // Пароль для авторизации на SMTP-сервере.
        private readonly String smtpServer;       // SMTP-сервер.        
        private readonly Int32 smtpPort;          // Порт для авторизации на SMTP-сервере.
        private readonly Boolean smtpUseSSL;      // Флаг - использовать SSL.
        private readonly Boolean translit;        // Переводит все русские символы в латинские.
        private readonly string alias;            // Alias for sending

        public MailSender()
        {
            email = ConfigurationManager.AppSettings["email"];
            smtpLogin = ConfigurationManager.AppSettings["smtpLogin"];
            smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
            smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            //string ss = ConfigurationManager.AppSettings["smtpPort"];
            smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
            smtpUseSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["smtpUseSSL"]);
            translit = Convert.ToBoolean(ConfigurationManager.AppSettings["translit"]);
            alias = ConfigurationManager.AppSettings["alias"];
        }

        public void Send(string addressee, string text)
        {
            if (addressee.Trim().Count() > 0)
            {
                var mailer = new Mailer(smtpLogin, smtpPassword, smtpServer, smtpPort, smtpUseSSL);

                mailer.Body = text; // String.Join(Environment.NewLine, wt); 
                mailer.Destination = addressee;// "APDistinct@gmail.com";  
                if(alias != null)
                  mailer.SetAlias(alias); 

                //  Вопрос - что писать. Доставать из App.config?
                //mailer.Subject = "Виртуальный Мониторинг обработал вашу заявку";
                

                mailer.SendMessage();
                //mailer.SendMessage();
            }
            else
            {
                // Что?
            }
        }

        public Task<SentMessageInfo> Send(MessageToUser msg, string msgText, CancellationToken ct)
        {
            return Task.Run<SentMessageInfo>(() => 
            {
                //Fix(msg, msgText);
                if(msg.ToTransport?.User == null)
                {
                    throw new Exception($"Пользователю {msg.ToUserId} недоступны Email-рассылки");
                }
                if (msg.ToTransport?.User?.Email == null)
                {
                    throw new Exception($"Пользователь {msg.ToUserId} не имеет Email-адреса");
                }
                
                Send(msg.ToTransport.User.Email, msgText);
                return new SentMessageInfo((string)null, DateTime.Now);
            });
        }

        private void Fix(MessageToUser msg, string msgText)
        {            
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString() + ".txt");
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(msgText ?? "Нет текста");
                sw.Flush();
                sw.WriteLine(msg != null ? msg.MsgId.ToString() : "msg != null");
                sw.Flush();
                sw.WriteLine(msg.ToTransport != null ? msg.ToTransport.Kind.ToString() : "msg.ToTransport != null");
                sw.Flush();
                sw.WriteLine(msg.ToTransport.User != null ? msg.ToTransport.User.UserId.ToString() : "msg.ToTransport.User != null");
                sw.Flush();
                sw.WriteLine(msg.ToTransport.User.Email != null ? msg.ToTransport.User.Email.ToString() : "msg.ToTransport.User.Email != null");
                sw.Flush();
                //List<string> list = new List<string>();
                //list.Add(msgText ?? "Нет текста");
                //list.Add(msg != null ? msg.MsgId.ToString() : "msg != null");
                //list.Add(msg.ToTransport != null ? msg.ToTransport.Kind.ToString() : "msg.ToTransport != null");
                //list.Add(msg.ToTransport.User != null ? msg.ToTransport.User.UserId.ToString() : "msg.ToTransport.User != null");
                //list.Add(msg.ToTransport.User.Email != null ? msg.ToTransport.User.Email.ToString() : "msg.ToTransport.User.Email != null");
            }
            
            //File.WriteAllLines(path, list);
        }       
    }
}
