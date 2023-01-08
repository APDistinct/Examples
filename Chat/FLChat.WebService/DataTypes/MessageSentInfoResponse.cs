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

namespace FLChat.WebService.DataTypes
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class MessageSentInfoResponse : MessageSentHistoryItem
    {
        private MessageStatsRowsView[] _rows;

        public MessageSentInfoResponse(MessageStatsGroupedView grouped, MessageStatsRowsView[] rows, Message msg, FileInfo fi = null) 
            : base(grouped, msg, fi) {
            _rows = rows;
        }

        public IEnumerable<MessageSentAddresseeInfo> Recipients => _rows.Select(i => new MessageSentAddresseeInfo(i));
    }
}
