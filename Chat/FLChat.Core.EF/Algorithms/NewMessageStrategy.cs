using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core.Routers;

namespace FLChat.Core.Algorithms
{
    /// <summary>
    /// Handle incoming new message
    /// </summary>
    public class NewMessageStrategy : IReceiveUpdateStrategy<ChatEntities> {
        private readonly IMessageRouter _router;
        private readonly ITransportIdSaver _idSaver;
        private readonly IFileLoader _uploadFile;
        private readonly IAvatarProvider _avatarProvider;

        /// <summary>
        /// Notificate about events during proccess outer message
        /// </summary>
        public interface IListener {
            /// <summary>
            /// New arrived user was created
            /// </summary>
            /// <param name="entities">database</param>
            /// <param name="message">incoming message</param>
            /// <param name="dbmessage">database message record</param>
            /// <param name="transport">new created user transport</param>
            void NewUserCreated(ChatEntities entities, IOuterMessage message, Transport transport);

            /// <summary>
            /// Result of accepting deep link
            /// </summary>
            /// <param name="entities">database</param>
            /// <param name="message">incoming message</param>
            /// <param name="dlResult">deep link accept result</param>
            /// <param name="transport">assigned transport</param>
            void DeepLinkAccepted(ChatEntities entities, IOuterMessage message, DeepLinkResult dlResult, Transport transport);

            /// <summary>
            /// Called before create new transport for user who accepted deep link
            /// </summary>
            /// <param name="entities">data entities</param>
            /// <param name="message">Transport message info</param>
            /// <param name="user">accepted deep link user</param>
            void BeforeAddTransport(ChatEntities entities, IDeepLinkData message, User user);
        }

        //private readonly IDeepLinkAcceptStrategy _deepLinkStrategy;
        private readonly IListener _listener;
        private readonly IDeepLinkStrategy _deepLink;

        /// <summary>
        /// If true, then search enabled and disabled transports. If transport was found is disabled, then enable this transport.
        /// If false, then search only enabled transport.
        /// Default value is false.
        /// </summary>
        public bool EnableTransportOnIncommingMessage { get; set; } = false;

        /// <summary>
        /// If true, then deeplink scenario will perform for message from already known user
        /// If false, then deeplink scenario will not perform, if user already known
        /// </summary>
        public bool ProcessDeepLinkForActiveUser { get; set; } = false;

        public NewMessageStrategy(
            IMessageRouter router, ITransportIdSaver idSaver,
            //IDeepLinkAcceptStrategy deepLink = null,
            IDeepLinkStrategy deepLink,
            IFileLoader uploadFile = null,
            IAvatarProvider avatarProvider = null,
            IListener listener = null) {
            _router = router ?? throw new ArgumentNullException(nameof(router));
            _idSaver = idSaver ?? throw new ArgumentNullException(nameof(idSaver));
            _deepLink = deepLink ?? throw new ArgumentNullException(nameof(deepLink));
            _uploadFile = uploadFile;
            _avatarProvider = avatarProvider;
            _listener = listener;
        }

        public NewMessageStrategy(IMessageRouter router, IDeepLinkStrategy deepLink, IFileLoader uploadFile = null,
            IAvatarProvider avatarProvider = null,
            IListener listener = null)
            : this(router, new TransportIdSaver(), deepLink, uploadFile: uploadFile, avatarProvider: avatarProvider, listener: listener) {
        }

        public IMessageRouter Router => _router;

        /// <summary>
        /// Handle new incoming message from outer world
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="message"></param>
        public void Process(ChatEntities entities, IOuterMessage message, out DeepLinkResult deepLinkResult) {
            if (entities.MessageTransportId
                .Where(mt => mt.TransportTypeId == (int)message.TransportKind && mt.TransportId == message.MessageId)
                .Any()) {
                deepLinkResult = null;
                return;
            }

            //search transport for incoming message
            Transport fromTransport = entities
                .Transport
                .GetTransportByOuterId(
                    message.TransportKind,
                    message.FromId,
                    q => q.Include(t => t.User),
                    onlyEnabled: !EnableTransportOnIncommingMessage);

            DAL.Model.Message answerTo = null;
            string deepLink = message.DeepLink;

            if (fromTransport != null && fromTransport.Enabled && deepLink != null && fromTransport.User.IsTemporary) {
                fromTransport.Enabled = false;
                fromTransport.TransportOuterId = "";
                entities.SaveChanges();
                fromTransport = null;
            }

            deepLinkResult = null;
            if (fromTransport == null
                || (deepLink != null && fromTransport.Enabled == false)
                || (ProcessDeepLinkForActiveUser && deepLink != null)) {

                if (deepLink != null) {
                    Transport newTransport = AcceptDeepLink(entities, fromTransport, message, deepLink, out answerTo, out deepLinkResult);
                    if (newTransport != null)
                        fromTransport = newTransport;
                }

                bool userWasCreated = false;
                if (fromTransport == null) {
                    fromTransport = CreateNewUser(entities, message);
                    userWasCreated = true;
                }

                if (deepLink != null)
                    _listener?.DeepLinkAccepted(entities, message, deepLinkResult, fromTransport);
                else if (userWasCreated)
                    _listener?.NewUserCreated(entities, message, fromTransport);

            } else {
                if (fromTransport.Enabled == false && EnableTransportOnIncommingMessage)
                    fromTransport.Enabled = true;
            }

            if (_avatarProvider != null)
                new AvatarLoader(_avatarProvider).TryLoadAvatar(entities, message, fromTransport);

            //register message body
            Message dbmsg = RegisterMessageBody(entities, fromTransport, message, answerTo);
            _idSaver.SaveFrom(entities, message.MessageId, dbmsg);

            //upload file if exists
            if (message.File != null && _uploadFile != null)
                UploadFile(entities, message, dbmsg);

            Guid? addressee = _router.RouteMessage(entities, message, dbmsg);
            if (addressee != null)
                RegisterMessageAddressee(entities, dbmsg, addressee.Value);

            entities.SaveChanges();
        }

