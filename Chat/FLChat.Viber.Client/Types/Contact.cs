using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Contact
    {
        /// <summary>
        /// contact’s username
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// contact’s phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// the avatar URL
        /// </summary>
        public string Avatar { get; set; }
    }
}
