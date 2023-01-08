using System;

namespace Chrono.Services.Models;

public interface ITimeReportGetRequest
{
    Guid UserId { get; set; }
    DateTime DateFrom { get; set; }
    int DayCount { get; set; }
}

public class TimeReportGetRequest : ITimeReportGetRequest
{
    public Guid UserId { get; set; }
    public DateTime DateFrom { get; set; }
    public int DayCount { get; set; }
}