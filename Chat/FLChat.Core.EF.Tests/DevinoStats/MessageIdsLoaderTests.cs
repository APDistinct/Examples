using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.DevinoStats.Tests
{
    [TestClass]
    public class MessageIdsLoaderTests
    {
        private ChatEntities entities;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void MessageIdsLoad_Test()
        {
            string sql =
            "SELECT[TransportId] " +
            "FROM[Msg].[MessageTransportId] mt " +
            "INNER JOIN[Msg].[WebChatDeepLink] wcdl on(wcdl.MsgId = mt.MsgId and wcdl.ToUserId = mt.ToUserId) " +
            "WHERE wcdl.IsFinished = 0";
            MessageIdsLoader loader = new MessageIdsLoader();
            var arr1 =  entities.Database.SqlQuery<string>(sql).ToArray();
            var arr2 = loader.Load(entities);
            CollectionAssert.AreEquivalent(arr1, arr2);
        }
    }
}
