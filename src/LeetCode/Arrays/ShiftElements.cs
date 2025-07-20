using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class ShiftElements
{
    public static void ShiftRight(int[] nums, int k)
    {
        int length = nums.Length;
        if (length < 2)
            return;

        k %= length;
        if (k == 0)
            return;

        Reverse(nums, 0, length - 1);
        Reverse(nums, 0, k - 1);
        Reverse(nums, k, length - 1);
    }

    private static void Reverse(int[] nums, int start, int end)
    {
        for (int i = 0; i <= (end - start) / 2; i++)
            Swap(ref nums[i + start], ref nums[end - i]);
    }

    private static void Swap(ref int a, ref int b) => (a, b) = (b, a);
}

public class ShiftElementsTests
{
    [Theory]
    [ClassData(typeof(ShiftElementsTestData))]
    public void Test(int[] nums, int n, int[] expected)
    {
        ShiftElements.ShiftRight(nums, n);
        Assert.Equal(expected, nums);
    }
}

public class ShiftElementsTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public ShiftElementsTestData()
    {
        Add([], 0, []);

        Add([1], 0, [1]);
        Add([1], 1, [1]);
        Add([1], 10, [1]);

        Add([1, 2, 3], 0, [1, 2, 3]);
        Add([1, 2, 3], 3, [1, 2, 3]);
        Add([1, 2, 3, 4], 4, [1, 2, 3, 4]);

        Add([1, 2, 3, 4, 5], 1, [5, 1, 2, 3, 4]);
        Add([1, 2, 3, 4, 5], 2, [4, 5, 1, 2, 3]);
        Add([1, 2, 3, 4, 5], 3, [3, 4, 5, 1, 2]);

        Add([1, 2, 3, 4, 5], 6, [5, 1, 2, 3, 4]);
        Add([1, 2, 3, 4, 5], 7, [4, 5, 1, 2, 3]);
        Add([1, 2, 3, 4, 5], 8, [3, 4, 5, 1, 2]);
    }
}
