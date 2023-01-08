using Chrono.DAL.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public class ProjectDBModel
    {
        public Project Project { get; set; }
        public IEnumerable<TimeReport>? WorkItems { get; set; }
    }
}
