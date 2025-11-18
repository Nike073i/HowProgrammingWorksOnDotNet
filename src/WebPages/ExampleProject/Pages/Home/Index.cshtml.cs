using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HowProgrammingWorksOnDotNet.WebPages.ExampleProject.Pages.Home;

public class IndexModel : PageModel
{
    public IActionResult OnGet(int? id) => Page();

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        await Task.Delay(10);
        return RedirectToPage("Index", new { id = 555 });
    }
}
