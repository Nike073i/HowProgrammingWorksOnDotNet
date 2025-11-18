using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc.SqlCommand;

public class AppDbContext : DbContext { }

public class SqlCommand
{
    [Fact]
    public void Usage()
    {
        using var context = new AppDbContext();
        string newName = "";
        string curName = "";
        var affectedRows = context.Database.ExecuteSql(
            $"UPDATE Customers SET Name={newName} WHERE Name={curName}"
        );
    }
}
