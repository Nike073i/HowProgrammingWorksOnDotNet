using Microsoft.EntityFrameworkCore;

namespace HowProgrammingWorksOnDotNet.Database.Efc;

public class IncludeFilter
{
    private record Something(List<Another> Anothers);

    private record Another(int Year);

    [Fact]
    public void Usage()
    {
        DbContext context = null;

        context.Set<Something>().Include(a => a.Anothers.Where(b => b.Year < 1900));
    }
}
