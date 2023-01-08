using System;
using FLChat.Core.MsgCompilers;
using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.Core.MsgCompilers.Tests
{
    [TestClass]
    public class DevinoMsgHtmlCompilerTests
    {
        //[TestMethod]
        //public void DevinoMsgUserHtmlCompiler_Test()
        //{
        //    string filename = "filen %id%";
        //    string template = "templ  %fullname% %text%  %senderavatar% %sendername%  %senderrank%" ;
        //    string avatarname = "avatarn/%id%";

        //    string sender_name = "sender name";
        //    string reciver_name = "reciver name";
        //    string rank_name = "General";

        //    Guid userguid = Guid.NewGuid();
        //    User user = new User()
        //    {
        //        Id = userguid,
        //        FullName = sender_name,
        //        Rank = new Rank() { Id = 1, Name = rank_name },
        //        UserAvatar = new UserAvatar() { UserId = userguid, }
        //    };

        //    MessageToUser mtu = new MessageToUser()
        //    {                
        //        ToTransport = new Transport()
        //        {
        //            User = new User()
        //            {
        //                Id = userguid,
        //                FullName = reciver_name,                     
        //            }
        //        },
        //        Message = new Message()
        //        {
        //            Text = "some text",
        //            FromTransport = new Transport()
        //            {
        //                User = user,
        //            },
                    
        //        },
        //    };
        //    DevinoMsgUserHtmlCompiler dmhc = new DevinoMsgUserHtmlCompiler(filename, avatarname, template);
        //    var ret = dmhc.MakeText(mtu);
        //    Assert.IsTrue(ret.Contains(sender_name));
        //    Assert.IsTrue(ret.Contains(reciver_name));
        //    Assert.IsTrue(ret.Contains(avatarname.Replace("%id%", mtu.Message.FromTransport.User.Id.ToString())));
        //    Assert.IsTrue(ret.Contains(rank_name));
        //    //Assert.IsTrue(ret.Contains(mtu.Message.Text));
        //    TextContains(ret, mtu.Message.Text);
        //}

        [TestMethod]
        public void DevinoMsgHtmlCompiler_Test()
        {
            string filename = "filen %id%";
            string template = "templ  %fullname% %text%  %senderavatar% %sendername%  %senderrank%   %senderfile%";
            string avatarname = "avatarn/%id%";
            string avatardefname = "avatardefname";

            string sender_name = "sender name";
            //string reciver_name = "reciver name";
            string rank_name = "General";

            Guid fileguid = Guid.NewGuid();            

            Guid userguid = Guid.NewGuid();
            User user = new User()
            {
                Id = userguid,
                FullName = sender_name,
                Rank = new Rank() { Id = 1, Name = rank_name },
                //UserAvatar = new UserAvatar() { UserId = userguid, }
            };            

            Message mtu = new Message()
            {
                //ToTransport = new Transport()
                //{
                //    User = new User()
                //    {
                //        Id = Guid.NewGuid(),
                //        FullName = reciver_name,
                //    }
                //},

                Text = "some \n\n text",
                FromTransport = new Transport()
                {
                    User = user,
                },
            };
            DevinoMsgHtmlCompiler dmhc = new DevinoMsgHtmlCompiler(filename, avatarname, avatardefname, template);

            //  User has no avatar
            var ret = dmhc.MakeText(mtu);
            avatarname = avatarname.Replace("%id%", mtu.FromTransport.User.Id.ToString());
            Assert.IsTrue(ret.Contains(sender_name));
            Assert.IsTrue(ret.Contains(rank_name));

            //Assert.IsTrue(ret.Contains(mtu.Text));
            TextContains(mtu.Text, ret);

            filename = filename.Replace("%id%", fileguid.ToString());
            Assert.IsFalse(ret.Contains(filename));

            Assert.IsFalse(ret.Contains(avatarname));
            Assert.IsTrue(ret.Contains(avatardefname));

            mtu.FromTransport.User.UserAvatar = new UserAvatar() { UserId = userguid, };
            ret = dmhc.MakeText(mtu);
            Assert.IsTrue(ret.Contains(avatarname));
            Assert.IsFalse(ret.Contains(avatardefname));

            mtu.FileInfo = new FileInfo() { Id = fileguid, };
            ret = dmhc.MakeText(mtu);
            Assert.IsTrue(ret.Contains(filename));
        }

        private void TextContains(string textin, string textout)
        {
            var arr = textin.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in arr)
            {
                Assert.IsTrue(textout.Contains(str));
                //Assert.IsTrue(textout.Contains($"<p>{str}<\\p>"));
            }
        }
    }
}
