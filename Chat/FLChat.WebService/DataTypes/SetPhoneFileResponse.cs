using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLChat.WebService.DataTypes
{
    public class SetPhoneFileResponse
    {
        [JsonProperty(PropertyName = "phones_count")]
        public int PhonesCount { get; set; }

        [JsonProperty(PropertyName = "users_count")]
        public int UsersCount { get; set; }
    }
}
