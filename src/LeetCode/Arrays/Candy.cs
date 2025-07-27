using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;
using Microsoft.AspNetCore.RateLimiting;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class Candy
{
    public static int Solution(int[] ratings)
    {
        int length = ratings.Length;
        int[] candies = new int[length];
        Array.Fill(candies, 1);
        for (int i = 1; i < length; i++)
            if (ratings[i] > ratings[i - 1])
                candies[i] = candies[i - 1] + 1;

        int counter = 0;
        for (int i = length - 1; i > 0; i--)
        {
            if (ratings[i - 1] > ratings[i])
                candies[i - 1] = Math.Max(candies[i - 1], candies[i] + 1);
            counter += candies[i - 1];
        }
        counter += candies[^1];
        return counter;
    }
}

public class CandyTests
{
    [Theory]
    [ClassData(typeof(CandyTestData))]
    public void Test(int[] ratings, int expected)
    {
        int actual = Candy.Solution(ratings);
        Assert.Equal(expected, actual);
    }
}

public class CandyTestData : TheoryDataContainer.TwoArg<int[], int>
{
    public CandyTestData()
    {
        Add([0], 1);
        Add([1, 0, 2], 5);
        Add([1, 2, 2], 4);
        Add([1, 2, 3, 4], 10);
        Add([4, 3, 2, 1], 10);
        Add([3, 3, 3, 3], 4);
        Add([1, 2, 3, 2, 1], 9);
        Add([3, 2, 1, 2, 3], 11);
        Add([1, 3, 2, 1, 2, 3, 1], 13);
        Add([1, 1, 2, 1, 1], 6);
        Add([1, 2, 2, 1], 6);
        Add([4, 3, 2, 1, 2, 3], 15);
        Add([1, 2, 1, 3, 1, 4], 9);
        Add([5, 4, 3, 2, 3], 12);
        Add([1, 2, 3, 3, 3, 2, 1], 13);
        Add([0, 1, 2, 3], 10);
        Add([3, 2, 1, 0], 10);
        Add([1, 1, 5, 1, 1], 6);
        Add([1, 5, 3, 2, 6, 4, 3], 13);
        Add([2, 2], 2);
        Add([1, 2], 3);
        Add([1, 2, 1, 2, 1, 2], 9);
    }
}
