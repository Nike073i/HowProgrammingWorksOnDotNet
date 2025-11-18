using Microsoft.AspNetCore.Mvc;

namespace HowProgrammingWorksOnDotNet.WebApi.ExampleByControllers.Controllers;

[ApiController]
[Route("api")]
public class HomeController : ControllerBase
{
    // curl -X POST -H 'Content-Type:application/json' -v http://localhost:5000/api/ping -d '"hello"'
    [HttpPost("ping")]
    [Consumes("application/json")]
    public IActionResult Ping([FromBody] string content) => Ok($"pong - {content}");
}
