using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.VKBotClient.Types
{
    public interface IVKAction
    {
        string Type { set; get; }
        string Payload { set; get; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VKAction : IVKAction
    {
        [JsonProperty]
        public string Type { set; get; }
        [JsonProperty]
        public string Payload { set; get; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VkKeyboardButton
    {
        public VkKeyboardButton(IVKAction action)
        {
            Action = action;
        }

        [JsonProperty]
        public IVKAction Action;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Color;
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VkKeyboard
    {
        public VkKeyboard()
        {
        }

        public VkKeyboard(VkKeyboardButton[][] buttons)
        {
            Buttons = buttons;
        }

        [JsonProperty]
        public bool OneTime { set; get; }

        [JsonProperty]
        public VkKeyboardButton[][] Buttons;
        //IEnumerable<VkKeyboardButton> Buttons;

        public string GetJson() => JsonConvert.SerializeObject(this);
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Text : VKAction, IVKAction
    {
        public Text()
        {
            Type = "text";
        }
        [JsonProperty]
        public string Label { set; get; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Location : VKAction, IVKAction
    {
        public Location()
        {
            Type = "location";
        }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VKPay	 : VKAction, IVKAction
    {
        public VKPay()
        {
            Type = "vkpay";
        }
        [JsonProperty]
        public string Hash { set; get; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class VKApps : VKAction, IVKAction
    {
        public VKApps()
        {
            Type = "open_app";
        }
        [JsonProperty]
        public string Label { set; get; }
        [JsonProperty]
        public string Hash { set; get; }
        [JsonProperty]
        public int AppId { set; get; }
        [JsonProperty]
        public int OwnerId { set; get; }
    }
}
