using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Buttons
{
    public class TransportButtonsSourceBuffered : ITransportButtonsSource
    {
        private Lazy<ExternalTransportButton[]> _buttons;

        public TransportButtonsSourceBuffered() {
            _buttons = new Lazy<ExternalTransportButton[]>(() => {
                using (ChatEntities entities = new ChatEntities()) {
                    return entities.ExternalTransportButton.OrderBy(b => b.Row).ThenBy(b => b.Col).ToArray();
                }
            }, true);
        }

        public IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu) {
            return TransportButtonsSourse.GetButtons(mtu, _buttons.Value);
        }
    }
}
