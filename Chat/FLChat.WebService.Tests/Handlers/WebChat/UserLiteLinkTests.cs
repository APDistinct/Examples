using System;
using System.Linq;
using System.Net;
using FLChat.Core.Algorithms;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers.WebChat;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.WebChat.Tests
{
    [TestClass]
    public class UserLiteLinkTests
    {
        UserLiteLink handler = new UserLiteLink();
        ChatEntities entities;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void TestMethodBadNumber_NO()
        {
            string code = "123abc";
            var e = Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, null, code));
            e.Check(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error);
        }

        [TestMethod]
        public void TestMethodWithoutUser_OK()
        {
            Random rnd = new Random();
            var c = entities.User.Max(u => u.FLUserNumber) ?? 0;
            string code = /*rnd.Next(10000000, 1000000000)*/(c+1).ToString();            
            var resp = handler.ProcessRequest(entities, null, code);

            Assert.AreEqual(resp.User, null);
            foreach (var item in resp.InviteButtons)
            {
                //Assert.IsTrue(item.Url.Contains(code));
                Assert.AreEqual(item.Status, DataTypes.TransportStatus.None);
            }
            CollectionAssert.AreEquivalent(
                entities.TransportType.Where(tt => tt.DeepLink != null).ToArray().Select(tt => tt.Kind).ToArray(),
                resp.InviteButtons.Select(i => i.Transport).ToArray());
        }

        [TestMethod]
        public void TestMethodAll_OK()
        {
            var c = entities.User.Select(z => z.FLUserNumber).OrderByDescending(u => u).FirstOrDefault() ?? 0;
            DAL.Model.User user = entities.GetUser(
                u => u.Enabled && u.FLUserNumber != null 
                && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Viber).Any()
                && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.VK).Any(),
                u => {
                    u.Enabled = true;
                    u.FLUserNumber = u.FLUserNumber ?? c + 1;
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        TransportTypeId = (int)TransportKind.Viber,
                        TransportOuterId = Guid.NewGuid().ToString() /*"Test for UserLiteLink"*/,
                    });
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        TransportTypeId = (int)TransportKind.VK,
                        TransportOuterId = Guid.NewGuid().ToString(),
                    });
                });

            //var gen = new LiteDeepLinkStrategy();

            string code = user.FLUserNumber.ToString() /*(c + 1).ToString()/*gen.Generate(user)*/;            
            var resp = handler.ProcessRequest(entities, entities.SystemBot, code);

            Assert.AreEqual(user.Id, resp.User.Id);
            Assert.AreEqual(user.FLUserNumber, resp.User.FLUserNumber);
            var list = user.Transports.ToList();
            foreach (var item in resp.InviteButtons)
            {
                var tr = list.Where(x => x.TransportTypeId == (int)item.Transport).FirstOrDefault();
                if(tr == null)
                {
                    Assert.AreEqual(item.Status, DataTypes.TransportStatus.None);
                }
                else
                {
                    if(tr.Enabled)
                        Assert.AreEqual(item.Status, DataTypes.TransportStatus.Subscribed);
                    else
                        Assert.AreEqual(item.Status, DataTypes.TransportStatus.Unsubscribed);
                }
                //Assert.IsTrue(item.Url.Contains(code));
            }
            CollectionAssert.AreEquivalent(
                entities.TransportType.Where(tt => tt.DeepLink != null).ToArray().Select(tt => tt.Kind).ToArray(),
                resp.InviteButtons.Select(i => i.Transport).ToArray());
        }
    }
}
