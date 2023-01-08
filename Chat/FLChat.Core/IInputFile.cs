using FLChat.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.Core
{
    /// <summary>
    /// Information about incoming file
    /// </summary>
    public interface IInputFile
    {
        /// <summary>
        /// File media type
        /// </summary>
        MediaGroupKind Type { get; }

        /// <summary>
        /// Url to file or some information about how to get file
        /// </summary>
        string Media { get; }

        /// <summary>
        /// File name, may be null
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// File media type, may be null
        /// </summary>
        string MediaType { get; }
    }
}
