using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Chrono.Services.Models;
using Chrono.Services.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Swashbuckle.AspNetCore.Annotations;

namespace NewChrono.Controllers;

//[Authorize]
[ApiController]
//[Route("[controller]")]
[Route("")]
//[Route("api/v1/getdata")]
public class DataGetController : AbstractController
{
    //private IChronoRepository repository;
    private readonly IDataGetService _service;

    public DataGetController(IDataGetService service, ILogger<DataGetController> logger) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="dateFrom"></param>
    /// <param name="dayCount"></param>
    /// <returns></returns>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectModel>))]
    [HttpGet("getprojects/userId={userId}&dateFrom={dateFrom}&dayCount={dayCount}")]
    public async Task<IActionResult> GetProjects([FromRoute] Guid userId, [FromRoute] DateTime dateFrom, [FromRoute] int dayCount)
        //public async Task<IActionResult> GetTimeReport([FromRoute]ITimeReportGetRequest getRequest)
    {
        //var result = await _service.GetTimeReport(getRequest.UserId, getRequest.DateFrom, getRequest.DayCount);
        var result = await _service.GetProjects(userId, dateFrom, dayCount);
        if (result == null /*|| result.Count() <= 0*/)  // Пустой список имеет право на существование. 
            //return Error(HttpStatusCode.NotFound, $"The user '{getRequest.UserId}' not found.");
            return Error(HttpStatusCode.NotFound, $"The user '{userId}' not found.");

        return Ok(new { TimeReport = result, });
    }

    /// <summary>
    ///     3.1 List of users for main user(by projects).
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserInfoSimple>))]
    [HttpGet("users/userslist/{userId}")]
    public async Task<IActionResult> GetUserUsers([FromRoute] Guid userId, [FromQuery] bool addDeleted = false)
    {
        var result = await _service.GetUserUsers(userId, addDeleted);

        return Ok(new { User = result, });
    }

    /// <summary>
    ///     3.1 List of projects for main user.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProjectInfoSimple>))]
    //[HttpGet("users/projectslist/userId={userId}&addDeleted={addDeleted}")]
    [HttpGet("users/projectslist/{userId}")]
    public async Task<IActionResult> GetUserProjects([FromRoute] Guid userId, [FromQuery] bool addDeleted = false)
    {
        var result = await _service.GetUserProjects(userId, addDeleted);

        return Ok(new { Projects = result, });
    }

    /// <summary>
    ///     A user that's found by his Sid.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserBaseInfo))]
    [HttpGet("users/sid/{sid}")]
    public async Task<IActionResult> GetUserBySid([FromRoute] string sid)
    {
        if (string.IsNullOrWhiteSpace(sid)) return Error(HttpStatusCode.BadRequest, $"The '{sid}' is invalid.");

        var result = await _service.GetUserBySid(sid);
        if (result == null) return Error(HttpStatusCode.NotFound, $"The user with Sid '{sid}' not found.");

        return Ok(new { User = result, });
    }
    /// <summary>
    ///     A user that's found by his Id.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UserBaseInfo))]
    [HttpGet("users/id/{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await _service.ReadUserById(id);
        if (result == null) return Error(HttpStatusCode.NotFound, $"The user '{id}' not found.");

        return Ok(new { User = result, });
    }
    /// <summary>
    ///     A user that's found by his UserName.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserBaseInfo>))]
    [HttpGet("users/username/{userName}")]
    public async Task<IActionResult> GetUserByUserName([FromRoute] string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) return Error(HttpStatusCode.BadRequest, $"The '{userName}' is invalid.");

        var result = await _service.GetUserByUserName(userName);
        if (result == null || result.Count() <= 0) return Error(HttpStatusCode.NotFound, $"The user '{userName}' not found.");

        return Ok(new { User = result, });
    }

    /// <summary>
    ///     A user that's found by his Email.
    /// </summary>
    [HttpGet("users/email/{email}")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserBaseInfo>))]
    public async Task<IActionResult> GetUsersByEmail([FromRoute] string email)
    {
        //if (!email.IsWellFormedEmailAddress())
        //{
        //    return Error(HttpStatusCode.BadRequest, $"The email address '{email}' is invalid.");
        //}
        if (string.IsNullOrWhiteSpace(email)) return Error(HttpStatusCode.BadRequest, $"The '{email}' is invalid.");

        var result = await _service.GetUsersByEmail(email);
        if (result == null || result.Count() <= 0) return Error(HttpStatusCode.NotFound, $"The user with Email '{email}' not found.");

        return Ok(new { Users = result, });
    }
}