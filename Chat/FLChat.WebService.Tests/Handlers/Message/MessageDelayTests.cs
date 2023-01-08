using System;
using System.Linq;
using System.Threading;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Message.Tests
{
    public class FakeMessageDelayTimeChecker : IMessageDelayTimeChecker
    {
        private readonly bool _delayCheck;
        private readonly bool _cancellChek;

        public FakeMessageDelayTimeChecker(bool delayCheck = true, bool cancellChek = true)
        {
            _delayCheck = delayCheck;
            _cancellChek = cancellChek;
        }

        public bool DelayCheck(DateTime dt)
        {
            return _delayCheck;
        }

        public bool CancellCheck(DateTime dt)
        {
            return _cancellChek;
        }
    }

    [TestClass]
    public class MessageDelayTests
    {
        ChatEntities entities;
        DAL.Model.User from;
        DAL.Model.User to;
        DAL.Model.Message msg;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();

            from = entities.GetUserQ(
              where: q => q.Where(u => u.Enabled == true),
              create: u => { u.Enabled = true; }
              );
            to = entities.GetUserQ(
              where: q => q.Where(u => u.Enabled == true && u.Id != from.Id),
              create: u => { u.Enabled = true; }
              );
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        /// <summary>
        /// Bad Message Id
        /// </summary>
        [TestMethod]
        public void TestMethod_BadIds()
        {
            MessageEditInfo mdi = new MessageEditInfo() { Ids = "12345"};
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, null, mdi));
        }

        /// <summary>
        /// Bad From user
        /// </summary>
        [TestMethod]
        public void TestMethod_BadFrom()
        {
            //msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow);
            MessageEditInfo mdi = new MessageEditInfo() { Ids = "12345" };
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));
        }

        /// <summary>
        /// Message with DelayedStart, param with DelayedStart Cancelled = true All OK
        /// </summary>
        [TestMethod]
        public void TestMethod_With_DelayedStart_Cancelledtrue()
        {
            // Cancelled = true, DelayedStart not null, cancellChek: true
            //  1.1.1.1
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker());
            var ret = md.ProcessRequest(entities, from, mdi);
            //entities.Entry(msg).Reload();
            Assert.IsNotNull(ret.Cancelled);
            Assert.IsTrue(ret.Cancelled.Value);
        }

        /// <summary>
        /// Message with DelayedStart, param without DelayedStart and Cancelled = true 
        /// </summary>
        [TestMethod]
        public void TestMethod_With_Cancelled()
        {
            //  1.1.1.2.1
            // Cancelled = true, DelayedStart null, cancellChek: false
            
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = null };
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker(cancellChek: false));
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));

            //  1.2
            // Cancelled = false, DelayedStart null
            //md = new MessageDelay(new FakeMessageDelayTimeChecker(cancellChek: false));
            mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = false, DelayedStart = null };
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));
        }

        /// <summary>
        /// Message with DelayedStart, param without Cancelled
        /// </summary>
        [TestMethod]
        public void TestMethod_Without_Cancelled()
        {
            //  2.1.1.1
            // DelayedStart not null  delayCheck : true
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker(delayCheck : true));
            var ret = md.ProcessRequest(entities, from, mdi);
            entities.Entry(msg).Reload();
            Assert.IsNotNull(ret.DelayedStart);
            Assert.AreEqual(ret.DelayedStart.ToString(), msg.DelayedStart.ToString());

            //  2.1.1.2
            // DelayedStart not null  delayCheck : false
            md = new MessageDelay(new FakeMessageDelayTimeChecker(delayCheck : false));
            mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));
        }

        /// <summary>
        /// Message without DelayedStart, param without DelayedStart and Cancelled = true 
        /// </summary>
        [TestMethod]
        public void TestMethod_Message_without_DelayedStart()
        {
            //  1.1.2.1
            // Cancelled = true, DelayedStart null, msg.DelayedStart null
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: null);
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = null };
            MessageDelay md = new MessageDelay(new FakeMessageDelayTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));

            //  2.1.2
            // Cancelled = null, DelayedStart not null, msg.DelayedStart null            
            mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            md = new MessageDelay(new FakeMessageDelayTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));
        }
    }
}
