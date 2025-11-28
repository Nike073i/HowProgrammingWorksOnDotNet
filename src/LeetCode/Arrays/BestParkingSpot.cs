namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.BestParkingSpot;

/*
    leetcode: 849 https://leetcode.com/problems/maximize-distance-to-closest-person/description/
    time: O(n)
    memory: O(1)
*/
public class Solution
{
    public static int MaxDistance(int[] spots)
    {
        int l = 0,
            r = 0;
        int max = 0;

        int FreeSpotsCount(int r, int l) =>
            l == 0 || r == spots.Length - 1 ? r - l + 1 : (r - l + 2) / 2;

        while (r < spots.Length)
        {
            if (r + 1 == spots.Length || spots[r + 1] != spots[r])
            {
                if (spots[l] == 0)
                    max = Math.Max(max, FreeSpotsCount(r, l));

                l = r + 1;
                r = l;
            }
            else
                r++;
        }
        return max;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMaxDistance(int[] spots, int expected)
    {
        int actual = Solution.MaxDistance(spots);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int>
{
    public SolutionTestData()
    {
        Add([1, 0, 0, 0, 1, 0, 1], 2);
        Add([1, 0, 0, 0], 3);
        Add([0, 0, 0, 1], 3);
        Add([1, 1, 1, 1], 0);
        Add([0, 0, 0, 0], 4);
        Add([0], 1);
        Add([1], 0);
        Add([0, 0], 2);
        Add([1, 0], 1);
        Add([0, 1], 1);
        Add([1, 1], 0);
        Add([1, 0, 1, 0, 1], 1);
        Add([0, 1, 0, 1, 0], 1);
        Add([1, 0, 0, 1, 0, 0, 0, 1], 2);
        Add([1, 0, 0, 1, 0, 0, 1], 1);
        Add([0, 0, 1, 0, 0, 0, 0], 4);
    }
}
