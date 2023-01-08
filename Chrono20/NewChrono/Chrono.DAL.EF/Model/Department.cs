using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Department
    {
        public Department()
        {
            UserDepartments = new HashSet<UserDepartment>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<UserDepartment> UserDepartments { get; set; }
    }
}
