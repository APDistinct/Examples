using System;

namespace Chrono.Services.Models;

public class WorkItemInfoIncome
{
    public Guid PmsystemId { get; set; }
    public string Project { get; set; } = "";
    public string OutId { get; set; } = "";
    public string Name { get; set; } = "";
    public string? WorkItemType { get; set; }
    public double? Estimate { get; set; }
}