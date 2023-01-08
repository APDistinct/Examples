using System;
using System.Linq;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.WebService.DataTypes;
using System.Net;
using FLChat.DAL;

namespace FLChat.WebService.Handlers.Auth.Tests
{
    [TestClass]
    public class LoginTests
    {
        ChatEntities entities;
        Random rnd = new Random();
        //FakeSender fakeSender = new FakeSender();
        Login login;

        //private class FakeSender : Login.ICodeSender
        //{
        //    public string Phone { get; private set; }
        //    public string Code { get; private set; }
        //    public bool IsSent { get; private set; } = false;

        //    public void Send(string phone, string code) {
        //        Phone = phone;
        //        Code = code;
        //        IsSent = true;
        //    }

        //    public void Clear() {
        //        Phone = null;
        //        Code = null;
        //        IsSent = false;
        //    }
        //}

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            login = new Login();

            //from = entities.GetUser(
            //    u => u.Enabled && u.EnabledInnerTransport,
            //    u => u.EnabledInnerTransport = true);
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void Login_MaxValue() {
            Login l = new Login();
            l.CodeDigitsCount = 4;
            Assert.AreEqual(4, l.CodeDigitsCount);
            Assert.AreEqual(10000, l.CodeMaxValue);

            l.CodeDigitsCount = 6;
            Assert.AreEqual(6, l.CodeDigitsCount);
            Assert.AreEqual(1000000, l.CodeMaxValue);
        }

        [TestMethod]
        public void Login_NotConsultant() {
            DAL.Model.User user = entities.GetUser(
                u => u.IsConsultant == false && u.Phone != null,
                u => { u.IsConsultant = false; u.Phone = rnd.Next(1000000000).ToString(); }
                );
            try {
                login.ProcessRequest(entities, null, new LoginRequest() { Phone = user.Phone });
                Assert.Fail("Not throwing exception on login for non consultant user");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.Unauthorized, e.GetHttpCode());
            }
        }

