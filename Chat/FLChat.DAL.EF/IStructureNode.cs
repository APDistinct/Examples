using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FLChat.DAL.Model;

namespace FLChat.DAL
{
    /// <summary>
    /// Represent main information about user's structure interface node
    /// </summary>
    public interface IStructureNodeInfo
    {
        /// <summary>
        /// Node id
        /// </summary>
        String NodeId { get; }

        /// <summary>
        /// Node caption in user interface
        /// </summary>
        String Caption { get; }
    }
}
