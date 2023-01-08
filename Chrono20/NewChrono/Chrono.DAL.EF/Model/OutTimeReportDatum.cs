using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class OutTimeReportDatum
    {
        public Guid Id { get; set; }
        public Guid OutTimeReportInfoId { get; set; }
        public Guid? TimeReportId { get; set; }
        public DateTime DateValue { get; set; }
        public double TimeValue { get; set; }
        public string UserValue { get; set; } = null!;
        public string WorkItemValue { get; set; } = null!;
        public int? Mark { get; set; }

        public virtual OutTimeReportInfo OutTimeReportInfo { get; set; } = null!;
        public virtual TimeReport? TimeReport { get; set; }
    }
}
