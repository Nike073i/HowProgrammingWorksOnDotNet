using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class MaxArea
{
    public static int Eval(int[] height)
    {
        int left = 0;
        int right = height.Length - 1;

        int max = -1;
        while (left <= right)
        {
            int square = Math.Min(height[left], height[right]) * (right - left);
            max = Math.Max(max, square);
            if (height[left] > height[right])
                right--;
            else
                left++;
        }

        return max;
    }
}

public class MaxAreaTests
{
    [Theory]
    [ClassData(typeof(MaxAreaTestData))]
    public void Test(int[] height, int expected)
    {
        int actual = MaxArea.Eval(height);
        Assert.Equal(expected, actual);
    }
}

public class MaxAreaTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public MaxAreaTestData()
    {
        Add([1, 8, 6, 2, 5, 4, 8, 3, 7], 49);
        Add([1, 1], 1);
        Add([5, 5, 5, 5, 5], 20);
        Add([1, 2, 3, 4, 5], 6);
        Add([5, 4, 3, 2, 1], 6);
        Add([1, 2, 1, 3, 1, 2, 1], 8);
        Add([8, 1, 1, 1, 1, 1, 8], 48);
        Add([1000, 1, 1, 1000], 3000);
        Add([4, 3, 2, 1, 4], 16);
        Add([1, 2, 1, 10], 4);
        Add([10, 1, 2, 1], 4);
        Add([5, 1, 1, 1, 5], 20);
        Add([1, 2, 3, 2, 1], 4);
        Add([], -1);
    }
}
