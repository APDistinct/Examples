using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.Viber.Client.Types;
using System.IO;
using Newtonsoft.Json;
using FLChat.DAL;
using System.Collections.Generic;
using System.Linq;
using FLChat.Viber.Bot.Exceptions;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberMessageAdapterTests
    {        
        [TestMethod]
        public void ViberMessageAdapter_Test() {
            CallbackData callbackData = Helper.ReadJson("callback_message");
            ViberMessageAdapter msg = new ViberMessageAdapter(callbackData);
            Assert.AreEqual(callbackData.MessageToken.ToString(), msg.MessageId);
            Assert.AreEqual(callbackData.Sender.Id, msg.FromId);
            Assert.AreEqual(callbackData.Sender.Name, msg.FromName);
            Assert.AreEqual(callbackData.Message.Text, msg.Text);
            Assert.IsNull(msg.PhoneNumber);
            Assert.IsNull(msg.DeepLink);
            Assert.IsNull(msg.ReplyToMessageId);
        }

        [TestMethod]
        public void ViberMessageAdapter_Other() {
            //IEnumerable<CallbackEvent> events = Enum
            //    .GetValues(typeof(CallbackEvent))
            //    .Cast<CallbackEvent>()
            //    .Except(new CallbackEvent[] { CallbackEvent.Message });
            //foreach (CallbackEvent e in events)
            //    Assert.ThrowsException<ViberAdapterException>(() => new ViberMessageAdapter(new CallbackData() { Event = e }));
            foreach (CallbackData data in Helper.CreateCallbackDataForAllEvents(new CallbackEvent[] { CallbackEvent.Message }))
                Assert.ThrowsException<ViberAdapterException>(() => new ViberMessageAdapter(data));
        }

        [TestMethod]
        public void ViberMessageAdapter_File() {
            ViberMessageAdapter adapter = new ViberMessageAdapter(new CallbackData() {
                Event = CallbackEvent.Message,
                Message = new Message() {
                    Type = MessageType.Text
                }
            });
            Assert.IsNull(adapter.File);

            adapter = new ViberMessageAdapter(new CallbackData() {
                Event = CallbackEvent.Message,
                Message = new Message() {
                    Type = MessageType.Picture,                    
                    Media = "media"
                }
            });
            Assert.IsNotNull(adapter.File);
        }

        [TestMethod]
        public void ViberMessageAdapter_MessageWithPicture() {
            CallbackData callbackData = Helper.ReadJson("callback_message_with_picture");
            ViberMessageAdapter msg = new ViberMessageAdapter(callbackData);
            Assert.IsNotNull(msg.File);
            Assert.IsNull(msg.Text);
            Assert.AreEqual(MediaGroupKind.Image, msg.File.Type);
            Assert.AreEqual("Без названия.jpg", msg.File.FileName);
            Assert.AreEqual("https://dl-media.viber.com/5/media/2/short/any/sig/image/0x0/c178/3b4a6f6874262408d71984c0dc76c9ce16b7e1ef63894ab8a026256ea1d6c178.jpg?Expires=1572882469&Signature=CyI27dkdoDdw6uJcVNcK-B8PbRUtJHarGFRlcyblKISswyfselfz28AY3eVhpf4nqX3cG8on4JwKcBZmjkKl4q7Y4bEuz-3p5k091DWGR1FWY48DkNnaJjRunNtpS~CV9PEJkzHo0doEBAjxjoGHJklHqzueDg7N-ddEvV2L-oUPeQyt4Cg2nEiftPypAdxYeyr4dGo6IEPWYfV14seGPjuUqzlCgsMa9chv5zdUew82HD~wFpKG8epRLh7o2c0o3YuKqU7X0sPB0F24jr1Tpj16PaUuudjktoXIM5kq-Iw~Shj07h8grtGJM32pCVskkA-ZWBsYvOkBAVHykelbcA__&Key-Pair-Id=APKAJ62UNSBCMEIPV4HA",
                msg.File.Media);
        }

        [TestMethod]
        public void ViberMessageAdapter_Contact()
        {
            CallbackData callbackData = Helper.ReadJson("callback_message_with_contact");
            ViberMessageAdapter msg = new ViberMessageAdapter(callbackData);
            Assert.AreEqual(callbackData.MessageToken.ToString(), msg.MessageId);
            Assert.AreEqual(callbackData.Sender.Id, msg.FromId);
            Assert.AreEqual(callbackData.Sender.Name, msg.FromName);
            Assert.AreEqual(callbackData.Message.Text, msg.Text);
            Assert.IsNotNull(msg.PhoneNumber);
            Assert.AreEqual(callbackData.Message.Contact.PhoneNumber, msg.PhoneNumber);
            Assert.IsNull(msg.DeepLink);
            Assert.IsNull(msg.ReplyToMessageId);
        }
    }
}
