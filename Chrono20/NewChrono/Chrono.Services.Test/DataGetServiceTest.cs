using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using Chrono.Services.Repositories;
using Chrono.Services.Services;

using Moq;

namespace Chrono.Services.Test;

public class DataGetServiceTest
{
    private static readonly string alias = Guid.NewGuid().ToString();
    private readonly ChronoContext context;
    private readonly IDataGetService service;

    public DataGetServiceTest()
    {
        context = InMemoryData.GetTestContext(alias);
        service = new DataGetService(new DataGetRepository(context, null));
    }

    [Fact]
    public async Task WriteIncomeUserTest()
    {
        UserInfoIncome userinfo1 = new()
        {
            Sid = Guid.NewGuid().ToString(),
            LocationFrom = DateTime.UtcNow,
            Location = (await InMemoryData.GetDefaultLocationAsync(context)).Id.ToString(),
            Deleted = false,
            Position = (await InMemoryData.GetDefaultPositionAsync(context)).Id.ToString(),
            DisplayName = "Test " + Guid.NewGuid(),
            Email = "Test " + Guid.NewGuid(),
            UserName = "Test " + Guid.NewGuid(),
        };

        var user1 = await service.WriteIncomeUser(userinfo1);
        Assert.NotNull(user1);
        Assert.Equal(userinfo1.Sid, user1.Sid);
        Assert.Equal(userinfo1.DisplayName, user1.DisplayName);
        Assert.Equal(userinfo1.Email, user1.Email);
        Assert.Equal(userinfo1.UserName, user1.UserName);
        var userId = user1.UserId;

        UserInfoIncome userinfo2 = new()
        {
            Sid = userinfo1.Sid,
            LocationFrom = DateTime.UtcNow,
            Location = (await InMemoryData.GetDefaultLocationAsync(context)).Id.ToString(),
            Deleted = true,
            Position = (await InMemoryData.GetDefaultPositionAsync(context)).Id.ToString(),
            DisplayName = "Test2 " + Guid.NewGuid(),
            Email = "Test2 " + Guid.NewGuid(),
            UserName = "Test2 " + Guid.NewGuid(),
        };
        var user2 = await service.WriteIncomeUser(userinfo2);
        Assert.NotNull(user2);
        Assert.Equal(userId, user2.UserId);
        Assert.Equal(userinfo2.Sid, user2.Sid);
        Assert.Equal(userinfo2.DisplayName, user2.DisplayName);
        Assert.Equal(userinfo2.Email, user2.Email);
        Assert.Equal(userinfo2.UserName, user2.UserName);
        Assert.NotEqual(userinfo1.UserName, user2.UserName);
        Assert.NotEqual(userinfo1.Email, user2.Email);
        Assert.NotEqual(userinfo1.DisplayName, user2.DisplayName);
    }

    [Fact]
    public async Task WriteIncomeProjectTest()
    {
        //var project = await InMemoryData.GetDefaultProjectAsync(context);
        var pmsystem = (await InMemoryData.GetDefaultPmsystemAsync(context)).Id;
        ProjectInfoIncome info1 = new()
        {
            IsDeleted = false,
            OutId = "Test1 " + Guid.NewGuid(),
            Name = "Test1 " + Guid.NewGuid(),
            PmsystemId = pmsystem,
            Uri = "Test " + Guid.NewGuid(),
        };
        var ret1 = await service.WriteIncomeProject(info1);
        Assert.NotNull(ret1);
        var id = ret1.Id;
        Assert.Equal(info1.Name, ret1.Name);
        ProjectInfoIncome info2 = new()
        {
            OutId = info1.OutId,
            PmsystemId = pmsystem,
            IsDeleted = false,
            Name = "Test2 " + Guid.NewGuid(),
            Uri = "Test2 " + Guid.NewGuid(),
        };
        var ret2 = await service.WriteIncomeProject(info2);
        Assert.NotNull(ret2);
        Assert.Equal(id, ret2.Id);
        Assert.Equal(info2.Name, ret2.Name);
        Assert.Equal(info2.Name, ret2.Name);
        Assert.NotEqual(info1.Name, ret2.Name);
    }

    [Fact]
    public async Task WriteIncomeWorkItemTest()
    {
        //string pname = (await InMemoryData.GetDefaultProjectAsync(context)).Name;
        //var pmsystem = (await InMemoryData.GetDefaultPmsystemAsync(context)).Id; 
        var project = await InMemoryData.GetDefaultProjectAsync(context);
        WorkItemInfoIncome info1 = new()
        {
            Estimate = 0,
            OutId = "1",
            Project = project.Name,
            PmsystemId = project.PmsystemId,
            //WorkItemType
            Name = "Test1 " + Guid.NewGuid(),
        };
        var ret1 = await service.WriteIncomeWorkItem(info1);
        var id = ret1.WorkItemId;
        Assert.NotNull(ret1);
        Assert.Equal(info1.Name, ret1.Name);
        Assert.Equal(info1.Estimate, ret1.Estimate);
        WorkItemInfoIncome info2 = new()
        {
            Estimate = 10,
            OutId = "1",
            Project = project.Name,
            PmsystemId = project.PmsystemId,
            Name = "Test2 " + Guid.NewGuid(),
        };
        var ret2 = await service.WriteIncomeWorkItem(info2);
        Assert.NotNull(ret2);
        Assert.Equal(id, ret2.WorkItemId);
        Assert.Equal(info2.Estimate, ret2.Estimate);
        Assert.Equal(info2.Name, ret2.Name);
        Assert.Equal(info2.WorkItemType, ret2.WorkItemType);
        Assert.NotEqual(info1.Name, ret2.Name);
    }

    [Fact]
    public async Task GetUserByIdTest()
    {
        var user = await InMemoryData.WriteUserAsync(context);
        var userId = user.UserId;
        var ret = await service.GetUserById(userId);
        Assert.NotNull(ret);
        Assert.Equal(userId, ret.UserId);
        Assert.Equal(user.Sid, ret.Sid);
        Assert.Equal(user.Email, ret.Email);
        Assert.Equal(user.DisplayName, ret.DisplayName);
        Assert.Equal(user.Deleted, ret.Deleted);
        Assert.Equal(user.Position.Name, ret.Position);
        Assert.Equal(user.UserName, ret.UserName);
    }

    [Fact]
    public async Task GetProjectsTest()
    {
        // arrange
        var repository = new Mock<IDataGetRepository>();
        var service = new DataGetService(repository.Object);
        var guid = Guid.NewGuid();
        var date = DateTime.UtcNow;
        var num = 3;
        repository.Setup(z => z.ReadProjectInfoAsync(guid, date, num)).ReturnsAsync(new List<ProjectModel>());
        // act
        var result = await service.GetProjects(guid, date, num);
        // assert
        Assert.NotNull(result);
        Assert.Empty(result);
        repository.Verify(z => z.ReadProjectInfoAsync(guid, date, num), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}