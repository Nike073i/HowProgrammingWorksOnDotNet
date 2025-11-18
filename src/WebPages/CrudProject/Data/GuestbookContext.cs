using Microsoft.EntityFrameworkCore;

namespace CrudProject.Data
{
    public class GuestbookContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Entry> Entries { get; set; }
    }
}
