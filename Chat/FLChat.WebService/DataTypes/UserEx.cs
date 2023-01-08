using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FLChat.DAL.Model;
using System.Data.Entity.SqlServer;

namespace FLChat.WebService.DataTypes
{
    public class UserEx
    {
        public User User { get; set; }
        public bool? HasChilds { get; set; }
        public bool BroadcastProhibitionStructure { get; set; }
    }

    public static class UserExExtentions
    {
        /// <summary>
        /// Order list of user: first cyrrilic symbols, then latin
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        static public IOrderedQueryable<UserEx> OrderByName(this IQueryable<UserEx> q) =>
            q.OrderByDescending(u => u.User.FullName != null ? 1 : 0)
            .ThenByDescending(u => SqlFunctions.Unicode(u.User.FullName) >= 1024 && SqlFunctions.Unicode(u.User.FullName) <= 1279 ? 1 : 0)
            .ThenBy(u => u.User.FullName);
    }
}
