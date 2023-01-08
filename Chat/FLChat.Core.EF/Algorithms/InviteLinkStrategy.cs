using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

using FLChat.Core.InviteLink;
using System.Data.Entity;
using FLChat.DAL;

namespace FLChat.Core.Algorithms
{
    public class InviteLinkStrategy : IDeepLinkStrategy, IDeepLinkGenerator
    {
        //private static string SK = "nuzwjdfiv03schyqg1b5r4ak9e2lxtm786po";
        //private static Random _rnd = new Random();
        private static object _lock = new object();

        public class Context
        {
            public Context(LinkType link, User user)
            {
                User = user;
                Link = link;
                //UserNumber = user.FLUserNumber;
                UserId = user.Id;
            }

            //public Context(int userNumber)
            //{
            //    Link = LinkType.LinkByNumber;
            //    UserNumber = userNumber;
            //}

            public Context(Guid userId)
            {
                Link = LinkType.LinkById;
                UserId = userId;
            }

            public enum LinkType
            {
                //LinkByNumber,
                LinkById
            }

            public LinkType Link { get; }

            public User User { get; }
            //public int? UserNumber { get; }
            public Guid? UserId { get; }

            //public int ScenarioStep { get; set; }

            /// <summary>
            /// Messages will route to user id
            /// </summary>
            public User RouteTo { get; set; }
        }
        private readonly bool _createUser;

        public InviteLinkStrategy(bool createUser = true)
        {
            _createUser = createUser;
        }

        public bool AcceptDeepLink(ChatEntities entities, IDeepLinkData message,
            out User user, out Message answerTo, out object context, out IDeepLinkStrategy sender)
        {

            answerTo = null;
            //decode deep link
            if (!Decode(message.DeepLink, out int? consNumber, out Guid? guid))
            {
                context = null;
                user = null;
                sender = null;
                return false;
            }

            //if (consNumber.HasValue)
            //{
            //    sender = this;

            //    //seek user
            //    user = entities.User.Where(u => u.FLUserNumber == consNumber).Include(u => u.Transports).SingleOrDefault();
            //    if (user != null)
            //        context = new Context(Context.LinkType.LinkByNumber, user);
            //    else
            //        context = new Context(consNumber.Value);
            //    return true;
            //}

            if(_createUser)
            {
                sender = this;
                //seek user
                var transport = entities.Transport
                .Where(mt => mt.TransportTypeId == (int)message.TransportKind && mt.TransportOuterId == message.FromId)
                .SingleOrDefault();
                if (transport != null)
                {
                    user = transport.User;
                }
                else
                {
                    var userOwner = entities.User.Where(u => u.Id == guid.Value).SingleOrDefault();
                    //user = new User() { IsTemporary = false, };
                    user = new User() { IsTemporary = false, FullName = (message as IOuterMessage)?.FromName };
                    if (userOwner != null)
                        user.OwnerUserId = userOwner.UserId;
                    entities.User.Add(user);
                    entities.SaveChanges();
                }
                context = new Context(Context.LinkType.LinkById, user);
                return true;
            }

            if (guid.HasValue)
            {
                sender = this;
                user = null;
                //seek user
                //user = entities.User.Where(u => u.Id == guid.Value).Include(u => u.Transports).SingleOrDefault();
                //if (user != null)
                //    context = new Context(Context.LinkType.LinkById, user);
                //else
                context = new Context(guid.Value);
                return true;
            }

            throw new Exception("Consultant id is null");
            //throw new Exception("Both consultant number and id are null");
        }

        private bool Decode(string code, out int? number, out Guid? guid)
        {
            try
            {
                string value = code.InviteLinkDecode();
                //try
                //{
                //    number = value.ExtractConsNumber();
                //    guid = null;
                //    return true;
                //}
                //catch (ArgumentException) { }

                guid = value.ExtractGuid();
                number = null;
                return true;
            }
            catch (Exception)
            {
                number = null;
                guid = null;
                return false;
            }
        }

        //  ???  Что-то вообще надо?
        public void AfterAddTransport(ChatEntities entities, IDeepLinkData message, Transport transport, object context)
        {
            //Context cntx = context as Context ?? throw new InvalidCastException("Type of context is not InviteLinkStrategy.Context");

            //if (cntx.User == null)
            //    return;

            //var q = (from p in entities.User_GetParents(cntx.User.UserId, null)
            //         join t in entities.Transport on p.UserId equals t.UserId
            //         where t.TransportTypeId == (int)TransportKind.FLChat
            //            && t.Enabled
            //         orderby p.Deep descending
            //         select p.UserId);
            //Guid? parent = q.Cast<Guid?>().FirstOrDefault();

            //if (parent != null)
            //{
            //    cntx.RouteTo = entities.User.Where(u => u.Id == parent.Value).Single();
            //    transport.ChangeAddressee(entities, cntx.RouteTo);
            //}
        }

        public string Generate(User u)
        {
            lock (_lock)
            {
                //if (u.FLUserNumber.HasValue)
                //    return u.FLUserNumber.Value.LiteLinkCode(SK, null, _rnd);
                //else
                    return u.Id.InviteLinkCode();
            }
        }
    }
}
