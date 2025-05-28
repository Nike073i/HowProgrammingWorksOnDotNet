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

        [Fact]
        public void ReverseList()
        {
            var list = new ListWithLimiter<int>();

            list.AddFirst(10);
            list.AddFirst(9);
            list.AddLast(11);
            list.AddLast(12);

            Assert.Equal([9, 10, 11, 12], list.Select(lv => lv.Value));

            list.Remove(9);
            list.AddFirst(8);
            list.RemoveLast();
            list.AddLast(13);
            list.RemoveFirst();
            list.AddFirst(7);

            Assert.Equal([7, 10, 11, 13], list.Select(lv => lv.Value));

            list.ReverseInPlace();

            Assert.Equal([13, 11, 10, 7], list.Select(lv => lv.Value));

            list.AddLast(14);
            list.AddFirst(5);
            list.AddLast(15);
            list.AddFirst(4);

            Assert.Equal([4, 5, 13, 11, 10, 7, 14, 15], list.Select(lv => lv.Value));
        }
    }
}
