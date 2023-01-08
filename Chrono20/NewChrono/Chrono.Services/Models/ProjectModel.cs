using System.Collections.Generic;

namespace Chrono.Services.Models;

public class ProjectModel : ProjectInfoSimple
{
    //public Guid Id { get; set; }
    //public string? Name { get; set; } 
    public IEnumerable<WorkItemModel>? WorkItems { get; set; }
}