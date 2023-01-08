using Chrono.DAL.EF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Models
{
    public class TimeReportDBModel
    {
        public User User { get; set; }
        public DateTime DateFrom { get; set; }
        public int DayCount { get; set; }
        public /*IEnumerable*/List<ProjectDBModel> Projects { get; set; } = new List<ProjectDBModel>();
    }
}
