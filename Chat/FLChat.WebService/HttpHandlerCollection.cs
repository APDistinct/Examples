using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.WebService.Handlers;
using FLChat.WebService.Handlers.Auth;
using FLChat.WebService.Handlers.Message;
using FLChat.WebService.Handlers.WebChat;
using FLChat.WebService.Handlers.User;
using FLChat.WebService.Handlers.Segments;
using FLChat.WebService.Handlers.File;
using FLChat.Core;
using FLChat.Core.MsgCompilers;
using FLChat.WebService.Handlers.Info;

namespace FLChat.WebService
{
    public static class Factory
    {
        public static IMessageTextCompilerWithCheck TextCompiler = IMessageTextCompilerExtentions.CreateTagTextCompiler(genLink: false);
    }

    #region Auth
    public class ConfigGetHttpHandler : CheckAuthHttpHandler
    {
        public ConfigGetHttpHandler() : base(new GetConfig().Adapt(), false)
        {
        }
    }

    public class LoginHttpHandler : CheckAuthHttpHandler
    {
        public LoginHttpHandler() : base(new Login().Adapt(), false)
        {
        }
    }

    public class TokenHttpHandler : CheckAuthHttpHandler
    {
        public TokenHttpHandler() : base(new GetToken(new AuthTokenFactory()).Adapt(), false)
        {
        }
    }

    public class LogoutHttpHandler : CheckAuthHttpHandler
    {
        public LogoutHttpHandler() : base(new Logout(), true)
        {
        }
    }
    #endregion

