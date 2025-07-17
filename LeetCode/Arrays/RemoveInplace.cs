using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

/* Вернуть кол-во оставшихся элементов*/
public class RemoveInplace
{
    public static int RemoveElement(int[] nums, int val)
    {
        int p = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] != val)
                nums[p++] = nums[i];
        }
        return p;
    }

    [Theory]
    [ClassData(typeof(RemoveInplaceTestData))]
    public void Tests(int[] nums, int val, int countAfter)
    {
        int actual = RemoveElement(nums, val);
        Assert.Equal(countAfter, actual);
    }

    public class RemoveInplaceTestData : TheoryDataContainer.ThreeArg<int[], int, int>
    {
        public RemoveInplaceTestData()
        {
            Add([], 0, 0);
            Add([], 5, 0);
            Add([1], 1, 0);
            Add([1], 2, 1);
            Add([2, 3, 4, 5], 2, 3);
            Add([1, 2, 3, 4], 4, 3);
            Add([1, 2, 3, 4, 5], 3, 4);
            Add([1, 2, 2, 2, 3], 2, 2);
            Add([5, 5, 5, 5], 5, 0);
            Add([1, 2, 3, 4], 5, 4);
            Add([1, 2, 2, 3, 2, 4, 2], 2, 3);
            Add([-1, -2, -3, -4], -3, 3);
            Add([int.MinValue, 0, int.MaxValue], int.MinValue, 2);
            Add([.. Enumerable.Repeat(1, 1000), .. Enumerable.Range(2, 1000)], 1, 1000);
        }
    }
}
