using System;

namespace Chrono.Services.Models;

public class WorkItemInfoIncomeInner
{
    public Guid ProjectId { get; set; }
    public string OutId { get; set; } = "";
    public string Name { get; set; } = null!;
    public int? WorkItemTypeId { get; set; }
    public double? Estimate { get; set; }
}