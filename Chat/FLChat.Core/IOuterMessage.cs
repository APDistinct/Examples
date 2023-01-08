using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL;

namespace FLChat.Core
{
    /// <summary>
    /// Common data for outer transport's message
    /// </summary>
    public interface IOuterMessage : IOuterMessageId, IDeepLinkData
    {

        //string FromId { get; }
        string FromName { get; }
        
        string Text { get; }

        string PhoneNumber { get; }

        string AvatarUrl { get; }

        /// <summary>
        /// If message has deep link, this property contains deep link code. Otherwise is null
        /// </summary>
        //string DeepLink { get; }

        /// <summary>
        /// Message's id which current message is replied for or null
        /// </summary>
        string ReplyToMessageId { get; }    
        
        /// <summary>
        /// information about file. Null if has not file 
        /// </summary>
        IInputFile File { get; }
    }
}
