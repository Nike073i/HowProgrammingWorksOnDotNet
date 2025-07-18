using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Aisd.Subsequences;

public class FindMaxMonotonicSubsequence
{
    private interface IState
    {
        public IState Next(int value);
        public int Count { get; }
    }

    private abstract class BaseState(int prev, int prevCount, int count) : IState
    {
        protected int Prev { get; set; } = prev;
        protected int PrevCount { get; set; } = prevCount;
        public int Count { get; protected set; } = count;

        public int Complete() => Count;

        public IState Next(int value)
        {
            if (value == Prev)
            {
                PrevCount++;
                Count++;
                return this;
            }
            if (IsMonotonic(value))
            {
                Prev = value;
                PrevCount = 1;
                Count++;
                return this;
            }
            return GetNextState(value);
        }

        protected abstract IState GetNextState(int value);

        protected abstract bool IsMonotonic(int value);
    }

    private class UndefinedState(int prev) : BaseState(prev, 1, 1)
    {
        protected override IState GetNextState(int value)
        {
            if (value > Prev)
                return new GrowingState(value, 1, Count + 1);
            return new FallingState(value, 1, Count + 1);
        }

        protected override bool IsMonotonic(int _) => false;
    }

    private class GrowingState(int prev, int prevCount, int count)
        : BaseState(prev, prevCount, count)
    {
        protected override IState GetNextState(int value) =>
            new FallingState(value, PrevCount, PrevCount + 1);

        protected override bool IsMonotonic(int value) => value > Prev;
    }

    private class FallingState(int prev, int prevCount, int count)
        : BaseState(prev, prevCount, count)
    {
        protected override IState GetNextState(int value) =>
            new GrowingState(value, PrevCount, PrevCount + 1);

        protected override bool IsMonotonic(int value) => value < Prev;
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public void Run(int[] sequence, int max)
    {
        if (sequence.Length < 3)
        {
            Assert.Equal(max, sequence.Length);
            return;
        }

        IState state = new UndefinedState(sequence[0]);
        int maxLength = 1;

        foreach (int val in sequence.Skip(1))
        {
            state = state.Next(val);
            maxLength = Math.Max(maxLength, state.Count);
        }

        Assert.Equal(max, maxLength);
    }

    private class TestData : TheoryDataContainer.TwoArg<int[], int>
    {
        public TestData()
        {
            Add([], 0);
            Add([1], 1);
            Add([1, 1], 2);
            Add([0, 1], 2);
            Add([1, 0], 2);
            Add([1, 1, 1, 1, 1, 1], 6);
            Add([1, 1, 1, 1, 2, 3, 4], 7);
            Add([1, 1, 1, 1, 0, -1, -2], 7);
            Add([1, 1, 2, 3, 4, 4, 4, 4], 8);
            Add([1, 1, 0, -1, -2, -3, -3, -3], 8);
            Add([1, 1, 2, 3, 4, 4, 4, 4, 5, 6, 7, 8], 12);
            Add([1, 2, 3, 3, 3, 2, 1, 0, -1], 7);
            Add([3, 2, 1, 1, 1, 2, 3, 4, 5], 7);
            Add([1, 2, 3, 2, 1, 2, 3, 4, 3, 2, 1], 4);
            Add([10, 20, 30, 20, 10, 5, 10, 20, 10, 5, 0], 4);
            Add([1, 2, 2, 3, 4, 4, 3, 2, 2, 1, 0, -1], 8);
            Add([5, 5, 6, 7, 6, 5, 4, 3, 4, 5, 6], 5);
            Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1], 9);
            Add([int.MinValue, int.MaxValue, int.MinValue], 2);
            Add([100, 90, 80, 70, 60, 50, 60, 70, 80, 90, 100], 6);
            Add([1, 3, 2, 4, 3, 5, 4, 6, 5, 7], 2);
            Add([10, 5, 15, 10, 20, 15, 25, 20], 2);
            Add([1, 2, 3, 2, 3, 4, 3, 4, 5, 4, 5, 6], 3);
            Add([0, 1000000, -1000000, 0], 2);
        }
    }
}
