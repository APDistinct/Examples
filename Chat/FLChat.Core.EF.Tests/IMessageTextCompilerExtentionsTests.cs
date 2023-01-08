using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;

namespace FLChat.Core.Tests
{
    [TestClass]
    public class IMessageTextCompilerExtentionsTests
    {
        [TestMethod]
        public void IMessageTextCompiler_UniteWithHashCompiler() {
            IMessageTextCompiler cmp1 = new SimpleMsgTextCompiler();
            IMessageTextCompiler result = cmp1.UniteWithHashCompiler();
            Assert.IsInstanceOfType(result, typeof(ChainCompiler));
            ChainCompiler chain = (ChainCompiler)result;
            CollectionAssert.AreEquivalent(
                new Type[] { typeof(SimpleMsgTextCompiler), typeof(TagReplaceTextCompiler) },
                chain.Select(i => i.GetType()).ToArray());
        }

        [TestMethod]
        public void IMessageTextCompiler_CreateTagTextCompiler_genLink() {
            IMessageTextCompilerWithCheck tc = IMessageTextCompilerExtentions.CreateTagTextCompiler(false, genLink: true);
            MessageToUser mtu = new MessageToUser() {
                ToTransport = new Transport() {
                    User = new User() {
                        FullName = "Ivan Ivanov",
                        City = new City() { Name = "NN" },
                        FLUserNumber = 10
                    },                    
                },
                Message = new Message() {
                    NeedToChangeText = true
                }
            };
            string text = tc.MakeText(mtu, "#ФИО #город #ссылка");            

            string url = Settings.Values.GetValue("LITE_LINK_DEEP_URL", "https://chat.faberlic.com/external/%code%");
            Assert.IsTrue(text.StartsWith("Ivan Ivanov NN " + url.Replace("%code%", "")));

            Assert.IsTrue(tc.IsChangable("#ФИО #город #ссылка"));
            Assert.IsTrue(tc.IsChangable("#ссылка"));
        }

        [TestMethod]
        public void IMessageTextCompiler_CreateTagTextCompiler_withoutGenLink() {
            IMessageTextCompilerWithCheck tc = IMessageTextCompilerExtentions.CreateTagTextCompiler(false, genLink: false);
            MessageToUser mtu = new MessageToUser() {
                ToTransport = new Transport() {
                    User = new User() {
                        FullName = "Ivan Ivanov",
                        City = new City() { Name = "NN" },
                        FLUserNumber = 10
                    },
                },
                Message = new Message() {
                    NeedToChangeText = true
                }
            };
            string text = tc.MakeText(mtu, "#ФИО #город #ссылка");

            Assert.AreEqual("Ivan Ivanov NN ссылка", text);

            Assert.IsTrue(tc.IsChangable("#ФИО #город #ссылка"));
            Assert.IsTrue(tc.IsChangable("#ссылка"));

        }

        [TestMethod]
        public void IMessageTextCompiler_MakeText_User() {
            User u = new User() {
                FullName = "123"
            };
            IMessageTextCompilerWithCheck tc = IMessageTextCompilerExtentions.CreateTagTextCompiler(false, genLink: false);
            string t = tc.MakeText("Hello #ФИО", u);
            Assert.AreEqual("Hello 123", t);
        }
    }
}
