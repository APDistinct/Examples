using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FLChat.DAL.Model;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class AuthorMsgTextCompilerTests
    {
        [TestMethod]
        public void AuthorMsgTextCompiler_Test() {
            AuthorMsgTextCompiler compiler = new AuthorMsgTextCompiler();
            MessageToUser mtu = new MessageToUser() {
                Message = new Message() {
                    Text = "some text",
                    FromTransport = new Transport() {
                        User = new User() {
                            FullName = "sender name"
                        }
                    }
                },                
            };
            Assert.AreEqual("*sender name*: some text", compiler.MakeText(mtu));
        }

        [TestMethod]
        public void AuthorMsgTextCompiler_Bot() {
            AuthorMsgTextCompiler compiler = new AuthorMsgTextCompiler();
            MessageToUser mtu = new MessageToUser() {
                Message = new Message() {
                    Text = "some_text",
                    FromTransport = new Transport() {
                        User = new User() {
                            FullName = "sender name",
                            IsBot = true
                        }
                    }
                },
            };
            Assert.AreEqual("some\\_text", compiler.MakeText(mtu));
        }

        [TestMethod]
        public void AuthorMsgTextCompiler_TestEscape() {
            AuthorMsgTextCompiler compiler = new AuthorMsgTextCompiler();
            MessageToUser mtu = new MessageToUser() {
                Message = new Message() {
                    Text = "some text _1 *2",
                    FromTransport = new Transport() {
                        User = new User() {
                            FullName = "sender_name"
                        }
                    }
                },
            };
            Assert.AreEqual("*sender\\_name*: some text \\_1 \\*2", compiler.MakeText(mtu));
        }
    }
}
