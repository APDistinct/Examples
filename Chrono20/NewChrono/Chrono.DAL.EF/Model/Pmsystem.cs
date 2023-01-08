using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Pmsystem
    {
        public Pmsystem()
        {
            Projects = new HashSet<Project>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
