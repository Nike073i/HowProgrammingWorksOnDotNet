using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc.Query;

// keyless-сущность не отслеживается
public class CarMakeViewModel
{
    public int MakeId { get; set; }
    public string Make { get; set; }
    public int CarId { get; set; }
    public bool IsDrivable { get; set; }
    public string Display { get; set; }
    public DateTime DateBuilt { get; set; }
    public string Color { get; set; }
    public string PetName { get; set; }

    [NotMapped]
    public string FullDetail => $" The {Color} {Make} is named {PetName}";
}

public class AppDbContext : DbContext
{
    public DbSet<CarMakeViewModel> CarMakesQueries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<CarMakeViewModel>()
            .HasNoKey()
            .ToSqlQuery(
                @"""
                SELECT m.Id MakeId, m.Name Make, i.Id CarId, i.IsDrivable, i.DisplayName, i.DateBuilt, i.Color, i.PetName
                FROM dbo.Makes m 
                INNER JOIN dbo.Inventory i ON i.MakeId = m.Id
                """
            )
            .ToTable(t => t.ExcludeFromMigrations());

        // or bind to view
        // modelBuilder.Entity<CarMakeViewModel>().HasNoKey().ToView("ViewName");
    }
}

public record QueryModel(string Field1, int Val2);

public class QueryUsage
{
    [Fact]
    public void Efc7Query()
    {
        DbContext dbContext = null;
        var result = dbContext.Database.SqlQueryRaw<QueryModel>(
            "SELECT VALUES('field1', 2) AS t(Field1, Val2)"
        );
    }
}
