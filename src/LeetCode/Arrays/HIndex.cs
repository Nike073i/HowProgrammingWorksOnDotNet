using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class HIndex
{
    public static int Calculate(int[] citations)
    {
        var sortedCitations = citations.Order().ToArray();

        for (int i = 0; i < sortedCitations.Length; i++)
        {
            if (sortedCitations[i] >= sortedCitations.Length - i)
                return sortedCitations.Length - i;
        }
        return 0;
    }
}

public class HIndexTests
{
    [Theory]
    [ClassData(typeof(HIndexTestData))]
    public void Tests(int[] citations, int expected)
    {
        int actual = HIndex.Calculate(citations);
        Assert.Equal(expected, actual);
    }
}

public class HIndexTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public HIndexTestData()
    {
        Add([], 0);
        Add([0], 0);
        Add([1], 1);
        Add([0, 0], 0);
        Add([1, 1], 1);
        Add([1, 2, 2], 2);
        Add([3, 0, 6, 1, 5], 3);
        Add([100], 1);
        Add([0, 0, 0, 0], 0);
        Add([1, 1, 1, 1], 1);
        Add([4, 4, 4, 4], 4);
        Add([1, 2, 3, 4, 5], 3);
        Add([10, 10, 10, 10, 10], 5);
        Add([1, 4, 1, 4, 2, 1, 3, 5, 6], 4);
        Add([0, 1, 2, 3, 4, 5, 6, 7, 8], 4);
        Add([0, 0, 0, 1, 1], 1);
        Add([0, 0, 1, 1, 1], 1);
        Add([0, 1, 1, 1, 1], 1);
        Add([1, 1, 1, 1, 1], 1);
    }
}
