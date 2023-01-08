using System;
using System.Linq;
using System.Net;
using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class DelUserTransportTests
    {
        ChatEntities entities;
        DelUserTransport handler = new DelUserTransport();
        int TransportKindLimit = 100;
        DAL.Model.User user;

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();
            user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.Viber).Any(),
                u => {
                    u.Enabled = true;                    
                    u.Transports.Add(new DAL.Model.Transport()
                    {
                        Enabled = true,
                        TransportTypeId = (int)TransportKind.Viber,
                        TransportOuterId = "Test for delete",                        
                    });
                });
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }
        /// <summary>
        /// Del transport with code < 100 - successful
        /// </summary>
        [TestMethod]
        public void DelUserTransportTestOK()
        {
            var tr = user.Transports.Where(t => t.TransportTypeId == (int)TransportKind.Viber).First();
            TransportDelRequest tdr = new TransportDelRequest()
            {
                Id = user.Id.ToString(),
                Transport = (TransportKind)tr.TransportTypeId,
            };

            var delUT = new DelUserTransport();
            var ret = delUT.ProcessRequest(entities, user, tdr);
            //Assert.IsNull(ret);
            var tret = user.Transports.Where(t => t.TransportTypeId == tr.TransportTypeId).First();
            Assert.IsFalse(tret.Enabled);
            Assert.IsNotNull(tret.TransportOuterId);
            Assert.IsFalse(tret.TransportOuterId.Any());
        }

        /// <summary>
        /// Del transport with code >= 100 - not successful
        /// </summary>
        [TestMethod]
        public void DelUserTransportTestNoBigCode()
        {
            TransportDelRequest tdr = new TransportDelRequest()
            {
                Id = user.Id.ToString(),
                TransportString = "Email",
                //Transport = DAL.TransportKind.Email,
            };

            try
            {
                var delUT = new DelUserTransport();
                delUT.ProcessRequest(entities, user, tdr);
                Assert.Fail($"Not throwing exception on attempt to delete transport with id >= {TransportKindLimit}");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }
        }

        /// <summary>
        /// Del transport with code >= 100 - not successful
        /// </summary>
        [TestMethod]
        public void DelUserTransportTestNoBadName()
        {
            TransportDelRequest tdr = new TransportDelRequest()
            {
                Id = user.Id.ToString(),
                TransportString = "Something",
                //Transport = DAL.TransportKind.Email,
            };

            try
            {
                var delUT = new DelUserTransport();
                delUT.ProcessRequest(entities, user, tdr);
                Assert.Fail($"Not throwing exception on attempt to delete Bad type of transport ");
            }
            catch (ErrorResponseException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.GetHttpCode());
            }
        }
    }
}
