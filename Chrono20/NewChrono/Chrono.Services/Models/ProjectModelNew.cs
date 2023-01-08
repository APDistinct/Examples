using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public interface IProjectModelNew : IProjectInfoSimple
    {
        ICollection<WorkItemModelNew> WorkItems { get; set; } 
    }
    public class ProjectModelNew : ProjectInfoSimple, IProjectModelNew, IProjectInfoSimple
    {
        public ProjectModelNew()
        { }
        public ProjectModelNew(IProjectInfoSimple infoSimple)
        { 
            Id = infoSimple.Id;
            Name = infoSimple.Name;
            OutId = infoSimple.OutId;
            Pmsystem = infoSimple.Pmsystem;
            IsDeleted = infoSimple.IsDeleted;
        }
        public /*IEnumerable*/ICollection<WorkItemModelNew> WorkItems { get; set; } = new List<WorkItemModelNew>();
    }
}
