using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.PolishNotation;

public class Solution
{
    public static int EvalRPN(string[] tokens)
    {
        var stack = new Stack<int>();

        foreach (var token in tokens)
        {
            if (ItOperator(token))
            {
                var right = stack.Pop();
                var left = stack.Pop();
                stack.Push(Eval(left, right, token));
            }
            else
                stack.Push(int.Parse(token));
        }
        return stack.Pop();
    }

    private static bool ItOperator(string token) =>
        token == "+" || token == "-" || token == "/" || token == "*";

    private static int Eval(int left, int right, string op) =>
        op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            _ => throw new Exception(),
        };
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string[] tokens, int expected)
    {
        int actual = Solution.EvalRPN(tokens);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<string[], int>
{
    public SolutionTestData()
    {
        Add(["2", "1", "+"], 3);
        Add(["2", "1", "-"], 1);
        Add(["2", "3", "*"], 6);
        Add(["6", "2", "/"], 3);
        Add(["-2", "3", "+"], 1);
        Add(["5", "-3", "-"], 8);
        Add(["-4", "-2", "*"], 8);
        Add(["7", "2", "/"], 3);
        Add(["9", "4", "/"], 2);
        Add(["-7", "2", "/"], -3);
        Add(["7", "-2", "/"], -3);
        Add(["2", "1", "+", "3", "*"], 9);
        Add(["4", "13", "5", "/", "+"], 6);
        Add(["10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+"], 22);
        Add(["3", "4", "+", "2", "*"], 14);
        Add(["3", "4", "2", "*", "+"], 11);
        Add(["3", "4", "+", "2", "/"], 3);
        Add(["1000", "500", "+"], 1500);
        Add(["1000000", "2", "*"], 2000000);
        Add(["5", "1", "/"], 5);
        Add(["-5", "1", "/"], -5);
        Add(["5", "0", "*"], 0);
        Add(["0", "5", "*"], 0);
        Add(["-5", "0", "*"], 0);
        Add(["5", "0", "+"], 5);
        Add(["0", "5", "+"], 5);
        Add(["5", "0", "-"], 5);
        Add(["0", "5", "-"], -5);
        Add(["4", "3", "-", "2", "*", "5", "+"], 7);
        Add(["5", "1", "2", "+", "4", "*", "+", "3", "-"], 14);
        Add(["42"], 42);
        Add(["-42"], -42);
        Add(["0"], 0);
        Add(["1", "2", "3", "+", "+"], 6);
        Add(["8", "4", "2", "/", "/"], 4);
        Add(["2", "3", "4", "*", "*"], 24);
        Add(["100", "50", "/", "2", "/", "1", "/"], 1);
        Add(["15", "7", "1", "1", "+", "-", "/", "3", "*", "2", "1", "1", "+", "+", "-"], 5);
        Add(["-10", "-2", "/"], 5);
        Add(["10", "-2", "/"], -5);
        Add(["-10", "2", "/"], -5);
        Add(["5", "3", "-", "2", "*"], 4);
        Add(["10", "6", "-", "3", "/"], 1);
        Add(["20", "10", "/", "3", "*", "5", "-", "2", "+"], 3);
        Add(["18", "3", "/", "2", "*", "6", "+"], 18);
        Add(["12", "4", "/", "3", "*", "7", "-"], 2);
        Add(["1", "2", "+", "3", "+", "4", "+", "5", "+"], 15);
        Add(["64", "2", "/", "2", "/", "2", "/", "2", "/"], 4);
        Add(["9", "2", "/"], 4);
        Add(["11", "3", "/"], 3);
        Add(["17", "5", "/"], 3);
        Add(["0", "0", "+"], 0);
        Add(["0", "0", "-"], 0);
        Add(["0", "0", "*"], 0);
        Add(["3", "4", "5", "*", "+"], 23);
        Add(["3", "4", "+", "5", "*"], 35);
    }
}