    #region Profile
    public class ProfileGetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileGetHttpHandler() : base(new GetUserInfo().Adapt()) { }
    }

    public class ProfileSetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileSetHttpHandler() : base(new SetUserInfo().Adapt()) { }
    }
    public class ProfileAvatarGetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileAvatarGetHttpHandler() : base(new GetProfileAvatar().Adapt()) { }
    }

    public class ProfileAvatarSetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileAvatarSetHttpHandler() : base(new SetProfileAvatar().Adapt()) { }
    }

    public class ProfileAvatarDelHttpHandler : CheckAuthHttpHandler
    {
        public ProfileAvatarDelHttpHandler() : base(new DelProfileAvatar().Adapt()) { }
    }

    public class ProfileContactsGetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileContactsGetHttpHandler() : base(new GetUserContacts().Adapt()) { }
    }

    public class ProfileChildsGetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileChildsGetHttpHandler() : base(new GetUserChilds() { CalcalulateStructureCapacity = true }.Adapt()) { }
    }

    public class ProfileChildsGetHttpHandler_v2 : CheckAuthHttpHandler
    {
        public ProfileChildsGetHttpHandler_v2() : base(new GetUserChilds().Adapt()) { }
    }

    public class ProfileParentsGetHttpHandler : CheckAuthHttpHandler
    {
        public ProfileParentsGetHttpHandler() : base(new GetUserParents().Adapt()) { }
    }

    public class ProfilePasswordSetHttpHandler : CheckAuthHttpHandler
    {
        public ProfilePasswordSetHttpHandler() : base(new SetProfilePassword().Adapt()) { }
    }

    public class ProfilePhonelistSetHttpHandler : CheckAuthHttpHandler
    {
        public ProfilePhonelistSetHttpHandler() : base(new SetPhoneFile().Adapt()) { }
    }

    public class ProfileChildsCountHttpHandler : CheckAuthHttpHandler
    {
        public ProfileChildsCountHttpHandler() : base(new GetUserChildsCount().Adapt()) { }
    }
    #endregion

    #region User
    public class UserGetHttpHandler : CheckAuthHttpHandler
    {
        public UserGetHttpHandler() : base(new GetUserInfo().Adapt(1)) { }
    }

    public class UserSetHttpHandler : CheckAuthHttpHandler
    {
        public UserSetHttpHandler() : base(new SetUserInfo().Adapt(1, SetUserInfo.KeyFieldName)) { }
    }

    public class UserAvatarGetHttpHandler : CheckAuthHttpHandler
    {
        public UserAvatarGetHttpHandler() : base(new GetUserAvatar().Adapt(2), false) { }
    }

    public class UserAvatarPostHttpHandler : CheckAuthHttpHandler
    {
        public UserAvatarPostHttpHandler() : base(new SetUserAvatar().Adapt(2)) { }
    }

    public class UserAvatarDelHttpHandler : CheckAuthHttpHandler
    {
        public UserAvatarDelHttpHandler() : base(new DelUserAvatar().Adapt(2)) { }
    }

    //public class UserContactsGetHttpHandler : CheckAuthHttpHandler
    //{
    //    public UserContactsGetHttpHandler() : base(new GetUserContacts().Adapt(2)) { }
    //}

    public class UserChildsGetHttpHandler : CheckAuthHttpHandler
    {
        public UserChildsGetHttpHandler() : base(
            new GetUserChilds() { CalcalulateStructureCapacity = true }.Adapt(2, GetUserChilds.Key))
        { }
    }

    public class UserChildsGetHttpHandler_v2 : CheckAuthHttpHandler
    {
        public UserChildsGetHttpHandler_v2() : base(new GetUserChilds().Adapt(2, GetUserChilds.Key)) { }
    }

    public class UserParentsGetHttpHandler : CheckAuthHttpHandler
    {
        public UserParentsGetHttpHandler() : base(new GetUserParents().Adapt(2)) { }
    }

    public class UserCreateHttpHandler : CheckAuthHttpHandler
    {
        public UserCreateHttpHandler() : base(new CreateUser().Adapt()) { }
    }

    public class UserGetTransportHttpHandler : CheckAuthHttpHandler
    {
        public UserGetTransportHttpHandler() : base(new GetUserTransport().Adapt(2)) { }
    }

    public class UserSetTransportHttpHandler : CheckAuthHttpHandler
    {
        public UserSetTransportHttpHandler() : base(new SetUserTransport().Adapt(2, SetUserTransport.Key)) { }
    }

    public class UserSearchHttpHandler : CheckAuthHttpHandler
    {
        public UserSearchHttpHandler() : base(new SearchUser().Adapt()) { }
    }

    public class UserSelectionCountHttpHandler : CheckAuthHttpHandler
    {
        public UserSelectionCountHttpHandler() : base(new UserSelectionCount().Adapt()) { }
    }

    public class UserChildsCountHttpHandler : CheckAuthHttpHandler
    {
        public UserChildsCountHttpHandler() : base(new GetUserChildsCount().Adapt(3)) { }
    }

    public class UserLiteLinkHttpHandler : CheckAuthHttpHandler
    {
        public UserLiteLinkHttpHandler() : base(new UserLiteLink(new FLChat.Core.Algorithms.LiteDeepLinkStrategy()).Adapt(2), bot: true) { }
    }
    #endregion

    #region Message
    public class MessagePostHttpHandler : CheckAuthHttpHandler
    {
        public MessagePostHttpHandler() : base(new SendMessage(msgCompiler: Factory.TextCompiler).Adapt())
        {
        }
    }

    public class MessageEditHttpHandler : CheckAuthHttpHandler
    {
        public MessageEditHttpHandler() : base(new MessageEdit().Adapt(1, MessageEdit.Key))
        {
        }
    }

    public class MessageReadPostHttpHandler : CheckAuthHttpHandler
    {
        public MessageReadPostHttpHandler() : base(new ReadMessageNotify().Adapt())
        {
        }
    }

    public class EventsHttpHandler : CheckAuthHttpHandler
    {
        public EventsHttpHandler() : base(new GetEvents(msgCompiler: Factory.TextCompiler).Adapt())
        {
        }
    }

    public class MessageHistoryHttpHandler : CheckAuthHttpHandler
    {
        public MessageHistoryHttpHandler() : base(new MessageHistory(msgCompiler: Factory.TextCompiler).Adapt())
        {
        }
    }

    public class MessageLimitHttpHandler : CheckAuthHttpHandler
    {
        public MessageLimitHttpHandler() : base(new MessageLimit().Adapt()) { }
    }

    public class MessageSentHistoryHandler : CheckAuthHttpHandler
    {
        public MessageSentHistoryHandler() : base(new MessageSentHistory().Adapt()) { }
    }

    public class MessageSentInfoHandler : CheckAuthHttpHandler
    {
        public MessageSentInfoHandler() : base(new MessageSentInfo().Adapt(1)) { }
    }
    #endregion

    #region WebChat
    public class WebChatReadHttpHandler : CheckAuthHttpHandler
    {
        public WebChatReadHttpHandler() : base(new ReadWebChatExt(FLChat.Core.Factory.CreateDeepLinkStrategy(false, false)).Adapt(1), bot: true) { }
    }

    public class WebChatAnswerHttpHandler : CheckAuthHttpHandler
    {
        public WebChatAnswerHttpHandler() : base(new AnswerWebChat().Adapt(1, "Code"), bot: true) { }
    }

    public class LiteLinkHttpHandler : CheckAuthHttpHandler
    {
        public LiteLinkHttpHandler() : base(new LiteLink().Adapt(1), bot: true) { }
    }

    #endregion

    #region Segment
    public class SegmentGetHttpHandler : CheckAuthHttpHandler
    {
        public SegmentGetHttpHandler() : base(new GetSegment().Adapt(1))
        {
        }
    }

    public class SegmentGetAllHttpHandler : CheckAuthHttpHandler
    {
        public SegmentGetAllHttpHandler() : base(new GetSegmentsAll().Adapt())
        {
        }
    }
    #endregion

    #region Structure
    public class StructureGetHttpHandler : CheckAuthHttpHandler
    {
        public StructureGetHttpHandler() : base(new GetStructure().Adapt(1, GetStructure.Key)) { }
    }

    public class StructureRootGetHttpHandler : CheckAuthHttpHandler
    {
        public StructureRootGetHttpHandler() : base(new GetStructure().Adapt()) { }
    }
    #endregion

    #region File
    public class FileSetHttpHandler : CheckAuthHttpHandler
    {
        public FileSetHttpHandler() : base(new SetFile().Adapt()) { }
    }
    public class FileGetHttpHandler : CheckAuthHttpHandler
    {
        public FileGetHttpHandler() : base(new GetFile().Adapt(1), false) { }
    }
    public class ImageGetHttpHandler : CheckAuthHttpHandler
    {
        public ImageGetHttpHandler() : base(new GetImage().Adapt(1), false) { }
    }
    #endregion

    #region Admin
    public class UserSearchAllHttpHandler : CheckAuthHttpHandler
    {
        public UserSearchAllHttpHandler() : base(new SearchUserAll().Adapt()) { }
    }

    public class GetSegmentDBHttpHandler : CheckAuthHttpHandler
    {
        public GetSegmentDBHttpHandler() : base(new GetSegmentDB().Adapt(1, GetSegmentDB.Key)) { }
    }

    public class SegmentManageHttpHandler : CheckAuthHttpHandler
    {
        public SegmentManageHttpHandler() : base(new ManageSegment().Adapt(1, ManageSegment.Key)) { }
    }

    public class DelUserTransportHttpHandler : CheckAuthHttpHandler
    {
        public DelUserTransportHttpHandler() : base(new DelUserTransport().Adapt(1, DelUserTransport.Key)) { }
    }

    public class AdminSegmentListHttpHandler : CheckAuthHttpHandler
    {
        public AdminSegmentListHttpHandler() : base(new SegmentsList().Adapt()) { }
    }

    public class AdminUserSegmentsHttpHandler : CheckAuthHttpHandler
    {
        public AdminUserSegmentsHttpHandler() : base(new UserSegments().Adapt(2)) { }
    }

    public class AdminMessageSentInfoHandler : CheckAuthHttpHandler
    {
        public AdminMessageSentInfoHandler() : base(new MessageSentInfo() { OnlySelfMessages = false }.Adapt(1)) { }
    }

    public class MessageTypeLimitSetHttpHandler : CheckAuthHttpHandler
    {
        public MessageTypeLimitSetHttpHandler() : base(new MessageTypeLimitSet().Adapt(1, MessageTypeLimitSet.Key)) { }
    }

    public class MessageTypeLimitGetHttpHandler : CheckAuthHttpHandler
    {
        public MessageTypeLimitGetHttpHandler() : base(new MessageTypeLimitGet().Adapt()) { }
    }

    public class AdminMessageSentHistoryAllHandler : CheckAuthHttpHandler
    {
        public AdminMessageSentHistoryAllHandler() : base(new MessageSentHistory() {
            Mode = MessageSentHistory.ModeEnum.All,
            IncludeUserInfo = true
        }.Adapt()) { }
    }

    public class AdminMessageSentHistoryUserHandler : CheckAuthHttpHandler
    {
        public AdminMessageSentHistoryUserHandler() 
            : base(new MessageSentHistory() { Mode = MessageSentHistory.ModeEnum.SelectedUser }.Adapt(2, MessageSentHistory.Key)) { }
    }

    public class GetRankHttpHandler : CheckAuthHttpHandler
    {
        public GetRankHttpHandler() : base(new GetRank().Adapt(),false) { }
    }

    #endregion
}
