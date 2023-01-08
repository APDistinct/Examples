using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.WebService.DataTypes;
using FLChat.WebService.MediaType;
using FLChat.FDAL.Model;
using System.Data.Entity;
using FLChat.WebService.Handlers.File;
using FLChat.Core;

namespace FLChat.WebService.Handlers.Message
{
    /// <summary>
    /// Send message
    /// </summary>
    public class SendMessage : IObjectedHandlerStrategy<SendMessageRequest, SendMessageResponse>
    {
        public bool IsReusable => true;
        public const string Prefix = MainDef.PrefixSeg; // "seg-";

        protected readonly IMediaTypeChecker _fileChecker;
        private readonly IMessageTextCompilerWithCheck _msgCompiler;

        public int SizeLimit { get; set; } = 1024 * 1024;

        public SendMessage(IMediaTypeChecker fileChecker = null, IMessageTextCompilerWithCheck msgCompiler = null)
        {
            if (fileChecker == null)
                fileChecker = new FileChecker();
            _fileChecker = fileChecker;
            _msgCompiler = msgCompiler;
        }

        public SendMessageResponse ProcessRequest(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            UploadFile(entities, currUserInfo, input);
            if (input.Type != MessageKind.Personal && input.ToPhoneList)
            {
                List<Guid> toUsers = currUserInfo.GetUser(entities).MatchedPhonesAddr.Select(a =>  a.UserId ).ToList();
                UserSelection selection = new UserSelection {Include = toUsers};

                input.Selection = selection;
                input.ToUsers = null;
                input.ToSegments = null;

            }

            switch (input.Type)
            {
                case MessageKind.Personal:
                    return SendPersonalMessage(entities, currUserInfo, input);
                //case MessageKind.Segment:
                //    return SendSegmentMessage(entities, currUserInfo, input);
                case MessageKind.Broadcast:
                    return SendBroadcastMessage(entities, currUserInfo, input);
                case MessageKind.Mailing:
                    return SendMailingMessage(entities, currUserInfo, input);
                default:
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support, "This type of message not support yet");
            }
        }

        private SendMessageResponse SendPersonalMessage(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            //check data is correct
            if (input.ToUser.HasValue == false)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Field to_user can't be empty");

            //check message addressee is't current user
            if (input.ToUser == currUserInfo.UserId)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support, "Can't send message to himself");

            //if addressee's transport has not mention
            if (input.ToTransport.HasValue == false)
            {
                //get default transport
                TransportKind? deftransportKind = (TransportKind?)entities
                    .UserDefaultTransportView
                    .Where(t => t.UserId == input.ToUser.Value)
                    .Select(t => t.DefaultTransportTypeId)
                    .SingleOrDefault();

                if (deftransportKind.HasValue)
                    input.ToTransport = deftransportKind; 
            }
            else
            {
                bool existTransport = entities.Transport.Where(t =>
                    t.Enabled == true && t.TransportType.Enabled
                    && t.UserId == input.ToUser.Value
                    && t.TransportTypeId == (int)input.ToTransport.Value
                    && t.User.Enabled == true).Select(t => true).SingleOrDefault();
                if (!existTransport)
                    input.ToTransport = null;
            }

            if (input.ToTransport.HasValue == false)
                return new SendMessagePersonalResponse()
                {
                    User = new SendMessagePersonalInfo() { Status = MessageStatus.TransportNotFound, ToUser = input.ToUser.Value }
                };

            //register message
            DAL.Model.Message msg = MakeMessage(currUserInfo.UserId, MessageKind.Personal, input);
            //    new DAL.Model.Message()
            //{
            //    FromTransportKind = TransportKind.FLChat,
            //    FromUserId = currUserInfo.UserId,
            //    Kind = MessageKind.Personal,
            //    Text = input.Text,
            //    FileId = input.FileId,
            //    DelayedStart = input.DelayedStart,
            //    NeedToChangeText = _msgCompiler?.IsChangable(input.Text) ?? false,
            //};
            msg.ToUsers.Add(new MessageToUser()
            {
                ToTransportKind = input.ToTransport.Value,
                ToUserId = input.ToUser.Value,
                IsSent = input.ToTransport.Value == TransportKind.FLChat ? true : false
            });

            entities.Message.Add(msg);
            entities.SaveChanges();

