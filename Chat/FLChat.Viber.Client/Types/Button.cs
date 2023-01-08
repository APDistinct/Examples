using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Types
{
    public class Button
    {
        public enum ActionTypeEnum
        {
            [EnumMember(Value = "reply")]
            Reply,
            
            [EnumMember(Value = "open-url")]
            OpenUrl,

            [EnumMember(Value = "location-picker")]
            LocationPicker,

            [EnumMember(Value = "share-phone")]
            Phone,

            [EnumMember(Value = "none")]
            None
        }

        public Button() {

        }

        public Button(string text, string actionBody, ActionTypeEnum actionType = ActionTypeEnum.Reply) {
            Text = text;
            ActionBody = actionBody;
            ActionType = actionType;
        }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public ActionTypeEnum ActionType { get; set; }

        [JsonProperty]
        public string ActionBody { get; set; }

        [JsonProperty]
        public string Text { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Columns { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Rows { get; set; }
    }
}
