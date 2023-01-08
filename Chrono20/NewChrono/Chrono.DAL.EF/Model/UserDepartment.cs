using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class UserDepartment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }

        public virtual Department Department { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
