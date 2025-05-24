using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists.Tests
{
    public abstract class ListTestCases
    {
        protected abstract IList<int> CreateList();
        protected abstract IList<int> CreateList(IEnumerable<int> values);

        [ClassData(typeof(AddInHeadTheoryData))]
        [Theory]
        public void AddInHead_ShouldBeReversedInput(IEnumerable<int> values)
        {
            // Arrange
            var list = CreateList();

            // Act
            values.ToList().ForEach(list.AddFirst);

            // Assert
            Assert.Equal(values.Reverse(), list.Select(lv => lv.Value));
        }

        private class AddInHeadTheoryData : TheoryDataContainer.OneArg<IEnumerable<int>>
        {
            public AddInHeadTheoryData()
            {
                Add([1, 2, 3]);
                Add([]);
                Add(Enumerable.Range(1, 25000));
            }
        }

        [ClassData(typeof(AddLastTestData))]
        [Theory]
        public void AddLast_ShouldPreserveOrder(IEnumerable<int> values)
        {
            // Arrange & Act
            var list = CreateList(values);

            // Assert
            Assert.Equal(values, list.Select(lv => lv.Value));
        }

        private class AddLastTestData : TheoryDataContainer.OneArg<IEnumerable<int>>
        {
            public AddLastTestData()
            {
                Add([10, 20, 30]);
                Add([]);
                Add(Enumerable.Range(0, 50_000));
            }
        }

        [ClassData(typeof(RemoveFirstTestData))]
        [Theory]
        public void RemoveFirst_ShouldReturnAndRemoveFirstElement(
            IEnumerable<int> input,
            ListValue<int>? expectedRemoved
        )
        {
            // Arrange
            var list = CreateList(input);

            // Act
            var removed = list.RemoveFirst();

            // Assert
            Assert.Equal(expectedRemoved, removed);
            Assert.Equal(input.Skip(1), list.Select(lv => lv.Value));
        }

        private class RemoveFirstTestData
            : TheoryDataContainer.TwoArg<IEnumerable<int>, ListValue<int>?>
        {
            public RemoveFirstTestData()
            {
                Add([1, 2, 3], ListValue<int>.Of(1));
                Add([42], ListValue<int>.Of(42));
                Add([], null);
            }
        }

        [ClassData(typeof(RemoveLastTestData))]
        [Theory]
        public void RemoveLast_ShouldReturnAndRemoveLastElement(
            IEnumerable<int> input,
            ListValue<int>? expectedRemoved
        )
        {
            // Arrange
            var list = CreateList(input);

            // Act
            var removed = list.RemoveLast();

            // Assert
            Assert.Equal(expectedRemoved, removed);
            Assert.Equal(input.SkipLast(1), list.Select(lv => lv.Value));
        }

        private class RemoveLastTestData
            : TheoryDataContainer.TwoArg<IEnumerable<int>, ListValue<int>?>
        {
            public RemoveLastTestData()
            {
                Add([1, 2, 3], ListValue<int>.Of(3));
                Add([99], ListValue<int>.Of(99));
                Add([], null);
            }
        }

        [ClassData(typeof(ClearTestData))]
        [Theory]
        public void Clear_ShouldRemoveAllElements(IEnumerable<int> values)
        {
            // Arrange
            var list = CreateList(values);

            // Act
            list.Clear();

            // Assert
            Assert.Empty(list);
        }

        private class ClearTestData : TheoryDataContainer.OneArg<IEnumerable<int>>
        {
            public ClearTestData()
            {
                Add([1, 2, 3]);
                Add([]);
                Add(Enumerable.Range(1, 10_000));
            }
        }

        [ClassData(typeof(EnumerationTestData))]
        [Theory]
        public void Enumeration_ShouldMatchExpectedOrder(IEnumerable<int> values)
        {
            // Arrange
            var list = CreateList(values);

            // Act & Assert
            Assert.Equal(values, list.Select(lv => lv.Value));
        }

        private class EnumerationTestData : TheoryDataContainer.OneArg<IEnumerable<int>>
        {
            public EnumerationTestData()
            {
                Add([1, 2, 3]);
                Add([]);
                Add(Enumerable.Repeat(5, 100));
            }
        }

        [ClassData(typeof(ListShouldContainElementTestData))]
        [Theory]
        public void List_ShouldContainElement(IEnumerable<int> values, int target, bool expected)
        {
            // Arrange
            var list = CreateList(values);

            // Act
            var contains = list.Contains(target);

            Assert.Equal(expected, contains);
        }

        private class ListShouldContainElementTestData
            : TheoryDataContainer.ThreeArg<IEnumerable<int>, int, bool>
        {
            public ListShouldContainElementTestData()
            {
                Add([1, 2, 3], 4, false);
                Add([1, 2, 3], 3, true);
                Add([], 0, false);
                Add(Enumerable.Repeat(5, 100), 5, true);
            }
        }

        [ClassData(typeof(InsertBeforeElementTestData))]
        [Theory]
        public void InsertBeforeElementTest(
            IEnumerable<int> values,
            int target,
            int value,
            IEnumerable<int> expected
        )
        {
            // Arrange
            var list = CreateList(values);

            // Act
            bool insertResult = list.InsertBefore(target, value);

            Assert.Equal(expected, list.Select(lv => lv.Value));
            Assert.True(list.Select(lv => lv.Value).SequenceEqual(values) ^ insertResult);
        }

        private class InsertBeforeElementTestData
            : TheoryDataContainer.FourArg<IEnumerable<int>, int, int, IEnumerable<int>>
        {
            public InsertBeforeElementTestData()
            {
                Add([1, 2, 3], 2, 4, [1, 4, 2, 3]);
                Add([1, 2, 3], 1, 4, [4, 1, 2, 3]);
                Add([1, 2, 3], 3, 4, [1, 2, 4, 3]);
                Add([1, 2, 3], 5, 4, [1, 2, 3]);
                Add([1, 2, 4, 2, 3], 2, 10, [1, 10, 2, 4, 2, 3]);
            }
        }
    }
}
