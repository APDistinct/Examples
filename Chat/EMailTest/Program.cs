using FLChat.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                Console.WriteLine("Use default data");
            string addr = /*"apdapdapdapd@yandex.ru"*/"APDistinct@gmail.com", text = "Test message. Тестовое послание.";
            if (args.Length > 0)
            {
                addr = args[0];
            }
            if (args.Length > 1)
            {
                text = args[1];
            }
            var mailSender = new MailSender();
            mailSender.Send(addr, text);
        }
    }
}
