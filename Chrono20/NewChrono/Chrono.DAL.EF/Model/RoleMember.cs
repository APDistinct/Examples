using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class RoleMember
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid RoleId { get; set; }

        public virtual Project Project { get; set; } = null!;
        public virtual ProjectRole Role { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
