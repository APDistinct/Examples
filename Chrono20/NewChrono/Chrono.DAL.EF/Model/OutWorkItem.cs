using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class OutWorkItem
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public string OutName { get; set; } = null!;

        public virtual WorkItem WorkItem { get; set; } = null!;
    }
}
