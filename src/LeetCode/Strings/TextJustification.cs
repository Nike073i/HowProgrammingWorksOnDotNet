using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public static class Solution
{
    public static IList<string> FullJustify(string[] words, int maxWidth)
    {
        var lines = new List<string>();

        int sumOfLength = words[0].Length;
        int count = 1;
        int startIndex = 0;

        for (int i = 1; i < words.Length; i++)
        {
            if ((sumOfLength + count + words[i].Length) > maxWidth)
            {
                lines.Add(Justify(words[startIndex..(startIndex + count)], maxWidth));
                startIndex = i;
                sumOfLength = 0;
                count = 0;
            }
            sumOfLength += words[i].Length;
            count++;
        }

        string lastLine = string.Join(" ", words[startIndex..]);
        lastLine += CreateSep(maxWidth - lastLine.Length);
        lines.Add(lastLine);
        return lines;
    }

    private static string Justify(Span<string> words, int maxWidth)
    {
        int sumOfLength = 0;
        foreach (var word in words)
            sumOfLength += word.Length;

        string result = "";

        int space = maxWidth - sumOfLength;
        int intervals = words.Length - 1;

        for (int i = 0; intervals > 0; i++, intervals--)
        {
            int length = (int)Math.Ceiling((double)space / intervals);
            space -= length;
            string sep = CreateSep(length);
            result += words[i] + sep;
        }

        result += words[^1] + CreateSep(space);
        return result;
    }

    private static string CreateSep(int length) => new(' ', length);
}

public class TextJustificationTests
{
    [Theory]
    [ClassData(typeof(TextJustificationTestData))]
    public void Test(string[] words, int maxWidth, string[] expected)
    {
        var actual = Solution.FullJustify(words, maxWidth);
        Assert.Equal(expected, actual);
    }
}

public class TextJustificationTestData : TheoryDataContainer.ThreeArg<string[], int, string[]>
{
    public TextJustificationTestData()
    {
        Add(
            ["This", "is", "an", "example", "of", "text", "justification."],
            16,
            ["This    is    an", "example  of text", "justification.  "]
        );

        Add(["Hello"], 5, ["Hello"]);

        Add(["Hello"], 10, ["Hello     "]);

        Add(
            ["What", "must", "be", "acknowledgment", "shall", "be"],
            16,
            ["What   must   be", "acknowledgment  ", "shall be        "]
        );

        Add(
            [
                "Science",
                "is",
                "what",
                "we",
                "understand",
                "well",
                "enough",
                "to",
                "explain",
                "to",
                "a",
                "computer.",
            ],
            20,
            [
                "Science  is  what we",
                "understand      well",
                "enough to explain to",
                "a computer.         ",
            ]
        );

        Add(["a", "b", "c", "d", "e"], 1, ["a", "b", "c", "d", "e"]);

        Add(
            ["longword", "verylong", "extremelong"],
            11,
            ["longword   ", "verylong   ", "extremelong"]
        );

        Add(
            ["Give", "me", "my", "Romeo;", "and,", "when", "he", "shall", "die,"],
            10,
            ["Give me my", "Romeo;    ", "and,  when", "he   shall", "die,      "]
        );

        Add(
            ["Listen", "to", "many,", "speak", "to", "a", "few."],
            6,
            ["Listen", "to    ", "many, ", "speak ", "to   a", "few.  "]
        );
    }
}
