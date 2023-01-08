using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL.Model;
using System.Linq;
using FLChat.DAL;
using Newtonsoft.Json.Linq;
using FLChat.WebService.DataTypes;

namespace FLChat.WebService.Handlers.User.Tests
{
    [TestClass]
    public class SetUserInfoTests
    {
        ChatEntities entities;
        SetUserInfo handler = new SetUserInfo();

        DAL.Model.User user;

        [TestInitialize]
        public void Init() {
            entities = new ChatEntities();

            user = entities.GetUser(
                u => u.Enabled && u.Transports.Where(t => t.Enabled == true && t.TransportTypeId == (int)TransportKind.FLChat).Any(),
                u => {
                    u.Enabled = true;
                    u.Phone = "1234567890";
                    u.Email = "fltest@ya.ru";
                    u.Transports.Add(new DAL.Model.Transport() {
                        TransportTypeId = (int)TransportKind.FLChat
                    });
                });
        }

        [TestCleanup]
        public void Clean() {
            entities.Dispose();
        }

        [TestMethod]
        public void SetUserInfoForActiveTest() {
            string phonesStr = new Random().Next().ToString();
            string nameStr = "James Bond";
            user.Email = Guid.NewGuid().ToString();
            entities.SaveChanges();
            string emailStr = user.Email;

            JObject json = new JObject {
                [SetUserInfo.KeyFieldName] = user.Id,
                ["full_name"] = nameStr,
                ["phone"] = phonesStr
            };
            var setUserInfo = new SetUserInfo();
            UserProfileInfo userPI = setUserInfo.ProcessRequest(entities, user, json);

            DAL.Model.User user0 = entities.User.Where(u => u.Id == user.Id).First();
            Assert.AreEqual(userPI.FullName, nameStr);
            Assert.AreEqual(userPI.Phone, phonesStr);
            Assert.AreEqual(userPI.Email, emailStr);
            Assert.AreEqual(userPI.Email, user0.Email);
            Assert.AreEqual(userPI.Phone, user0.Phone);
            Assert.AreEqual(userPI.Email, user0.Email);
        }

        [TestMethod]
        public void SetUserInfo_SetDefaultTransport() {
            DAL.Model.User user = entities.GetUser(u => u.Enabled && u.DefaultTransportTypeId == null, null, TransportKind.FLChat);

            handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["default_transport_type"] = "FLChat" });
            entities.Entry(user).Reload();
            Assert.AreEqual((int)TransportKind.FLChat, user.DefaultTransportTypeId.Value);

