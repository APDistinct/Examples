using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class UserInfo
    {
        public Guid UserId { get; set; }
        public string? SynchronizeName { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
