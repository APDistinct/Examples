using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class ScenarioTests
    {
        [TestMethod]
        public void Scenario_Test()
        {
            using (ChatEntities entities = new ChatEntities())
            {
                Scenario[] scenario = entities.Scenario.ToArray();
                Assert.AreNotEqual(0, scenario.Length);
                foreach (Scenario s in scenario)
                {
                    Assert.AreEqual(s.Id, Scenario.Values.GetValue(s.Name, 0));
                }
            }
        }
    }
}