        /// <summary>
        /// register message in database
        /// </summary>
        /// <param name="entities">database entities</param>
        /// <param name="from">Message's origin</param>
        /// <param name="message">info about incoming message</param>
        /// <returns>Database message object</returns>
        private Message RegisterMessageBody(ChatEntities entities, Transport from, IOuterMessage message, DAL.Model.Message answerTo) {
            Message dbmsg = new DAL.Model.Message() {
                Kind = MessageKind.Personal,
                FromUserId = from.UserId,
                FromTransportTypeId = from.TransportTypeId,
                Text = message.Text,

                FromTransport = from, 

                AnswerToId = answerTo?.Id,
                AnswerTo = answerTo
            };            
            entities.Message.Add(dbmsg);
            return dbmsg;
        }

        /// <summary>
        /// Register message's ToUser part
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="dbmsg">Message database object</param>
        /// <param name="addressee">addressee</param>
        private void RegisterMessageAddressee(ChatEntities entities, Message dbmsg, Guid addressee) {
            MessageToUser msgTo = new MessageToUser() {
                ToUserId = addressee,
                ToTransportKind = TransportKind.FLChat,
                IsSent = true
            };
            dbmsg.ToUsers.Add(msgTo);
        }

        /// <summary>
        /// create new user with transport
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="message">incoming message</param>
        /// <returns>new user transport</returns>
        private Transport CreateNewUser(ChatEntities entities, IOuterMessage message) {
            User user = new User() {
                Enabled = true,
                FullName = message.FromName,
                IsConsultant = false,
                IsTemporary = true,
            };
            Transport transport = new Transport() {
                TransportTypeId = (int)message.TransportKind,
                TransportOuterId = message.FromId,
                User = user,
                Enabled = true
            };
            user.Transports.Add(transport);
            entities.User.Add(user);
            return transport;
        }

        /// <summary>
        /// Seek deep link and link new transport for user
        /// </summary>
        /// <param name="entities">database</param>
        /// <param name="foundTransport">transport which found by transport user's id</param>
        /// <param name="message">incoming message</param>
        /// <param name="deepLink">deep link code</param>
        /// <param name="answerTo">answer to message</param>
        /// <param name="deepLinkResult">deep link accepted result</param>
        /// <returns></returns>
        private Transport AcceptDeepLink(ChatEntities entities, 
            Transport foundTransport,
            IOuterMessage message, 
            string deepLink, 
            out DAL.Model.Message answerTo,
            out DeepLinkResult deepLinkResult
            ) {
            bool result = _deepLink.AcceptDeepLink(entities, message,
                out User user, out answerTo, out object context, out IDeepLinkStrategy sender);

            if (!result) {
                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Unknown);
                return null;
            }

            if (user == null) {
                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Rejected, context);
                return null;
            }
            
