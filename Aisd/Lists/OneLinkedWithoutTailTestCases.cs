namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public class OneLinkedWithoutTailTestCases : ListTestCases
    {
        protected override IList<int> CreateList() => new OneLinkedWithoutTail<int>();

        protected override IList<int> CreateList(IEnumerable<int> values) =>
            new OneLinkedWithoutTail<int>(values);

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
