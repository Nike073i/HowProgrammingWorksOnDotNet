namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class ListWithLimitersTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new ListWithLimiters<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new ListWithLimiters<int>(values);
    }
}
