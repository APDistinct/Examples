using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.Viber.Client.Types;
using System.Collections.Generic;

namespace FLChat.Viber.Bot.Adapters.Tests
{
    [TestClass]
    public class ViberInputFileAdapterTests
    {
        [TestMethod]
        public void ViberInputFileAdapter_Type() {
            string url = "www.someurl.com/patch/12.jpg";
            Assert.AreEqual(MediaGroupKind.Image, CreateAdapter(MessageType.Picture, url).Type);
            Assert.AreEqual(MediaGroupKind.Document, CreateAdapter(MessageType.File, url).Type);
            Assert.AreEqual(MediaGroupKind.Document, CreateAdapter(MessageType.Video, url).Type);
            IEnumerable<MessageType> types = Enum
                .GetValues(typeof(MessageType))
                .Cast<MessageType>()
                .Except(new MessageType[] { MessageType.Picture, MessageType.File, MessageType.Video });
            foreach (MessageType mt in types)
                Assert.ThrowsException<InvalidOperationException>(() => CreateAdapter(mt, url));
        }

        [TestMethod]
        public void ViberInputFileAdapter_Url() {
            string url = "www.someurl.com/patch/12.jpg";
            Assert.AreEqual(url, CreateAdapter(MessageType.Picture, url).Media);
        }

        [TestMethod]
        public void ViberInputFileAdapter_FileName() {
            Assert.AreEqual("12.jpg", CreateAdapter(MessageType.Picture, "www.someurl.com/patch/12.jpg").FileName);
            Assert.AreEqual("", CreateAdapter(MessageType.Picture, "www.someurl.com/patch/").FileName);
            Assert.AreEqual("12.jpg", CreateAdapter(MessageType.Picture, "12.jpg").FileName);
            string url = "https://dl-media.viber.com/5/media/2/short/any/sig/image/0x0/c178/3b4a6f6874262408d71984c0dc76c9ce16b7e1ef63894ab8a026256ea1d6c178.jpg?Expires=1572882469&Signature=CyI27dkdoDdw6uJcVNcK-B8PbRUtJHarGFRlcyblKISswyfselfz28AY3eVhpf4nqX3cG8on4JwKcBZmjkKl4q7Y4bEuz-3p5k091DWGR1FWY48DkNnaJjRunNtpS~CV9PEJkzHo0doEBAjxjoGHJklHqzueDg7N-ddEvV2L-oUPeQyt4Cg2nEiftPypAdxYeyr4dGo6IEPWYfV14seGPjuUqzlCgsMa9chv5zdUew82HD~wFpKG8epRLh7o2c0o3YuKqU7X0sPB0F24jr1Tpj16PaUuudjktoXIM5kq-Iw~Shj07h8grtGJM32pCVskkA-ZWBsYvOkBAVHykelbcA__&Key-Pair-Id=APKAJ62UNSBCMEIPV4HA";
            Assert.AreEqual("3b4a6f6874262408d71984c0dc76c9ce16b7e1ef63894ab8a026256ea1d6c178.jpg", CreateAdapter(MessageType.Picture, url).FileName);
        }

        public static CallbackData CreateData(MessageType type, string media) {
            return new CallbackData() {
                Message = new Message() {
                    Type = type,
                    Media = media
                }
            };
        }

        public static ViberInputFileAdapter CreateAdapter(MessageType type, string media) {
            return new ViberInputFileAdapter(CreateData(type, media));
        }
    }
}
