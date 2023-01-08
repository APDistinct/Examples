using System;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Transport.Tests
{
    [TestClass]
    public class EmailSendTest
    {
        //[TestMethod]
        //public void /*System.Threading.Tasks.Task */EmailSendOneTestAsync()
        //{
        //    string phone = "apdapdapdapd@yandex.ru"/*"APDistinct@gmail.com"*/, text = "Первый блин";
        //    var mailSender = new MailSender();
        //    mailSender.Send(phone, text);
        //}

        [TestMethod]
        public void subjectCompileTest()
        {
            //string filename = "filen %id%";
            //string template = "templ  %fullname% %text%  %senderavatar% %sendername%  %senderrank%";
            //string avatarname = "avatarn/%id%";
            //string avatardefname = "avatardefname";
            string sender_name = "Консультант № 1";
            string reciver_name = "reciver name";

            string Pattern = "Сообщение от личного консультанта Faberlic %sendername%";
            DevinoMsgHtmlCompiler MsgCompiler = new DevinoMsgHtmlCompiler("", "", "", Pattern);
            Guid userguid = Guid.NewGuid();

            User user = new User()
            {
                Id = userguid,
                FullName = sender_name,
                //Rank = new Rank() { Id = 1, Name = rank_name },
                UserAvatar = new UserAvatar() { UserId = userguid, }
            };

            MessageToUser mtu = new MessageToUser()
            {

                ToTransport = new FLChat.DAL.Model.Transport()
                {
                    User = new User()
                    {
                        Id = userguid,
                        FullName = reciver_name,
                    }
                },
                Message = new Message()
                {
                    Text = "some text",
                    FromTransport = new DAL.Model.Transport()
                    {
                        User = user,
                    },

                },
            };
            
            string subject = MsgCompiler.MakeText(mtu, "");
            Assert.AreEqual(subject, Pattern.Replace("%sendername%", sender_name));
        }
    }
}
