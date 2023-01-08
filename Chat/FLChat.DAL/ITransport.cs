using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace FLChat.DAL
{

    [DataContract]
    public enum TransportKind
    {
        [EnumMember(Value = "Test")]
        Test = -1,
        [EnumMember(Value = "FLChat")]
        FLChat = 0,
        Telegram = 1,
        WhatsApp = 2,
        Viber = 3,
        VK = 4,
        OK = 5,

        /// <summary>
        /// Send message in web-chat
        /// </summary>
        WebChat = 100,

        /// <summary>
        /// Send message by sms
        /// </summary>
        Sms = 150,

        /// <summary>
        /// Send message by email
        /// </summary>
        Email = 151,
    }

    public interface ITransport
    {
        TransportKind Kind { get; set; }
    }
}
