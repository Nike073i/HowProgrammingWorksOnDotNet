using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc;

public class QueryProfiling
{
    [Fact]
    public void GetSql()
    {
        IQueryable smthQuery = null;
        // не работает с агрегацией, Any/All
        var sql = smthQuery.ToQueryString();
        // ...
    }
}
