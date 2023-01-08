using FLChat.TelegramBot.Adapters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace FLChat.TelegramBot
{
    public static class ResourceHelper
    {
        public static TelegramMessageAdapter Read(string fn) {
            string json = System.IO.File.ReadAllText(".\\Json\\" + fn + ".json");
            Update upd = JsonConvert.DeserializeObject<Update>(json);
            return new TelegramMessageAdapter(upd.Message);
        }

        public static Telegram.Bot.Types.File ReadFile(string fn) {
            string json = System.IO.File.ReadAllText(".\\Json\\" + fn + ".json");
            var obj = JsonConvert.DeserializeObject<Telegram.Bot.Types.File>(json);
            return obj;
        }
    }
}
