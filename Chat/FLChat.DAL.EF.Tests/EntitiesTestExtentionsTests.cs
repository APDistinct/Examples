using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class EntitiesTestExtentionsTests
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

        /// <summary>
        /// Check create segment with unique name (create two segments at same time)
        /// </summary>
        [TestMethod]
        public void EntitiesTestExtentions_GetSegment() {
            Segment s1 = null, s2 = null;

            try {
                string descr1 = "new segment" + DateTime.Now.ToString();
                string descr2 = "new segment2" + DateTime.Now.ToString();
                s1 = entities.GetSegment(s => s.Descr == descr1, s => s.Descr = descr1);
                s2 = entities.GetSegment(s => s.Descr == descr2, s => s.Descr = descr2);
            } finally { 
                if (s1 != null)
                    entities.Entry(s1).State = System.Data.Entity.EntityState.Deleted;
                if (s1 != null)
                    entities.Entry(s1).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }            
        }
    }
}

