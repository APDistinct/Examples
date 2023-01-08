using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FLChat.Viber.Client.Types
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class Sender
    {
        public const int NAME_MAX_LENGTH = 28;

        public Sender() {

        }

        public Sender(string name) {
            Name = name;
            if (Name.Length > NAME_MAX_LENGTH)
                Name = Name.Substring(0, NAME_MAX_LENGTH);
        }

        /// <summary>
        /// Sender’s Vi ber name
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Sender’s avatar URL
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Avatar { get; set; }

        public static implicit operator Sender(string name) => new Sender(name);
    }
}
