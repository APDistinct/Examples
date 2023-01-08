using System;
using System.Linq;
using System.Collections.Generic;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.WebService.DataTypes;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class SegmentTests
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
        public void Segment_Create() {
            Segment s = new Segment() {
                Name = DateTime.Now.ToString(),
                Descr = "some descr"
            };
            entities.Segment.Add(s);
            entities.SaveChanges();
            entities.Entry(s).Reload();

            Assert.AreNotEqual(Guid.Empty, s.Id);
            Assert.IsFalse(s.IsDeleted);

            entities.Entry(s).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }        

        [TestMethod]
        public void SegmentExtentions_LoadShortInfoSegments() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true && s.Tag == null,
                s => {
                    s.IsDeleted = false;
                    s.ShowInShortProfile = true;
                    s.Name = DateTime.Now.ToString();
                    s.Descr = s.Name;
                });
            User user = entities.GetUser(u => u.Segments.Contains(segment), u => u.Segments.Add(segment));
            Dictionary<Guid, List<string>> dict = entities.LoadShortInfoSegments(new Guid[] { user.Id });
            Assert.IsNotNull(dict);
            Assert.IsTrue(dict.ContainsKey(user.Id));
            Assert.IsTrue(dict[user.Id].Contains(segment.Name));
        }

        [TestMethod]
        public void SegmentExtentions_LoadShortInfoSegments_Tag() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true && s.Tag != null,
                s => {
                    s.IsDeleted = false;
                    s.ShowInShortProfile = true;
                    s.Name = DateTime.Now.ToString();
                    s.Descr = s.Name;
                    s.Tag = "tag1";
                });
            User user = entities.GetUser(u => u.Segments.Contains(segment), u => u.Segments.Add(segment));
            Dictionary<Guid, List<string>> dict = entities.LoadShortInfoSegments(new Guid[] { user.Id });
            Assert.IsNotNull(dict);
            Assert.IsTrue(dict.ContainsKey(user.Id));
            Assert.IsTrue(dict[user.Id].Contains(segment.Tag));
        }
    }
}
