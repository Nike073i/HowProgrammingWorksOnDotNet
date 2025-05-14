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
            Assert.Equal(values.Reverse(), list);
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
            Assert.Equal(values, list);
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
            Assert.Equal(input.Skip(1), list);
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
            Assert.Equal(input.SkipLast(1), list);
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
            Assert.Equal(values, list);
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
    }
}
