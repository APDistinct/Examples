using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class MediaTypeTests
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
        public void MediaTypeExtentions_FindMediaType() {
            MediaType orig = entities.GetMediaType();
            Assert.AreEqual(orig.Id, entities.FindMediaType(orig.Name, onlyEnabled: true).Id);
            Assert.AreEqual(orig.Id, entities.FindMediaType(orig.Name, onlyEnabled: false).Id);

            orig = entities.GetMediaType(enabled: false);
            Assert.IsNull(entities.FindMediaType(orig.Name, onlyEnabled: true));
            Assert.AreEqual(orig.Id, entities.FindMediaType(orig.Name, onlyEnabled: false).Id);
        }

        [TestMethod]
        public void MediaTypeExtentions_FindOrCreateMediaType() {
            MediaType orig = entities.GetMediaType();
            Assert.AreEqual(orig.Id, entities.FindOrCreateMediaType(orig.Name, MediaGroupKind.Image, onlyEnabled: true).Id);
            Assert.AreEqual(orig.Id, entities.FindOrCreateMediaType(orig.Name, MediaGroupKind.Image, onlyEnabled: false).Id);

            orig = entities.GetMediaType(enabled: false);
            Assert.IsNull(entities.FindOrCreateMediaType(orig.Name, MediaGroupKind.Image, onlyEnabled: true));
            Assert.AreEqual(orig.Id, entities.FindOrCreateMediaType(orig.Name, MediaGroupKind.Image, onlyEnabled: false).Id);

            string name = Guid.NewGuid().ToString();
            MediaType mt = entities.FindOrCreateMediaType(name, MediaGroupKind.Document, true);
            entities.SaveChanges();
            orig = entities.MediaType.Where(t => t.Name == name).Single();
            Assert.AreEqual(mt.Id, orig.Id);
        }
    }
}
