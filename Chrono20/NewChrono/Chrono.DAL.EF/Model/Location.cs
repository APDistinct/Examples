using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Location
    {
        public Location()
        {
            LocationHistories = new HashSet<LocationHistory>();
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Type { get; set; }

        public virtual ICollection<LocationHistory> LocationHistories { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
