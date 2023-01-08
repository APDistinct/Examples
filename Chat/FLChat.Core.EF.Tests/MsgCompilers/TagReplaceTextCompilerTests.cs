using System;
using System.Collections.Generic;
using FLChat.Core.MsgCompilers;
using FLChat.Core.Tests;
using FLChat.DAL.Model;
using FLChat.WebService.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class TagReplaceTextCompilerTests
    {
        private TagReplaceTextCompiler hrtc = null;
        private MessageToUser mtu;
        private User user;
        private readonly string refStr = "TeStT !";
        private string stUser1 = "User1";
        private string stUser2 = "User2";

        [TestInitialize]
        public void Init()
        {
            var fdlg = new FakeDeepLinkGenerator(refStr);
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict 
                = new Dictionary<string, Func<MessageToUser, string>>()
                {
                    { "ФИО", mtu => mtu.ToTransport.User.FullName ?? "" },
                    { "город", mtu => mtu.ToTransport.User.City?.Name ?? "" },
                    {"ссылка", mtu =>  fdlg.Generate(mtu.ToTransport.User)}
                };
            hrtc = new TagReplaceTextCompiler(ReplaceDict);
            
            user = new User()
            {
                FullName = "someNsomeN",
                City = new City() { Id = 1, Name = "CityOfTulun", }
            };

            mtu = new MessageToUser()
            {
                ToTransport = new Transport()
                {
                    User = user,
                },
                Message = new Message()
                {
                    Text = "some text",
                    NeedToChangeText = true,
                },
            };
        }

        [TestCleanup]
        public void Clean()
        {
        }

        /// <summary>
        /// Строка для тестов, содержит все тэги
        /// </summary>
        /// <returns></returns>
        private string TextWithAllTags()
        {
            string text = "";
            foreach (var str in hrtc._hashList)
            {
                text += hrtc.Tag + str + "  ";
            }
            return text;
        }

        /// <summary>
        /// Простая успешная замена всех тэгов по одному разу
        /// </summary>
        [TestMethod]
        public void SimpleReplaceTest()
        {
            string text = TextWithAllTags();
            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, text);
            foreach (var str in hrtc.ReplaceDict.Keys)
            {                
                Assert.IsTrue(ret.Contains(hrtc.ReplaceDict[str](mtu)));                
            }
        }

        /// <summary>        
        /// Пропуск подстановки и возврат исходной строки со включенным признаком, но при отсутствии тэгов в тексте
        /// </summary>
        [TestMethod]
        public void NoTagsReplaceTest()
        {
            string text = $"  ##--  # ::;;";
            string strfind = hrtc.ReplaceDict[hrtc._hashList[0]](mtu);

            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreSame(text, ret);
        }

        /// <summary>
        /// Успешная замена тэга несколько раз при включенном признаке
        /// Тэги для подстановки в самом начале и в самом конце строки
        /// Пропуск подстановки и возврат исходной строки при выключенном признаке
        /// </summary>
        [TestMethod]
        public void ManyTagsReplaceTest()
        {
            string text = $"{hrtc.Tag}{hrtc._hashList[0]}  ##--  # {hrtc.Tag}{hrtc._hashList[0]}";
            string strfind = hrtc.ReplaceDict[hrtc._hashList[0]](mtu);

            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret.WordsCount(strfind), 2);

            mtu.Message.NeedToChangeText = false;
            ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret.WordsCount(strfind), 0);
            Assert.AreEqual(text, ret);
        }

        /// <summary>
        /// Успешная замена тэга в середине строки. Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void OneTagInMidleReplaceTest()
        {
            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            string strfind = $"{s1}{hrtc.ReplaceDict[hrtc._hashList[0]](mtu)}{s2}"; ;

            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Успешная замена тэга на текст самого же тэга( с добавками - ?). Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void OneTagWithTagReplaceTest()
        {
            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            mtu.ToTransport.User.FullName = $"{hrtc.Tag}{hrtc._hashList[0]}";
                        
            mtu.Message.NeedToChangeText = true;
            string strfind = $"{s1}{hrtc.ReplaceDict[hrtc._hashList[0]](mtu)}{s2}"; 
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Успешная замена тэга на пустую строку. Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void OneTagWithEmptyReplaceTest()
        {
            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            mtu.ToTransport.User.FullName = "";

            mtu.Message.NeedToChangeText = true;
            string strfind = $"{s1}{s2}";
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Успешная замена тэга маленького размера. Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void LittleTagReplaceTest()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
                = new Dictionary<string, Func<MessageToUser, string>>()
                {
                    { "t", mtu => mtu.ToTransport.User.FullName ?? "" },
                };
            hrtc = new TagReplaceTextCompiler(ReplaceDict);

            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            mtu.Message.NeedToChangeText = true;
            string strfind = $"{s1}{hrtc.ReplaceDict[hrtc._hashList[0]](mtu)}{s2}";
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Функция в словаре возвращает null. Замена на пустую строку. Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void FuncNullReturnReplaceTest()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
                = new Dictionary<string, Func<MessageToUser, string>>()
                {
                    { "t", mtu => null },
                };
            hrtc = new TagReplaceTextCompiler(ReplaceDict);

            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            mtu.Message.NeedToChangeText = true;
            string strfind = $"{s1}{s2}";
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Функция в словаре - null. Замена на пустую строку. Полное совпадение строк.
        /// </summary>
        [TestMethod]
        public void FuncNullValueReplaceTest()
        {
            Dictionary<string, Func<MessageToUser, string>> ReplaceDict
                = new Dictionary<string, Func<MessageToUser, string>>()
                {
                    { "t", null },
                };
            hrtc = new TagReplaceTextCompiler(ReplaceDict);

            string s1 = "вав  в влда ##";
            string s2 = "  ##--  # ";
            string text = $"{s1}{hrtc.Tag}{hrtc._hashList[0]}{s2}";
            mtu.Message.NeedToChangeText = true;
            string strfind = $"{s1}{s2}";
            var ret = hrtc.MakeText(mtu, text);
            Assert.AreEqual(ret, strfind);
        }

        /// <summary>
        /// Возврат null при посылке текста null
        /// </summary>
        [TestMethod]
        public void ReplaceTest_NullText()
        {
            string text = TextWithAllTags();

            mtu.Message.NeedToChangeText = true;
            var ret = hrtc.MakeText(mtu, null);
            Assert.IsNull(ret);
        }

        ///// <summary>
        ///// Успешная замена тэга в середине строки. Полное совпадение строк.
        ///// </summary>
        //[TestMethod]
        //public void OneTagReplaceTest_WithFunc()
        //{            
        //    Dictionary<string, Func<MessageToUser, string>> replaceDict
        //        = new Dictionary<string, Func<MessageToUser, string>>()
        //        {
        //            { "ФИО", mtu => mtu.ToTransport.User.FullName ?? "" },
                    
        //        };
        //    var hrt = new TagReplaceTextCompiler(replaceDict, Replace1);

        //    string s1 = "вав  в влда ##";
        //    string s2 = "  ##--  # ";
        //    string text = $"{s1}{hrt.Tag}{hrt._hashList[0]}{s2}";
        //    var mtu1 = Replace1(mtu);
        //    string strfind = $"{s1}{hrt.ReplaceDict[hrt._hashList[0]](mtu1)}{s2}"; ;

        //    mtu.Message.NeedToChangeText = true;
        //    var ret = hrt.MakeText(mtu, text);
        //    Assert.AreEqual(ret, strfind);
        //}

        private MessageToUser Replace1(MessageToUser mtu)
        {
            return ReplaceUser(stUser1);
        }

        private MessageToUser Replace2(MessageToUser mtu)
        {
            return ReplaceUser(stUser2);
        }

        private MessageToUser ReplaceUser(string stUser)
        {
            var user = new User()
            {
                FullName = stUser,
                City = new City() { Id = 1, Name = "CityOfTulun", }
            };

            var mtuu = new MessageToUser()
            {
                ToTransport = new Transport()
                {
                    User = user,
                },
                Message = new Message()
                {
                    Text = "some text",
                    NeedToChangeText = true,
                },
            };
            return mtuu;
        }

    }
}
