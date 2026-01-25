namespace HowProgrammingWorksOnDotNet.LeetCode.BruteforceAndBacktracing.RestoreIp;

/*
    leetcode: https://leetcode.com/problems/restore-ip-addresses/description
    time: O(1)
    memory: O(1)
*/
public class Solution
{
    public static IList<string> RestoreIpAddresses(string s)
    {
        var result = new List<string>();
        var octets = new Stack<int>();
        RestoreIp(s);
        return result;

        void RestoreIp(string s)
        {
            if (octets.Count == 4 && string.IsNullOrEmpty(s))
            {
                var tmp = octets.ToArray();
                result.Add(string.Join(".", tmp.Reverse()));
                return;
            }
            if (octets.Count == 4 || string.IsNullOrEmpty(s))
            {
                return;
            }

            if (s[0] == '0')
            {
                Track(0, s[1..]);
                return;
            }

            for (int i = 0, n = 0; i < Math.Min(s.Length, 3); i++)
            {
                n = n * 10 + s[i] - '0';
                if (n > 255)
                    return;

                Track(n, s[(i + 1)..]);
            }
        }

        void Track(int n, string s)
        {
            octets.Push(n);
            RestoreIp(s);
            octets.Pop();
        }
    }
}
