using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class LocationTests
    {
        [TestMethod]
        public void Location_Deserialize() {
            string json = File.ReadAllText(".\\Json\\location.json");
            Location location = JsonConvert.DeserializeObject<Location>(json);
            Assert.AreEqual(50.76891, location.Latitude);
            Assert.AreEqual(6.11499, location.Longitude);
        }
    }
}
