namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.BinarySearchInRotatedSortedArray;

/*
    leetcode: 33 https://leetcode.com/problems/search-in-rotated-sorted-array
    time: O(logn)
    memory: O(1)
    notes:
    - step 1 - Ищем оффсет при помощи бинарного поиска. Сдвинутые элементы - больше последнего элемента.
    - step 2 - Используемый обычный бинарный поиск по "скорректированному" массиву. Используем корректировку индекса при помощи `(offset + i) % length`
*/
public class Solution
{
    public static int Search(int[] nums, int target)
    {
        static (int, int) BinarySearch(int l, int r, Predicate<int> isGood)
        {
            while (r - l > 1)
            {
                int middle = l + (r - l) / 2;
                if (isGood(middle))
                    l = middle;
                else
                    r = middle;
            }
            return (l, r);
        }

        bool IsGreaterThanLast(int index) => nums[index] > nums[^1];
        (int _, int offset) = BinarySearch(l: -1, r: nums.Length, isGood: IsGreaterThanLast);

        int GetIndexByOffset(int index) => (offset + index) % nums.Length;
        bool IsLessOrEqualThanTarget(int index) => nums[GetIndexByOffset(index)] <= target;

        (int l, _) = BinarySearch(l: 0, r: nums.Length, isGood: IsLessOrEqualThanTarget);

        int resultIndex = GetIndexByOffset(l);
        return nums[resultIndex] == target ? resultIndex : -1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSearch(int[] nums, int target, int expected)
    {
        int actual = Solution.Search(nums, target);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int, int>
{
    public SolutionTestData()
    {
        Add([4, 5, 6, 7, 0, 1, 2], 0, 4);
        Add([4, 5, 6, 7, 0, 1, 2], 3, -1);
        Add([1], 0, -1);
        Add([1], 1, 0);
        Add([3, 1], 1, 1);
        Add([3, 1], 3, 0);
        Add([1, 3], 1, 0);
        Add([5, 1, 3], 5, 0);
        Add([5, 1, 3], 3, 2);
        Add([4, 5, 6, 7, 8, 1, 2, 3], 8, 4);
        Add([1, 3], 2, -1);
        Add([6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 0, 1, 2, 3, 4, 5], 15, 9);
        Add([6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 0, 1, 2, 3, 4, 5], 3, 18);
    }
}
