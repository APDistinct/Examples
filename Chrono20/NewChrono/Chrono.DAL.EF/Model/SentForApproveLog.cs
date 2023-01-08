using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class SentForApproveLog
    {
        public Guid UserId { get; set; }
        public DateTime SentDate { get; set; }
    }
}
