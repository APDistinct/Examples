using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Repositories
{
    public class TimeReportRepositoryFake : ITimeReportRepository
    {
        public Task<List<TimeReportDBModel>> GetTimeDBReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> wiIdList, DateTime dateFrom, int dayCount)
        {
            throw new NotImplementedException();
        }

        public Task<List<TimeReport>> GetTimeReports(IEnumerable<Guid> userIdList, IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount)
        {
            throw new NotImplementedException();
        }

        public async Task<WorkItemModelNew?> GetUserTimeReport(Guid uId, Guid wId, DateTime dateFrom, int dayCount)
        {
            var result = new WorkItemModelNew()
            {
                WorkItemId = wId,
                Name = "",
                WorkItemType = "",
                Estimate = 8,
                ItemTimes = new List<DayInfoNew>()
            {
                new DayInfoNew() {ReportDate = dateFrom, BillHours = 0, Hours = 8, IsEditable = true, IsOvertime = false, ReportStatus = ""},
            },
            };
            return await Task.FromResult<WorkItemModelNew?>(result);
        }

        public Task<List<Guid>> GetUserWIList(Guid uId, DateTime dateFrom, int dayCount)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkItemInfoBase>> GetWIList(Guid uId, DateTime dateFrom, int dayCount)
        {
            var list = new List<WorkItemInfoBase>()
        {
            new WorkItemInfoBase()
            {
                ProjectId = Guid.NewGuid(),
                WorkItemId = Guid.NewGuid(),
            }
        };
            return await Task.FromResult<List<WorkItemInfoBase>>(list);
        }

        public Task<int> SetTimeReports(ITimeReportSetRequest setRequest, bool Transaction = true)
        {
            throw new NotImplementedException();
        }
    }
}
