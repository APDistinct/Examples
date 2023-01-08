using System;
using System.Linq;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.Handlers.Message;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.Message.Tests
{
    public class FakeMessageEditTimeChecker : IMessageEditTimeChecker
    {
        private readonly bool _delay;

        public FakeMessageEditTimeChecker(bool delay = true)
        {
            _delay = delay;            
        }

        public bool Delay(DateTime dt)
        {
            return _delay;
        }
    }

    [TestClass]
    public class MessageEditTests
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
            MessageEditInfo mdi = new MessageEditInfo() { Ids = "12345" };
            MessageEdit md = new MessageEdit(new FakeMessageEditTimeChecker());
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
            MessageEdit md = new MessageEdit(new FakeMessageEditTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.ProcessRequest(entities, to, mdi));
        }

        /// <summary>
        /// Message with DelayedStart, param with Cancelled = true All OK
        /// </summary>
        [TestMethod]
        public void DelayCancell_With_Cancelledtrue()
        {
            // Cancelled = true, DelayedStart not null, cancellChek: true
            //  1.1.1.1
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            Assert.IsNull(msg.DalayedCancelled);
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageEditDelayCancell md = new MessageEditDelayCancell(new FakeMessageEditTimeChecker());
            var ret = md.Perform(entities, msg, mdi);

            entities.Entry(msg).Reload();
            Assert.IsNotNull(msg.DalayedCancelled);            
        }

        /// <summary>
        /// Message with DelayedStart, param with  Cancelled = true 
        /// </summary>
        [TestMethod]
        public void DelayCancell_With_Cancelled()
        {
            //  1.1.1.2.1
            // Cancelled = true, DelayedStart null, delay: false  Timeout
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = null };
            MessageEditDelayCancell md = new MessageEditDelayCancell(new FakeMessageEditTimeChecker(delay: false));
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));

            //  1.2
            // Cancelled = false, msg.DelayedStart not null
            //md = new MessageDelay(new FakeMessageDelayTimeChecker(cancellChek: false));
            mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = false, DelayedStart = null };
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));
        }

        /// <summary>
        /// Message without DelayedStart, param with Cancelled = true 
        /// </summary>
        [TestMethod]
        public void DelayCancell_Message_without_DelayedStart()
        {
            //  1.1.2.1
            // Cancelled = true, msg.DelayedStart null
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: null);
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = true, DelayedStart = null };
            MessageEditDelayCancell md = new MessageEditDelayCancell(new FakeMessageEditTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));
        }

        /// <summary>
        /// Message with DelayedStart, param without Cancelled
        /// </summary>
        [TestMethod]
        public void DelayStart_With_MsgDelayedStart()
        {
            //  2.1.1.1
            // DelayedStart not null  delay : true timeout - OK
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(1));
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageEditDelayStart md = new MessageEditDelayStart(new FakeMessageEditTimeChecker(delay: true));
            var ret = md.Perform(entities, msg, mdi);
            entities.Entry(msg).Reload();
            Assert.IsNotNull(msg.DelayedStart);
            Assert.AreEqual(mdi.DelayedStart.ToString(), msg.DelayedStart.ToString());

            //  2.1.1.2
            // DelayedStart not null  delayCheck : false timeout - NO
            md = new MessageEditDelayStart(new FakeMessageEditTimeChecker(delay: false));
            mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));
        }

        /// <summary>
        /// Message without DelayedStart, param without DelayedStart and Cancelled = true 
        /// </summary>
        [TestMethod]
        public void DelayStart_Message_without_DelayedStart()
        {
            //  2.1.2
            // Cancelled = null, DelayedStart not null, msg.DelayedStart null
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: null);
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageEditDelayStart md = new MessageEditDelayStart(new FakeMessageEditTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));
        }

        /// <summary>
        /// Message without DelayedStart, param without DelayedStart and Cancelled = true 
        /// </summary>
        [TestMethod]
        public void DelayStart_Message_with_DalayedCancelled()
        {
            // Cancelled = null, DelayedStart not null, msg.DelayedStart not null  msg.DalayedCancelled  not null
            msg = entities.SendMessage(from: from.Id, to: to.Id, delayedStart: DateTime.UtcNow.AddDays(2));
            msg.DalayedCancelled = DateTime.UtcNow;
            entities.SaveChanges();
            MessageEditInfo mdi = new MessageEditInfo() { Ids = msg.Id.ToString(), Cancelled = null, DelayedStart = DateTime.UtcNow.AddDays(2) };
            MessageEditDelayStart md = new MessageEditDelayStart(new FakeMessageEditTimeChecker());
            Assert.ThrowsException<ErrorResponseException>(() => md.Perform(entities, msg, mdi));
        }
    }
}
