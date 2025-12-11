namespace HowProgrammingWorksOnDotNet.LeetCode.BruteforceAndBacktracing.GenerateParenthesis;

/*
    leetcode: 22 https://leetcode.com/problems/generate-parentheses/description/
    time: O(n * 2^(2*n)) == O(n*4^n). n за счет конкатенации
    memory: O(n * 4^n)
*/
public class Solution
{
    public static IList<string> GenerateParenthesis(int n)
    {
        IList<string> result = [];
        Generate(result, n * 2, "(", 1);
        return result;
    }

    private static void Generate(IList<string> list, int n, string prefix, int balance)
    {
        if (prefix.Length == n)
        {
            list.Add(prefix);
            return;
        }

        if (balance == 0)
            Generate(list, n, prefix + "(", balance + 1);
        else if (n - prefix.Length == balance)
            Generate(list, n, prefix + ")", balance - 1);
        else
        {
            Generate(list, n, prefix + ")", balance - 1);
            Generate(list, n, prefix + "(", balance + 1);
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestGenerateParenthesis(int n, List<string> expected)
    {
        var actual = Solution.GenerateParenthesis(n);
        Assert.True(expected.Order().SequenceEqual(actual.Order()));
    }
}

public class SolutionTestData : TheoryData<int, List<string>>
{
    public SolutionTestData()
    {
        Add(1, ["()"]);
        Add(2, ["(())", "()()"]);
        Add(3, ["((()))", "(()())", "(())()", "()(())", "()()()"]);
        Add(
            4,
            [
                "(((())))",
                "((()()))",
                "((())())",
                "((()))()",
                "(()(()))",
                "(()()())",
                "(()())()",
                "(())(())",
                "(())()()",
                "()((()))",
                "()(()())",
                "()(())()",
                "()()(())",
                "()()()()",
            ]
        );
    }
}
