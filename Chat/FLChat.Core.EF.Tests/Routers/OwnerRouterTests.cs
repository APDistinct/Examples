using System;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.Routers.Tests
{
    [TestClass]
    public class OwnerRouterTests
    {
        OwnerRouter router = new OwnerRouter();

        [TestMethod]
        public void OwnerRouter_Success() {
            Guid ownerId = Guid.NewGuid();
            Transport from = new Transport() {
                User = new User() {
                    OwnerUserId = ownerId,
                    OwnerUser = new User() {
                        Id = ownerId,
                        Transports = new Transport[] {
                            new Transport() {
                                Kind = DAL.TransportKind.FLChat
                            }
                        }
                    }
                }
            };
            Message msg = new Message() {
                FromTransport = from
            };
            Assert.AreEqual(ownerId, router.RouteMessage(null, null, msg));
        }

        [TestMethod]
        public void OwnerRouter_OwnerIsNull() {
            Transport from = new Transport() {
                User = new User() {
                    OwnerUserId = null,
                    OwnerUser = null
                }
            };
            Message msg = new Message() {
                FromTransport = from
            };
            Assert.IsNull(router.RouteMessage(null, null, msg));
        }

        [TestMethod]
        public void OwnerRouter_OwnerHasNotFLChat() {
            Guid ownerId = Guid.NewGuid();
            Transport from = new Transport() {
                User = new User() {
                    OwnerUserId = ownerId,
                    OwnerUser = new User() {
                        Id = ownerId,
                        Transports = new Transport[] {
                            new Transport() {
                                Kind = DAL.TransportKind.Telegram
                            }
                        }
                    }
                }
            };
            Message msg = new Message() {
                FromTransport = from
            };
            Assert.IsNull(router.RouteMessage(null, null, msg));
        }

    }
}
