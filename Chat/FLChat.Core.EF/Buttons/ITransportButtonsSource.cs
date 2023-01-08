using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.Core.Buttons
{
    public interface ITransportButton
    {
        string Caption { get; }
        string Command { get; }

        /// <summary>
        /// returns true if button will be shown, and false if button will be hide
        /// </summary>
        /// <param name="mtu"></param>
        /// <returns></returns>
        bool Filter(MessageToUser mtu);
    }

    public interface ITransportButtonsSource
    {
        IEnumerable<IEnumerable<ITransportButton>> GetButtons(MessageToUser mtu);
    }
}
