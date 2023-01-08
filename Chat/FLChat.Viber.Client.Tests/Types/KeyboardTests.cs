using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class KeyboardTests
    {
        [TestMethod]
        public void Keyboard() {
            Keyboard kb = new Types.Keyboard();
            string json = JsonConvert.SerializeObject(kb);
            JObject jo = JObject.Parse(json);
            CollectionAssert.AreEquivalent(new string[] { "Buttons", "Type" }, jo.Properties().Select(p => p.Name).ToArray());
            Assert.AreEqual("keyboard", (string)jo["Type"]);
        }
    }
}
