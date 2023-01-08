using System;
using System.Data.Entity;
using System.Linq;
using Devino.Viber;
using FLChat.DAL;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.ReportDB.Tests
{
    [TestClass]
    public class DevinoStatsSaverTests
    {
        private ChatEntities entities;
        private DevinoStatsSaver saver = new DevinoStatsSaver();
        //DAL.Model.User[] users;


        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();


            //users = entities.GetUsers(4,
            //    u => u.Enabled,
            //    u => {
            //        u.Enabled = true;
            //    });

            //entities.SaveChanges();
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        [TestMethod]
        public void DevinoStatsSaver_Test_Replace()
        {
            int? mOld, mNew, ret;
            mOld = null;
            mNew = 0;
            ret = saver.Replace(mOld, mNew);
            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Value, mNew.Value);

            mOld = 1;
            mNew = null;
            ret = saver.Replace(mOld, mNew);
            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Value, mOld.Value);

            mOld = 1;
            mNew = 2;
            ret = saver.Replace(mOld, mNew);
            Assert.IsNotNull(ret);
            Assert.AreEqual(ret.Value, mNew.Value);

            mOld = null;
            mNew = null;
            ret = saver.Replace(mOld, mNew);
            Assert.IsNull(ret);
            Assert.AreEqual(ret, mOld);
        }

        [TestMethod]
        public void DevinoStatsSaver_Test_Save()
        {
            //TransportIdSaver saver = new TransportIdSaver();
            var mess = entities.MessageTransportId.Where(x => x.TransportTypeId == (int)TransportKind.WebChat)
                       .Include(m => m.MessageToUser.WebChatDeepLink).FirstOrDefault();
            if (mess == null) Assert.Fail("No messages MessageTransportId");

            var wcdl = mess.MessageToUser.WebChatDeepLink.FirstOrDefault();
            //  Начальное состояние
            if (wcdl == null) Assert.Fail("No messages WebChatDeepLink");
            wcdl.SentTo = null;
            wcdl.SmsStatus = null;
            wcdl.ViberStatus = null;
            wcdl.UpdatedTime = null;
            wcdl.IsFinished = false;
            entities.SaveChanges();

            var trId = mess.TransportId;
            //  Первое изменение
            var dm = new DevinoMessageStatus()
            {
                TransportId = trId,
                SentTo = (int)TransportKind.Viber,
                ViberStatus = (int)ViberStatus.Enqueued,
                SmsStatus = null,
                UpdatedTime = DateTime.UtcNow,
                IsFinished = false,
                Update = true,
            };

            DevinoMessageStatus[] dms = new DevinoMessageStatus[] {dm};
            saver.Save(dms, entities);
            entities.Entry(wcdl).Reload();
                        
            Assert.AreEqual(wcdl.SentTo, dm.SentTo);
            Assert.AreEqual(wcdl.SmsStatus, dm.SmsStatus);
            Assert.IsNotNull(wcdl.ViberStatus);
            Assert.AreEqual(wcdl.ViberStatus.Value, dm.ViberStatus.Value);
            Assert.AreEqual(wcdl.IsFinished, dm.IsFinished);
                        
            // Перезаписываемые изменения
            dm.SentTo = (int)TransportKind.Viber;
            dm.ViberStatus = (int)ViberStatus.Sent;
            dm.IsFinished = false;
            dm.Update = true;
            
            saver.Save(dms, entities);
            entities.Entry(wcdl).Reload();

            //Assert.AreEqual(wcdl.SentTo, dm.SentTo);
            //Assert.AreEqual(wcdl.SmsStatus, dm.SmsStatus);
            Assert.IsNotNull(wcdl.ViberStatus);
            Assert.AreEqual(wcdl.ViberStatus.Value, dm.ViberStatus.Value);
            Assert.AreEqual(wcdl.IsFinished, dm.IsFinished);

            //  Финальное изменение - не фиксируется, только признак завершения
            dm.SentTo = (int)TransportKind.Sms;
            dm.ViberStatus = (int)ViberStatus.Failed;
            dm.SmsStatus = (int)SmsStatus.Delivered;
            dm.IsFinished = false;
            dm.Update = true;

            saver.Save(dms, entities);
            entities.Entry(wcdl).Reload();

            Assert.AreEqual(wcdl.SentTo, dm.SentTo);
            Assert.IsNotNull(wcdl.SmsStatus);
            Assert.AreEqual(wcdl.SmsStatus, dm.SmsStatus);
            Assert.IsNotNull(wcdl.ViberStatus);
            Assert.AreEqual(wcdl.ViberStatus.Value, dm.ViberStatus.Value);
            Assert.AreEqual(wcdl.IsFinished, dm.IsFinished);

            dm.SentTo = (int)TransportKind.Sms;
            dm.ViberStatus = (int)ViberStatus.Unknown;
            //dm.SmsStatus = (int)SmsStatus.Delivered;
            dm.IsFinished = true;
            dm.Update = false;

            saver.Save(dms, entities);
            entities.Entry(wcdl).Reload();

            Assert.AreEqual(wcdl.SentTo, dm.SentTo);
            Assert.IsNotNull(wcdl.SmsStatus);
            Assert.AreEqual(wcdl.SmsStatus, dm.SmsStatus);
            Assert.IsNotNull(wcdl.ViberStatus);
            Assert.AreNotEqual(wcdl.ViberStatus.Value, dm.ViberStatus.Value);
            Assert.IsTrue(wcdl.IsFinished);
            Assert.AreEqual(wcdl.IsFinished, dm.IsFinished);

        }
    }
}
