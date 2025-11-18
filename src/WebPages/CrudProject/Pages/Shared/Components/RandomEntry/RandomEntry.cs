using CrudProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace CrudProject.Pages.Shared.Components.RandomEntry
{
    public class RandomEntryViewComponent(GuestbookContext context) : ViewComponent
    {
        private readonly GuestbookContext _context = context;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var totalCount = await _context.Entries.CountAsync();

            if (totalCount == 0)
                return new ContentViewComponentResult("Empty"); // Или другой View. Например Empty.cshtml в этом же компоненте

            var random = new Random();
            var randomIndex = random.Next(0, totalCount);

            var randomEntry = await _context
                .Entries.OrderBy(e => random.Next())
                .FirstOrDefaultAsync();

            return View(randomEntry);
        }
    }
}
