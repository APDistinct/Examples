using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;

namespace FLChat.DAL.DataTypes
{
    public class StructureNodeFullInfo
    {
        public StructureNodeFullInfo(StructureNodeVirtual node, List<StructureNodeVirtual> childNodes, List<User> users, int? totalCount) {
            Node = node;
            ChildNodes = childNodes;
            Users = users;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Information about current node
        /// </summary>
        public StructureNodeVirtual Node { get; }

        /// <summary>
        /// List of child nodes
        /// </summary>
        public List<StructureNodeVirtual> ChildNodes { get; }

        /// <summary>
        /// List of users
        /// </summary>
        public List<User> Users { get; }

        /// <summary>
        /// total count of users
        /// </summary>
        public int? TotalCount { get; }
    }
}
