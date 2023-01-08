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

[ApiController]
[Route("[controller]")]
public class TimeReportController : AbstractController
{
    private readonly ITimeReportService _service;

    public TimeReportController(ITimeReportService service, ILogger<TimeReportController> logger) : base(logger)
    {
        _service = service;
    }

    /// <summary>
    ///     3.3.1.1 Запрос и получение информации для отображения
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(TimeReportModelNew))]
    //[HttpPost("gettimereport/userId={userId}&dateFrom={dateFrom}&dayCount={dayCount}")]
    [HttpPost("gettimereport")]
    //public async Task<IActionResult> GetTimeReport([FromRoute] Guid userId, [FromRoute] DateTime dateFrom, [FromRoute] int dayCount)        
    public async Task<IActionResult> GetTimeReport([FromBody] TimeReportGetRequest getRequest)
    {
        var result = await _service.GetTimeReportN(getRequest.UserId, getRequest.DateFrom, getRequest.DayCount);
        if(result.User == null)
        {
            return Error(HttpStatusCode.NotFound, $"The user '{getRequest.UserId}' not found.");
        }
        //var userId = getRequest.UserId;
        //var result = await _service.GetTimeReport(userId, dateFrom, dayCount);
        //if (result == null)
        //{

        //    //return Error(HttpStatusCode.NotFound, $"The user '{getRequest.UserId}' not found.");
        //    return Error(HttpStatusCode.NotFound, $"The user '{userId}' not found.");
        //}
        return Ok(result);
        //return Ok(new { TimeReport = result });
    }

    /// <summary>
    ///     3.3.1.1 Передача изменённых данных для сохранения
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(int))]
    //[HttpPost("gettimereport/userId={userId}&dateFrom={dateFrom}&dayCount={dayCount}")]
    [HttpPost("settimereport")]
    //public async Task<IActionResult> GetTimeReport([FromRoute] Guid userId, [FromRoute] DateTime dateFrom, [FromRoute] int dayCount)        
    public async Task<IActionResult> SetTimeReport([FromBody] TimeReportSetRequest setRequest)
    {
        int result;
        try
        {
            result = await _service.SetTimeReports(setRequest);
        }
        catch(Exception ex)
        {
            return Error(HttpStatusCode.NotFound, ex.Message);
        }
        //var userId = getRequest.UserId;
        //var result = await _service.GetTimeReport(userId, dateFrom, dayCount);
        //if (result == null)
        //{

        //    //return Error(HttpStatusCode.NotFound, $"The user '{getRequest.UserId}' not found.");
        //    return Error(HttpStatusCode.NotFound, $"The user '{userId}' not found.");
        //}
        return Ok(result);
        //return Ok(new { TimeReport = result });
    }
    /// <summary>
    ///     3.1 List of reports by projects.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimeReportModelNew>))]
    [HttpPost("gettimereportsbyprojectsforview")]
    public async Task<IActionResult> GetTimeReportsByProjectsForView([FromBody] TimeReportsGetRequest getRequest)
    {
        var result = await _service.GetTimeReportsByProjectsForView(getRequest.IdList, getRequest.DateFrom, getRequest.DayCount, getRequest.addDeleted);

        return Ok(result);
    }
    /// <summary>
    ///     3.1 List of reports by users.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimeReportModelNew>))]
    [HttpPost("gettimereportsbyusersforview")]
    public async Task<IActionResult> GetTimeReportsByUsersForView([FromBody] TimeReportsGetRequest getRequest)
    {
        var result = await _service.GetTimeReportsByUsertsForView(getRequest.IdList, getRequest.DateFrom, getRequest.DayCount, getRequest.addDeleted);

        return Ok(result);
    }
    /// <summary>
    ///     3.1 List of reports by projects.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimeReportModel>))]
    [HttpPost("gettimereportsbyprojects")]
    public async Task<IActionResult> GetTimeReportsByProjects([FromBody] TimeReportsGetRequest getRequest)
    {
        var result = await _service.GetTimeReportsByProjects(getRequest.IdList, getRequest.DateFrom, getRequest.DayCount);

        return Ok(result);
    }

    /// <summary>
    ///     3.1 List of reports by users.
    /// </summary>
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<TimeReportModel>))]
    [HttpPost("gettimereportsbyusers")]
    public async Task<IActionResult> GetTimeReports([FromBody] TimeReportsGetRequest getRequest)
    {
        var result = await _service.GetTimeReports(getRequest.IdList, getRequest.DateFrom, getRequest.DayCount);

        return Ok(result);
    }
}