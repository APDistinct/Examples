using System;

namespace Chrono.Services.Models;

public class WorkItemInfoSimple
{
    public Guid WorkItemId { get; set; }
    public string? Name { get; set; }
    public string? WorkItemType { get; set; }
    public double? Estimate { get; set; } = 0;
}