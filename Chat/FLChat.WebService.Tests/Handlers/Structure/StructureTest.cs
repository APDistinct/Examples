using System;
using System.Collections.Generic;
using System.Linq;
using FLChat.DAL.Model;
using FLChat.WebService.Handlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FLChat.DAL;
using FLChat.WebService.DataTypes;
using System.Data.Entity;

namespace FLChat.WebService.Tests.Handlers.Structure
{
    [TestClass]
    public class StructureTest
    {
        ChatEntities entities;        
        GetStructure handler = new GetStructure() { MaxCount = int.MaxValue };

        [TestInitialize]
        public void Init()
        {
            entities = new ChatEntities();            
        }

        [TestCleanup]
        public void Clean()
        {
            entities.Dispose();
        }

        private Tuple<Segment,User> GetSegmentWithChildUser() {
            //seek segment with member with UserOwnerId != null in nodes
            Segment seg = entities.StructureNodeSegment.Where(sns => sns.Segment.IsDeleted == false 
                    && sns.Segment.Members.Where(u => u.Enabled && u.OwnerUserId != null 
                        && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                        && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()).Any())
                .Select(sns => sns.Segment)
                .FirstOrDefault();
            if (seg == null) {
                StructureNode startStructureNode = entities.GetStructureNode(s => s != null, s => { });
                Assert.IsNotNull(startStructureNode, "There is no any node");  //  Нет ничего, даже корневого

                //seek any segment with member with UserOwnerId != null
                seg = entities.Segment.Where(s => s.IsDeleted == false 
                    && s.Members.Where(u => u.Enabled && u.OwnerUserId != null
                        && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()
                        && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)TransportKind.FLChat).Any()).Any())
                    .FirstOrDefault();
                if (seg == null) {
                    seg = entities.GetSegment(s => s.IsDeleted == false, s => s.IsDeleted = false);
                    User user = entities.GetUserWithOwner();
                    seg.Members.Add(user);
                }

                entities.StructureNodeSegment.Add(new StructureNodeSegment() { NodeId = startStructureNode.Id, SegmentId = seg.Id });
                entities.SaveChanges();
            }

