using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Country
    {
        public Country()
        {
            Users = new HashSet<User>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
