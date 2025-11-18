using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.WebMvc.CrudProject.Data
{
    public class GuestbookContext : DbContext
    {
        public GuestbookContext(DbContextOptions options) : base(options) { }

        public DbSet<Entry> Entries { get; set; }
    }
}
