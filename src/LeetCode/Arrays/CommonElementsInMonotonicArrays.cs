namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.CommonElementsInMonotonicArrays;

/*
    task: Получить общие элементы двух неубывающих коллекций
    time: O(n + m)
    memory: O(n)
*/
public class Solution
{
    public static List<int> GetCommon(int[] a, int[] b)
    {
        var output = new List<int>();

        int first = 0,
            second = 0;

        while (first < a.Length && second < b.Length)
        {
            if (a[first] == b[second])
            {
                output.Add(a[first]);
                first++;
                second++;
            }
            else if (a[first] > b[second])
                second++;
            else
                first++;
        }

        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] a, int[] b, List<int> expected)
    {
        var actual = Solution.GetCommon(a, b);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int[], List<int>>
{
    public SolutionTestData()
    {
        Add([-3, 0, 2, 4, 7, 8, 9], [1, 2, 3, 4, 5], [2, 4]);
        Add([1, 2, 3], [4, 5, 6], []);
        Add([1, 2, 3, 4], [1, 2, 3, 4], [1, 2, 3, 4]);
        Add([], [1, 2, 3], []);
        Add([1, 2, 3], [], []);
        Add([], [], []);
        Add([5], [5], [5]);
        Add([1, 3, 5], [2, 4, 5], [5]);
        Add([10, 20, 30], [1, 2, 3], []);
        Add([1, 2, 3], [10, 20, 30], []);
        Add([1, 1, 2, 2, 3], [1, 2, 3], [1, 2, 3]);
        Add([1, 2, 3], [1, 1, 2, 2, 3], [1, 2, 3]);
        Add([-5, -3, -1, 0, 2], [-4, -3, -1, 1, 2], [-3, -1, 2]);
        Add([1, 3, 5, 7, 9], [2, 3, 4, 5, 6, 8], [3, 5]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [2, 4, 6, 8, 10], [2, 4, 6, 8, 10]);
        Add([1, 10, 20, 30, 40], [5, 10, 15, 20, 25], [10, 20]);
    }
}
