using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class LimitInfoTests
    {
        [TestMethod]
        public void LimitInfo_Serialization() {
            LimitInfo li = new LimitInfo(
                new DAL.Model.MessageType() { Id = (int)DAL.MessageKind.Broadcast }, 0, 0);
            string json = JsonConvert.SerializeObject(li);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "type", "limit_for_day", "limit_for_once", "already_sent", "selection_count",
                    "exceed_day_limit", "exceed_once_limit", "exhausted" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("Broadcast", (string)jo["type"]);
        }

        [TestMethod]
        public void LimitInfo_Constr_MessageType() {
            LimitInfo li = new LimitInfo(
                new DAL.Model.MessageType() {
                    Id = (int)DAL.MessageKind.Broadcast,
                    LimitForDay = 100,
                    LimitForOnce = 50
                }, 
                10, 20);
            Assert.AreEqual(DAL.MessageKind.Broadcast, li.Type);
            Assert.AreEqual(100, li.LimitForDay);
            Assert.AreEqual(50, li.LimitForOnce);
            Assert.AreEqual(10, li.AlreadySent);
            Assert.AreEqual(20, li.SelectionCount);
        }

        [TestMethod]
        public void LimitInfo_Constr_LimitInfoResult() {
            LimitInfo li = new LimitInfo(DAL.MessageKind.Broadcast, new DAL.DataTypes.LimitInfoResult() {
                DayLimit = 10,
                ExceedDay = false,
                ExceedOnce = false,
                OnceLimit = 20,
                SelectionCount = 30,
                SentOverToday = 40
            });            
            Assert.AreEqual(DAL.MessageKind.Broadcast, li.Type);
            Assert.AreEqual(10, li.LimitForDay);
            Assert.AreEqual(20, li.LimitForOnce);
            Assert.AreEqual(40, li.AlreadySent);
            Assert.AreEqual(30, li.SelectionCount);
        }

        [TestMethod]
        public void LimitInfo_Exhausted() {
            LimitInfo li = new LimitInfo(new DAL.Model.MessageType() { Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = null }, int.MaxValue, 0);
            Assert.IsFalse(li.Exhausted);

            li = new LimitInfo(new DAL.Model.MessageType() { Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10 }, 9, null);
            Assert.IsFalse(li.Exhausted);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10
            }, 10, null);
            Assert.IsTrue(li.Exhausted);
        }

        [TestMethod]
        public void LimitInfo_ExceedCount() {
            LimitInfo li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = null,
                LimitForOnce = null
            }, 0, 10);
            Assert.IsNull(li.ExceedDayLimit);
            Assert.IsNull(li.ExceedOnceLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = null,
                LimitForOnce = 10
            }, 0, 10);
            Assert.AreEqual(0, li.ExceedOnceLimit);
            Assert.IsNull(li.ExceedDayLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = null,
                LimitForOnce = 10
            }, 0, 12);
            Assert.AreEqual(2, li.ExceedOnceLimit);
            Assert.IsNull(li.ExceedDayLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10,
                LimitForOnce = null
            }, 0, 10);
            Assert.AreEqual(0, li.ExceedDayLimit);
            Assert.IsNull(li.ExceedOnceLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10,
                LimitForOnce = null
            }, 0, 12);
            Assert.AreEqual(2, li.ExceedDayLimit);
            Assert.IsNull(li.ExceedOnceLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10,
                LimitForOnce = null
            }, 5, 5);
            Assert.AreEqual(0, li.ExceedDayLimit);
            Assert.IsNull(li.ExceedOnceLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10,
                LimitForOnce = null
            }, 5, 7);
            Assert.AreEqual(2, li.ExceedDayLimit);
            Assert.IsNull(li.ExceedOnceLimit);

            li = new LimitInfo(new DAL.Model.MessageType() {
                Id = (int)DAL.MessageKind.Broadcast,
                LimitForDay = 10,
                LimitForOnce = 4
            }, 5, 7);
            Assert.AreEqual(2, li.ExceedDayLimit);
            Assert.AreEqual(3, li.ExceedOnceLimit);
        }
    }
}
