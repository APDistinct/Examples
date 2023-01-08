using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class BusinessLine
    {
        public BusinessLine()
        {
            UserBusinessLines = new HashSet<UserBusinessLine>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<UserBusinessLine> UserBusinessLines { get; set; }
    }
}
