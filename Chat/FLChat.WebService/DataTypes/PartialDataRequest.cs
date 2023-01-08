using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.WebService.DataTypes
{
    public interface IPartialData
    {
        int? Offset { get; set; }

        int? Count { get; set; }

        int? MaxCount { get; set; }
    }

    public static class IPartialDataExtentions
    {
        public static int? Count(this IPartialData data) {
            if (data.Count.HasValue == false && data.MaxCount.HasValue == false)
                return null;
            return Math.Min(Math.Max(0, data.Count ?? int.MaxValue), Math.Max(0, data.MaxCount ?? int.MaxValue));
        }

        public static IEnumerable<T> TakePart<T>(this IEnumerable<T> list, IPartialData partialData, bool skip = true) {
            if (partialData != null) {
                if (skip)
                    list = list.Skip(partialData.Offset ?? 0);
                int? cnt = partialData.Count();
                if (cnt.HasValue)
                    return list.Take(cnt.Value);
                else
                    return list;
            } else
                return list;
        }

        public static IQueryable<T> TakePart<T>(this IQueryable<T> list, IPartialData partialData, bool skip = true) {
            if (partialData != null) {
                if (skip)
                    list = list.Skip(partialData.Offset ?? 0);
                int? cnt = partialData.Count();
                if (cnt.HasValue)
                    return list.Take(cnt.Value);
                else
                    return list;
            } else
                return list;
        }
    }

    public class PartialDataRequest : IPartialData
    {
        [JsonProperty(PropertyName = "offset")]
        public string OffsetString { get; set; }

        [JsonProperty(PropertyName = "count")]
        public string CountString { get; set; }

        [JsonIgnore]
        public int? Offset { get => ToNullableInt(OffsetString); set => OffsetString = value.ToString(); }

        [JsonIgnore]
        public int? Count { get => ToNullableInt(CountString); set => CountString = value.ToString(); }

        [JsonIgnore]
        public int? MaxCount { get ; set; }

        private static int? ToNullableInt(string s) => s != null ? int.Parse(s) : (int?)null;

    }

    public class PartialDataResponse
    {
        public PartialDataResponse(IPartialData data) {
            if (data != null) {
                Offset = data.Offset ?? 0;
                RequestedCount = data.Count();
                MaxCount = data.MaxCount;
            }
        }

        /// <summary>
        /// Offset of current data
        /// </summary>
        [JsonProperty(PropertyName = "offset", Order = 0)]
        public int Offset { get; }

        /// <summary>
        /// Count of records in current data
        /// </summary>
        [JsonProperty(PropertyName = "count", Order = 1)]
        public int Count { get; set; }

        /// <summary>
        /// Maximum allowed count of records in one request
        /// </summary>
        [JsonProperty(PropertyName = "max_count", Order = 2)]
        public int? MaxCount { get;  }

        /// <summary>
        /// Requested count
        /// </summary>
        [JsonProperty(PropertyName = "req_count", Order = 3)]
        public int? RequestedCount { get;  }

        /// <summary>
        /// Total count of records
        /// </summary>
        [JsonProperty(PropertyName = "total_count", Order = 5)]
        public int? TotalCount { get; set; }
    }
}
