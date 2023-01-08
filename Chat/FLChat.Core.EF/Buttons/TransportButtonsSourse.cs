using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Buttons
{
    public class TransportButtonsSourse : ITransportButtonsSource
    {
        public IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu) {
            using (ChatEntities entities = new ChatEntities()) {
                ExternalTransportButton[] buttons = entities.ExternalTransportButton.OrderBy(b => b.Row).ThenBy(b => b.Col).ToArray();
                return GetButtons(mtu, buttons);
            }
        }

        public static IEnumerable<IEnumerable<ITransportButton>> GetButtons(            
            MessageToUser mtu,
            ExternalTransportButton[] buttons) {

            List<List<ITransportButton>> list = new List<List<ITransportButton>>();

            int? row = null;
            List<ITransportButton> cur = null;
            foreach (ExternalTransportButton button in buttons) {
                TransportButton btn = new TransportButton(button);
                if (btn.Filter(mtu)) {
                    if (row != button.Row) {
                        cur = new List<ITransportButton>();
                        list.Add(cur);
                        row = button.Row;
                    }
                    cur.Add(btn);
                }
            }

            return list;
        }
    }
}
