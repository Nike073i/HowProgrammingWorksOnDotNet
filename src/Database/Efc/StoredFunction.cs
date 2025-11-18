using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc;

public record QueryModel();

public class ApplicationDbContext : DbContext
{
    // or [DbFunction(Name = "CalculateDiscountedPrice", Schema = "dbo")]
    public static decimal ScalarFunction(decimal price, decimal discountPercent) =>
        throw new NotSupportedException("This method can only be called in LINQ queries");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDbFunction(typeof(ApplicationDbContext).GetMethod(nameof(ScalarFunction)))
            .HasName("CalculateDiscountedPrice")
            .HasSchema("dbo");

        modelBuilder.HasDbFunction(() => TableFunction(default));
    }

    public IQueryable<QueryModel> TableFunction(int param) =>
        FromExpression(() => TableFunction(param));
}

public static class StaticClassWithFunctions
{
    [DbFunction(Name = "SearchProducts", Schema = "dbo")]
    public static int GetSmthScalar(DateTime startDate, DateTime endDate) =>
        throw new NotSupportedException("This method can only be called in LINQ queries");
}
