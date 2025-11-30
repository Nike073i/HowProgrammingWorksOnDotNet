namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.ValidRoundParentheses;

public class Solution
{
    public static bool IsValid(string input)
    {
        int balance = 0;

        foreach (var c in input)
        {
            if (c == '(')
                balance++;
            else if (balance == 0)
                return false;
            else
                balance--;
        }
        return balance == 0;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsValid(string input, bool expected)
    {
        bool actual = Solution.IsValid(input);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<string, bool>
{
    public SolutionTestData()
    {
        Add("()", true);
        Add("(())", true);
        Add("()()", true);
        Add("((()))", true);
        Add("(()())", true);
        Add("", true);
        Add("(", false);
        Add(")", false);
        Add("())", false);
        Add("(()", false);
        Add(")(", false);
        Add("((())", false);
        Add("()))", false);
        Add("((()())(()))", true);
        Add("()()()()()()", true);
        Add("(((((((", false);
        Add("))))))", false);
        Add("()()()())", false);
    }
}
