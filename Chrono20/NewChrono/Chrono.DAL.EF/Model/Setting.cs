using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; } = null!;
        public string? Value { get; set; }
        public bool HasUserInteface { get; set; }
        public int Type { get; set; }
        public string Description { get; set; } = null!;
    }
}
