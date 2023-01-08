using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL
{
    /// <summary>
    /// Class for set and get Message specific information
    /// </summary>
    public interface IMessageSpecific
    {
        /// <summary>
        /// Is need to show 'get phone number' button
        /// </summary>
        bool IsPhoneButton { get; set; }
    }
}
