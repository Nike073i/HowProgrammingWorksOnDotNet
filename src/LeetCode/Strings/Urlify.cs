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

    public static void Unurlify(char[] url)
    {
        int slow = 0,
            fast = 0;

        while (fast < url.Length)
        {
            if (fast + 2 < url.Length && url[fast..(fast + 3)].SequenceEqual(['%', '2', '0']))
            {
                url[slow++] = ' ';
                fast += 3;
            }
            else
                url[slow++] = url[fast++];
        }
        while (slow < url.Length)
            url[slow++] = '#';
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(UrlifyTestData))]
    public void TestUrlify(char[] input, int textLength, char[] expected)
    {
        Solution.Urlify(input, textLength);
        Assert.Equal(expected, input);
    }

    [Theory]
    [ClassData(typeof(UnurlifyTestData))]
    public void TestUnurlify(char[] url, char[] expected)
    {
        Solution.Unurlify(url);
        Assert.Equal(expected, url);
    }
}

public class UrlifyTestData : TheoryData<char[], int, char[]>
{
    public UrlifyTestData()
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

public class UnurlifyTestData : TheoryData<char[], char[]>
{
    public UnurlifyTestData()
    {
        Add(
            ['M', 'r', '%', '2', '0', 'J', 'o', 'h', 'n', '#'],
            ['M', 'r', ' ', 'J', 'o', 'h', 'n', '#', '#', '#']
        );
        Add(
            ['a', '%', '2', '0', 'b', '%', '2', '0', 'c'],
            ['a', ' ', 'b', ' ', 'c', '#', '#', '#', '#']
        );
        Add(['%', '2', '0', 'a', 'b'], [' ', 'a', 'b', '#', '#']);
        Add(['a', 'b', '%', '2', '0'], ['a', 'b', ' ', '#', '#']);
        Add(['a', 'b', 'c', '#', '#'], ['a', 'b', 'c', '#', '#']);
        Add(['%', '2', '0', '%', '2', '0', '#'], [' ', ' ', '#', '#', '#', '#', '#']);
    }
}
