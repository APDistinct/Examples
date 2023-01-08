using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace NewChrono.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AbstractController : ControllerBase
{
    protected readonly ILogger _logger;

    public AbstractController(ILogger logger)
    {
        _logger = logger;
    }

    [NonAction]
    public virtual ObjectResult Error(HttpStatusCode statusCode, string message)
    {
        //var action = ControllerContext?.ActionDescriptor?.DisplayName ?? "";
        //_logger.LogError($"{statusCode}: {action} - {message}");
        //return Problem(detail: message, statusCode: (int)statusCode, type: action);

        _logger.LogError($"{statusCode}: {message}");
        return Problem(message, statusCode: (int)statusCode, type: "");
    }
}