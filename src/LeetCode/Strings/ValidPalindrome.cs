using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

/*
    leetcode: 125 https://leetcode.com/problems/valid-palindrome/description/
    time: O(n)
    memory: O(1)
*/
public class ValidPalindrome
{
    public static bool IsPalindrome(string s)
    {
        int left = 0;
        int right = s.Length - 1;

        while (left < right)
        {
            if (!char.IsLetterOrDigit(s[left]))
                left++;
            else if (!char.IsLetterOrDigit(s[right]))
                right--;
            else if (char.ToLowerInvariant(s[right]) != char.ToLowerInvariant(s[left]))
                return false;
            else
            {
                left++;
                right--;
            }
        }
        return true;
    }
}

public class ValidPalindromeTests
{
    [Theory]
    [ClassData(typeof(ValidPalindromeTestData))]
    public void Test(string s, bool expected)
    {
        bool actual = ValidPalindrome.IsPalindrome(s);
        Assert.Equal(expected, actual);
    }
}

public class ValidPalindromeTestData : TheoryDataContainer.TwoArg<string, bool>
{
    public ValidPalindromeTestData()
    {
        Add("A man, a plan, a canal: Panama", true);
        Add("", true);
        Add("a", true);
        Add("racecar", true);
        Add("RaceCar", true);
        Add("hello", false);
        Add("12321", true);
        Add("12345", false);
        Add("!@#$%^&*()", true);
        Add("No 'x' in Nixon", true);
        Add("A Toyota's a Toyota", true);
        Add("    ", true);
        Add("racecarx", false);
        Add("Was it a car or a cat I saw", true);
        Add("!@#a#@!", true);
        Add("A1B2B1A", true);
    }
}
