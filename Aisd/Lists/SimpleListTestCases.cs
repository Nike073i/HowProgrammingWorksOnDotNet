namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class SimpleListTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new SimpleList<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new SimpleList<int>(values);
    }
}