            return Tuple.Create(
                seg, 
                seg.Members.Where(u => u.Enabled && u.OwnerUserId != null).First());
        }

        [TestMethod]
        public void GetRootNodeTest()
        {
            var user = entities.GetUser(u => u.Enabled, u => { });
            var startStructureNode = entities.GetStructureNode
                (s => s.ParentNodeId == null && s.Id == Guid.Empty,
                s => { s.ParentNodeId = null; s.Id = Guid.Empty; });
            
            var s1 = startStructureNode.Childs;
            var s2 = handler.ProcessRequest(entities, user, (string)null);
            Assert.IsTrue(s2.NodeList.Count() > s1.Count);
            Assert.IsTrue(0 == s2.UserList.Count());
            Assert.IsTrue(s1.Select(x => x.Caption).OrderBy(x => x).ToList().Intersect(
                s2.NodeList.Select(x => x.Name).OrderBy(x => x).ToList()).Count() > 0);
            Assert.AreEqual(!s2.Node.Final, s2.NodeList.Any());
        }

        [TestMethod]
        public void GetSegmentNodeTest()
        {
            var segmId = entities.StructureNodeSegment.Select(x => x.SegmentId).FirstOrDefault();
            Segment segm = null;
            if(segmId == null)
            {
                var startStructureNode = entities.GetStructureNode(s =>  s != null, s => { });
                Assert.IsNotNull(startStructureNode, "There is no any node");  //  Нет ничего, даже корневого
                segm = entities.GetSegment(x => x.Id != Guid.Empty && x.IsDeleted == false, x => x.IsDeleted = false);
                entities.StructureNodeSegment.Add(new StructureNodeSegment() { NodeId = startStructureNode.Id, SegmentId = segm.Id });
            }
            else
            {
                segm = entities.Segment.Where(x => x.Id == segmId ).FirstOrDefault();
                segm.IsDeleted = false;
            }
            // Вопрос - если нет ничего, то что тестировать? Создавать самим?
            Assert.IsNotNull(segmId, "There is no segments node");            
            Assert.IsNotNull(segm, "There is no segment for node");
            var user = segm.Members.Where(x => x.OwnerUserId != null).FirstOrDefault();
            DAL.Model.User u0 = null;
            if(user == null)
            {
                user = entities.GetUser(u => u.Enabled, u => { });
                user.Segments.Add(segm);                
            }
            if(user.OwnerUserId == null)
            {
                u0 = entities.GetUser(u => u.Enabled && u.Id != user.Id, u => { });
                user.OwnerUserId = u0.Id;
            }
            else
            {
                u0 = entities.User.Where(x => x.Id == user.OwnerUserId).FirstOrDefault();
            }
            entities.SaveChanges();           
            
            //var s1 = startStructureNode.Childs;
            var s2 = handler.ProcessRequest(entities, u0, segm.NodeId);
            Assert.IsTrue(s2.UserList.Select(x => x.Id).Contains(user.Id));
            Assert.IsTrue(0 == s2.NodeList.Count());
        }

        [TestMethod]
        public void GetSegmentNode_LastMessageTest() {
            Tuple<Segment, User> tuple = GetSegmentWithChildUser();

            //Message msg = entities.SendMessage(tuple.Item2.OwnerUserId.Value, tuple.Item2.Id);
            Guid msgid = entities.LastMessageView
                .Where(m => m.UserId == tuple.Item2.OwnerUserId.Value && m.UserOppId == tuple.Item2.Id)
                .Select(m => m.MsgId)
                .Single();

            //perform request
            var resp = handler.ProcessRequest(entities, tuple.Item2.OwnerUser, tuple.Item1.NodeId);

            //check message in child
            UserInfoShort info = resp.UserList.Where(u => u.Id == tuple.Item2.Id).Single();
            Assert.IsNotNull(info.LastMessage);
            Assert.AreEqual(msgid, info.LastMessage.Id);
        }

        [TestMethod]
        public void GetStructure_Tags() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s => s.ShowInShortProfile = true);
            Segment segmentRequest = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == false, 
                null);

            User user = entities.GetUser(
                u => u.Segments.Contains(segment) && u.Segments.Contains(segmentRequest) && u.OwnerUserId != null,
                u => {
                    u.Segments.Add(segment);
                    u.Segments.Add(segmentRequest);
                    u.OwnerUser = entities.GetUser(null, null);
                });

            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segmentRequest.Id.ToString());
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNotNull(ui.Tags);
            Assert.IsTrue(ui.Tags.Contains(segment.Name));
        }

        [TestMethod]
        public void GetStructure_TagsEmpty() {
            Segment segment = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == true,
                s =>s.ShowInShortProfile = true);
            Segment segmentRequest = entities.GetSegment(
                s => s.IsDeleted == false && s.ShowInShortProfile == false, 
                null);

            User user = entities.GetUser(
                u => u.Segments.Contains(segment) == false && u.Segments.Contains(segmentRequest) && u.OwnerUserId != null,
                u => {
                    u.Segments.Add(segment);
                    u.Segments.Add(segmentRequest);
                    u.OwnerUser = entities.GetUser(null, null);
                });
            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segmentRequest.Id.ToString());
            UserInfoShort ui = resp.UserList.Where(u => u.Id == user.Id).Single();
            Assert.IsNull(ui.Tags);
        }

        [TestMethod]
        public void GetStructure_BroadcastProhibitionStructure() {
            //get user: 1. who has owner and childs
            //          2. owner set broadcast prohibition for this users
            //          3. and childs included to segment
            Segment segment = entities.GetSegment(s => true, null);
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled && u3.Segments.Where(s => s.Id == segment.Id).Any()).Any()
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser)),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    u.BroadcastProhibitionBy.Add(users[0]);
                    segment.Members.Add(users[1]);
                }
                );
            User child = user.ChildUsers.Where(u3 => u3.Enabled && segment.Members.Contains(u3)).First();
            //owner reqwuest user's childs, they all must has flag <BroadcastProhibitionStructure>
            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segment.Id.ToString());            

            bool? flag = resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure).Single();
            Assert.IsTrue(flag.Value);
        }

        [TestMethod]
        public void GetStructure_BroadcastProhibitionStructure_false() {
            //get user: 1. who has owner and childs
            //          2. owner has not set broadcast prohibition for this user
            //          3. and childs included to segment
            Segment segment = entities.GetSegment(s => true, null);
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => u3.Enabled && u3.Segments.Where(s => s.Id == segment.Id).Any()).Any()
                    && u.BroadcastProhibitionBy.Contains(u.OwnerUser) == false),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    //u.BroadcastProhibitionBy.Add(users[0]);
                    segment.Members.Add(users[1]);
                }
                );
            User child = user.ChildUsers.Where(u3 => u3.Enabled && segment.Members.Contains(u3)).First();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segment.Id.ToString());

            bool? flag = resp.UserList.Where(u => u.Id == child.Id).Select(u => u.BroadcastProhibitionStructure).Single();
            Assert.IsFalse(flag.Value);
        }

        [TestMethod]
        public void GetStructure_BroadcastProhibition() {
            //get user: 1. who has owner and childs
            //          2. owner set broadcast prohibition for one of his childs
            //          3. owner has not set broadcast prohibition for one of his childs
            Segment segment = entities.GetSegment(s => true, null);
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.ChildUsers.Where(u3 => 
                        u3.Enabled 
                        && u3.BroadcastProhibitionBy.Contains(u.OwnerUser)
                        && u3.Segments.Where(s =>s.Id == segment.Id).Any()).Any()
                    && u.ChildUsers.Where(u3 => 
                        u3.Enabled 
                        && u3.BroadcastProhibitionBy.Contains(u.OwnerUser) == false
                        && u3.Segments.Where(s =>s.Id == segment.Id).Any()).Any()),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(3, u2 
                        => u2.OwnerUserId == null 
                        && u2.Enabled
                        && u2.BroadcastProhibitionBy.Any() == false
                        && u2.Segments.Any() == false, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.ChildUsers.Add(users[2]);
                    users[1].BroadcastProhibitionBy.Add(users[0]);
                    segment.Members.Add(users[1]);
                    segment.Members.Add(users[2]);
                }
                );
            DAL.Model.User bpu = user.ChildUsers.Where(u => u.BroadcastProhibitionBy.Contains(user.OwnerUser)).First();
            DAL.Model.User bpu_wo = user.ChildUsers.Where(u => u.BroadcastProhibitionBy.Contains(user.OwnerUser) == false).First();

            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segment.Id.ToString());

            Assert.IsTrue(resp.UserList.Where(u => u.Id == bpu.Id).Single().BroadcastProhibition);
            Assert.IsFalse(resp.UserList.Where(u => u.Id == bpu_wo.Id).Single().BroadcastProhibition);
        }

        [TestMethod]
        public void GetStructure_HasChilds() {
            //get user: 1. who has owner and childs
            //          2. include to segment
            Segment segment = entities.GetSegment(s => true, null);
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.Segments.Where(s => s.Id == segment.Id).Any()
                    && u.ChildUsers.Any()),
                u => {
                    DAL.Model.User[] users = entities.GetUsers(2, u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = users[0].Id;
                    u.ChildUsers.Add(users[1]);
                    u.Segments.Add(segment);
                }
                );
            //owner reqwuest user's childs, they all must has flag <BroadcastProhibitionStructure>
            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segment.Id.ToString());

            bool? flag = resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds).Single();
            Assert.IsTrue(flag.Value);
        }

        [TestMethod]
        public void GetStructure_HasChilds_false() {
            //get user: 1. who has owner 
            //          2. include to segment
            //          3. has not any child
            Segment segment = entities.GetSegment(s => true, null);
            DAL.Model.User user = entities.GetUserQ(
                q => q.Where(u =>
                    u.OwnerUserId != null
                    && u.Segments.Where(s => s.Id == segment.Id).Any()
                    && u.ChildUsers.Any() == false),
                u => {
                    DAL.Model.User owner = entities.GetUser(u2 => u2.OwnerUserId == null, null);
                    u.OwnerUserId = owner.Id;
                    //u.BroadcastProhibitionBy.Add(users[0]);
                    u.Segments.Add(segment);
                }
                );
            var resp = handler.ProcessRequest(entities, user.OwnerUser, Segment.Prefix + segment.Id.ToString());

            bool? flag = resp.UserList.Where(u => u.Id == user.Id).Select(u => u.HasChilds).Single();
            Assert.IsFalse(flag.Value);
        }
    }
}
