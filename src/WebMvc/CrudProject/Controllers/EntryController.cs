using HowProgrammingWorksOnDotNet.WebMvc.CrudProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PracticalAspNetCore.Controllers
{
    public class EntryController(GuestbookContext db) : Controller
    {
        private GuestbookContext _db = db;

        public async Task<IActionResult> Index()
        {
            var entries = await _db.Entries.AsNoTracking().ToListAsync();
            return View(entries);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Entry entry)
        {
            if (!ModelState.IsValid)
                return View("Edit", entry);

            _db.Attach(entry).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entry = await _db.Entries.SingleOrDefaultAsync(e => e.Id == id);
            return View(entry);
        }

        [HttpPost]
        // curl -X POST -H 'Content-Type:application/x-www-form-urlencoded' -v http://localhost:5000/Entry/Create -d 'Content=Nikita&Email=nike073i@mail.ru'
        public async Task<IActionResult> Create(Entry entry)
        {
            if (!ModelState.IsValid)
                return View(entry);

            await _db.Entries.AddAsync(entry);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var entry = await _db.Entries.SingleOrDefaultAsync(e => e.Id == id);
            entry.Likes += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var entry = await _db.Entries.FindAsync(id);
            _db.Entries.Remove(entry!);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
