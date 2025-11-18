using Microsoft.AspNetCore.Mvc;

namespace HowProgrammingWorksOnDotNet.WebMvc.ExampleProject.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{
    public IActionResult Index() => View();
}
