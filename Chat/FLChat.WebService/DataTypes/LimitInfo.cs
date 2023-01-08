using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using FLChat.DAL;
using Newtonsoft.Json.Serialization;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class LimitInfo
    {
        public LimitInfo(MessageType type, int alreadySent, int? selectionCount) {
            Type = type.Kind;
            LimitForDay = type.LimitForDay;
            LimitForOnce = type.LimitForOnce;
            AlreadySent = alreadySent;
            SelectionCount = selectionCount;
        }

        public LimitInfo(MessageKind kind, DAL.DataTypes.LimitInfoResult li) {
            Type = kind;
            LimitForDay = li.DayLimit;
            LimitForOnce = li.OnceLimit;
            AlreadySent = li.SentOverToday;
            SelectionCount = li.SelectionCount;
        }

        //[JsonIgnore]
        //private MessageType _type { get; }

        [JsonConverter(typeof(StringEnumConverter))]
        public MessageKind Type { get; }

        public int? LimitForDay { get; }

        public int? LimitForOnce { get; }

        public int? AlreadySent { get; }

        public int? SelectionCount { get; }

        public bool Exhausted => LimitForDay.HasValue && AlreadySent >= LimitForDay.Value;

        public int? ExceedDayLimit {
            get {
                if (SelectionCount == null || LimitForDay == null)
                    return null;
                if (SelectionCount.Value + AlreadySent > LimitForDay.Value)
                    return Math.Min(SelectionCount.Value, SelectionCount.Value + (AlreadySent ?? 0) - LimitForDay.Value);
                return 0;
            }
        }

        public int? ExceedOnceLimit {
            get {
                if (SelectionCount == null || LimitForOnce == null)
                    return null;
                if (SelectionCount.Value > LimitForOnce.Value)
                    return SelectionCount.Value - LimitForOnce.Value;
                return 0;
            }
        }
    }
}
