namespace HowProgrammingWorksOnDotNet.Language.Linq;

public class Pagination
{
    private string CollToString<T>(IEnumerable<T> coll) => string.Join(", ", coll);

    [Fact]
    public void PageByChunk()
    {
        var data = Enumerable.Range(1, 23);
        var pageSize = 5;

        var pages = data.Chunk(pageSize).ToArray();

        foreach (var (pageData, pageNumber) in pages.Zip(Enumerable.Range(1, pages.Length)))
            Console.WriteLine($"Страница - {pageNumber}: {CollToString(pageData)}");
    }

    // Not supported in EFC
    [Fact]
    public void PageByRange()
    {
        IEnumerable<T> GetPage<T>(IEnumerable<T> data, int pageSize, int currentPage) =>
            data.Take(new Range((currentPage - 1) * pageSize, currentPage * pageSize));

        IEnumerable<(IEnumerable<T>, int)> Paginate<T>(IEnumerable<T> data, int pageSize)
        {
            int currentPage = 1;
            bool hasMoreData = true;
            while (hasMoreData)
            {
                var pageData = GetPage(data, pageSize, currentPage);

                if (pageData.Any())
                {
                    yield return (pageData, currentPage);
                    currentPage++;
                }
                else
                    hasMoreData = false;
            }
        }

        var data = Enumerable.Range(1, 23);
        var pageSize = 5;

        foreach (var (pageData, pageNumber) in Paginate(data, pageSize))
            Console.WriteLine($"Страница - {pageNumber}: {string.Join(", ", pageData)}");
    }
}
