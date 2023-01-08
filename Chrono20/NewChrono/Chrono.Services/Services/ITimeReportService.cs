using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Chrono.Services.Models;

namespace Chrono.Services.Services;

public interface ITimeReportService
{
    Task<TimeReportModel> GetTimeReport(Guid userId, DateTime dateFrom, int dayCount);

    //Task<IEnumerable<ProjectModel>> GetProjects(Guid userIdList, DateTime dateFrom, int dayCount);
    Task<int> SetTimeReports(ITimeReportSetRequest setRequest);
    Task<IEnumerable<TimeReportModel>> GetTimeReports(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount);
    Task<IEnumerable<TimeReportModel>> GetTimeReportsByProjects(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount);
    Task<TimeReportModelNew> GetTimeReportN(Guid userId, DateTime dateFrom, int dayCount);
    Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByProjectsForView(IEnumerable<Guid> projectIdList, DateTime dateFrom, int dayCount, bool addDeleted = false);
    Task<IEnumerable<TimeReportModelNew>> GetTimeReportsByUsertsForView(IEnumerable<Guid> userIdList, DateTime dateFrom, int dayCount, bool addDeleted = false);
}