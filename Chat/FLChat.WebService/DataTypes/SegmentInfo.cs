using FLChat.DAL.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public class SegmentInfo
    {
        public SegmentInfo(Segment segment, int? membersCount = null) {
            Id = segment.Id;
            Name = segment.Name.ToString();
            Count = membersCount;
            Descr = segment.Descr;
            IsDeleted = segment.IsDeleted;
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "descr")]
        public string Descr { get; }

        [JsonProperty(PropertyName = "count", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Count { get; }

        [JsonProperty(PropertyName = "is_deleted", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDeleted { get; }
    }
}
