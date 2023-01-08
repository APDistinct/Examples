using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class StructureNodeTests
    {
        ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void StructureNode_Create() {
            StructureNode n = new StructureNode() {
                Id = Guid.NewGuid(),
                Name = "Create test",      
                ParentNodeId = Guid.Empty
            };
            entities.StructureNode.Add(n);
            entities.SaveChanges();

            Assert.AreNotEqual(Guid.Empty, n.Id);

            entities.Entry(n).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }

        [TestMethod]
        public void StructureNode_GetInfoTest_Set1()
        {
            User user = entities.GetUser(u => u.Enabled, u => { });
            var tupleret = entities.ExecuteStructureNode_GetInfo(null/*"nod-"+Guid.Empty.ToString()*/, user.Id, null, null);
            Assert.IsNotNull(tupleret);
            Assert.IsNotNull(tupleret.Node);
        }

        [TestMethod]
        public void StructureNode_GetInfoTest_Set2()
        {
            User user = entities.GetUser(u => u.Enabled, u => { });
            var tupleret = entities.ExecuteStructureNode_GetInfo(null/*"nod-"+Guid.Empty.ToString()*/, user.Id, null, null);
            if (tupleret != null)
            {
                Assert.IsNotNull(tupleret.ChildNodes);
            }
        }
    }
}
