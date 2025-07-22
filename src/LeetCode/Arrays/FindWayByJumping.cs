using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public interface IFinderWayByJumping
{
    bool CanJump(int[] nums);
}

public class SpaceCoveringMethodFinderWayByJumping : IFinderWayByJumping
{
    public bool CanJump(int[] nums)
    {
        int lastCovering = 0;
        int lastPosition = nums.Length - 1;
        for (int i = 0; i < lastPosition && lastCovering < lastPosition; i++)
        {
            if (i > lastCovering)
                return false;

            lastCovering = Math.Max(lastCovering, i + nums[i]);
        }
        return true;
    }
}

public class SpaceCoveringMethodFinderWayByJumpingTests : FindWayByJumpingTests
{
    protected override IFinderWayByJumping CreateFinder() =>
        new SpaceCoveringMethodFinderWayByJumping();
}

public abstract class FindWayByJumpingTests
{
    protected abstract IFinderWayByJumping CreateFinder();

    [Theory]
    [ClassData(typeof(FindWayByJumpingTestData))]
    public void Usage(int[] nums, bool expected)
    {
        var finder = CreateFinder();
        bool actual = finder.CanJump(nums);
        Assert.Equal(expected, actual);
    }
}

public class FindWayByJumpingTestData : TheoryDataContainer.TwoArg<int[], bool>
{
    public FindWayByJumpingTestData()
    {
        Add([0], true);
        Add([1, 0], true);
        Add([3, 2, 1, 0, 4], false);
        Add([2, 3, 1, 1, 4], true);
        Add([.. Enumerable.Repeat(1, 100_000)], true);
        Add([4, 0, 0, 0, 1, 0, 0, 0, 0, 1], false);
        Add([5, 4, 3, 2, 1, 0, 1, 0], false);
        Add([2, 0, 3, 0, 0, 1, 0], true);
        Add([0, 0, 0, 0, 5], false);
        Add([int.MaxValue, 0, 0, 0, 1], true);
        Add([1, 1, 1, 1, 1, 1, 1], true);
        Add([3, 0, 2, 0, 0, 1], false);
        Add([2, 1, 0, 1, 2, 0], false);

        int[] hugeArray = [.. Enumerable.Repeat(1, 100_000)];
        hugeArray[50_000] = 0;
        Add(hugeArray, false);
    }
}