            user.DefaultTransportTypeId = null;
            entities.SaveChanges();
        }

        [TestMethod]
        public void SetUserInfo_SetDefaultTransportFail() {
            DAL.Model.TransportType transportType = entities.TransportType.Where(tt => tt.Enabled && tt.VisibleForUser == false).FirstOrDefault();
            if (transportType == null)
                return;

            DAL.Model.User user = entities.GetUser(u => u.Enabled && u.DefaultTransportTypeId == null, null, (TransportKind)transportType.Id);

            Assert.ThrowsException<ErrorResponseException>(() =>
                handler.ProcessRequest(entities, user, new JObject() {
                    [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                    ["default_transport_type"] = transportType.Kind.ToString() })
                );
        }

        [TestMethod]
        public void SetUserInfo_Comment() {
            DAL.Model.User cmtOnUser = entities.GetUser(
                u => u.Enabled && u.CommentsOnMe.Where(c => c.User.Enabled && c.Text != "").Any(),
                u => u.CommentsOnMe = new Comment[] {
                    new Comment() {
                        UserId = entities.GetUser(u2 => u2.Enabled && u2.Id != u.Id, null).Id,
                        Text = "some text"
                    }
                });
            DAL.Model.User user = entities.GetUser(u => u.Enabled && u.Comments.Where(c => c.UponUserId == cmtOnUser.Id).Any() == false,
                null);

            Assert.IsNull(user.Comments.Where(c => c.UponUserId == cmtOnUser.Id).FirstOrDefault());

            //assing comment
            UserProfileInfo info = handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = cmtOnUser.Id.ToString(),
                ["comment"] = "text1"
            });
            Assert.AreEqual("text1", info.Comment);
            Assert.AreEqual("text1", entities.Comment.Where(c => c.UserId == user.Id && c.UponUserId == cmtOnUser.Id).Select(c => c.Text).Single());

            //change comment
            info = handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = cmtOnUser.Id.ToString(),
                ["comment"] = "text2"
            });
            Assert.AreEqual("text2", info.Comment);
            Assert.AreEqual("text2", entities.Comment.Where(c => c.UserId == user.Id && c.UponUserId == cmtOnUser.Id).Select(c => c.Text).Single());

            //remove comment
            info = handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = cmtOnUser.Id.ToString(),
                ["comment"] = ""
            });
            Assert.AreEqual("", info.Comment);
            Assert.IsNull(entities.Comment.Where(c => c.UserId == user.Id && c.UponUserId == cmtOnUser.Id).FirstOrDefault());

            //assign again
            info = handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = cmtOnUser.Id.ToString(),
                ["comment"] = "text1"
            });
            Assert.AreEqual("text1", info.Comment);
            Assert.AreEqual("text1", entities.Comment.Where(c => c.UserId == user.Id && c.UponUserId == cmtOnUser.Id).Select(c => c.Text).Single());

            //remove comment with null value
            info = handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = cmtOnUser.Id.ToString(),
                ["comment"] = JValue.CreateNull()
            });
            Assert.AreEqual("", info.Comment);
            Assert.IsNull(entities.Comment.Where(c => c.UserId == user.Id && c.UponUserId == cmtOnUser.Id).FirstOrDefault());

            //set comment for himself
            Assert.ThrowsException<ErrorResponseException>(() => handler.ProcessRequest(entities, user, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["comment"] = "text"
            }));
        }

        [TestMethod]
        public void SetUserInfo_OwnerId() {
            DAL.Model.User child = entities.GetUserQ(hasOwner: true);
                //u => u.OwnerUserId != null,
                //u => u.OwnerUserId = entities.GetUser(null, null).Id
                //);
            DAL.Model.User owner = child.OwnerUser;

            handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = child.Id.ToString(),
                ["owner_user_id"] = null
            });

            entities.Entry(child).Reload();
            Assert.IsNull(child.OwnerUserId);

            handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = child.Id.ToString(),
                ["owner_user_id"] = owner.Id.ToString()
            });

            entities.Entry(child).Reload();
            Assert.AreEqual(owner.Id, child.OwnerUserId);
        }

        [TestMethod]
        public void SetUserInfo_BroadcastProhibition() {
            DAL.Model.User user = entities.GetUser(u => u.BroadcastProhibitionBy.Any() == false, null);
            DAL.Model.User owner = entities.GetUser(u => u.Id != user.Id, null);
            UserProfileInfo pi;
            
            //set broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["broadcast_prohibition"] = true
            });
            Assert.IsTrue(pi.BroadcastProhibition);
            entities.Entry(user).Collection(u => u.BroadcastProhibitionBy).Load();
            Assert.IsTrue(user.BroadcastProhibitionBy.Select(u => u.Id).Contains(owner.Id));

            //again set broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["broadcast_prohibition"] = true
            });
            Assert.IsTrue(pi.BroadcastProhibition);
            entities.Entry(user).Collection(u => u.BroadcastProhibitionBy).Load();
            Assert.IsTrue(user.BroadcastProhibitionBy.Select(u => u.Id).Contains(owner.Id));

            //clear broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["broadcast_prohibition"] = false
            });
            Assert.IsFalse(pi.BroadcastProhibition);
            entities.Entry(user).Collection(u => u.BroadcastProhibitionBy).Load();
            Assert.IsFalse(user.BroadcastProhibitionBy.Select(u => u.Id).Contains(owner.Id));

            //again clear broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject() {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                ["broadcast_prohibition"] = false
            });
            Assert.IsFalse(pi.BroadcastProhibition);
            entities.Entry(user).Collection(u => u.BroadcastProhibitionBy).Load();
            Assert.IsFalse(user.BroadcastProhibitionBy.Select(u => u.Id).Contains(owner.Id));
        }

        [TestMethod]
        public void SetUserInfo_PersonalProhibition()
        {
            DAL.Model.User user = entities.GetUser(u => u.PersonalProhibitionMain.Any() == false, null);
            DAL.Model.User owner = entities.GetUser(u => u.Id != user.Id, null);
            UserProfileInfo pi;
            string fname = "personal_prohibition";

            //set broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = true
            });
            Assert.IsTrue(pi.PersonalProhibition);
            entities.Entry(user).Collection(u => u.PersonalProhibitionMain).Load();
            Assert.IsTrue(user.PersonalProhibitionMain.Select(u => u.Id).Contains(owner.Id));

            //again set broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = true
            });
            Assert.IsTrue(pi.PersonalProhibition);
            entities.Entry(user).Collection(u => u.PersonalProhibitionMain).Load();
            Assert.IsTrue(user.PersonalProhibitionMain.Select(u => u.Id).Contains(owner.Id));

            //clear broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = false
            });
            Assert.IsFalse(pi.PersonalProhibition);
            entities.Entry(user).Collection(u => u.PersonalProhibitionMain).Load();
            Assert.IsFalse(user.PersonalProhibitionMain.Select(u => u.Id).Contains(owner.Id));

            //again clear broadcast prohibition
            pi = handler.ProcessRequest(entities, owner, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = false
            });
            Assert.IsFalse(pi.PersonalProhibition);
            entities.Entry(user).Collection(u => u.PersonalProhibitionMain).Load();
            Assert.IsFalse(user.PersonalProhibitionMain.Select(u => u.Id).Contains(owner.Id));
        }

        [TestMethod]
        public void SetUserInfo_Rank()
        {
            string fname = "rank";
            DAL.Model.Rank rank = entities.Rank.FirstOrDefault();
            if(rank == null)
            {
                entities.Rank.Add(new Rank() { Name = Guid.NewGuid().ToString() });
                entities.SaveChanges();
                rank = entities.Rank.FirstOrDefault();
            }
            
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u => u.Enabled == true && u.RankId != null),
                u => { u.Enabled = true; u.RankId = rank.Id; }
            );

            handler.ProcessRequest(entities, user, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = null
            });

            entities.Entry(user).Reload();
            Assert.IsNull(user.RankId);

            handler.ProcessRequest(entities, user, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = rank.Name
            });

            entities.Entry(user).Reload();
            Assert.AreEqual(rank.Id, user.RankId);

            //AssertFailedException
            Assert.ThrowsException<ErrorResponseException>(() =>
            handler.ProcessRequest(entities, user, new JObject()
            {
                [SetUserInfo.KeyFieldName] = user.Id.ToString(),
                [fname] = Guid.NewGuid().ToString()
            })
            );
        }
    }
}
