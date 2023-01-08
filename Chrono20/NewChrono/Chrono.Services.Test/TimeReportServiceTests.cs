using Chrono.Services.Models;
using Chrono.Services.Repositories;
using Chrono.Services.Services;
using Moq;

namespace Chrono.Services.Test;

public class TimeReportServiceTests
{
    [Fact]
    public async Task GetTimeReportNTest()
    {
        // arrange
        var datarepository = new DataGetRepositoryFake(); // Mock<IDataGetRepository>();
        var reportrepository = new TimeReportRepositoryFake();  // Mock<ITimeReportRepository>();
        var service = new TimeReportService(reportrepository, datarepository);
        //var service = new TimeReportService(reportrepository.Object, datarepository.Object);
        var userId = Guid.NewGuid();
        var dateFrom = DateTime.UtcNow;
        var dayCount = 3;
        var wId = Guid.NewGuid();
        var prId = Guid.NewGuid();
        var list = new List<WorkItemInfoBase>();
        var userInfo = new Models.UserBaseInfo() { UserId = userId };
        WorkItemModelNew? workItem = new WorkItemModelNew();
        //datarepository.Setup(z => z.ReadUserById(userId)).ReturnsAsync(userInfo);
        //datarepository.Setup(z => z.ReadProjectById(prId)).ReturnsAsync(await ReadProjectById(prId));
        //reportrepository.Setup(z => z.GetUserTimeReport(userId, wId, dateFrom, dayCount)).ReturnsAsync(GetUserTimeReport(userId, wId, dateFrom, dayCount));
        //reportrepository.Setup(z => z.GetWIList(userId, dateFrom, dayCount)).ReturnsAsync(GetWIList(userId, dateFrom, dayCount));

        // act
        var result = await service.GetTimeReportN(userId, dateFrom, dayCount);

        // assert
        Assert.NotNull(result);
        //Assert.Empty(result);
        //repository.Verify(z => z.ReadProjectInfoAsync(guid, date, num), Times.Once);
        //repository.VerifyNoOtherCalls();
    }
//    WorkItemModelNew? GetUserTimeReport(Guid uId, Guid wId, DateTime dateFrom, int dayCount)
//    {
//        var result = new WorkItemModelNew()
//        {
//            WorkItemId = wId,
//            Name = "",
//            WorkItemType = "",
//            Estimate = 8,
//            ItemTimes = new List<DayInfoNew>()
//            {
//                new DayInfoNew() {ReportDate = dateFrom, BillHours = 0, Hours = 8, IsEditable = true, IsOvertime = false, ReportStatus = ""},
//            },
//        };
//        return result;
//    }
//    private async Task< ProjectInfoSimple?> ReadProjectById(Guid projectId)
//    {
//        var pis = new ProjectInfoSimple()
//        {
//            Id = projectId,
//            Name = "",
//            IsDeleted = false,
//            OutId = "",
//            Pmsystem = "",
//        };
//        return await Task.FromResult<ProjectInfoSimple?>(pis) ;
//    }
//    private List<WorkItemInfoBase> GetWIList(Guid uId, DateTime dateFrom, int dayCount)
//    {
//        var list = new List<WorkItemInfoBase>()
//        {
//            new WorkItemInfoBase()
//            {
//                ProjectId = Guid.NewGuid(),
//                WorkItemId = Guid.NewGuid(),
//            }
//        };
//        return list;
//    }
//    private List<WorkItemInfoBase> GetWIList(List<WorkItemInfoBase> list)
//    {        
//        return list;
//    }
}