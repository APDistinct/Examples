using Chrono.DAL.EF.Model;
using Chrono.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chrono.Services.Repositories
{
    public class DataGetRepositoryFake : IDataGetRepository
    {
        public Task<IEnumerable<ProjectInfoSimple>> GetUserProjects(Guid userId, bool addDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<Project>> GetUserProjectsAsync(Guid userId, bool addDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserInfoSimple>?> GetUsersByProjects(IEnumerable<Guid> projectIds, bool addDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserInfoSimple>?> GetUserUsers(Guid userId, bool addDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>?> GetUserUsersAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<Country> ReadOrCreateCountryByNameAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task<Location> ReadOrCreateLocationByNameAsync(string data)
        {
            throw new NotImplementedException();
        }

        public Task<Position> ReadOrCreatePositionByNameAsync(string data)
        {
            throw new NotImplementedException();
        }

        public async Task<IProjectInfoSimple?> ReadProjectById(Guid projectId)
        {
            var pis = new ProjectInfoSimple()
            {
                Id = projectId,
                Name = "",
                IsDeleted = false,
                OutId = "",
                Pmsystem = "",
            };
            return await Task.FromResult<ProjectInfoSimple?>(pis);
        }

        public Task<Project?> ReadProjectByNameAsync(string name, Guid systemId)
        {
            throw new NotImplementedException();
        }

        public Task<Project?> ReadProjectByOutIdAsync(string outId, Guid systemId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectModel>?> ReadProjectInfoAsync(Guid userId, DateTime dateFrom, int dayCount)
        {
            throw new NotImplementedException();
        }

        public Task<User?> ReadUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserBaseInfo>> ReadUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<UserBaseInfo?> ReadUserById(Guid uid)
        {
            var pis = new UserBaseInfo()
            {
                UserId = uid,
                Deleted = false,
                
            };
            return await Task.FromResult(pis);
        }

        public Task<UserBaseInfo?> ReadUserBySid(string sid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserBaseInfo>> ReadUserByUserName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<WorkItem?> ReadWorkItemByIdAsync(string outId, Guid projectId)
        {
            throw new NotImplementedException();
        }

        public Task<Project> WriteProjectAsync(ProjectInfoIncomeInner projectInfo)
        {
            throw new NotImplementedException();
        }

        public Task<User> WriteUserAsync(UserInfoIncomeInner userInfo)
        {
            throw new NotImplementedException();
        }

        public Task<WorkItem?> WriteWorkItemAsync(WorkItemInfoIncomeInner workitemInfo)
        {
            throw new NotImplementedException();
        }
    }
}
