namespace HowProgrammingWorksOnDotNet.LeetCode.Stack.DailyTemperatures;

/*
    leetcode: 739 https://leetcode.com/problems/daily-temperatures/
    time: O(n)
    memory: O(n)
    notes:
    - Применяется монотонно-убывающий стэк, поскольку нам нужно помнить только те дни, температура которых <= текущего. Если текущий > элемента стэка, то он ближайший "теплый".
    example: Описывается в виде [currentState] -- (addelement) --> [lessElements], [newState]
    - []                            -- (0,73) --> [],                    [ (0,73) ]
    - [ (0,73) ]                    -- (1,74) --> [ (0,73) ],            [ (1,74) ]
    - [ (1,74) ]                    -- (2,75) --> [ (1,74) ],            [ (2,75) ]
    - [ (2,75) ]                    -- (3,71) --> [],                    [ (2,75), (3,71) ]
    - [ (2,75), (3,71) ]            -- (4,69) --> [],                    [ (2,75), (3,71), (4,69) ]
    - [ (2,75), (3,71), (4,69) ]    -- (5,72) --> [ (4,69), (3,71) ],    [ (2,75), (5,72) ]
    - [ (2,75), (5,72) ]            -- (6,76) --> [ (5,72), (2,75) ],    [ (6,76) ]
    - [ (6,76) ]                    -- (7,73) --> [],                    [ (6,76), (7,73) ]

*/
public class Solution
{
    private class MonotonicallyDecreasingStack
    {
        private readonly Stack<(int, int)> _stack = new();

        public IEnumerable<(int, int)> Push(int index, int value)
        {
            while (_stack.Count != 0 && _stack.Peek().Item2 < value)
                yield return _stack.Pop();
            _stack.Push((index, value));
        }
    }

    public static int[] DailyTemperatures(int[] temperatures)
    {
        var stack = new MonotonicallyDecreasingStack();

        int[] output = new int[temperatures.Length];

        for (int i = 0; i < temperatures.Length; i++)
        {
            var less = stack.Push(i, temperatures[i]);
            foreach (var l in less)
            {
                output[l.Item1] = i - l.Item1;
            }
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestDailyTemperatures(int[] temperatures, int[] expected)
    {
        int[] actual = Solution.DailyTemperatures(temperatures);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int[], int[]>
{
    public SolutionTestData()
    {
        Add([73, 74, 75, 71, 69, 72, 76, 73], [1, 1, 4, 2, 1, 1, 0, 0]);
        Add([30, 40, 50, 60], [1, 1, 1, 0]);
        Add([30, 60, 90], [1, 1, 0]);
        Add([90, 80, 70, 60], [0, 0, 0, 0]);
        Add([70, 70, 70, 70], [0, 0, 0, 0]);
        Add([100], [0]);
        Add([0], [0]);
        Add([], []);
        Add([30, 20, 10, 40], [3, 2, 1, 0]);
        Add([10, 20, 30, 20, 10, 40], [1, 1, 3, 2, 1, 0]);
        Add([50, 40, 30, 40, 50, 60], [5, 3, 1, 1, 1, 0]);
        Add([int.MinValue, int.MaxValue], [1, 0]);
        Add([100, 99, 98, 97, 96, 100], [0, 4, 3, 2, 1, 0]);
    }
}
