using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class LocationHistory
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }

        public virtual Location Location { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
