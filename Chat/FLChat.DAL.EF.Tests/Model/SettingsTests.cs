using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model.Tests
{
    [TestClass]
    public class SettingsTests
    {
        private readonly string InviteLinkStr = "INVITELINK_WORK";
        private readonly string CommonLinkStr = "COMMONLINK_WORK";
        private readonly string GuardStr = "TMP_CACHE";
        private readonly string GuardVal = "Simple";

        //ChatEntities entities;
        //bool needToDelete;
        //Settings settingsElement;

        [TestInitialize]
        public void Init()
        {
            //entities = new ChatEntities();
            //needToDelete = false;
        }

        [TestCleanup]
        public void Clean()
        {
            //if (needToDelete)
            //{
            //    entities.Settings.Remove(settingsElement);
            //    entities.SaveChanges();
            //}
            //entities.Dispose();
        }

        [TestMethod]
        public void Settings_Test()
        {
            using (ChatEntities entities = new ChatEntities())
            {
                Settings[] settings = entities.Settings.ToArray();
                Assert.AreNotEqual(0, settings.Length);
                foreach (Settings s in settings)
                {
                    Assert.AreEqual(s.Value, Settings.Values.GetValue(s.Name, ""));
                }
            }
        }

        [TestMethod]
        public void InviteLink_Test()
        {
            AllTest(InviteLinkStr, "1", (s) => Settings.IsInviteLinkWork);
            //var invL = entities.Settings.Where(x => x.Name == InviteLinkStr).FirstOrDefault();
            //bool needToDelete = false;
            //if(invL == null)
            //{
            //    invL = new Settings() { Name = InviteLinkStr, Value = "1", Descr = "Test for .." };
            //    entities.Settings.Add(invL);
            //    entities.SaveChanges();
            //    needToDelete = true;
            //}

            //Assert.AreEqual(Settings.IsInviteLinkWork, invL.Value == "1");

            //if(needToDelete)
            //{
            //    entities.Settings.Remove(invL);
            //    entities.SaveChanges();
            //}
        }

        [TestMethod]
        public void CommonLink_Test()
        {
            AllTest(CommonLinkStr, "1", (s) => Settings.IsCommonLinkWork);
        }

        [TestMethod]
        public void Guard_Test()
        {
            AllTest(GuardStr, GuardVal, (s) => Settings.IsGuardWork);
        }

        private void AllTest(string name, string val, Func<string, bool> testVal)
        {
            using (ChatEntities entities = new ChatEntities())
            {
                Settings settingsElement = entities.Settings.Where(x => x.Name == name).FirstOrDefault();
                bool needToDelete = false;
                if (settingsElement == null)
                {
                    settingsElement = new Settings() { Name = name, Value = val, Descr = "Test for .." };
                    entities.Settings.Add(settingsElement);
                    entities.SaveChanges();
                    needToDelete = true;
                }
                try
                {
                    Assert.AreEqual(testVal(name), settingsElement.Value == val);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (needToDelete)
                    {
                        entities.Settings.Remove(settingsElement);
                        entities.SaveChanges();
                    }
                }
            }
        }
    }
}
