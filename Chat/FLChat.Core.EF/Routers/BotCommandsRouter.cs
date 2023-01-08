using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL;
using FLChat.DAL.Model;
using FLChat.Core.Algorithms;
using System.Globalization;

namespace FLChat.Core.Routers
{
    public abstract class BotCommandsRouter : IMessageRouter
    {
        public enum CommandsEnum
        {
            SelectAddressee,
            MyScore,
            MyProfile,
            Url,
            SwitchAddressee,
            Back,
            Hello,
            Phone,
            Ok,
            No
        }

        public const string CMD_SELECT_ADDRESSEE = "cmd:select_addressee";
        public const string CMD_SCORE = "cmd:score";
        public const string CMD_PROFILE = "cmd:profile";
        public const string URL_PREFIX = "url:";
        public const string ADDRESSEE_SWITCH = "switch:";
        public const string CMD_BACK = "cmd:back";
        public const string CMD_PHONE = "cmd:phone";
        public const string CMD_HELLO = "cmd:hello";
        public const string ANSWER_OK = "answer:ok";
        public const string ANSWER_NO = "answer:no";

        private readonly String _title = DAL.Model.Settings.Values.GetValue("OWNER_TITLE", "Наставник");

        //public BotCommandsRouter(string text = "Наставник")
        //{
        //    _title = text;
        //}

        public virtual Guid? RouteMessage(ChatEntities entities, IOuterMessage message, Message dbmsg) {
            CommandsEnum? cmd = IsItCommand(entities, message, dbmsg, out string arg);
            if (cmd.HasValue) {
                if (PerformCommand(entities, message, dbmsg, cmd.Value, arg))
                    return Global.SystemBotId;
            } 
            return null;
        }

        /// <summary>
        /// Returns type of command or null
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public static CommandsEnum? GetCommandType(string cmd, out string arg) {
            arg = null;
            if (String.IsNullOrEmpty(cmd))
                return null;

            switch (cmd)
            {
                case CMD_SELECT_ADDRESSEE:
                    return CommandsEnum.SelectAddressee;

                case CMD_SCORE:
                    return CommandsEnum.MyScore;

                case CMD_PROFILE:
                    return CommandsEnum.MyProfile;

                case CMD_BACK:
                    return CommandsEnum.Back;

                case CMD_PHONE:
                    return CommandsEnum.Phone;

                case CMD_HELLO:
                    return CommandsEnum.Hello;

                case ANSWER_OK:
                    return CommandsEnum.Ok;

                case ANSWER_NO:
                    return CommandsEnum.No;
            }

            if (cmd.StartsWith(URL_PREFIX)) {
                arg = cmd.Substring(URL_PREFIX.Length);
                return CommandsEnum.Url;
            }

            if (cmd.StartsWith(ADDRESSEE_SWITCH)) {
                arg = cmd.Substring(ADDRESSEE_SWITCH.Length);
                return CommandsEnum.SwitchAddressee;
            }

            return null;
        }

        protected abstract CommandsEnum? IsItCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, out string arg);

        protected bool PerformCommand(ChatEntities entities, IOuterMessage message, Message dbmsg, CommandsEnum cmd, string arg) {            
            switch (cmd) {
                case CommandsEnum.SelectAddressee://"cmd:select_addressee":
                    PerformSelectAddressee(entities, message, dbmsg);                    
                    return true;

                case CommandsEnum.MyScore: //"cmd:score":
                    MakeMessage(entities, dbmsg, MakeScoreText(dbmsg.FromTransport.User));
                    return true;

                case CommandsEnum.MyProfile://"cmd:profile":
                    MakeMessage(entities, dbmsg, MakeProfileText(dbmsg.FromTransport.User));
                    return true;

                case CommandsEnum.Url:
                    MakeMessage(entities, dbmsg, arg);
                    return true;

                case CommandsEnum.SwitchAddressee:
                    SwitchAddressee(entities, dbmsg, arg);
                    return true;

                case CommandsEnum.Back:
                    MakeMessage(entities, dbmsg, "Назад в меню");
                    return true;
            }

            return false;
            //if (cmd.StartsWith(URL_PREFIX)) {
            //    MakeMessage(entities, dbmsg, cmd.Substring(URL_PREFIX.Length));
            //    return;
            //}
        }

        protected abstract void PerformSelectAddressee(ChatEntities entities, IOuterMessage message, Message dbmsg);

        protected string MakeScoreText(User user) {
            bool hasGrades = user.LoBonusScores.HasValue || user.GoBonusScores.HasValue || user.OlgBonusScores.HasValue;
            if (hasGrades)
                return String.Concat(
                    "ЛО: ", GetValueOrDash(user.LoBonusScores), Environment.NewLine,                    
                    "ОЛГ: ", GetValueOrDash(user.OlgBonusScores), Environment.NewLine,
                    "ГО: ", GetValueOrDash(user.GoBonusScores)
                    );
            else
                return "У вас нет баллов";
        }

        protected string MakeProfileText(User user) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Concat("Имя: ", user.FullName ?? String.Empty));
            if (user.Phone != null)
            {
                string str = user.Phone ?? "";
                if (!string.IsNullOrWhiteSpace(str))
                    str = str.StartsWith("+") ? str : "+" + str;
                sb.AppendLine(String.Concat("Телефон: ", str));
            }
            if (user.Email != null)
                sb.AppendLine(String.Concat("Почта: ", user.Email));
            if (user.OwnerUserId.HasValue)
                sb.AppendLine(String.Concat(_title+": ", user.OwnerUser.FullName ?? String.Empty));
            if (user.RankId.HasValue)
                sb.AppendLine(String.Concat("Статус: ", user.Rank?.Name));
            if (user.FLUserNumber.HasValue)
                sb.AppendLine(String.Concat("Номер: ", user.FLUserNumber.ToString()));
            return sb.ToString();
        }

        private string GetValueOrDash(decimal? value) => value.HasValue 
            ? value.Value.ToString(CultureInfo.GetCultureInfo("ru")) 
            : "-";

        private Message MakeMessage(ChatEntities entities, Message dbmsg, string text) {
            Message reply = new Message() {
                Kind = MessageKind.Personal,
                FromTransport = entities.SystemBotTransport,
                AnswerTo = dbmsg,
                Text = text,
                ToUsers = new MessageToUser[] {
                        new MessageToUser() {
                            ToTransport = dbmsg.FromTransport
                        }
                    }
            };
            entities.Message.Add(reply);
            return reply;
        }

        private void SwitchAddressee(ChatEntities entities, Message dbmsg, string arg) {
            Guid? guid = String.IsNullOrEmpty(arg) ? (Guid?)null : Guid.Parse(arg);
            CallbackSelectAddressee.ChangeAddressee(entities, dbmsg.FromTransport, guid);
        }        
    }
}
