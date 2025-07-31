using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public interface IInt2RomanConverter
{
    string Convert(int number);
}

public abstract class IntegerToRomanTests
{
    protected abstract IInt2RomanConverter CreateConverter();

    [Theory]
    [ClassData(typeof(IntegerToRomanTestData))]
    public void Test(int number, string expected)
    {
        var converter = CreateConverter();
        string actual = converter.Convert(number);
        Assert.Equal(expected, actual);
    }
}

public class IntegerToRomanTestData : TheoryDataContainer.TwoArg<int, string>
{
    public IntegerToRomanTestData()
    {
        Add(1, "I");
        Add(3, "III");
        Add(4, "IV");
        Add(5, "V");
        Add(9, "IX");
        Add(10, "X");
        Add(40, "XL");
        Add(50, "L");
        Add(90, "XC");
        Add(100, "C");
        Add(400, "CD");
        Add(500, "D");
        Add(900, "CM");
        Add(1000, "M");
        Add(58, "LVIII");
        Add(1994, "MCMXCIV");
        Add(1449, "MCDXLIX");
        Add(3999, "MMMCMXCIX");
    }
}

public class Solution1 : IInt2RomanConverter
{
    public string Convert(int num)
    {
        if (num >= 4000 || num < 1)
            throw new Exception();

        string[] pairs = ["IVX", "XLC", "CDM"];

        string result = "";

        int tmp = num;
        for (int i = 0; i < 3 && tmp > 0; i++, tmp /= 10)
        {
            int value = tmp % 10;
            if (value == 0)
                continue;
            else if (value < 4)
                result = string.Concat(new string(pairs[i][0], value), result);
            else if (value == 4)
                result = string.Concat(pairs[i][0], pairs[i][1], result);
            else if (value < 9)
                result = string.Concat(pairs[i][1], new string(pairs[i][0], value - 5), result);
            else
                result = string.Concat(pairs[i][0], pairs[i][2], result);
        }
        if (tmp > 0)
        {
            result = string.Concat(new string('M', tmp), result);
        }
        return result;
    }
}

public class Solution1Tests : IntegerToRomanTests
{
    protected override IInt2RomanConverter CreateConverter() => new Solution1();
}

public class Solution2 : IInt2RomanConverter
{
    public string Convert(int num)
    {
        if (num >= 4000 || num < 1)
            throw new Exception();

        List<(int, string)> pairs =
        [
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I"),
        ];

        string result = "";
        foreach (var pair in pairs)
        {
            while (num >= pair.Item1)
            {
                result += pair.Item2;
                num -= pair.Item1;
            }
        }
        return result;
    }
}

public class Solution2Tests : IntegerToRomanTests
{
    protected override IInt2RomanConverter CreateConverter() => new Solution2();
}

public class Solution3 : IInt2RomanConverter
{
    public string Convert(int num) =>
        new string('I', num)
            .Replace("IIIII", "V")
            .Replace("VV", "X")
            .Replace("XXXXX", "L")
            .Replace("LL", "C")
            .Replace("CCCCC", "D")
            .Replace("DD", "M")
            .Replace("VIIII", "IX")
            .Replace("IIII", "IV")
            .Replace("LXXXX", "XC")
            .Replace("XXXX", "XL")
            .Replace("DCCCC", "CM")
            .Replace("CCCC", "CD");
}

public class Solution3Tests : IntegerToRomanTests
{
    protected override IInt2RomanConverter CreateConverter() => new Solution3();
}