        [TestMethod]
        public void Login_ComplexSuccess() {
            DAL.Model.User user = entities.GetUser(
                u => u.IsConsultant == true && u.Phone != null,
                u => { u.IsConsultant = true; u.Phone = rnd.Next(1000000000).ToString(); }
                );
            //clear sms code for user
            if (user.SmsCode != null) {
                entities.Entry(user.SmsCode).State = System.Data.Entity.EntityState.Deleted;
                user.SmsCode = null;
                entities.SaveChanges();
            }

            LoginRequest req = new LoginRequest() { Phone = user.Phone };

            //first request
            LoginResponse resp = login.ProcessRequest(entities, null, req);
            Assert.IsNotNull(resp);
            Assert.AreEqual(LoginResponse.StatusEnum.Sent, resp.Status);
            Assert.IsNull(resp.WaitingTime);
            SmsCode dbcode = entities.SmsCode.Where(s => s.UserId == user.Id).Single();
            Assert.IsTrue(dbcode.Code > 0);
            Assert.AreEqual(login.ExpireBy, dbcode.ExpireBySec);
            Assert.IsTrue(dbcode.IssueDate < DateTime.Now.AddSeconds(5));

            MessageToUser msg = entities.MessageToUser.Where(mtu => mtu.Message.FromUserId == Global.SystemBotId
                && mtu.ToUserId == user.Id).OrderByDescending(mtu => mtu.Idx).FirstOrDefault();
            Assert.IsNotNull(msg);
            Assert.IsFalse(msg.IsSent);
            Assert.IsFalse(msg.IsFailed);
            Assert.IsFalse(msg.Message.IsDeleted);
            Assert.IsTrue(msg.Message.Text.Contains(dbcode.Code.ToString()));
            long idx = msg.Idx;

            int firstCode = dbcode.Code;
            DateTime firstDate = dbcode.IssueDate;

            //second request, must be Wait status and has non sent, because second request come too early
            resp = login.ProcessRequest(entities, null, req);
            Assert.IsNotNull(resp);            
            Assert.AreEqual(LoginResponse.StatusEnum.Waiting, resp.Status);
            Assert.IsNotNull(resp.WaitingTime);
            Assert.IsTrue(resp.WaitingTime.Value > 0);
            dbcode = entities.SmsCode.Where(s => s.UserId == user.Id).Single();
            Assert.AreEqual(firstCode, dbcode.Code);
            Assert.AreEqual(login.ExpireBy, dbcode.ExpireBySec);
            Assert.AreEqual(firstDate, dbcode.IssueDate);
            
            Assert.IsTrue(login.ExpireBy > login.MinIntervalBetweenSms, "Expire time must be greather then minimal distance");

            msg = entities.MessageToUser.Where(mtu => mtu.Message.FromUserId == Global.SystemBotId
                && mtu.ToUserId == user.Id).OrderByDescending(mtu => mtu.Idx).FirstOrDefault();
            Assert.AreEqual(idx, msg.Idx);

            //spend time
            dbcode.IssueDate = firstDate.AddSeconds( -(login.MinIntervalBetweenSms + 1));
            entities.SaveChanges();

            //we "waited" some time and make another try
            // we will done and sms code must be same, because time has not expired
            resp = login.ProcessRequest(entities, null, req);
            Assert.IsNotNull(resp);
            Assert.AreEqual(LoginResponse.StatusEnum.Sent, resp.Status);
            Assert.IsNull(resp.WaitingTime);
            dbcode = entities.SmsCode.Where(s => s.UserId == user.Id).Single();
            Assert.AreEqual(firstCode, dbcode.Code);
            Assert.AreEqual(login.ExpireBy, dbcode.ExpireBySec);
            Assert.IsTrue(dbcode.IssueDate < DateTime.Now.AddSeconds(5));

            msg = entities.MessageToUser.Where(mtu => mtu.Message.FromUserId == Global.SystemBotId
                && mtu.ToUserId == user.Id).OrderByDescending(mtu => mtu.Idx).FirstOrDefault();
            Assert.AreNotEqual(idx, msg.Idx);   //new message
            Assert.IsNotNull(msg);
            Assert.IsFalse(msg.IsSent);
            Assert.IsFalse(msg.IsFailed);
            Assert.IsFalse(msg.Message.IsDeleted);
            Assert.IsTrue(msg.Message.Text.Contains(dbcode.Code.ToString()));
            idx = msg.Idx;

            //spend time
            dbcode.IssueDate = dbcode.IssueDate.AddSeconds(-(login.ExpireBy + 1));

            //make another request
            // we will done and sms code must be new
            resp = login.ProcessRequest(entities, null, req);
            Assert.IsNotNull(resp);
            Assert.AreEqual(LoginResponse.StatusEnum.Sent, resp.Status);
            Assert.IsNull(resp.WaitingTime);
            dbcode = entities.SmsCode.Where(s => s.UserId == user.Id).Single();
            Assert.AreNotEqual(firstCode, dbcode.Code);
            Assert.AreEqual(login.ExpireBy, dbcode.ExpireBySec);
            Assert.IsTrue(dbcode.IssueDate < DateTime.Now.AddSeconds(5));

            msg = entities.MessageToUser.Where(mtu => mtu.Message.FromUserId == Global.SystemBotId
                && mtu.ToUserId == user.Id).OrderByDescending(mtu => mtu.Idx).FirstOrDefault();
            Assert.AreNotEqual(idx, msg.Idx);   //new message
            Assert.IsNotNull(msg);
            Assert.IsFalse(msg.IsSent);
            Assert.IsFalse(msg.IsFailed);
            Assert.IsFalse(msg.Message.IsDeleted);
            Assert.IsTrue(msg.Message.Text.Contains(dbcode.Code.ToString()));
            idx = msg.Idx;
        }

        [TestMethod]
        public void Login_EmptyPhone() {
            Login l = new Login();

            try {
                l.ProcessRequest(entities, null, new LoginRequest() { Phone = "" });
                Assert.Fail("Not throwing exception on login for non consultant user");
            } catch (ErrorResponseException e) {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }
        }
    }
}
