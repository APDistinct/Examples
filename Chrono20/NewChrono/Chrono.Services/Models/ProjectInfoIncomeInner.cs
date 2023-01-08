using System;

namespace Chrono.Services.Models;

public class ProjectInfoIncomeInner
{
    public Guid PmsystemId { get; set; }
    public string OutId { get; set; } = "";
    public string Name { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public Guid ProjectCollectionId { get; set; }
    public bool IsDeleted { get; set; }
}