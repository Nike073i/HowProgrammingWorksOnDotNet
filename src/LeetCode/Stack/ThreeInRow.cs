namespace HowProgrammingWorksOnDotNet.LeetCode.Stack.ThreeInRow;

/*
    task: Удалить все тройные элементы и вернуть кол-во удаленных
    time: O(n)
    memory: O(n)
    examples:
    - [ 1, 2, 2, 1, 1, 1, 2, 3, 3, 3, 3 ] -> [ 1, 3 ], 9
*/
public class Solution
{
    public static int Play(int[] nums)
    {
        var stack = new Stack<int>();

        foreach (var n in nums)
        {
            if (stack.Count > 1 && stack.Peek() == n)
            {
                var t1 = stack.Pop();
                var t2 = stack.Pop();
                if (t2 != n)
                {
                    stack.Push(t2);
                    stack.Push(t1);
                    stack.Push(n);
                }
            }
            else
                stack.Push(n);
        }
        return nums.Length - stack.Count;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestPlay(int[] nums, int expected)
    {
        int actual = Solution.Play(nums);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([1, 2, 2, 1, 1, 1, 2, 3, 3, 3, 3], 9);
        Add([1, 1, 1], 3);
        Add([1, 1, 1, 2, 2, 2], 6);
        Add([1, 2, 3, 4, 5], 0);
        Add([1, 1, 2, 2, 3, 3], 0);
        Add([1, 1, 1, 1, 1, 1], 6);
        Add([1, 1, 1, 1], 3);
        Add([1, 1, 1, 2, 1, 1, 1], 6);
        Add([1, 2, 1, 1, 1, 2, 1], 3);
        Add([], 0);
        Add([1], 0);
        Add([1, 1], 0);
        Add([1, 1, 1, 2, 2, 2, 3, 3, 3], 9);
        Add([1, 2, 1, 2, 1, 2], 0);
        Add([1, 1, 2, 2, 2, 1, 1], 6);
        Add([1, 1, 2, 1, 1, 1, 2, 1], 3);
    }
}
