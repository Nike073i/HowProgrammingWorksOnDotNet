using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

// Оставить не более N копий
// n > 0;
// Вернуть кол-во оставшихся элементов
public class LeaveOnlyNDuplicates(int n)
{
    public int Execute(int[] nums)
    {
        if (nums.Length <= n)
            return nums.Length;

        int p = n;
        for (int i = n; i < nums.Length; i++)
        {
            if (nums[i] != nums[p - n])
                nums[p++] = nums[i];
        }

        return p;
    }
}

public class LeaveOnlyNDuplicatesTests
{
    [Theory]
    [ClassData(typeof(LeaveOnlyNDuplicatesTestData))]
    public void Tests(int[] nums, int n, int countAfter)
    {
        int actual = new LeaveOnlyNDuplicates(n).Execute(nums);
        Assert.Equal(countAfter, actual);
    }

    public class LeaveOnlyNDuplicatesTestData : TheoryDataContainer.ThreeArg<int[], int, int>
    {
        public LeaveOnlyNDuplicatesTestData()
        {
            Add([], 1, 0);
            Add([], 2, 0);
            Add([1], 1, 1);
            Add([1], 2, 1);
            Add([1, 2, 3], 1, 3);
            Add([1, 2, 3], 2, 3);
            Add([1, 1, 2, 2, 3, 3], 1, 3);
            Add([1, 1, 1, 2, 2, 3], 2, 5);
            Add([7, 7, 7, 7], 1, 1);
            Add([7, 7, 7, 7], 2, 2);
            Add([0, 0, 0, 1, 2, 3], 2, 5);
            Add([1, 2, 3, 3, 3, 3], 3, 5);
            Add([1, 2, 2, 2, 3], 2, 4);
            Add([1, 1, 1, 1, 2, 2, 2, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6], 3, 14);
            Add([1, 1, 2, 2, 3], 1, 3);
        }
    }
}
