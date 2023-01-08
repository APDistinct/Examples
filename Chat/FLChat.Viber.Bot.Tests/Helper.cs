using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using FLChat.Viber.Client.Types;

namespace FLChat.Viber.Bot
{
    public static class Helper
    {
        public static CallbackData ReadJson(string fn) {
            string json = File.ReadAllText("./Json/" + fn + ".json");
            return JsonConvert.DeserializeObject<CallbackData>(json);
        }

        public static IEnumerable<CallbackData> CreateCallbackDataForAllEvents(IEnumerable<CallbackEvent> exclude) {
            return Enum
                .GetValues(typeof(CallbackEvent))
                .Cast<CallbackEvent>()
                .Except(exclude)
                .Select(e => new CallbackData() { Event = e });
        }
    }
}
