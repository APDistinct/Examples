using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FLChat.DAL.Model;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class UserSelectionTests
    {
        [TestMethod]
        public void UserSelection_FieldNames() {
            UserSelection us = new UserSelection();
            string json = JsonConvert.SerializeObject(us);
            JObject jo = JObject.Parse(json);

            CollectionAssert.AreEquivalent(
                new string[] { "include_with_structure", "exclude_with_structure", "exclude", "include", "segments" }, 
                jo.Properties().Select(p => p.Name).ToArray());
        }

        [TestMethod]
        public void UserSelection_StructureSelectionType() {
            string json_deep = "{'user_id': '" + Guid.NewGuid().ToString() + "', 'type': 'deep' }";
            string json_shallow = "{'user_id': '" + Guid.NewGuid().ToString() + "', 'type': 'shallow' }";
            var s = JsonConvert.DeserializeObject<UserSelection.UserStructureSelection>(json_deep);
            Assert.AreEqual(UserSelection.SelectionType.Deep, s.Type);

            s = JsonConvert.DeserializeObject<UserSelection.UserStructureSelection>(json_shallow);
            Assert.AreEqual(UserSelection.SelectionType.Shallow, s.Type);
        }

        [TestMethod]
        public void UserSelection_DeepEnumToInt() {
            List<UserSelection.UserStructureSelection> list = null;
            Assert.IsNull(list.DeepEnumToInt());
            list = new List<UserSelection.UserStructureSelection>() {
                new UserSelection.UserStructureSelection() { Type = UserSelection.SelectionType.Deep, UserId = Guid.NewGuid() },
                new UserSelection.UserStructureSelection() { Type = UserSelection.SelectionType.Shallow, UserId = Guid.NewGuid() },
            };

            Tuple<Guid, int?>[] res = list.DeepEnumToInt().ToArray();
            Assert.AreEqual(list.Count, res.Length);

            CollectionAssert.AreEqual(list.Select(i => i.UserId).ToArray(), res.Select(i => i.Item1).ToArray());
            Assert.IsNull(res[0].Item2);
            Assert.AreEqual(1, res[1].Item2);
        }

        [TestMethod]
        public void UserSelectionExtentions_ToSegmentGuids() {
            Guid[] guids = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
            string[] strings = new string[] { Segment.Prefix + guids[0].ToString(), guids[1].ToString() };
            CollectionAssert.AreEqual(guids, strings.ToSegmentGuids().ToArray());
        }
    }
}
