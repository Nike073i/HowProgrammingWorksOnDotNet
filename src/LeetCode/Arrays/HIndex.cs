using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.HIndex;

/*
    leetcode: 274 https://leetcode.com/problems/h-index/description/
    time: O(N) для ByCount.
    memory: O(N) для ByCount.
*/
public class HIndex
{
    public static int CalculateBySort(int[] citations)
    {
        var sortedCitations = citations.Order().ToArray();

        for (int i = 0; i < sortedCitations.Length; i++)
        {
            if (sortedCitations[i] >= sortedCitations.Length - i)
                return sortedCitations.Length - i;
        }
        return 0;
    }

    public static int CalculateBySortReverse(int[] citations)
    {
        int[] sortedCitations = [.. citations.Order()];
        int index = 0;

        /*
            for (int i = sortedCitations.Length - 1; i >= 0 && sortedCitations[i] > index; i--, index++) { }
        */

        for (int i = sortedCitations.Length - 1; i >= 0; i--)
        {
            if (sortedCitations[i] > index)
                index++;
            else
                return index;
        }
        return index;
    }

    public static int CalculateByCount(int[] citations)
    {
        int publications = citations.Length;
        int[] counts = new int[publications + 1];

        foreach (int count in citations)
            counts[Math.Min(count, publications)]++;

        int acc = 0;
        for (int i = counts.Length - 1; i >= 0; i--)
        {
            acc += counts[i];
            if (acc >= i)
                return i;
        }

        return acc;
    }
}

public class HIndexTests
{
    [Theory]
    [ClassData(typeof(HIndexTestData))]
    public void TestBySort(int[] citations, int expected)
    {
        int actual = HIndex.CalculateBySort(citations);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(HIndexTestData))]
    public void TestReverse(int[] citations, int expected)
    {
        int actual = HIndex.CalculateBySortReverse(citations);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(HIndexTestData))]
    public void TestByCount(int[] citations, int expected)
    {
        int actual = HIndex.CalculateByCount(citations);
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
