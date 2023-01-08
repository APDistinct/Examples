using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;
using System.Reflection;

namespace FLChat.Viber.Client.Types.Tests
{
    [TestClass]
    public class CallbackDataTests
    {
        [TestMethod]
        public void CallbackData_Deserialize_Common() {
            CallbackData data = Read("callback_message");
            Assert.AreEqual(CallbackEvent.Message, data.Event);
            Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(1457764197627), data.Timestamp);
            Assert.AreEqual(4912661846655238145, data.MessageToken);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Message() {
            CallbackData data = Read("callback_message");
            Assert.AreEqual(CallbackEvent.Message, data.Event);
            CheckFieldsIsNull(data, nameof(data.Sender), nameof(data.Message));

            User sender = data.Sender;
            Assert.AreEqual("01234567890A=", sender.Id);
            Assert.AreEqual("John McClane", sender.Name);
            Assert.AreEqual("http://avatar.example.com", sender.Avatar);
            Assert.AreEqual("UK", sender.Country);
            Assert.AreEqual("en", sender.Language);
            Assert.AreEqual(1, sender.ApiVersion);

            Message msg = data.Message;
            Assert.AreEqual(MessageType.Text, msg.Type);
            Assert.AreEqual("a message to the service", msg.Text);
            Assert.AreEqual(@"http://example.com", msg.Media);
            Assert.IsNotNull(msg.Location);
            Assert.AreEqual("tracking data", msg.TrackingData);
            Assert.IsNull(msg.Contact);
            Assert.IsNull(msg.FileName);
            Assert.IsNull(msg.FileSize);
            Assert.IsNull(msg.Duration);
            Assert.IsNull(msg.StickerId);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Subscribed() {
            CallbackData data = Read("callback_subscribed");
            Assert.AreEqual(CallbackEvent.Subscribed, data.Event);
            CheckFieldsIsNull(data, nameof(data.User));

            User user = data.User;
            Assert.AreEqual("01234567890A=", user.Id);
            Assert.AreEqual("John McClane", user.Name);
            Assert.AreEqual("http://avatar.example.com", user.Avatar);
            Assert.AreEqual("UK", user.Country);
            Assert.AreEqual("en", user.Language);
            Assert.AreEqual(1, user.ApiVersion);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Unsubscribed() {
            CallbackData data = Read("callback_unsubscribed");
            Assert.AreEqual(CallbackEvent.Unsubscribed, data.Event);
            CheckFieldsIsNull(data, nameof(data.UserId));
            Assert.AreEqual("01234567890A=", data.UserId);
        }

        [TestMethod]
        public void CallbackData_Deserialize_ConversationStarted() {
            CallbackData data = Read("callback_conversation_started");
            Assert.AreEqual(CallbackEvent.ConversationStarted, data.Event);
            CheckFieldsIsNull(data, new string[] { nameof(data.User), nameof(data.Type), nameof(data.Context), nameof(data.Subscribed) });

            Assert.AreEqual(ConversationStartedType.Open, data.Type);
            Assert.AreEqual("context information", data.Context);
            Assert.AreEqual(false, data.Subscribed);
        }

        [TestMethod]
        public void CallbackData_Deserialize_ConversationStarted_Subscribed() {
            CallbackData data = Read("callback_conversation_started_subscribed");
            Assert.AreEqual(CallbackEvent.ConversationStarted, data.Event);

            Assert.AreEqual(ConversationStartedType.Open, data.Type);
            Assert.AreEqual(true, data.Subscribed);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Delivered() {
            CallbackData data = Read("callback_delivered");
            Assert.AreEqual(CallbackEvent.Delivered, data.Event);
            CheckFieldsIsNull(data, nameof(CallbackData.UserId));

            Assert.AreEqual("01234567890A=", data.UserId);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Seen() {
            CallbackData data = Read("callback_seen");
            Assert.AreEqual(CallbackEvent.Seen, data.Event);
            CheckFieldsIsNull(data, nameof(CallbackData.UserId));

            Assert.AreEqual("01234567890A=", data.UserId);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Failed() {
            CallbackData data = Read("callback_failed");
            Assert.AreEqual(CallbackEvent.Failed, data.Event);
            CheckFieldsIsNull(data, nameof(CallbackData.UserId), nameof(CallbackData.Description));

            Assert.AreEqual("01234567890A=", data.UserId);
            Assert.AreEqual("failure description", data.Description);
        }

        [TestMethod]
        public void CallbackData_Deserialize_Webhook() {
            CallbackData data = Read("callback_webhook");
            Assert.AreEqual(CallbackEvent.Webhook, data.Event);
            CheckFieldsIsNull(data, nameof(CallbackData.ChatHostname));
        }

        private static readonly string[] commonFields 
            = new string [] { nameof(CallbackData.Event), nameof(CallbackData.Timestamp), nameof(CallbackData.MessageToken) };

        private void CheckFieldsIsNull(CallbackData data, string exclude) => CheckFieldsIsNull(data, new string[] { exclude });
        private void CheckFieldsIsNull(CallbackData data, string exclude1, string exclude2) 
            => CheckFieldsIsNull(data, new string[] { exclude1, exclude2 });

        private void CheckFieldsIsNull(CallbackData data, string []exclude) {
            IEnumerable<string> all = commonFields.Concat(exclude);
            foreach (PropertyInfo pi in typeof(CallbackData).GetProperties()) {
                object value = pi.GetGetMethod().Invoke(data, null);
                if (all.Contains(pi.Name))
                    Assert.IsNotNull(value, "property " + pi.Name + " is not null");
                else
                    Assert.IsNull(value, "property " + pi.Name + " is null");
            }
        }

        private CallbackData Read(string fn) {
            string json = File.ReadAllText("./Json/" + fn + ".json");
            return JsonConvert.DeserializeObject<CallbackData>(json);
        }
    }
}
