using CrudProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrudProject.Pages.Entry.Edit
{
    public class EditModel(GuestbookContext db) : PageModel
    {
        private readonly GuestbookContext _db = db;

        // Здесь привязка к свойству для того, чтобы была возможность обращатьс к данным с cshtml
        [BindProperty] // Binding like 'asp-for="Entry.Name"', поскольку это привязка к конкретному свойству
        public Data.Entry Entry { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Entry = await _db.Entries.FindAsync(id);

            if (Entry == null)
                return RedirectToPage("../Index/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _db.Attach(Entry).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToPage("../Index/Index");
        }
    }
}
