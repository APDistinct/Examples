using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public class WorkItemInfoBase
    {
        public Guid WorkItemId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