            if (foundTransport != null 
                && foundTransport.UserId != user.Id && foundTransport.User.IsTemporary == false) {
                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Rejected, context);
                return foundTransport;
            }

            //create transport for user
            Transport transport = user.Transports.Get(message.TransportKind);
            if (transport == null) {
                _listener?.BeforeAddTransport(entities, message, user);

                //user has not that type of transport, add new
                transport = new Transport() {
                    Enabled = message.IsTransportEnabled,
                    Kind = message.TransportKind,
                    TransportOuterId = message.FromId,
                    User = user,
                    UserId = user.Id
                };
                user.Transports.Add(transport);

                entities.SaveChanges();

                sender?.AfterAddTransport(entities, message, transport, context);

                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Accepted, context);
                return transport;
            } else {
                if (transport.TransportOuterId == message.FromId) {
                    if (transport.Enabled) {
                        sender?.AfterAddTransport(entities, message, transport, context);
                        deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.AcceptedEarly, context);
                        return transport;
                    } else {
                        if (message.IsTransportEnabled) {
                            transport.Enabled = true;
                            entities.SaveChanges();
                        }
                        sender?.AfterAddTransport(entities, message, transport, context);
                        deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.AcceptedEarly, context);
                        return transport;
                    }
                } else {                    
                    //if (String.IsNullOrEmpty(transport.TransportOuterId)) {
                    _listener?.BeforeAddTransport(entities, message, user);

                    transport.Enabled = message.IsTransportEnabled;
                    transport.TransportOuterId = message.FromId;
                    entities.SaveChanges();

                    sender?.AfterAddTransport(entities, message, transport, context);

                    //context = new Context(webchat.Id, webchat.MsgId);
                    deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Accepted, context);
                    return transport;
                    //} else {
                    //    deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Rejected, context);
                    //    answerTo = null;
                    //    return null;
                    //}
                }
            }

            
            /*answerTo = null;
            WebChatDeepLink webchat = entities
                .WebChatDeepLink
                .Where(wc => wc.Link == deepLink && wc.ExpireDate >= DateTime.UtcNow)
                .SingleOrDefault();

            if (webchat == null) {
                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Unknown);
                return null;
            }

            //deep link was already accepted
            bool accepted = webchat.AcceptedTransportKind.Contains(message.TransportKind);

            answerTo = webchat.MessageToUser.Message;

            //get deep link user
            User user = entities.User.Where(u => u.Id == webchat.MessageToUser.ToUserId).Include(u => u.Transports).Single();  //webchat.MessageToUser.ToTransport.User;

            //transport will create in enabled or disabled state
            bool enableTransport = _deepLinkStrategy != null 
                ? _deepLinkStrategy.IsTransportEnabled(entities, message) 
                : true;

            //create transport for user
            Transport transport = user.Transports.Get(message.TransportKind);
            if (accepted == false || transport == null) {
                _deepLinkStrategy?.BeforeAddTransport(entities, message, user);

                if (transport != null) {
                    transport.Enabled = enableTransport;
                    transport.TransportOuterId = message.FromId;
                } else {
                    //user has not that type of transport, add new
                    transport = new Transport() {
                        Enabled = enableTransport,
                        Kind = message.TransportKind,
                        TransportOuterId = message.FromId,
                        User = user,
                        UserId = user.Id
                    };
                    user.Transports.Add(transport);
                }

                entities.SaveChanges();
                if (!accepted)
                    webchat.AcceptedTransportType.Add(entities.TransportType.Where(tt => tt.Id == (int)message.TransportKind).Single());

                //create event for user accepted deep link and attached new transport
                Event ev = new Event() {
                    Kind = EventKind.DeepLinkAccepted,
                    CausedByUserId = user.Id,
                    CausedByUserTransportTypeId = transport.TransportTypeId,
                    ToUsers = new User[] {
                        webchat.MessageToUser.Message.FromTransport.User
                    }
                };
                entities.Event.Add(ev);

                //set sent invite user as new user's messages addressee
                transport.ChangeAddressee(entities, webchat.MessageToUser.Message.FromTransport.User);

                deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Accepted, webchat.Id, webchat.MsgId);
                return transport;
            } else {
                if (transport.TransportOuterId == message.FromId) {
                    deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.AcceptedEarly, webchat.Id, webchat.MsgId);
                    if (transport.Enabled)
                        return transport;
                    else {
                        if (enableTransport) {
                            transport.Enabled = true;
                            entities.SaveChanges();
                        }
                        return transport;
                    }
                } else {
                    answerTo = null;
                    deepLinkResult = new DeepLinkResult(DeepLinkResultStatus.Rejected, webchat.Id, webchat.MsgId);
                    return null;
                }
            }*/
        }

        //private Message SeekAnswerToMessage(ChatEntities entities, TMessage message) {
        //    if (message.ReplyToMessageId == null)
        //        return null;

        //    Message replyTo = entities.MessageTransportId
        //        .Where(mid => mid.TransportTypeId == (int)message.TransportKind && mid.TransportId == message.ReplyToMessageId)
        //        .Select(mid => mid.Message)
        //        .SingleOrDefault();
        //    if (replyTo == null)
        //        return null; //may be throw exception?

        //    return replyTo;
        //}

        private void UploadFile(ChatEntities entities, IOuterMessage message, Message msg) {
            try {
                DownloadFileResult result = _uploadFile.Download(message.File);
                FileInfo fi = entities.SaveFile(result, msg.FromUserId);
                msg.FileId = fi.Id;
            } catch (Exception e) {
                msg.MessageError.Add(e.ToMessageError());
            }
        }
    }
}
