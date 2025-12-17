namespace HowProgrammingWorksOnDotNet.LeetCode.BruteforceAndBacktracing.CombinationSum;

/*
    leetcode: 39 https://leetcode.com/problems/combination-sum
*/
public class Solution
{
    public static IList<IList<int>> CombinationSum(int[] candidates, int target)
    {
        var output = new List<IList<int>>();
        var curr = new Stack<int>();
        TryCandidate(0, 0);
        return output;

        void TryCandidate(int i, int sum)
        {
            if (sum == target)
            {
                output.Add([.. curr]);
                return;
            }

            if (i >= candidates.Length || sum > target)
                return;

            TryCandidate(i + 1, sum);

            if (sum + candidates[i] <= target)
            {
                curr.Push(candidates[i]);
                TryCandidate(i, sum + candidates[i]);
                curr.Pop();
            }
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCombinationSum(int[] candidates, int target, IList<IList<int>> expected)
    {
        var actual = Solution.CombinationSum(candidates, target);

        var sortedExpected = expected
            .Select(list => list.Order())
            .OrderBy(list => string.Join(",", list));

        var sortedActual = actual
            .Select(list => list.Order())
            .OrderBy(list => string.Join(",", list));

        Assert.True(
            sortedExpected.Zip(sortedActual).All(tuple => tuple.First.SequenceEqual(tuple.Second))
        );
    }
}

public class SolutionTestData : TheoryData<int[], int, IList<IList<int>>>
{
    public SolutionTestData()
    {
        Add(
            [2, 3, 6, 7],
            7,
            [
                [2, 2, 3],
                [7],
            ]
        );

        Add(
            [2, 3, 5],
            8,
            [
                [2, 2, 2, 2],
                [2, 3, 3],
                [3, 5],
            ]
        );

        Add([2], 1, []);

        Add(
            [5],
            5,
            [
                [5],
            ]
        );

        Add([], 7, []);

        Add(
            [1, 2, 3],
            0,
            [
                [],
            ]
        );

        Add(
            [10, 20, 30],
            50,
            [
                [10, 10, 10, 10, 10],
                [10, 10, 10, 20],
                [10, 20, 20],
                [10, 10, 30],
                [20, 30],
            ]
        );

        Add(
            [1, 2, 3, 4],
            6,
            [
                [1, 1, 1, 1, 1, 1],
                [1, 1, 1, 1, 2],
                [1, 1, 1, 3],
                [1, 1, 2, 2],
                [1, 1, 4],
                [1, 2, 3],
                [2, 2, 2],
                [2, 4],
                [3, 3],
            ]
        );
    }
}
