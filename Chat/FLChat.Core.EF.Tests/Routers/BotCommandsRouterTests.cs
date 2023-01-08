using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class BotCommandsRouterTests
    {
        //BotCommandsRouter router = new BotCommandsRouter();

        [TestMethod]
        public void BotCommandsRouter_GetCommandType() {
            string arg;
            Assert.AreEqual(BotCommandsRouter.CommandsEnum.SelectAddressee, BotCommandsRouter.GetCommandType("cmd:select_addressee", out arg));
            Assert.AreEqual(BotCommandsRouter.CommandsEnum.MyScore, BotCommandsRouter.GetCommandType("cmd:score", out arg));
            Assert.AreEqual(BotCommandsRouter.CommandsEnum.MyProfile, BotCommandsRouter.GetCommandType("cmd:profile", out arg));
            Assert.AreEqual(BotCommandsRouter.CommandsEnum.Url, BotCommandsRouter.GetCommandType("url:www.ya.ru", out arg));
            Assert.AreEqual("www.ya.ru", arg);

            Assert.IsNull(BotCommandsRouter.GetCommandType("some text", out arg));
        }

        //[TestMethod]
        //public void BotCommandsRouter_Nothing() {
        //    using (ChatEntities entities = new ChatEntities()) {
        //        Transport from = new Transport() {
        //            User = new User() { FullName = "name" }
        //        };
        //        Message dbmsg = new Message() {
        //            FromTransport = from
        //        };

        //        FakeOuterMessage msg = new FakeOuterMessage() { };
        //        Assert.IsNull(router.RouteMessage(entities, msg, dbmsg));
        //    }
        //}

        //[TestMethod]
        //public void BotCommandsRouter_Score() {
        //    using (ChatEntities entities = new ChatEntities()) {
        //        Transport from = new Transport() {
        //            User = new User() { FullName = "name" }
        //        };
        //        Message dbmsg = new Message() {
        //            FromTransport = from
        //        };

        //        FakeOuterMessage msg = new FakeOuterMessage() { IsScoreRequest = true };
        //        Assert.AreEqual(DAL.Global.SystemBotId, router.RouteMessage(entities, msg, dbmsg));

        //        Message newmsg = entities.ChangeTracker.Entries<DAL.Model.Message>().FirstOrDefault().Entity;
        //        Assert.AreEqual(entities.SystemBotTransport, newmsg.FromTransport);
        //        Assert.AreEqual(dbmsg, newmsg.AnswerTo);
        //        Assert.AreEqual(from, newmsg.ToUsers.Single().ToTransport);

        //       // entities.Entry(newmsg).State = System.Data.Entity.EntityState.Detached;
        //    }
        //}

        //[TestMethod]
        //public void BotCommandsRouter_Profile() {
        //    using (ChatEntities entities = new ChatEntities()) {
        //        Transport from = new Transport() {
        //            User = new User() { FullName = "name" }
        //        };
        //        Message dbmsg = new Message() {
        //            FromTransport = from
        //        };

        //        FakeOuterMessage msg = new FakeOuterMessage() { IsProfileRequested = true };
        //        Assert.AreEqual(DAL.Global.SystemBotId, router.RouteMessage(entities, msg, dbmsg));

        //        Message newmsg = entities.ChangeTracker.Entries<DAL.Model.Message>().FirstOrDefault().Entity;
        //        Assert.AreEqual(entities.SystemBotTransport, newmsg.FromTransport);
        //        Assert.AreEqual(dbmsg, newmsg.AnswerTo);
        //        Assert.AreEqual(from, newmsg.ToUsers.Single().ToTransport);

        //        // entities.Entry(newmsg).State = System.Data.Entity.EntityState.Detached;
        //    }
        //}
    }
}
