using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.VKBotClient.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace FLChat.VKBot.Tests
{
    [TestClass]
    public class KeyboardTests
    {
        [TestMethod]
        public void Keyboard()
        {
            var keyboard = new VkKeyboard()
            {
                OneTime = false,
                Buttons = new[]
                {
                    new[]
                    {
                        new VkKeyboardButton(new Text(){Label = "0", Payload = "100"}),
                        new VkKeyboardButton(new Text(){Label = "1", Payload = "110"}),
                    },
                    new[]
                    {
                        new VkKeyboardButton(new Text(){Label = "20", Payload = "200"}),
                    }
                }
            };

            var result = keyboard.GetJson();
        }
    }
}
