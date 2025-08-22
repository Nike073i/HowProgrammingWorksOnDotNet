using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.ValidParentheses;

public class Solution
{
    private static readonly Dictionary<char, char> pairs = new()
    {
        { '(', ')' },
        { '{', '}' },
        { '[', ']' },
    };

    public static bool IsValid(string s)
    {
        var stack = new Stack<char>();

        foreach (var c in s)
        {
            bool opening = pairs.TryGetValue(c, out char closing);
            if (opening)
                stack.Push(closing);
            else if (stack.Count == 0 || stack.Pop() != c)
                return false;
        }
        return stack.Count == 0;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string s, bool expected)
    {
        bool actual = Solution.IsValid(s);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.TwoArg<string, bool>
{
    public SolutionTestData()
    {
        Add("", true);
        Add("()", true);
        Add("{}", true);
        Add("[]", true);
        Add("()[]{}", true);
        Add("({[]})", true);
        Add("[{()}]", true);
        Add("({})", true);
        Add("[[[]]]", true);
        Add("{{{{}}}}", true);
        Add("((()))", true);
        Add("(){}[]", true);
        Add("({[]})", true);
        Add("[(){}]", true);
        Add("{([])}", true);
        Add("(([]){})", true);
        Add("{()[{}]}", true);
        Add("([{}])", true);
        Add("(((((())))))", true);
        Add("{[()()]}", true);
        Add("(", false);
        Add(")", false);
        Add("{", false);
        Add("]", false);
        Add("[", false);
        Add("}", false);
        Add("(]", false);
        Add("([)]", false);
        Add("{(})", false);
        Add("[{]}", false);
        Add("())", false);
        Add("()))", false);
        Add("{[}]", false);
        Add("([})", false);
        Add("))", false);
        Add("]]", false);
        Add("}}", false);
        Add(")])}", false);
        Add("(((", false);
        Add("[[[", false);
        Add("{{{", false);
        Add("({[", false);
        Add("()()()()()(", false);
        Add("{[()]}}", false);
        Add("((((((())", false);
        Add("{[()]}{", false);
        Add("(", false);
        Add(")", false);
        Add("[", false);
        Add("]", false);
        Add("{", false);
        Add("}", false);
        Add("(}", false);
        Add("{)", false);
        Add("[)", false);
        Add("(]", false);
        Add("[}", false);
        Add("{]", false);
        Add("((((((((((()))))))))))", true);
        Add("{{{{{{{{{{}}}}}}}}}}", true);
        Add("[[[[[[[[[[]]]]]]]]]]", true);
        Add("()({[]})[]", true);
        Add("[{()()}{}]", true);
        Add("({[()]})", true);
        Add("([)]", false);
        Add("{[()}]", false);
        Add("({[}])", false);
        Add("(){}[](){}[]", true);
        Add("({)}", false);
        Add("[{()}]", true);
        Add("((((((((((((((((((((()))))))))))))))))))))", true);
        Add("((())", false);
        Add("({[]}", false);
        Add("()()(", false);
        Add("()((()))", true);
        Add("{}[{()}]", true);
        Add("[()]{}{()()}", true);
        Add("())))", false);
        Add("[]]]]", false);
        Add("(((()", false);
        Add("{{{{}", false);
        Add("[[[[]", false);
    }
}
