using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class GroupInfoSet 
    {
        public GroupInfoSet()
        {
            _commonInfo = new Dictionary<string, JToken>();
        }
        [JsonExtensionData]
        //public IDictionary<string, JToken> _additionalData;
        public IDictionary<string, JToken> _commonInfo; // { get; set; }
        [JsonProperty(PropertyName = "group_id")]
        public string Id ;
        [JsonProperty(PropertyName = "members")]
        public IEnumerable<Guid> Members;
        [JsonProperty(PropertyName = "admins")]
        public IEnumerable<Guid> Admins;
        [JsonProperty(PropertyName = "remove_members")]
        public IEnumerable<Guid> RemoveMmembers;
        [JsonProperty(PropertyName = "remove_admins")]
        public IEnumerable<Guid> RemoveAdmins;

        //[OnDeserialized]
        //private void OnDeserialized(StreamingContext context)
        //{            
        //    _commonInfo = _additionalData;
        //}

        //[JsonProperty(PropertyName = "group_id")]
        //public Guid Id ;
        //[JsonProperty(PropertyName = "name")]
        //public string Name ;
        //[JsonProperty(PropertyName = "created_by_user_id")]
        //public Guid CreatedByUserId ;
        //[JsonProperty(PropertyName = "is_equal")]
        //public bool IsEqual ;
        //[JsonProperty(PropertyName = "created_date")]
        //public DateTime CreatedDate ;         
    }
}
