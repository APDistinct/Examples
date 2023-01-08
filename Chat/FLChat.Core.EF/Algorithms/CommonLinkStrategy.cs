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
    public class CommonLinkStrategy : IDeepLinkStrategy, IDeepLinkGenerator
    {
        private static object _lock = new object();
        private const string IdPrefix = "com";

        public class Context
        {
            public Context(LinkType link, User user)
            {
                User = user;
                Link = link;
                //UserNumber = user.FLUserNumber;
                UserId = user.Id;
            }

            public Context(Guid? userId)
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

        public CommonLinkStrategy(bool createUser = true)
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

            if (_createUser)
            {
                sender = this;
                //seek user
                //var userOwner = entities.User.Where(u => u.Id == guid.Value).SingleOrDefault();
                //user = new User() { IsTemporary = false, };
                var transport = entities.Transport
                .Where(mt => mt.TransportTypeId == (int)message.TransportKind && mt.TransportOuterId == message.FromId)
                .SingleOrDefault();
                if (transport != null)
                {
                    user = transport.User;
                }
                else
                {
                    user = new User() { IsTemporary = false, FullName = (message as IOuterMessage)?.FromName };
                    //if (userOwner != null)
                    //    user.OwnerUserId = userOwner.UserId;
                    entities.User.Add(user);
                    entities.SaveChanges();
                }
                context = new Context(Context.LinkType.LinkById, user);
                return true;
            }

            //if (guid.HasValue)
            {
                sender = this;
                user = null;
                //seek user
                //user = entities.User.Where(u => u.Id == guid.Value).Include(u => u.Transports).SingleOrDefault();
                //if (user != null)
                //    context = new Context(Context.LinkType.LinkById, user);
                //else
                context = new Context(guid);
                return true;
            }

            throw new Exception("Consultant id is null");
            //throw new Exception("Both consultant number and id are null");
        }

        private bool Decode(string code, out int? number, out Guid? guid)
        {
            number = null;
            guid = null;
            return true;
            //try
            //{
            //    string value = code.InviteLinkDecode();
            //    //try
            //    //{
            //    //    number = value.ExtractConsNumber();
            //    //    guid = null;
            //    //    return true;
            //    //}
            //    //catch (ArgumentException) { }

            //    guid = value.ExtractGuid();
            //    number = null;
            //    return true;
            //}
            //catch (Exception)
            //{
            //    number = null;
            //    guid = null;
            //    return false;
            //}
        }

        //  ???  Что-то вообще надо?
        public void AfterAddTransport(ChatEntities entities, IDeepLinkData message, Transport transport, object context)
        {
            
        }

        public string Generate(User u)
        {
            lock (_lock)
            {
                return CommonLinkCode(u.Id);
                //return u.Id.CommonLinkCode();
            }
        }

        /*static public*/ private string CommonLinkCode(/*this */Guid guid)
        {
            string n = guid.ToString("N");
            return IdPrefix + n;
        }
    }
}
