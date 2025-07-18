using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

/* Вернуть кол-во оставшихся элементов*/
public class RemoveDublicatesInSortedArrayInplace
{
    public static int RemoveDuplicates(int[] nums)
    {
        if (nums.Length < 2)
            return nums.Length;

        int p = 1;
        for (int i = 1; i < nums.Length; i++)
            if (nums[i] != nums[i - 1])
                nums[p++] = nums[i];

        return p;
    }

    [Theory]
    [ClassData(typeof(RemoveDublicatesInSortedArrayInplaceTestData))]
    public void Tests(int[] nums, int countAfter)
    {
        int actual = RemoveDuplicates(nums);
        Assert.Equal(countAfter, actual);
    }

    public class RemoveDublicatesInSortedArrayInplaceTestData
        : TheoryDataContainer.TwoArg<int[], int>
    {
        public RemoveDublicatesInSortedArrayInplaceTestData()
        {
            Add([], 0);
            Add([1], 1);
            Add([1, 2, 3], 3);
            Add([1, 1, 2, 2, 3, 3], 3);
            Add([7, 7, 7, 7], 1);
            Add([0, 0, 1, 2, 3], 4);
            Add([1, 2, 3, 3, 3], 3);
            Add([1, 2, 2, 2, 3], 3);
            Add([1, 1, 2, 2, 2, 3, 4, 4, 5, 5, 5, 5, 6], 6);
        }
    }
}
