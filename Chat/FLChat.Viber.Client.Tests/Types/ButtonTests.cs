using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class ButtonTests
    {
        [TestMethod]
        public void Button_Serialize_Min() {
            Button btn = new Button("text", "action", Button.ActionTypeEnum.None);
            string json = JsonConvert.SerializeObject(btn);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "Text", "ActionType", "ActionBody" },
                jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("none", (string)jo["ActionType"]);
        }

        [TestMethod]
        public void Button_Serialize_All() {
            Button btn = new Button("text", "action", Button.ActionTypeEnum.None) {
                Rows = 2,
                Columns = 2
            };
            string json = JsonConvert.SerializeObject(btn);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(
                new string[] { "Text", "ActionType", "ActionBody", "Columns", "Rows" },
                jo.Properties().Select(p => p.Name).ToArray());            
        }
    }
}
