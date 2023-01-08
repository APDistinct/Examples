using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;

namespace FLChat.Core.Buttons.Tests
{
    [TestClass]
    public class TransportButtonsSourseTests
    {
        [TestMethod]
        public void TransportButtonsSourse_Filter_HideForTemporary() {
            ExternalTransportButton obj1 = new ExternalTransportButton() {
                HideForTemporary = true,
                Caption = "hidden"
            };
            ExternalTransportButton obj2 = new ExternalTransportButton() {
                HideForTemporary = false,
                Caption = "open"
            };
            ExternalTransportButton[] btns = new ExternalTransportButton[] { obj1, obj2 };

            MessageToUser mtu = new MessageToUser() {
                ToTransport = new Transport() {
                    User = new User() {
                        IsTemporary = true
                    }
                },
            };
            var list = TransportButtonsSourse.GetButtons(mtu, btns);
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(1, list.First().Count());
            Assert.AreEqual("open", list.First().First().Caption);
        }

        [TestMethod]
        public void TransportButtonsSourse_Filter_MtuNull() {
            ExternalTransportButton obj1 = new ExternalTransportButton() {
                HideForTemporary = true,
                Caption = "hidden"
            };
            ExternalTransportButton obj2 = new ExternalTransportButton() {
                HideForTemporary = false,
                Caption = "open"
            };
            ExternalTransportButton[] btns = new ExternalTransportButton[] { obj1, obj2 };
            
            var list = TransportButtonsSourse.GetButtons(null, btns);
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual(1, list.First().Count());
            Assert.AreEqual("open", list.First().First().Caption);
        }
    }
}
