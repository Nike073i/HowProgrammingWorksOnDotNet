namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class ListWithLimitersTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new ListWithLimiters<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new ListWithLimiters<int>(values);

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
