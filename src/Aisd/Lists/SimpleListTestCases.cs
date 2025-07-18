namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class SimpleListTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new SimpleList<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new SimpleList<int>(values);

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
