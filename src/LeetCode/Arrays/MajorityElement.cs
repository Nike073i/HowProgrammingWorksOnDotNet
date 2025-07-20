using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public interface IMajorityElementFinder
{
    int Find(int[] nums);
}

public abstract class MajorityElementFinderTests
{
    protected abstract IMajorityElementFinder CreateFinder();

    [Theory]
    [ClassData(typeof(MajorityElementFinderTestData))]
    public void Tests(int[] nums, int expected)
    {
        var finder = CreateFinder();
        int actual = finder.Find(nums);
        Assert.Equal(expected, actual);
    }
}

public class MajorityElementFinderTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public MajorityElementFinderTestData()
    {
        Add([], -1);

        Add([1], 1);

        Add([3, 2, 3], 3);
        Add([2, 2, 1, 1, 1, 2, 2], 2);
        Add([1, 1, 2, 2, 2], 2);

        Add([1, 2, 3], -1);
        Add([1, 1, 2, 2], -1);
        Add([1, 1, 1, 2, 2, 2], -1);
        Add([.. Enumerable.Repeat(5, 1000), .. Enumerable.Range(1, 999)], 5);
    }
}

public class DictionaryMajorityElementFinderTests : MajorityElementFinderTests
{
    protected override IMajorityElementFinder CreateFinder() =>
        new DictionaryMajorityElementFinder();
}

public class DictionaryMajorityElementFinder : IMajorityElementFinder
{
    public int Find(int[] nums)
    {
        int n = nums.Length / 2;
        var dict = new Dictionary<int, int>();
        foreach (int i in nums)
        {
            int newCount = dict.TryGetValue(i, out int counter) ? counter + 1 : 1;
            if (newCount > n)
                return i;

            dict[i] = newCount;
        }

        return -1;
    }
}

public class WithSortingMajorityElementFinderTests : MajorityElementFinderTests
{
    protected override IMajorityElementFinder CreateFinder() =>
        new WithSortingMajorityElementFinder();
}

public class WithSortingMajorityElementFinder : IMajorityElementFinder
{
    public int Find(int[] nums)
    {
        if (nums.Length == 1)
            return nums[0];

        int[] sorted = nums.Order().ToArray();
        int n = nums.Length / 2;
        int k = 1;

        for (int i = 1; i < sorted.Length; i++)
        {
            if (sorted[i] == sorted[i - 1])
            {
                k++;
                if (k > n)
                    return sorted[i];
            }
            else
                k = 1;
        }
        return -1;
    }
}
