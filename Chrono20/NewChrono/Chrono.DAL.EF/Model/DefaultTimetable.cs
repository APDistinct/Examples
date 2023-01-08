﻿using System;
using System.Collections.Generic;

namespace Chrono.DAL.EF.Model
{
    public partial class DefaultTimetable
    {
        public int Id { get; set; }
        public double Monday { get; set; }
        public double Tuesday { get; set; }
        public double Wednesday { get; set; }
        public double Thursday { get; set; }
        public double Friday { get; set; }
        public double Saturday { get; set; }
        public double Sunday { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
