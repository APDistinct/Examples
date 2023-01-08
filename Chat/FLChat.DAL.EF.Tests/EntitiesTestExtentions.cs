using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FLChat.DAL.Model
{
    public static class EntitiesTestExtentions
    {
        /// <summary>
        /// Search user or create new
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="wf">Where condition</param>
        /// <param name="nf">Adjust new created user function</param>
        /// <returns></returns>
        public static User GetUser(this ChatEntities entities, Func<User, bool> wf, Action<User> nf, bool enabled = true) {
            User result = null;
            result = entities.GetUserRequest(wf, nf, enabled);

            if (result == null) {
                result = new User() {
                    Enabled = enabled,
                    FullName = "created by test"//,
                    //InsertDate = DateTime.Now
                };
                nf?.Invoke(result);
                entities.User.Add(result);
                entities.SaveChanges();

                result = entities.GetUserRequest(wf, nf, enabled);
                Assert.IsNotNull(result, "Created new instance of user does not correspond to condition");
            }
            return result;
        }

        private static User GetUserRequest(this ChatEntities entities, Func<User, bool> wf, Action<User> nf, bool enabled = true) {
            if (wf == null)
                wf = (u) => true;
             return entities.User
                .Where(u => u.Enabled == enabled)
                .Where(u => u.Id != Guid.Empty)
                .Where(wf)
                .FirstOrDefault();
        }
        /// <summary>
        /// Extract list of users or create new
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="cnt">count of users</param>
        /// <param name="wf">where condition</param>
        /// <param name="nf">fill user fields function</param>
        /// <returns>list of users</returns>
        public static User[] GetUsers(this ChatEntities entities, int cnt, Func<User, bool> wf, Action<User> nf) {
            if (wf == null)
                wf = (u) => u.Enabled;
            User[] result = entities.User.Where(u => u.Id != Guid.Empty).Where(wf).Take(cnt).ToArray();

            int miss = cnt - result.Length;
            for (int i = 0; i < miss; ++i) {
                User tmp = new User() {
                    Enabled = true,
                    FullName = "created by test"
                };
                nf?.Invoke(tmp);
                entities.User.Add(tmp);
            }
            if (miss > 0) {
                entities.SaveChanges();
                result = entities.User.Where(wf).Take(cnt).ToArray();
                Assert.AreEqual(cnt, result.Length, "Created new instances of user does not correspond to select condition");
            }
            return result;
        }

        /// <summary>
        /// Search user or create new with specific transport
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="wf">Where condition</param>
        /// <param name="nf">Adjust new created user function</param>
        /// <param name="tk">Transport kind</param>
        /// <returns></returns>
        public static User GetUser(this ChatEntities entities, Func<User, bool> wf, Action<User> nf, TransportKind tk, bool enabled = true) {
            User user = entities.GetUser(
                u => (wf != null ? wf(u) : true) && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)tk).Any(),
                u => {
                    nf?.Invoke(u);
                    u.Transports.Add(NewTransport(tk));
                },
                enabled: enabled);
            return user;
        }

        public static User GetUserQ(this ChatEntities entities,
            Func<IQueryable<User>, IQueryable<User>> where = null,
            Action<User> create = null,
            bool enabled = true,
            TransportKind? transport = null,
            Guid[] notSameToUsers = null,
            Guid? notSameToUser = null,
            bool? hasOwner = null,
            TransportKind? ownerTransport = null,
            bool? hasChilds = null) {
            if (notSameToUser.HasValue)
                notSameToUsers = new Guid[] { notSameToUser.Value };

            User result = entities.GetUserRequest(where, enabled, transport, notSameToUsers, hasOwner, ownerTransport, hasChilds);
            if (result == null) {
                result = new User() {
                    Enabled = enabled,
                    FullName = "created by test " + DateTime.Now.ToString()
                };
                if (transport.HasValue) {
                    result.Transports.Add(new Transport() {
                        Kind = transport.Value,
                        Enabled = true,
                        TransportOuterId = transport.Value == TransportKind.FLChat ? null : Guid.NewGuid().ToString()
                    });
                }
                if (hasOwner ?? false)
                    result.OwnerUser = entities.GetUserQ(transport: ownerTransport, hasChilds: false);
                if (hasChilds.HasValue)
                    result.ChildUsers.Add(entities.GetUserQ(hasOwner: false));
                create?.Invoke(result);
                entities.User.Add(result);
                entities.SaveChanges();
                entities.Entry(result).Reload();

                result = entities.GetUserRequest(where, enabled, transport, notSameToUsers, hasOwner, ownerTransport, hasChilds);
                Assert.IsNotNull(result, "Created new instance of user does not correspond to condition");
            }
            return result;
        }

        private static User GetUserRequest(this ChatEntities entities,
            Func<IQueryable<User>, IQueryable<User>> where,
            bool enabled,
            TransportKind? transport,
            Guid[] notSameToUsers,
            bool? hasOwner,
            TransportKind? ownerTransport,
            bool? hasChilds) {
            IQueryable<User> q = entities.User.Where(u => u.Enabled == enabled && u.IsBot == false);
            if (transport.HasValue)
                q = q.Where(u => u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)transport).Any());
            if (notSameToUsers != null && notSameToUsers.Length > 0)
                q = q.Where(u => notSameToUsers.Contains(u.Id) == false);
            if (hasOwner.HasValue)
                q = q.Where(u => u.OwnerUserId.HasValue == hasOwner.Value);
            if (ownerTransport.HasValue)
                q = q.Where(u => u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)ownerTransport).Any());
            if (hasChilds.HasValue)
                q = q.Where(u => u.ChildUsers.Where(c => c.Enabled).Any() == hasChilds.Value);
            if (where != null)
                q = where(q);
            return q.FirstOrDefault();
        }

        /// <summary>
        /// Search or create user with owner. User and owner has transport of specific type
        /// </summary>
        /// <param name="entities">Database entities</param>
        /// <param name="tk">user transport kind</param>
        /// <param name="tkOwner">user's owner transport kind</param>
        /// <returns>User with owner</returns>
        public static User GetUserWithOwner(this ChatEntities entities, 
            TransportKind tk = TransportKind.FLChat, TransportKind tkOwner = TransportKind.FLChat) {
            return entities.GetUser(
                u => u.Enabled && u.OwnerUserId != null
                    && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)tk).Any()
                    && u.OwnerUser.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)tkOwner).Any(),
                u => {
                    u.OwnerUserId = entities.GetUser(u2 => u2.Enabled && u2.Id != u.Id && u2.OwnerUserId == null, null, tkOwner).Id;
                },
                tk);
        }

        /// <summary>
        /// Extract list of users or create new with specific transport
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="cnt">count of users</param>
        /// <param name="wf">where condition</param>
        /// <param name="nf">fill user fields function</param>
        /// <param name="tk">Transport kind</param>
        /// <returns>list of users</returns>
        public static User[] GetUsers(this ChatEntities entities, int cnt, Func<User, bool> wf, Action<User> nf, TransportKind tk) {
            User[] users = entities.GetUsers(cnt,
                u => wf(u) && u.Transports.Where(t => t.Enabled && t.TransportTypeId == (int)tk).Any(),
                u => {
                    nf?.Invoke(u);
                    u.Transports = new Transport[] { NewTransport(tk) };
                });
            return users;
        }

        static readonly TransportKind[] tkNulls = new TransportKind[] { TransportKind.FLChat, TransportKind.Email, TransportKind.Sms, TransportKind.WebChat };

        public static Transport NewTransport(TransportKind tk) {
            
            Transport t = new Transport() { Enabled = true, Kind = tk };
            if (!tkNulls.Contains(tk))
                t.TransportOuterId = tk.ToString() + Guid.NewGuid().ToString();
            return t;
        }

        /// <summary>
        /// Search segment or create new with number of active users
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="wf">Where condition</param>
        /// <param name="nf">Adjust new created user function</param>
        /// <param name="uNum">Transport kind</param>
        /// <returns></returns>
        //public static Segment GetSegment(this ChatEntities entities, Func<Segment, bool> swf, Action<Segment> snf, int uNum, Func<User, bool> uwf, Action<User> unf)
        //{
        //    Segment result = null;
        //    result = entities.Segment.Where(u => u.Id != Guid.Empty).Where(swf).FirstOrDefault();

        //    if (result == null)
        //    {
        //        result = new Segment()
        //        {
        //            IsDeleted = false,
        //            Name = "created by test"//,
        //            //InsertDate = DateTime.Now
        //        };
        //        entities.Segment.Add(result);
        //    }
        //    snf?.Invoke(result);  //Или позже?

        //    int miss = uNum - result.Members.Where(uwf).Count();
        //    Func<User, bool> newuwf = (User u) => uwf(u) && u.Segments.All(x => x.Id != result.Id);

        //    User[] tmp = GetUsers(entities, miss, newuwf, unf);
        //    for (int i = 0; i < miss; ++i)
        //    {
        //        //User tmp = GetUser(entities, uwf, unf);
        //        result.Members.Add(tmp[i]);
        //    }            

        //    entities.SaveChanges();
        //    result = entities.Segment.Where(swf).FirstOrDefault();
        //    Assert.IsNotNull(result, "Created new instance of user does not correspond to condition");
        //    return result;
        //}

        public static Segment GetSegment(this ChatEntities entities, Func<Segment, bool> swf, Action<Segment> snf)
        {
            Segment result = null;
            result = entities.Segment.Where(u => u.Id != Guid.Empty).Where(swf).FirstOrDefault();
            if (result != null)
                return result;

            result = new Segment()
            {
                IsDeleted = false,
                Name = (new Guid()).ToString(),
                Descr = "not null description",
            };
            entities.Segment.Add(result);
            entities.SaveChanges();
            result.Name = "created_by_test_" + result.Id.ToString();
            snf?.Invoke(result);  //Или позже?

            entities.SaveChanges();
            result = entities.Segment.Where(swf).FirstOrDefault();
            Assert.IsNotNull(result, "Created new instance of user does not correspond to condition");
            return result;
        }

        public static StructureNode GetStructureNode(this ChatEntities entities, Func<StructureNode, bool> swf, Action<StructureNode> snf)
        {
            StructureNode result = null;
            result = entities.StructureNode./*Where(u => u.Id != Guid.Empty).*/Where(swf).FirstOrDefault();
            if (result != null)
                return result;

            result = new StructureNode()
            {
                Name = (new Guid()).ToString(),
                ParentNodeId = Guid.Empty,
                IsShowSegments = false,
                IsShowParentUsers  = false,
                Order  = 0,
            };
            entities.StructureNode.Add(result);
            entities.SaveChanges();
            result.Name = "created_by_test_" + result.Id.ToString();
            snf?.Invoke(result);  //Или позже?

            entities.SaveChanges();
            result = entities.StructureNode.Where(swf).FirstOrDefault();
            Assert.IsNotNull(result, "Created new instance of user does not correspond to condition");
            return result;
        }

        public static Message SendMessage(
            this ChatEntities entities,
            Guid from,
            Guid to,
            TransportKind fromt = TransportKind.FLChat,
            TransportKind tot = TransportKind.FLChat,
            string text = "Test message",
            MessageKind kind = MessageKind.Personal,
            string fileMediaType = null,
            string fileName = null,
            int fileLength = 100,
            bool autoGenWebChatCode = true,
            DateTime? delayedStart = null)
        {

            //send message
            Message msg = new Message() {
                FromUserId = from,
                FromTransportKind = fromt,
                Kind = kind,
                Text = text,
                DelayedStart = delayedStart,
            };

            if (fileMediaType != null) {
                FileInfo fi = new FileInfo() {
                    FileOwnerId = Global.SystemBotId,
                    MediaType = entities.FindOrCreateMediaType(fileMediaType, MediaGroupKind.Document, false),
                    FileName = fileName ?? "somefile.dat",
                    FileLength = fileLength,
                };
                msg.FileInfo = fi;
            }

            msg.ToUsers.Add(new MessageToUser() {
                IsSent = (tot == TransportKind.FLChat),
                ToTransportKind = tot,
                ToUserId = to
            });
            entities.Message.Add(msg);
            entities.SaveChanges();

            if (tot == TransportKind.WebChat && autoGenWebChatCode)
                new FLChat.Core.Algorithms.WebChat.WebChatCodeGenerator().Gen(msg.ToUser);

            return msg;
        }

        public static MediaType GetMediaType(this ChatEntities entities,
            Func<IQueryable<MediaType>, IQueryable<MediaType>> where = null,
            string name = null,
            Action<MediaType> create = null,
            bool enabled = true,
            MediaGroupKind group = MediaGroupKind.Image) {
            MediaType mt = entities.QueryMediaType(where, name, enabled, group);
            if (mt == null) {
                mt = new MediaType() {
                    Name = name ?? Guid.NewGuid().ToString(),
                    CanBeAvatar = false,
                    Kind = group,
                    Enabled = enabled,
                };
                create?.Invoke(mt);
                entities.MediaType.Add(mt);
                entities.SaveChanges();
            }
            return entities.QueryMediaType(where, name, enabled, group) ?? throw new Exception("Created and searched media type are not equal");
        }

        private static MediaType QueryMediaType(this ChatEntities entities,
            Func<IQueryable<MediaType>, IQueryable<MediaType>> where,
            string name,
            bool enabled,
            MediaGroupKind group) {
            IQueryable<MediaType> q = entities.MediaType
                .Where(mt => mt.Enabled == enabled)
                .Where(mt => mt.MediaTypeGroupId == (int)group);
            if (name != null)
                q = q.Where(mt => mt.Name == name);
            if (where != null)
                q = where(q);
            return q.FirstOrDefault();
        }
    }
}
