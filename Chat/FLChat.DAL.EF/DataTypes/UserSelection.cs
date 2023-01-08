using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.DataTypes
{
    /// <summary>
    /// Represent selection of users for calling database functions and procedures
    /// </summary>
    public class UserSelection
    {
        /// <summary>
        /// Selected user's with his structure
        /// </summary>
        public IEnumerable<Tuple<Guid, int?>> IncludeWithStructure { get; set; }

        /// <summary>
        /// Users, excluded from selection with his structure
        /// </summary>
        public IEnumerable<Guid> ExcludeWithStructure { get; set; }

        /// <summary>
        /// Include users
        /// </summary>
        public IEnumerable<Guid> Include { get; set; }

        /// <summary>
        /// Exluded users
        /// </summary>
        public IEnumerable<Guid> Exclude { get; set; }

        /// <summary>
        /// Selected segments
        /// </summary>
        public IEnumerable<Guid> Segments { get; set; }
    }
}
