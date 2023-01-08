using System;

namespace Chrono.Services.Models;

public class ProjectInfoIncome
{
    public Guid PmsystemId { get; set; }
    public string OutId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Uri { get; set; } = "";
    public string? ProjectCollectionName { get; set; }
    public bool IsDeleted { get; set; }
}