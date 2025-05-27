namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class ListWithLimitersTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new ListWithLimiter<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new ListWithLimiter<int>(values);

        public override void ShiftLeft(
            IEnumerable<int> values,
            int target,
            int count,
            IEnumerable<int> expected
        ) { }

        public override void ShiftRight(
            IEnumerable<int> values,
            int target,
            int count,
            IEnumerable<int> expected
        ) { }
    }
}
