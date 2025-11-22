namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.Urlify;

public class Solution
{
    public static void Urlify(char[] text, int textLength)
    {
        int slow = text.Length - 1,
            fast = textLength - 1;

        while (fast >= 0)
        {
            if (text[fast] == ' ')
            {
                text[slow--] = '0';
                text[slow--] = '2';
                text[slow--] = '%';
                fast--;
            }
            else
            {
                text[slow--] = text[fast--];
            }
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestUrlify(char[] input, int textLength, char[] expected)
    {
        Solution.Urlify(input, textLength);
        Assert.Equal(expected, input);
    }
}

public class SolutionTestData : TheoryData<char[], int, char[]>
{
    public SolutionTestData()
    {
        Add(
            ['M', 'r', ' ', 'J', 'o', 'h', 'n', '#', '#', '#'],
            8,
            ['M', 'r', '%', '2', '0', 'J', 'o', 'h', 'n', '#']
        );
        Add(
            ['a', ' ', 'b', ' ', 'c', '#', '#', '#', '#'],
            5,
            ['a', '%', '2', '0', 'b', '%', '2', '0', 'c']
        );
        Add([' ', 'a', 'b', '#', '#'], 3, ['%', '2', '0', 'a', 'b']);
        Add(['a', 'b', ' ', '#', '#'], 3, ['a', 'b', '%', '2', '0']);
        Add(['a', 'b', 'c', '#', '#'], 5, ['a', 'b', 'c', '#', '#']);
        Add([' ', ' ', '#', '#', '#', '#', '#'], 3, ['%', '2', '0', '%', '2', '0', '#']);
    }
}
