using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class ProjectRole
    {
        public ProjectRole()
        {
            InverseParent = new HashSet<ProjectRole>();
            RoleMembers = new HashSet<RoleMember>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
        public int Type { get; set; }

        public virtual ProjectRole? Parent { get; set; }
        public virtual ICollection<ProjectRole> InverseParent { get; set; }
        public virtual ICollection<RoleMember> RoleMembers { get; set; }
    }
}
