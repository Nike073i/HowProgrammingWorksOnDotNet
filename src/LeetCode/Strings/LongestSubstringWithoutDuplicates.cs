namespace HowProgrammingWorksOnDotNet.LeetCode.Strings.LongestSubstringWithoutDuplicates;

/*
    leetcode: 3 https://leetcode.com/problems/longest-substring-without-repeating-characters/submissions/1841057302/
    time: O(n)
    memory: O(k), где k - размер алфавита. Если только en-lowercase - O(1)
*/
public class Solution
{
    public static int LengthOfLongestSubstring(string s)
    {
        var hashSet = new HashSet<char>();
        int max = 0;
        int l = 0,
            r = -1;
        while (l < s.Length)
        {
            if (r + 1 < s.Length && hashSet.Add(s[r + 1]))
                r++;
            else
            {
                max = Math.Max(max, r - l + 1);
                hashSet.Remove(s[l]);
                l++;
            }
        }
        return max;
    }
}

public class SolutionTestData : TheoryData<string, int>
{
    public SolutionTestData()
    {
        Add("abcabcbb", 3);
        Add("bbbbb", 1);
        Add("pwwkew", 3);
        Add("", 0);
        Add("a", 1);
        Add("abcdef", 6);
        Add("abcabcabc", 3);
        Add("abacabacab", 3);
        Add("abcdefaaa", 6);
        Add("aaabcdef", 6);
        Add("aaaabcdefaaa", 6);
        Add("a!b@c#a!b@", 6);
        Add("a1b2c3a1b2", 6);
        Add("abc def ghi", 7);
        Add("абвгдабв", 5);
        Add(string.Join("", Enumerable.Repeat("abc", 100)), 3);
        Add(new string(Enumerable.Range(0, 1000).Select(i => (char)i).ToArray()), 1000);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(string s, int expected)
    {
        var actual = Solution.LengthOfLongestSubstring(s);
        Assert.Equal(expected, actual);
    }
}
