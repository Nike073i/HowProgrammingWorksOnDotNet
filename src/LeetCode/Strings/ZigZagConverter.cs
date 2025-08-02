using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public class ZigZagConverter
{
    public static string Convert(string input, int numRows)
    {
        if (numRows == 1)
            return input;

        string result = "";
        for (int i = 0; i < numRows; i++)
        {
            var steps = GetStep(i + 1, numRows);
            int j = i;
            foreach (var step in steps)
            {
                if (j >= input.Length)
                    break;
                result += input[j];
                j += step;
            }
        }
        return result;
    }

    private static IEnumerable<int> GetStep(int curRow, int totalRows)
    {
        int length = (totalRows - 1) * 2;

        if (curRow == 1 || curRow == totalRows || totalRows == 2)
            return Alternate(length, length);

        int a = length - (curRow - 1) * 2;
        int b = length - a;
        return Alternate(a, b);
    }

    private static IEnumerable<int> Alternate(int a, int b)
    {
        int[] values = [a, b];
        for (int i = 0; ; i++)
            yield return values[i % 2];
    }
}

public class ZigZagConverterTests
{
    [Theory]
    [ClassData(typeof(ZigZagConverterTestData))]
    public void Usage(string input, int nrows, string expected)
    {
        string actual = ZigZagConverter.Convert(input, nrows);
        Assert.Equal(expected, actual);
    }
}

public class ZigZagConverterTestData : TheoryDataContainer.ThreeArg<string, int, string>
{
    public ZigZagConverterTestData()
    {
        Add("123456789abcde", 3, "159d2468ace37b");
        Add("PAYPALISHIRING", 3, "PAHNAPLSIIGYIR");
        Add("PAYPALISHIRING", 4, "PINALSIGYAHRPI");
        Add("A", 1, "A");
        Add("AB", 1, "AB");
        Add("ABC", 2, "ACB");
        Add("", 3, "");
        Add("AB", 5, "AB");
        Add("HELLOWORLD", 2, "HLOOLELWRD");
        Add("ZIGZAGCONVERSION", 5, "ZNIOVNGCEOZGRIAS");
        Add("TESTINGZIGZAG", 4, "TGGENZASIIZTG");
    }
}
