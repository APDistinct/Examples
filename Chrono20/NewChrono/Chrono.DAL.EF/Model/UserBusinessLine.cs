using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class UserBusinessLine
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BusinessLineId { get; set; }

        public virtual BusinessLine BusinessLine { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
