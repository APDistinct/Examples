using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class OutProjectInfoPattern
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string DateName { get; set; } = null!;
        public string TimeName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string WorkItemName { get; set; } = null!;

        public virtual Project Project { get; set; } = null!;
    }
}
