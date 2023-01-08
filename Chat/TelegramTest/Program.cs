using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MihaZupan;
using Telegram.Bot;

namespace TelegramTest
{
    class Program
    {
        static void Main(string[] args) {
            //var proxy = new HttpToSocks5Proxy("185.17.120.252", 1080, "tguser", "d72c76bcf19db9aa458a9862cdca9f3b");
            TelegramBotClient client = new TelegramBotClient("682112025:AAGCGaTM3hS44BjTJ7L7FeTDWjbzKTzyyaQ");
            var me = client.GetMeAsync().Result;
            Console.WriteLine(
              $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
            );

            Telegram.Bot.Types.Message m = client.SendTextMessageAsync(new Telegram.Bot.Types.ChatId(1233444), "test text").Result;
            Console.WriteLine(m.MessageId);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(m);
            System.IO.File.WriteAllText("tg.json", json);
        }
    }
}
