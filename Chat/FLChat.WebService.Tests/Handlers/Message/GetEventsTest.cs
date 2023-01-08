using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FLChat.WebService.Handlers.Message.Tests
{
    [TestClass]
    public class GetEventsTest
    {
        ChatEntities entities;
        GetEvents handler;
        DAL.Model.User[] users;
        Event[] events;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();
            handler = new GetEvents();

            users = entities.GetUsers(4,
                u => u.Enabled,
                u => {
                    u.Enabled = true;
                });

            events = new Event[5];
            for (int i = 0; i < events.Length; ++i) {
                Event e = entities.Event.Add(new Event() {
                    CausedByUserId = users[0].Id,
                    Kind = EventKind.Test,
                    ToUsers = new DAL.Model.User[] {
                        users[1],
                        users[i % 2 == 0 ? 2 : 3]
                    }
                });
                events[i] = e;
            }
            entities.SaveChanges();
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        /// <summary>
        /// Test events response for different users
        /// </summary>
        [TestMethod]
        public void GetEvents_Request() {
            long id = events.Select(e => e.Id).Min() - 1;

            //for user #1
            EventsResponse response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = id });
            Assert.IsNotNull(response);
            Assert.AreEqual(handler.MaxCount, response.MaxCount);
            Assert.AreEqual(events.Last().Id, response.LastId);
            Assert.AreEqual(events.Length, response.Events.Count());
            CollectionAssert.AreEqual(
                events.Select(e => e.Id).OrderBy(e => e).ToArray(),
                response.Events.Select(e => e.Id).OrderBy(e => e).ToArray()
                );

            foreach (EventInfo info in response.Events) {
                Assert.AreEqual(EventKind.Test, info.Kind);
            }

            //for user #2
            Guid userid = users[2].Id;
            response = handler.ProcessRequest(entities, users[2], new EventsRequest() { LastEventId = id });
            Assert.AreNotEqual(events.Length, response.Events.Count());
            CollectionAssert.AreEqual(
                events.Where(e => e.ToUsers.Where(u => u.Id == userid).Any()).Select(e => e.Id).OrderBy(e => e).ToArray(),
                response.Events.Select(e => e.Id).OrderBy(e => e).ToArray()
                );
        }

        /// <summary>
        /// Test limited events response (use Count field)
        /// </summary>
        [TestMethod]
        public void GetEvents_RequestLimited() {
            long id = events.Select(e => e.Id).Min() - 1;

            int cnt = events.Length / 2 + 1;
            Assert.IsTrue(cnt < events.Length);

            //first request for half events
            EventsResponse response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = id, Count = cnt });
            Assert.AreEqual(cnt, response.Events.Count());
            CollectionAssert.AreEqual(
                events.Select(e => e.Id).OrderBy(e => e).Take(cnt).ToArray(),
                response.Events.Select(e => e.Id).OrderBy(e => e).ToArray()
                );

            //second request for another half events
            response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = response.LastId });
            Assert.AreEqual(events.Length - cnt, response.Events.Count());
            CollectionAssert.AreEqual(
                events.Select(e => e.Id).OrderBy(e => e).Skip(cnt).ToArray(),
                response.Events.Select(e => e.Id).OrderBy(e => e).ToArray()
                );

            //third request - empty data
            response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = response.LastId });
            Assert.AreEqual(0, response.Events.Count());
            Assert.AreEqual(events.Last().Id, response.LastId);
        }

        /// <summary>
        /// requested events count > MaxCount
        /// </summary>
        [TestMethod]
        public void GetEvents_RequestOverLimit() {
            long id = events.Select(e => e.Id).Min() - 1;
            int cnt = events.Length / 2 + 1;
            GetEvents handler = new GetEvents() { MaxCount = cnt };

            //first request for half events
            EventsResponse response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = id, Count = events.Length * 2 });
            Assert.AreEqual(cnt, response.Events.Count());
            Assert.AreEqual(cnt, response.MaxCount);
            Assert.AreEqual(events[cnt - 1].Id, response.LastId);
            CollectionAssert.AreEqual(
                events.Select(e => e.Id).OrderBy(e => e).Take(cnt).ToArray(),
                response.Events.Select(e => e.Id).OrderBy(e => e).ToArray()
                );
        }

        [TestMethod]
        public void GetEvents_RequestEmpty() {
            EventsResponse response = handler.ProcessRequest(entities, users[1], new EventsRequest() { LastEventId = events.Last().Id });
            Assert.AreEqual(0, response.Events.Count());
            Assert.AreEqual(events.Last().Id, response.LastId);
        }

        /// <summary>
        /// When send message events was extracted, message's IsDelivered flag must be changed
        /// </summary>
        [TestMethod]
        public void GetEvents_MessageBecomeDelivered() {
            DAL.Model.User[] users = entities.GetUsers(2, u => u.Enabled, null, TransportKind.FLChat);
            DAL.Model.User from = users[0];
            DAL.Model.User to = users[1];

            //make messages
            List<DAL.Model.Message> msgs = new List<DAL.Model.Message>();
            for (int i = 0; i < 3; ++i) {
                msgs.Add(entities.Message.Add(new DAL.Model.Message() {
                    FromTransportKind = TransportKind.FLChat,
                    FromUserId = from.Id,
                    Kind = MessageKind.Personal,
                    Text = "Message #" + i.ToString(),
                    ToUsers = new MessageToUser[] { new MessageToUser() {
                        ToTransportKind = TransportKind.FLChat,
                        IsSent = true,
                        ToUserId = to.Id
                    } }
                }));
            }
            entities.SaveChanges();

            //check delivered before
            Assert.IsFalse(msgs.Select(m => m.ToUsers.Single().IsDelivered).Distinct().Single());

            //extract all events for user to
            List<Guid> eventsMsg = new List<Guid>();
            do {
                EventsResponse response = handler.ProcessRequest(entities, to, new EventsRequest() { });
                Assert.AreNotEqual(0, response.Events.Count());
                eventsMsg.AddRange(response.Events.Where(e => e.Kind == EventKind.MessageIncome).Select(m => m.Message.Id));
            } while (eventsMsg.Intersect(msgs.Select(m => m.Id)).Count() < msgs.Count()); //while has not extracted all our messages

            //reload
            foreach (DAL.Model.Message msg in msgs)
                entities.Entry(msg.ToUsers.Single()).Reload();

            //check delivered after
            Assert.IsTrue(msgs.Select(m => m.ToUsers.Single().IsDelivered).Distinct().Single());
        }

        [TestMethod]
        public void GetEvents_UpdateLastGetEventsProperty() {
            DAL.Model.User user = new DAL.Model.User();
            entities.User.Add(user);
            entities.SaveChanges();

            Assert.IsNull(user.LastGetEvents);

            handler.ProcessRequest(entities, user, new EventsRequest() { Count = 1 });
            entities.Entry(user).Reload();
            Assert.IsNotNull(user.LastGetEvents);
            Assert.IsTrue((DateTime.UtcNow - user.LastGetEvents.Value).TotalSeconds < 1);

            Task.Delay(TimeSpan.FromSeconds(2));

            handler.ProcessRequest(entities, user, new EventsRequest() { Count = 1 });
            entities.Entry(user).Reload();
            Assert.IsNotNull(user.LastGetEvents);
            Assert.IsTrue((DateTime.UtcNow - user.LastGetEvents.Value).TotalSeconds < 1);

            foreach (var t in user.Transports)
                entities.Entry(t).State = System.Data.Entity.EntityState.Deleted;
            entities.Entry(user).State = System.Data.Entity.EntityState.Deleted;
            entities.SaveChanges();
        }
    }
}
