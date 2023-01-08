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
    public class User : Sender
    {
        /// <summary>
        /// Uni que Vi ber user i d of the message sender
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Id { get; set; }

        /// <summary>
        /// Sender’s 2 l etter country code
        /// </summary>
        [JsonProperty]
        public string Country { get; set; }

        /// <summary>
        /// Sender’s phone language. Will  be returned according to the device language
        /// </summary>
        [JsonProperty]
        public string Language { get; set; }

        /// <summary>
        /// The maximal  Viber version that is supported by all of the user’s devices
        /// </summary>
        [JsonProperty]
        public int ApiVersion { get; set; }

        /// <summary>
        /// The operating system type and version of the user’s primary device.
        /// </summary>
        [JsonProperty]
        public string PrimaryDeviceOs { get; set; }

        /// <summary>
        /// The Viber version installed on the user’s primary device
        /// </summary>
        [JsonProperty]
        public string ViberVersion { get; set; }

        /// <summary>
        /// Mobile country code
        /// </summary>
        [JsonProperty]
        public int Mcc { get; set; }

        /// <summary>
        ///  	Mobile network code
        /// </summary>
        [JsonProperty]
        public int Mnc { get; set; }

        /// <summary>
        /// The user’s device type
        /// </summary>
        [JsonProperty]
        public string DeviceType { get; set; }
    }
}
