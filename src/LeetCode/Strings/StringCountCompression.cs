namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.StringCountCompression;

/*
    task: Run-Length Encoding Compession
    leetcode: 443 https://leetcode.com/problems/string-compression/
    memory: O(1)
    time: O(n)
*/
public class Solution
{
    public static int Compress(char[] chars)
    {
        int fast = 0,
            slow = 0,
            input = 0;

        while (fast < chars.Length)
        {
            if (fast + 1 == chars.Length || chars[fast] != chars[fast + 1])
            {
                int count = fast - slow + 1;
                chars[input++] = chars[slow];

                if (count > 1)
                {
                    foreach (var d in count.ToString())
                        chars[input++] = d;
                }

                slow = fast + 1;
                fast = slow;
            }
            else
                fast++;
        }

        return input;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCompress(char[] input, char[] expectedArray)
    {
        int actualLength = Solution.Compress(input);
        Assert.Equal(expectedArray, input[..actualLength]);
    }
}

public class SolutionTestData : TheoryData<char[], char[]>
{
    public SolutionTestData()
    {
        Add(['a', 'a', 'b', 'b', 'c', 'c', 'c'], ['a', '2', 'b', '2', 'c', '3']);
        Add(['a'], ['a']);
        Add(
            ['a', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b', 'b'],
            ['a', 'b', '1', '2']
        );
        Add(['a', 'b', 'c'], ['a', 'b', 'c']);
        Add(['a', 'a', 'a', 'b', 'b', 'a', 'a'], ['a', '3', 'b', '2', 'a', '2']);
        Add([], []);
        Add(['a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a', 'a'], ['a', '1', '0']);
        Add(['a', 'a', 'a', 'b', 'b', 'c', 'c', 'c', 'c'], ['a', '3', 'b', '2', 'c', '4']);
        Add(['x', 'x', 'y', 'y', 'z', 'z', 'z'], ['x', '2', 'y', '2', 'z', '3']);
    }
}
