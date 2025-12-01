namespace HowProgrammingWorksOnDotNet.LeetCode.BruteforceAndBacktracing.PhoneNumber;

/*
    leetcode: 17 https://leetcode.com/problems/letter-combinations-of-a-phone-number/
    time: O(4^n) // O(n * 4^n)
    memory: O(n * 4^n)
*/
public class Solution
{
    private static readonly Dictionary<char, string> _digit2letters = new()
    {
        { '2', "abc" },
        { '3', "def" },
        { '4', "ghi" },
        { '5', "jkl" },
        { '6', "mno" },
        { '7', "pqrs" },
        { '8', "tuv" },
        { '9', "wxyz" },
    };

    public static void AddLetters(List<string> output, string prefix, string digits, int index)
    {
        if (index == digits.Length)
        {
            output.Add(prefix);
            return;
        }

        foreach (var l in _digit2letters[digits[index]])
            AddLetters(output, prefix + l, digits, index + 1);
    }

    public static IList<string> LetterCombinations(string digits)
    {
        var output = new List<string>();
        AddLetters(output, "", digits, 0);
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestLetterCombinations(string digits, List<string> expected)
    {
        var actual = Solution.LetterCombinations(digits);

        var sortedExpected = expected.Order();
        var sortedActual = actual.Order();

        Assert.True(sortedExpected.SequenceEqual(sortedActual));
    }
}

public class SolutionTestData : TheoryData<string, List<string>>
{
    public SolutionTestData()
    {
        Add("23", ["ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf"]);
        Add("2", ["a", "b", "c"]);
        Add("3", ["d", "e", "f"]);
        Add("7", ["p", "q", "r", "s"]);
        Add("9", ["w", "x", "y", "z"]);
        Add("34", ["dg", "dh", "di", "eg", "eh", "ei", "fg", "fh", "fi"]);
        Add(
            "79",
            [
                "pw",
                "px",
                "py",
                "pz",
                "qw",
                "qx",
                "qy",
                "qz",
                "rw",
                "rx",
                "ry",
                "rz",
                "sw",
                "sx",
                "sy",
                "sz",
            ]
        );
        Add(
            "234",
            [
                "adg",
                "adh",
                "adi",
                "aeg",
                "aeh",
                "aei",
                "afg",
                "afh",
                "afi",
                "bdg",
                "bdh",
                "bdi",
                "beg",
                "beh",
                "bei",
                "bfg",
                "bfh",
                "bfi",
                "cdg",
                "cdh",
                "cdi",
                "ceg",
                "ceh",
                "cei",
                "cfg",
                "cfh",
                "cfi",
            ]
        );

        Add("22", ["aa", "ab", "ac", "ba", "bb", "bc", "ca", "cb", "cc"]);
        Add(
            "777",
            [
                "ppp",
                "ppq",
                "ppr",
                "pps",
                "pqp",
                "pqq",
                "pqr",
                "pqs",
                "prp",
                "prq",
                "prr",
                "prs",
                "psp",
                "psq",
                "psr",
                "pss",
                "qpp",
                "qpq",
                "qpr",
                "qps",
                "qqp",
                "qqq",
                "qqr",
                "qqs",
                "qrp",
                "qrq",
                "qrr",
                "qrs",
                "qsp",
                "qsq",
                "qsr",
                "qss",
                "rpp",
                "rpq",
                "rpr",
                "rps",
                "rqp",
                "rqq",
                "rqr",
                "rqs",
                "rrp",
                "rrq",
                "rrr",
                "rrs",
                "rsp",
                "rsq",
                "rsr",
                "rss",
                "spp",
                "spq",
                "spr",
                "sps",
                "sqp",
                "sqq",
                "sqr",
                "sqs",
                "srp",
                "srq",
                "srr",
                "srs",
                "ssp",
                "ssq",
                "ssr",
                "sss",
            ]
        );
    }
}
