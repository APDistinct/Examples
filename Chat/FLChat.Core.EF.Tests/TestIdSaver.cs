using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.Core.Tests
{
    class TestIdSaver : ITransportIdSaver
    {
        private readonly List<Tuple<string, MessageToUser>> _ids = new List<Tuple<string, MessageToUser>>();

        public IEnumerable<Tuple<string, MessageToUser>> Ids => _ids;

        public string[] LastIds { get; private set; }

        public void SaveTo(ChatEntities entities, string id, MessageToUser msg) {
            _ids.Add(new Tuple<string, MessageToUser>(id, msg));
        }

        public void SaveFrom(ChatEntities entities, string id, Message msg) {
            throw new NotImplementedException();
        }

        public void SaveTo(ChatEntities entities, string[] ids, MessageToUser msg) {
            _ids.Add(new Tuple<string, MessageToUser>(ids[0], msg));
            LastIds = ids;
        }
    }
}
