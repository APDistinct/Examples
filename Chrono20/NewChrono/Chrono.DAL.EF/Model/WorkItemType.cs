using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class WorkItemType
    {
        public WorkItemType()
        {
            WorkItems = new HashSet<WorkItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<WorkItem> WorkItems { get; set; }
    }
}
