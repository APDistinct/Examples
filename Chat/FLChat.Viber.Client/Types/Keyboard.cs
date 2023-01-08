using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace FLChat.Viber.Client.Types
{
    public class Keyboard
    {
        public enum KeyboardType
        {
            [EnumMember(Value = "keyboard")]
            Keyboard
        }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyboardType Type { get; set; } = KeyboardType.Keyboard;

        [JsonProperty()]
        public List<Button> Buttons { get; set; } = new List<Button>();
    }
}
