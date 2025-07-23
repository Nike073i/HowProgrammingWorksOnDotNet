using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

/*
    Предполагается, что путь **существует**
    В каждом интервале ищем следующий интвервал (next), который покрывает больше всех
    Как только текущий интервал подходит к концу - делаем "прыжок", next становится текущим интервалом
*/
public class FindMinimalWayByJumping
{
    public static int Jump(int[] nums)
    {
        if (nums.Length < 2)
            return 0;

        int start = 0;
        int next = 0;
        int count = 1;

        for (int i = 1; i < nums.Length && start + nums[start] < nums.Length - 1; i++)
        {
            if (i > start + nums[start])
            {
                count++;
                start = next;
            }
            if (i + nums[i] > nums[next] + next)
                next = i;
        }

        return count;
    }
}

public class FindMinimalWayByJumpingTests
{
    [Theory]
    [ClassData(typeof(FindMinimalWayByJumpingTestData))]
    public void Usage(int[] nums, int expected)
    {
        int actual = FindMinimalWayByJumping.Jump(nums);
        Assert.Equal(expected, actual);
    }
}

public class FindMinimalWayByJumpingTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public FindMinimalWayByJumpingTestData()
    {
        Add([], 0);
        Add([1], 0);
        Add([2, 3, 1, 1, 4], 2);
        Add([2, 3, 0, 1, 4], 2);
        Add([1, 2, 3], 2);
        Add([1, 1, 1, 1], 3);
        Add([5, 1, 1, 1, 1, 1, 1], 2);
        Add([1, 2, 1, 1, 1], 3);
        Add([7, 0, 9, 6, 9, 6, 1, 7, 9, 0, 1, 2, 9, 0, 3], 2);
        Add([1, 3, 2], 2);
    }
}
