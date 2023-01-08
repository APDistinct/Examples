using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class WorkItemStateHistory
    {
        public Guid Id { get; set; }
        public Guid WorkItemStateId { get; set; }
        public Guid ChangeUserId { get; set; }
        public DateTime ChangeDate { get; set; }
        public int WorkItemStatusId { get; set; }

        public virtual User ChangeUser { get; set; } = null!;
        public virtual WorkItemState WorkItemState { get; set; } = null!;
        public virtual WorkItemStatus WorkItemStatus { get; set; } = null!;
    }
}
