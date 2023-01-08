using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Bill
    {
        public Bill()
        {
            TimeReportExternalBills = new HashSet<TimeReport>();
            TimeReportInternalBills = new HashSet<TimeReport>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateBill { get; set; }
        public bool IsExternal { get; set; }

        public virtual ICollection<TimeReport> TimeReportExternalBills { get; set; }
        public virtual ICollection<TimeReport> TimeReportInternalBills { get; set; }
    }
}
