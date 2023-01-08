using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SmsRu;
using SmsRu.Enumerations;


// Для использоввания данного класса надо в App.config добавить следующую информацию

//<appSettings>
//  <!-- Заполнить желательно все значения.Переменные используются в классе SmsRuProvider, там описано их назначение.-->
//  <add key = "smsRuLogin" value= "89145671450" />
//  < add key= "smsRuPassword" value= "qwerty1357" />
//  < add key= "smsRuApiID" value= "93F630F8-8B4F-E1CB-D44C-720A3CE49776" />
//  < add key= "partnerID" value= "7453" />
//  < add key= "logFolder" value= "C:\SmsRu\"/>
//  < add key= "smsRuEmail" value= "" />
//  < add key= "email" value= "virtual.monitoring@yandex.ru" />
//  < add key= "smtpServer" value= "smtp.yandex.ru" />
//  < add key= "smtpPort" value= "25" />
//  < add key= "smtpLogin" value= "virtual.monitoring@yandex.ru" />
//  < add key= "smtpPassword" value= "Virtual2017Monitoring" />
//  < add key= "smtpUseSSL" value= "true" />
//  < add key= "translit" value= "false" />
//  < add key= "test" value= "false" />
//  <!-- Номер телефона, на который заключён договор рассылки СМС. Тоже нужен для отправки-->
//  < add key= "contractPhone" value= "79145671450" />
//</ appSettings >


using FLChat.Core;
using FLChat.DAL.Model;
using System.Threading;

namespace FLChat.Transport
{
    public class SmsSender : ITextSender, IMessageSender
    {
        private string contractPhone = "79145671450";
        // Номер базового телефона читаем из конфигурации
        
        ISmsProvider sms = new SmsRuProvider();
        public SmsSender()
        {
            contractPhone = ConfigurationManager.AppSettings["contractPhone"];  // 
        }
        public void Send(string addressee, string text)
        {
            string ret = sms.Send(contractPhone, addressee, text);
            //  Далее анализ ответа и реакция.

        }

        public Task<SentMessageInfo> Send(MessageToUser msg, string text, CancellationToken ct) {
            return Task.Run<SentMessageInfo>(() => {
                Send(msg.ToTransport.User.Phone, text);
                return new SentMessageInfo((string)null, DateTime.Now);
            });
        }


        //Console.WriteLine("Метод Send:");
        //    Console.WriteLine("------");
        //    Console.WriteLine(sms.Send("79098449603", new String[] { "79098449603", "79098449603", "79098449603"}, "Первый блин", EnumAuthenticationTypes.Simple));
        //    Console.WriteLine("------");
        //    Console.WriteLine(sms.Send("79098449603", "79098449603", "Блин № 2"));
        //    Console.WriteLine("------");
        //    //Console.WriteLine(sms.Send("79098449603", new String[] { "79098449603", "79098449603" }, DateTime.Now.ToLongTimeString()));
        //    //Console.WriteLine(sms.SendMultiple("79098449603", new Dictionary<String, String>() { { "+79098449603", "А не послать ли нам СМС?" }, { "+79098449603", "А почему бы и нет..." } }));

        //    //Console.WriteLine(sms.Send("79161234567", new String[] { "79161234567", "79161234567", "79161234567" }, DateTime.Now.ToLongTimeString(), EnumAuthenticationTypes.StrongApi));
        //    DateTime tomorrow = DateTime.Now + new TimeSpan(24, 0, 0);
        //Console.WriteLine(sms.Send("79098449603", new String[] { "79098449603", "79098449603"}, "Будем надеяться, дошло.", tomorrow, EnumAuthenticationTypes.StrongApi));
        //    Console.WriteLine("------");

        //Console.WriteLine("\nМетод CheckCost:");
        //Console.WriteLine(sms.CheckCost("+79141641370", "Сообщение длинной 1 SMS: написано кириллицей,не может превышать 70 зн.", EnumAuthenticationTypes.Simple));
        //Console.WriteLine(sms.CheckCost("79141641370", "Сообщение длинной 6 SMS: В стандарте также предусмотрена возможность отправлять сегментированные сообщения. В таких сообщениях в заголовке пользовательских данных помещается информация о номере сегмента сообщения и общем количестве сегментов. Максимальная длина сегмента при этом уменьшается за счет этого заголовка. Как правило, каждый сегмент тарифицируется как отдельное сообщение.", EnumAuthenticationTypes.Strong));
        //Console.WriteLine(sms.CheckCost("+79141641370", "Сообщение длинной 3 SMS:  Сегментирование поддерживают почти все современные телефоны, но часто в телефонах вводится ограничение на количество сегментов в сообщении.", EnumAuthenticationTypes.StrongApi));

        //public void SendShortSmsWork(int workId)
        //{
        //    string phone = "", text = "";
        //    WorkReporter workReporter = new WorkReporter();
        //    workReporter.ShortSmsText(workId, ref phone, ref text);
        //    Send(phone, text);
        //    //bool ShortSmsText(int workId, ref string phone, ref string _out)
        //}
    }
}




