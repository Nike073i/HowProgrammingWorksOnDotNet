using CrudProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CrudProject.Pages.Entry.Index
{
    public class IndexRazorPagesModel(GuestbookContext db) : PageModel
    {
        private readonly GuestbookContext _db = db;

        public IList<Data.Entry> Entries { get; private set; } = [];
        public string? Search { get; set; }
        public bool Flag => true;

        public async Task OnGetAsync(string? search)
        {
            Search = search;
            Entries = await _db.Entries.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id)
        {
            var entry = await _db.Entries.FindAsync(id);
            entry!.Likes += 1;
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var entry = await _db.Entries.FindAsync(id);
            _db.Entries.Remove(entry!);
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
