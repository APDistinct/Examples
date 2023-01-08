using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Info.Tests
{
    [TestClass]
    public class GetRankTests
    {
        readonly GetRank handler = new GetRank();
        ChatEntities entities;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();            
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void GetRankTest()
        {
            var list = entities.Rank.Select(r => r.Name).ToList();
            var response = handler.ProcessRequest(entities, null, null);
            Assert.AreEqual(list.Count, response.Ranks.Count());
            CollectionAssert.AreEquivalent(list, response.Ranks.ToList());
        }
    }
}
