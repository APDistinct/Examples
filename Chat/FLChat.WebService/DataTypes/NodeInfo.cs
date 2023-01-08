using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;
using FLChat.DAL.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace FLChat.WebService.DataTypes
{
    public class NodeInfo
    {
        private readonly StructureNodeVirtual _node;

        public NodeInfo(StructureNodeVirtual node)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
        }

        [JsonProperty(PropertyName = "id")]
        public string Id => _node.Id;

        [JsonProperty(PropertyName = "name")]
        public string Name => _node.Name;

        [JsonProperty(PropertyName = "count")]
        public int Count => _node.Count;

        [JsonProperty(PropertyName = "final")]
        public bool Final => _node.Final;
    }
}
