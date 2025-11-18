using Microsoft.AspNetCore.Mvc;

namespace HowProgrammingWorksOnDotNet.WebMvc.ExampleProject.Controllers;

// По умолчанию [Route("[controller]/[action]/{id?}")]
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [HttpPost]
    [ActionName("Create")]
    // Можно переписать как угодно -  [Route("[action]/[controller]/{id?}")]
    public IActionResult PostHandler([FromBody] PostBindingModel model)
    {
        if (!ModelState.IsValid)
            // return View("BadRequest");
            return BadRequest("message...");

        return View(); // View - Create.cshtml а не PostHandler
    }
}

public record PostBindingModel(string Name, int Age);
