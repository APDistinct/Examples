using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Tests
{
    [TestClass]
    public class SettingsDictTests
    {
        [TestMethod]
        public void SettingsDict_GetValueInt() {
            SettingsDict dict = new SettingsDict(new Dictionary<string, string>() {
                { "A", "0" },
                { "B", "1" }
                });
            Assert.AreEqual(0, dict.GetValue("A", 3));
            Assert.AreEqual(1, dict.GetValue("B", 3));
            Assert.AreEqual(3, dict.GetValue("C", 3));
        }

        [TestMethod]
        public void SettingsDict_GetValues() {
            SettingsDict dict = new SettingsDict(new Dictionary<string, string>() {
                { SettingsDict.SettingNames.ONLINE_PERIOD_SEC.ToString(), "50" }
                });
            Assert.AreEqual(50, dict.OnlinePeriodSec);
        }
    }
}
