using System;
using System.Collections.Generic;

namespace Chrono.Services.Models;

public class TimeReportModel
{
    public UserBaseInfo? User { get; set; }
    public DateTime DateFrom { get; set; }
    public int DayCount { get; set; }
    public IEnumerable<ProjectModel>? Projects { get; set; }
}