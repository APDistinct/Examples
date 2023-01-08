using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;
using FLChat.Core.Algorithms;
using FLChat.Core;
using FLChat.Viber.Bot.Algorithms;
using FLChat.Viber.Client.Types;
using FLChat.Viber.Bot.Adapters;
using FLChat.Viber.Client.Requests;

namespace FLChat.Viber.Bot.Tests
{
    [TestClass]
    public class ViberUpdateHandlerTests
    {
        ViberUpdateHandler Create(
            IReceiveUpdateStrategy<ChatEntities> msg = null,
            IMessageStatusChangedStrategy<ChatEntities> status = null,
            IConversationStartedStrategy conversation = null,
            ISubscribeStrategy<ChatEntities> subscribed = null,
            IUnsubscribeStrategy<ChatEntities> unsubscribed = null) {
            return new ViberUpdateHandler() {
                NewMessageHandler = msg ?? new NewMessageAction(),
                ChangeMessageStatusHandler = status ?? new MessageStatusChangedAction(),
                ConversationStartedHandler = conversation ?? new ConversationStartedAction(),
                SubscribeHandler = subscribed ?? new SubscribeAction(),
                UnsubscribeHandler = unsubscribed ?? new UnsubscribeAction()
            };
        }

        [TestMethod]
        public void ViberUpdateHandler_Message() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Message };
            ViberUpdateHandler handler = Create(msg: new NewMessageAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_DeepLink() {
            bool called = false;
            object result = null;
            CallbackData data = new CallbackData() { Event = CallbackEvent.ConversationStarted, Context = "some data", Subscribed = false };
            ViberUpdateHandler handler = Create(msg: new NewMessageAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }),
            conversation: new ConversationStartedAction((d) => {
                Assert.AreSame(data, d);
                called = true;
                result = new SendTextMessageRequest(new Sender(""), null, "text");
                return (SendTextMessageRequest)result;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNotNull(response);
            Assert.AreSame(response, result);
            Assert.IsTrue(called);
        }       

        [TestMethod]
        public void ViberUpdateHandler_ConversationStarted() {
            bool called = false;
            object result = null;
            CallbackData data = new CallbackData() { Event = CallbackEvent.ConversationStarted, Context = null };
            ViberUpdateHandler handler = Create(conversation: new ConversationStartedAction((d) => {
                Assert.AreSame(data, d);
                called = true;
                result = new SendTextMessageRequest(new Sender(""), null, "text");
                return (SendTextMessageRequest)result;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNotNull(response);
            Assert.AreSame(response, result);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_Delivered() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Delivered };
            ViberUpdateHandler handler = Create(status: new MessageStatusChangedAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_Read() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Seen };
            ViberUpdateHandler handler = Create(status: new MessageStatusChangedAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_Failed() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Failed };
            ViberUpdateHandler handler = Create(status: new MessageStatusChangedAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_Subscribed() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Subscribed };
            ViberUpdateHandler handler = Create(subscribed: new SubscribeAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }

        [TestMethod]
        public void ViberUpdateHandler_Unsubscribed() {
            bool called = false;
            CallbackData data = new CallbackData() { Event = CallbackEvent.Unsubscribed };
            ViberUpdateHandler handler = Create(unsubscribed: new UnsubscribeAction((d) => {
                Assert.AreSame(data, (d as ViberAdapter).Callback);
                called = true;
            }));

            object response = handler.MakeUpdate(data);

            Assert.IsNull(response);
            Assert.IsTrue(called);
        }
    }
}
