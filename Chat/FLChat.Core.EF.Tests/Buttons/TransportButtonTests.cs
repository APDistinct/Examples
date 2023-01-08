using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

namespace FLChat.Core.Buttons.Tests
{
    [TestClass]
    public class TransportButtonTests
    {
        private ChatEntities entities;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
        }

        [TestCleanup]
        public void Cleanup() {
            entities.Dispose();
        }

        [TestMethod]
        public void TransportButton_Filter_HideForTemporary() {
            ExternalTransportButton obj = new ExternalTransportButton() {
                HideForTemporary = true
            };
            TransportButton btn = new TransportButton(obj);
            MessageToUser mtu = new MessageToUser() {
                ToTransport = new Transport() {
                    User = new User() {
                        IsTemporary = true
                    }
                },
                //Message = new Message() {
                //    FromTransport = new Transport() {
                //        User = new User() {
                //            IsBot = true
                //        }
                //    }
                //}
            };

            Assert.IsFalse(btn.Filter(mtu));
            obj.HideForTemporary = false;
            Assert.IsTrue(btn.Filter(mtu));

            obj.HideForTemporary = true;
            mtu.ToTransport.User.IsTemporary = false;
            Assert.IsTrue(btn.Filter(mtu));
        }
    }
}
