using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class OutUser
    {
        public Guid UserId { get; set; }
        public string OutName { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
