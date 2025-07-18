namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class LinkedListCircleTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new LinkedListCircle<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new LinkedListCircle<int>(values);
    }
}
