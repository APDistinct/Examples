using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.WebService.DataTypes.Tests
{
    [TestClass]
    public class EventInfoTests
    {
        [TestMethod]
        public void EventInfo_IncomeMessage() {
            Guid senderId = Guid.NewGuid();
            Guid currUser = Guid.NewGuid();
            Assert.AreNotEqual(senderId, currUser, "Couple of generetad guids are equal! Shit happens");

            Event ev = new Event() {
                Kind = EventKind.MessageIncome,
                CausedByUserId = senderId,
                CausedByTransportKind = TransportKind.FLChat,
                Message = new Message() {
                    Kind = MessageKind.Personal,
                    PostTm = DateTime.Now,
                    FromUserId = senderId,
                    FromTransportKind = TransportKind.FLChat,
                    Text = "test",
                    ToUsers = new MessageToUser[] {
                        new MessageToUser() {
                            IsSent = true,
                            IsRead = false,
                            ToUserId = currUser,
                            ToTransportKind = TransportKind.FLChat,
                            ToTransport = new DAL.Model.Transport() {
                                TransportType = new TransportType() {
                                    InnerTransport = true
                                }
                            }
                        }
                    }
                }                
            };
            ev.Message.ToUsers.First().Message = ev.Message;

            foreach (EventKind kind in Enum.GetValues(typeof(EventKind))) {
                ev.Kind = kind;
                EventInfo info = new EventInfo(ev, currUser);
                Assert.AreEqual(ev.Kind, info.Kind);
                if (kind == EventKind.MessageIncome) {
                    Assert.IsNull(info.MessageStatus);

                    Assert.IsNotNull(info.Message);
                    Assert.IsTrue(info.Message.Incoming);
                    Assert.AreEqual(senderId, info.Message.FromUserId);
                    Assert.AreEqual(TransportKind.FLChat, info.Message.FromTransport);
                    Assert.AreEqual(ev.Message.PostTm, info.Message.PostTm);
                    Assert.AreEqual(ev.Message.Text, info.Message.Text);

                    Assert.IsInstanceOfType(info.Message, typeof(MessageIncomeInfo));
                    MessageIncomeInfo msgIncomeInfo = info.Message as MessageIncomeInfo;
                    Assert.IsFalse(msgIncomeInfo.IsRead);
                } else {
                    Assert.IsNull(info.Message);
                }
            }
        }

        [TestMethod]
        public void EventInfo_MessageStatus() {
            Guid senderId = Guid.NewGuid();
            Guid causedId = Guid.NewGuid();
            Assert.AreNotEqual(senderId, causedId);
            Event ev = new Event() {
                Kind = EventKind.MessageSent,
                CausedByUserId = causedId,
                CausedByTransportKind = TransportKind.Telegram,
                Message = new Message() {
                    Kind = MessageKind.Personal,
                    PostTm = DateTime.Now,
                    FromUserId = senderId,
                    FromTransportKind = TransportKind.FLChat,
                    Text = "test",
                    ToUsers = new MessageToUser[] {
                        new MessageToUser() {
                            IsSent = true,
                            IsRead = false,
                            ToUserId = causedId,
                            ToTransportKind = TransportKind.FLChat,
                        }
                    }
                }
            };
            ev.Message.ToUsers.First().Message = ev.Message;

            foreach (EventKind kind in Enum.GetValues(typeof(EventKind))) {
                ev.Kind = kind;
                if (kind == EventKind.MessageSent
                    || kind == EventKind.MessageDelivered
                    || kind == EventKind.MessageRead
                    || kind == EventKind.MessageFailed) 
                {
                    EventInfo info = new EventInfo(ev, senderId);
                    Assert.AreEqual(ev.Kind, info.Kind);
                    Assert.IsNull(info.Message);

                    Assert.IsNotNull(info.MessageStatus);
                    Assert.AreEqual(causedId, info.MessageStatus.UserId);
                    Assert.AreEqual(TransportKind.Telegram, info.MessageStatus.TransportKind);
                } else if (kind == EventKind.MessageIncome) {
                    Assert.ThrowsException<ErrorResponseException>(() => new EventInfo(ev, senderId));
                } else {
                    EventInfo info = new EventInfo(ev, senderId);
                    Assert.IsNull(info.MessageStatus);
                }
            }
        }

        [TestMethod]
        public void EventInfo_Serialize() {
            EventInfo ev = new EventInfo(new Event() {
                Id = 0,
                Kind = EventKind.Test,
            }, Guid.Empty);
            string jsonString = JsonConvert.SerializeObject(ev);

            JObject json = JObject.Parse(jsonString);
            Assert.AreEqual("Test", json["kind"]);
        }

        [TestMethod]
        public void Event_DeepLinkAccepted() {
            EventInfo ev = new EventInfo(new Event() {
                Id = 0,
                Kind = EventKind.DeepLinkAccepted,
                CausedByUserId = Guid.NewGuid(),
                CausedByTransportKind = TransportKind.Test
            }, Guid.Empty);

            Assert.IsNull(ev.Message);
            Assert.IsNull(ev.MessageStatus);            
        }
    }
}
