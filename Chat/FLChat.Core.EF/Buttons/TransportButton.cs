using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using FLChat.Core.Routers;

namespace FLChat.Core.Buttons
{
    public class TransportButton : ITransportButton
    {
        private ExternalTransportButton _btn;

        public TransportButton(ExternalTransportButton btn) {
            _btn = btn;
        }

        public string Caption => _btn.Caption;

        public string Command => _btn.Command;

        /// <summary>
        /// returns true if button will be shown, and false if button will be hide
        /// </summary>
        /// <param name="mtu"></param>
        /// <returns></returns>
        public bool Filter(MessageToUser mtu) {
            if (_btn.HideForTemporary && (mtu == null ||  mtu.ToTransport.User.IsTemporary))
                return false;

            BotCommandsRouter.CommandsEnum? cmd = BotCommandsRouter.GetCommandType(Command, out string arg);
            if (cmd == BotCommandsRouter.CommandsEnum.SelectAddressee) {
                if (mtu == null)
                    return false;
                //will show 'select addressee' button:
                //  1.if message sender isn't bot, because that way user has addressee for select
                if (!mtu.Message.FromTransport.User.IsBot)
                    return true;
                // 2. Or user has any addressee for select
                using (ChatEntities entities = new ChatEntities())
                using (var trans = entities.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted)) {
                    return entities.GetAddresseesForExternalTrans(mtu.ToUserId).Any();
                }
            }
            return true;
        }
    }
}
