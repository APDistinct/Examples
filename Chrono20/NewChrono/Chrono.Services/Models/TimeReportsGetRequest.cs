using System;
using System.Collections.Generic;

namespace Chrono.Services.Models;

public interface ITimeReportsGetRequest
{
    IEnumerable<Guid> IdList { get; set; }
    DateTime DateFrom { get; set; }
    int DayCount { get; set; }
    bool addDeleted { get; set; }
}

public class TimeReportsGetRequest : ITimeReportsGetRequest
{
    public IEnumerable<Guid> IdList { get; set; } = new List<Guid>();
    public DateTime DateFrom { get; set; }
    public int DayCount { get; set; }
    public bool addDeleted { get; set; } = false;
}