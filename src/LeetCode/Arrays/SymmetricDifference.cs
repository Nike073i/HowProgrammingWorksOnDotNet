namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.SymmetricDifference;

/*
    time: O(n + m)
    memory: O(n + m)
*/
public class Solution
{
    public static List<int> FindDifference(int[] nums1, int[] nums2)
    {
        var output = new List<int>();

        int p1 = 0,
            p2 = 0;

        while (p1 < nums1.Length && p2 < nums2.Length)
        {
            if (nums1[p1] == nums2[p2])
            {
                p1++;
                p2++;
            }
            else if (nums1[p1] < nums2[p2])
                output.Add(nums1[p1++]);
            else
                output.Add(nums2[p2++]);
        }

        while (p1 < nums1.Length)
            output.Add(nums1[p1++]);
        while (p2 < nums2.Length)
            output.Add(nums2[p2++]);

        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSolution(int[] nums1, int[] nums2, List<int> expected)
    {
        var actual = Solution.FindDifference(nums1, nums2);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int[], List<int>>
{
    public SolutionTestData()
    {
        Add([1, 2, 3], [2, 4, 6], [1, 3, 4, 6]);
        Add([1, 2, 3], [1, 2, 3], []);
        Add([], [1, 2, 3], [1, 2, 3]);
        Add([1, 2, 3], [], [1, 2, 3]);
        Add([], [], []);
        Add([1, 3, 5], [2, 4, 6], [1, 2, 3, 4, 5, 6]);
        Add([-3, -2, -1], [-2, 0, 1], [-3, -1, 0, 1]);
        Add([100, 200], [200, 300], [100, 300]);
        Add([5], [5], []);
        Add([5], [6], [5, 6]);
        Add([1, 2], [2, 3], [1, 3]);
        Add([1, 3], [2, 4], [1, 2, 3, 4]);
        Add([1, 2, 3], [2, 4, 6], [1, 3, 4, 6]);
        Add([1, 2, 3, 4], [3, 4, 5, 6], [1, 2, 5, 6]);
        Add([1, 3, 5], [2, 4, 6], [1, 2, 3, 4, 5, 6]);
        Add([1, 2, 3], [1, 2, 3], []);
        Add([1, 2], [1, 2, 3, 4, 5], [3, 4, 5]);
        Add([1, 2, 3, 4, 5], [4, 5], [1, 2, 3]);
    }
}