            MessageToUser mtu = msg.ToUsers.Single();
            //return answer
            return new SendMessagePersonalResponse()
            {
                MessageId = msg.Id,
                FileId = msg.FileId,
                User = new SendMessagePersonalInfo()
                {
                    Status = mtu.GetMessageStatus(),
                    ToTransport = mtu.ToTransportKind,
                    ToUser = mtu.ToUserId
                }
            };
        }

        //private SendMessageResponse SendSegmentMessage(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        //{
        //    //check data is correct
        //    if (input.ToSegment == null)
        //        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Field to_segment can't be empty");

        //    //check message addressee is't current user
        //    //if (input.ToUser == currUserInfo.UserId)
        //    //    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support, "Can't send message to himself");

        //    //if addressee's transport has not mention
        //    //if (input.ToTransport.HasValue == false)
        //    //{
        //    //    //get default transport
        //    //    TransportKind? deftransportKind = (TransportKind?)entities
        //    //        .UserDefaultTransportView
        //    //        .Where(t => t.UserId == input.ToUser.Value)
        //    //        .Select(t => t.DefaultTransportTypeId)
        //    //        .SingleOrDefault();

        //    //    if (deftransportKind.HasValue)
        //    //        input.ToTransport = deftransportKind;
        //    //}
        //    //else
        //    //{
        //    //    bool existTransport = entities.Transport.Where(t =>
        //    //        t.Enabled == true && t.TransportType.Enabled
        //    //        && t.UserId == input.ToUser.Value
        //    //        && t.TransportTypeId == (int)input.ToTransport.Value
        //    //        && t.User.Enabled == true).Select(t => true).SingleOrDefault();
        //    //    if (!existTransport)
        //    //        input.ToTransport = null;
        //    //}

        //    //if (input.ToTransport.HasValue == false)
        //    //    return new SendMessagePersonalResponse() { Status = MessageStatus.TransportNotFound };

        //    // TODO: Send message with outer transport? Is here? 
        //    //if (input.ToTransport.Value != TransportKind.FLChat && input.ToTransport.Value != TransportKind.Telegram)
        //    //    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.not_support, "Only FLChat messages is supported now");

        //    //register message
        //    DAL.Model.Message msg = new DAL.Model.Message()
        //    {
        //        FromTransportKind = TransportKind.FLChat,
        //        FromUserId = currUserInfo.UserId,
        //        Kind = MessageKind.Segment,
        //        Text = input.Text
        //    };
        //    //  Получить Id сегмента из его Caption
        //    Guid guid;
        //    if (Guid.TryParse(input.ToSegment.Substring(Prefix.Length), out guid))
        //    {
        //        msg.MessageToSegment.Add(new MessageToSegment()
        //        {
        //            SegmentId = guid
        //        });
        //    }
        //    else
        //    {
        //        throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "Field to_segment has bad segment's GUID");
        //    }

        //    entities.Message.Add(msg);
        //    entities.SaveChanges();

        //    //return answer
        //    return new SendMessageResponse()
        //    {
        //        MessageId = msg.Id,
        //        //Status = msg.ToUsers.Single().GetMessageStatus()
        //    };
        //}

        private SendMessageResponse SendPhonesMessage(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            var phones = input.ToPhones.Distinct().ToList();
            var users = entities.User.Where(a => phones.Contains(a.Phone));
            var toUsers = users.Select(a => new UserSendInfo {ToUser = a.UserId});
            input.ToUsers = toUsers;

            return SendBroadcastMessage(entities, currUserInfo, input);
        }

        private SendMessageResponse SendBroadcastMessage(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            VerifyBroadcastInput(input);

            //AddProhibitionList(entities, currUserInfo, input);

            List<MessageToUser> directUsers = new List<MessageToUser>();

            //register message
            DAL.Model.Message msg = MakeMessage(currUserInfo.UserId, MessageKind.Broadcast, input);
            //DAL.Model.Message msg = new DAL.Model.Message()
            //{
            //    FromTransportKind = TransportKind.FLChat,
            //    FromUserId = currUserInfo.UserId,
            //    Kind = MessageKind.Broadcast,
            //    Text = input.Text,
            //    FileId = input.FileId,
            //    DelayedStart = input.DelayedStart,
            //    NeedToChangeText = _msgCompiler?.IsChangable(input.Text) ?? false,
            //};

            //  Разбор и отправка отдельным юзерам
            List<Guid> listusersNO = new List<Guid>();
            if (input.ToUsers?.Any() ?? false)
            {
                //  Не указан транспорт
                List<Guid> list = input.ToUsers.Where(x => x.ToUser != null)
                    .Where(x => x.ToTransport == null).Select(x => x.ToUser.Value).ToList();
                var inputOK = input.ToUsers
                    //.Where(x => x.ToUser != null)
                    .Where(x => x.ToTransport != null).ToList();
                //  
                var dic = entities.GetUserTransport(list.Distinct());
                //  Без транспорта совсем
                listusersNO = dic.Where(d => d.Value == null).Select(x => x.Key).ToList();
                // Добавляем тех, у кого транспорт нашёлся
                inputOK.AddRange(dic.Where(d => d.Value != null)
                    .Select(x => new UserSendInfo() { ToUser = x.Key, ToTransport = x.Value }));

                //  А вот тут вопрос - может стоит здесь перехватывать различные проблемы, потому как у нас не один, а много
                //  И как-то отсылать их в списке неотправленных со специальным признаком?

                foreach (var msend in inputOK.Distinct())
                {
                    MessageToUser mtu = new MessageToUser()
                    {
                        ToTransportKind = msend.ToTransport.Value,
                        ToUserId = msend.ToUser.Value,
                        IsSent = msend.ToTransport == TransportKind.FLChat ? true : false
                    };
                    msg.ToUsers.Add(mtu);
                    directUsers.Add(mtu);
                }
            }
            //var trans = entities.Database.BeginTransaction();
            entities.Message.Add(msg);
            //entities.SaveChanges(); //must have two save changed! EF so funny

            if (input.Selection != null)
                AddSelection(entities, msg, input.Selection);

            //  Разбор и отправка по сегментам
            if (input.ToSegments?.Any() ?? false)
            {
                foreach (var msend in input.ToSegments)
                {
                    if (Guid.TryParse(msend.Substring(Prefix.Length), out Guid guid))
                    {
                        msg.MessageToSegment.Add(new MessageToSegment()
                        {
                            SegmentId = guid
                        });
                    }
                }
            }
            //entities.Message.Add(msg);

            entities.SaveChanges();
            bool result = entities.Message_ProduceToUsers(msg.Id,
                out DAL.DataTypes.LimitInfoResult limit,
                out List<MessageToUser> addressee);

            if (!result)
            {
                //trans.Rollback();
                throw new ErrorResponseException(HttpStatusCode.Forbidden, new MessageLimitErrorResponse(new LimitInfo(msg.Kind, limit)));
            }

            //trans.Commit();
            //entities.Entry(msg).Collection(m => m.ToUsers).Load();

            List<SendMessagePersonalInfo> usersRet = new List<SendMessagePersonalInfo>();
            //  Поставленные на отправку с известным транспортом 
            usersRet.AddRange(directUsers.Concat(addressee).Select(m => new SendMessagePersonalInfo()
            {
                ToUser = m.ToUserId,
                ToTransport = m.ToTransportKind,
                Status = m.GetMessageStatus()
            }).ToList());
            //  Точно не отправленные - без транспорта
            usersRet.AddRange(listusersNO.Select(x => new SendMessagePersonalInfo()
            {
                ToUser = x,
                Status = MessageStatus.TransportNotFound
            }).ToList());

            //return answer
            return new SendMessageBroadcastResponse()
            {
                MessageId = msg.Id,
                FileId = msg.FileId,
                Users = usersRet,
            };
        }

        private SendMessageResponse SendMailingMessage(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            VerifyBroadcastInput(input);

            //AddProhibitionList(entities, currUserInfo, input);

            List<MessageToUser> directUsers = new List<MessageToUser>();

            //register message
            DAL.Model.Message msg = MakeMessage(currUserInfo.UserId, MessageKind.Mailing, input);
            //DAL.Model.Message msg = new DAL.Model.Message()
            //{
            //    FromTransportKind = TransportKind.FLChat,
            //    FromUserId = currUserInfo.UserId,
            //    Kind = MessageKind.Mailing,
            //    Text = input.Text,
            //    FileId = input.FileId,
            //    DelayedStart = input.DelayedStart,
            //    NeedToChangeText = _msgCompiler?.IsChangable(input.Text) ?? false,
            //};

            //  Разбор и отправка отдельным юзерам
            List<Guid> listusersNO = new List<Guid>();
            if (input.ToUsers?.Any() ?? false)
            {
                // Общая идея - не смотреть на то, какой тип транспорта установлен при отправке
                // Всем расставляется тип рассылки, на 15.08.2019 это только Email
                // Делаем запрос на наличие транспорта Email, всем, у кого он есть отправка идёт, у кого нет - не идёт...

                List<Guid> list = input.ToUsers.Where(x => x.ToUser != null)
                    /*.Where(x => x.ToTransport == null)*/.Select(x => x.ToUser.Value).ToList();
                //var inputOK = input.ToUsers                    
                //    .Where(x => x.ToTransport != null).ToList();

                //  
                var dic = entities.GetUserMailingTransport(list.Distinct());
                //  Без транспорта совсем
                listusersNO = dic.Where(d => d.Value == null).Select(x => x.Key).ToList();
                // Добавляем тех, у кого транспорт нашёлся
                var inputOK = (dic.Where(d => d.Value != null)
                    .Select(x => new UserSendInfo() { ToUser = x.Key, ToTransport = x.Value }));

                //  А вот тут вопрос - может стоит здесь перехватывать различные проблемы, потому как у нас не один, а много
                //  И как-то отсылать их в списке неотправленных со специальным признаком?

                foreach (var msend in inputOK.Distinct())
                {
                    MessageToUser mtu = new MessageToUser()
                    {
                        ToTransportKind = msend.ToTransport.Value,
                        ToUserId = msend.ToUser.Value,
                        IsSent = msend.ToTransport == TransportKind.FLChat ? true : false
                    };
                    msg.ToUsers.Add(mtu);
                    directUsers.Add(mtu);
                }
            }

            //var trans = entities.Database.BeginTransaction();
            entities.Message.Add(msg);
            //entities.SaveChanges(); //must have two save changed! EF so funny

            if (input.Selection != null)
                AddSelection(entities, msg, input.Selection);

            //  Разбор и отправка по сегментам
            if (input.ToSegments?.Any() ?? false)
            {
                foreach (var msend in input.ToSegments)
                {
                    if (Guid.TryParse(msend.Substring(Prefix.Length), out Guid guid))
                    {
                        msg.MessageToSegment.Add(new MessageToSegment()
                        {
                            SegmentId = guid
                        });
                    }
                }
            }
            //entities.Message.Add(msg);

            entities.SaveChanges(); //must have two save changed! EF so funny

            bool result = entities.Message_ProduceToUsers(msg.Id,
                out DAL.DataTypes.LimitInfoResult limit,
                out List<MessageToUser> addressee);

            if (!result)
            {
                //trans.Rollback();
                throw new ErrorResponseException(HttpStatusCode.Forbidden, new MessageLimitErrorResponse(new LimitInfo(msg.Kind, limit)));
            }

            //trans.Commit();

            //entities.Entry(msg).Collection(m => m.ToUsers).Load();

            List<SendMessagePersonalInfo> usersRet = new List<SendMessagePersonalInfo>();
            //  Поставленные на отправку с известным транспортом 
            usersRet.AddRange(directUsers.Concat(addressee).Select(m => new SendMessagePersonalInfo()
            {
                ToUser = m.ToUserId,
                ToTransport = m.ToTransportKind,
                Status = m.GetMessageStatus()
            }).ToList());
            //  Точно не отправленные - без транспорта
            usersRet.AddRange(listusersNO.Select(x => new SendMessagePersonalInfo()
            {
                ToUser = x,
                Status = MessageStatus.TransportNotFound
            }).ToList());

            //return answer
            return new SendMessageBroadcastResponse()
            {
                MessageId = msg.Id,
                Users = usersRet,
            };
        }

        private static void AddProhibitionList(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            var excludedUsers = currUserInfo.GetUser(entities).PersonalProhibitionSlave.Select(a => a.Id).ToList();
            if (excludedUsers.Any())
            {
                if (input.Selection == null)
                {
                    input.Selection = new UserSelection {Exclude = new List<Guid>()};
                }

                if (input.Selection.Exclude == null)
                {
                    input.Selection.Exclude = new List<Guid>();
                }

                input.Selection.Exclude.AddRange(excludedUsers);
            }
        }

        private void UploadFile(ChatEntities entities, IUserAuthInfo currUserInfo, SendMessageRequest input)
        {
            // Заполнена одна из переменных - файл или ссылка на него
            if (input.File != null && input.FileId != null)
            {
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, "file and file_id are both not empty");
            }
            FilePerformer filePerformer = new FilePerformer();
            if (input.FileId != null)
            {
                //  А есть ли он такой в нашем хранилище?
                //if (!FileFind(input.FileId.Value))

                if (!filePerformer.FileFind(entities, input.FileId.Value))
                {
                    throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error, $"file {input.FileId } not found");
                }
            }
            if (input.File != null)
            {
                // Проверка на допустимость файла

                //int mediaTypeId = 1; //0; 
                //int mediaTypeGroupId = 0;
                //// 
                //var mt = input.File.DataBin.GetFileMediaType();
                //_fileChecker.Check(entities, input.File.DataBin, mt, out mediaTypeId, out mediaTypeGroupId);
                //if (mediaTypeGroupId != (int)input.File.FileMediaType) {
                //    throw new ErrorResponseException(
                //        (int)HttpStatusCode.UnsupportedMediaType,
                //        new ErrorResponse(ErrorResponse.Kind.not_support, $"File {input.File.FileName} media data is invalid"));
                //}
                //input.File.FileMediaTypeId = mediaTypeId;

                //  Сохранение файла
                //FileEntities fileEntities = new FileEntities();
                //transInfo = entities.Database.BeginTransaction();
                //transData = fileEntities.Database.BeginTransaction();
                //var guid = SaveFileInfo(input.File, currUserInfo.UserId, entities);
                //SaveFileData(input.File, guid, fileEntities);

                //var guid = filePerformer.SaveFile(input.File, currUserInfo.UserId, entities);
                input.FileId = filePerformer.PerformMessage(input.File, currUserInfo.UserId, entities/*, _fileChecker*/);
                // Id сохранённого файла
            }
        }

        private void AddSelection(ChatEntities entities, DAL.Model.Message msg, UserSelection selection)
        {
            //entities.UserSelectionToMessageToUser(
            //msg.Id,
            //msg.FromUserId,
            //selection.Convert(),
            //msg.Kind.DefaultTransportViewName());
            if (selection.IncludeWithStructure != null)
                foreach (var rec in selection.IncludeWithStructure)
                    msg.MessageToSelection.Add(new MessageToSelection()
                    {
                        UserId = rec.UserId,
                        WithStructure = true,
                        Include = true,
                        StructureDeep = (rec.Type == UserSelection.SelectionType.Deep ? null : (int?)1)
                    });
            if (selection.ExcludeWithStructure != null)
                foreach (var rec in selection.ExcludeWithStructure)
                    msg.MessageToSelection.Add(new MessageToSelection()
                    {
                        UserId = rec,
                        WithStructure = true,
                        Include = false
                    });
            if (selection.Include != null)
                foreach (var rec in selection.Include)
                    msg.MessageToSelection.Add(new MessageToSelection()
                    {
                        UserId = rec,
                        WithStructure = false,
                        Include = true
                    });
            if (selection.Exclude != null)
                foreach (var rec in selection.Exclude)
                    msg.MessageToSelection.Add(new MessageToSelection()
                    {
                        UserId = rec,
                        WithStructure = false,
                        Include = false
                    });
            if (selection.Segments != null)
                foreach (var rec in selection.Segments.ToSegmentGuids())
                    msg.MessageToSegment.Add(new MessageToSegment()
                    {
                        SegmentId = rec
                    });
        }

        private void VerifyBroadcastInput(SendMessageRequest input)
        {
            bool toUsers = input.ToUsers?.Any() ?? false;
            bool toSegms = input.ToSegments?.Any() ?? false;

            if ((input.ToUsers != null || input.ToSegments != null) && input.Selection != null)
                throw new ErrorResponseException(HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    "Only one of fields [selection] or [to_users, to_segments] must be present");
            if (toSegms == false && toUsers == false && (input.Selection?.IsExists ?? false) == false)
                throw new ErrorResponseException
                    (HttpStatusCode.BadRequest, ErrorResponse.Kind.input_data_error,
                    "Fields [to_segment], [to_user] and [selection] can't be empty or null at same time");
        }

        private DAL.Model.Message MakeMessage(Guid userId, MessageKind kind, SendMessageRequest input)
        {
            //DAL.Model.Message msg = 
            return   new DAL.Model.Message()
            {
                FromTransportKind = TransportKind.FLChat,
                FromUserId = userId,
                Kind = kind,
                Text = input.Text,
                FileId = input.FileId,
                DelayedStart = input.DelayedStart,
                NeedToChangeText = _msgCompiler?.IsChangable(input.Text) ?? false,
            };
        }
    }
}
