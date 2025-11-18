using CrudProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace CrudProject.Pages.Entry.Create
{
    public class CreateModel(GuestbookContext db, IStringLocalizer<CreateModel> localizer)
        : PageModel
    {
        private readonly GuestbookContext _db = db;

        // Binding like 'asp-for="Entry.Name"' Эта привязка позволяет "сохранить данные" при ошибке от серверной-валидации
        [BindProperty]
        public CreateBindingModel Entry { get; set; }

        [ViewData]
        // Для csharp кода используем IStringLocalizer по классу модели (CreateModel.ru.resx)
        public string Title => localizer["Title"];

        public List<SelectListItem> Categories { get; set; } =
            [new("Не выбрана", "none"), new("Category A", "cat_a"), new("Category B", "cat_b")];

        [BindProperty]
        public string SelectedCategory { get; set; }

        public IActionResult OnGet()
        {
            SelectedCategory = Categories[0].Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Console.WriteLine("Выбранная категория - " + SelectedCategory);

            var newEntry = new Data.Entry { Content = Entry.Content, Email = Entry.Email };

            _db.Entries.Add(newEntry);
            await _db.SaveChangesAsync();

            TempData["created-id"] = newEntry.Id;
            return RedirectToPage("../Index/Index");
        }
    }
}
