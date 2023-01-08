using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes.Converters;
using static FLChat.WebService.DataTypes.MessageSentHistoryStats;

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageSentHistoryItem : MessageContentInfo
    {
        public MessageSentHistoryItem(MessageStatsGroupedView stats, Message msg, FileInfo fi = null) : base(msg, fi) {
            Stats = new MessageSentHistoryStats(stats, msg.DalayedCancelled != null);
            //if (msg.DalayedCancelled != null)
                //Stats.State = MessageSentHistoryStats.SendingState.Cancelled;
        }
        //public override SendingState State => msg.DalayedCancelled != null ? MessageSentHistoryStats.SendingState.Cancelled : State;
        public MessageSentHistoryStats Stats { get; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UserInfoBase User { get; set; }
    }
}
