using FLChat.DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.DataTypes;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes
{
    public class StructureResponse : PartialDataResponse
    {
        [JsonIgnore]
        private readonly StructureNodeFullInfo _node;

        public StructureResponse(StructureNodeFullInfo node, IPartialData partial) : this(node, null, null, null, null, partial) {
        }

        public StructureResponse(StructureNodeFullInfo node, User currUser, 
            Dictionary<Guid, MessageToUser> userLastMessages, 
            Dictionary<Guid, int> unread,
            Dictionary<Guid, List<string>> tags,            
            IPartialData partial,
            HashSet<Guid> broadcastProhibitionStructure = null,
            HashSet<Guid> hasChilds = null) : base(partial)
        {
            _node = node;
            UserList = node.Users.ToUserInfoShort(currUser, userLastMessages, unread, tags, 
                broadcastProhibitionStructure, hasChilds);
        }

        [JsonProperty(PropertyName = "owner")]
        public NodeInfo Node => new NodeInfo(_node.Node);

        [JsonProperty(PropertyName = "users")]
        public IEnumerable<UserInfoShort> UserList { get; }

        [JsonProperty(PropertyName = "nodes")]
        public IEnumerable<NodeInfo> NodeList =>_node.ChildNodes.Select(x => new NodeInfo(x));
    }
}
