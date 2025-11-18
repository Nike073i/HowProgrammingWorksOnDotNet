using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc.BaseQuery;

// обычная сущность
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<Product> Products => Set<Product>();
}

public class BaseQueryExample
{
    [Fact]
    public void Usage()
    {
        var context = new AppDbContext();
        string searchTerm = "something";
        int minPrice = 100;

        var baseQuery = context.Products.FromSqlInterpolated(
            $@"
                    SELECT * FROM Products 
                    WHERE Name LIKE {searchTerm} AND Price > {minPrice}"
        );

        // отслеживаются
        var products = baseQuery
            .Where(b => b.Name.Contains("dotnet"))
            .OrderBy(b => b.Name)
            //.Include(b => b.Posts) // Жадно загружаем связанные посты!
            .ToList();

        // Базовый запрос будет использоваться в качестве "выборки" в секции FROM
    }
}
