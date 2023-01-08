using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Project
    {
        public Project()
        {
            OutProjectInfoPatterns = new HashSet<OutProjectInfoPattern>();
            RoleMembers = new HashSet<RoleMember>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Uri { get; set; } = null!;
        public string OutId { get; set; } = null!;
        public Guid PmsystemId { get; set; }
        public Guid ProjectCollectionId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Pmsystem Pmsystem { get; set; } = null!;
        public virtual ProjectCollection ProjectCollection { get; set; } = null!;
        public virtual ICollection<OutProjectInfoPattern> OutProjectInfoPatterns { get; set; }
        public virtual ICollection<RoleMember> RoleMembers { get; set; }
    }
}
