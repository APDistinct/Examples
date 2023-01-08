using FLChat.Core.Buttons;
using FLChat.Core.Routers;
using FLChat.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    public class ScenarioButtons : IScenarioButtons
    {
        public const string ButtonOKCaption = "Да";
        public IEnumerable<IEnumerable<ITransportButton>> GetButtons(string scenarioName, int stepNumber)
        {
            switch (scenarioName)
            {
                case ScenarioType.Invite:
                case ScenarioType.Common:
                    return MakeKb(stepNumber);
            }
            return null;
        }
        
        private IEnumerable<IEnumerable<ITransportButton>> MakeKb(int stepNumber)
        {
            List<List<ITransportButton>> list = new List<List<ITransportButton>>();
            switch (stepNumber)
            {
                case 0:
                    {
                        ExternalTransportButton nn = new ExternalTransportButton()
                        {
                            Caption = "Продолжить",
                            Command = BotCommandsRouter.CMD_HELLO,
                            Row = 1,
                            Col = 1,
                            HideForTemporary = false,
                        };
                        var cur = new List<ITransportButton>();
                        cur.Add(new TransportButton(nn));
                        list.Add(cur);
                    }
                    break;
                case 1:
                    {
                        ExternalTransportButton nn = new ExternalTransportButton()
                        {
                            Caption = "Поделиться телефоном",
                            Command = BotCommandsRouter.CMD_PHONE,
                            Row = 1,
                            Col = 1,
                            HideForTemporary = false,
                        };
                        var cur = new List<ITransportButton>();
                        cur.Add(new TransportButton(nn));
                        list.Add(cur);
                        //list.Add( new List<ITransportButton>)
                    }
                    break;
                case 2:
                    //  Ничего
                    break;
                case 3:
                    GetOK_NObottons(list);
                    break;
                case 4:
                    //  Ничего
                    break;
                case 5:
                    break;
            }
            return list;
        }

        private void GetOK_NObottons(List<List<ITransportButton>> list)
        {
            ExternalTransportButton bOK = new ExternalTransportButton()
            {
                Caption = ButtonOKCaption,
                Command = BotCommandsRouter.ANSWER_OK,
                Row = 1,
                Col = 0,
                HideForTemporary = false,
            };
            ExternalTransportButton bNO = new ExternalTransportButton()
            {
                Caption = "Нет",
                Command = BotCommandsRouter.ANSWER_NO,
                Row = 1,
                Col = 1,
                HideForTemporary = false,
            };
            var cur = new List<ITransportButton>
            {
                new TransportButton(bOK),
                new TransportButton(bNO)
            };
            list.Add(cur);
        }
    }
}
