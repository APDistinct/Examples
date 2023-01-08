using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBot.Adapters;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FLChat.VKBot.Tests.VkInputFileAdapterTests
{
    [TestClass]
    public class AdapterConstructorShould
    {
        [TestMethod]
        public void Init()
        {
            Message message = GetMessage();

            var adapter = new VkInputFileAdapter(message);

            Assert.IsTrue(VkInputFileAdapter.IsContainsFile(message));

            Assert.AreEqual("HJOK5H27t58.jpg", adapter.FileName);
            Assert.AreEqual(DAL.MediaGroupKind.Image, adapter.Type);
            Assert.AreEqual(@"https://sun9-21.userapi.com/c854228/v854228358/1677a8/HJOK5H27t58.jpg", adapter.Media);
            Assert.AreEqual(null, adapter.MediaType);
            Assert.AreEqual(@"https://sun9-21.userapi.com/c854228/v854228358/1677a8/HJOK5H27t58.jpg", adapter.Url);
        }


        private Message GetMessage()
        {
            var json = ReadJsonFile();

            CallbackData callbackData = JsonConvert.DeserializeObject<CallbackData>(json);
            Message message = ((JObject)callbackData.Object).ToObject<Message>();
            return message;
        }

        private string ReadJsonFile()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Json\VkMessage.json");
            string file = File.ReadAllText(path);
            return file;
        }
    }
}
