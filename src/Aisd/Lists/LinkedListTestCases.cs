namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class LinkedListTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new LinkedList<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new LinkedList<int>(values);
    }
}
