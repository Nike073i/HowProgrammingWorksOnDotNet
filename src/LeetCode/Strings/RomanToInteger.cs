using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Strings;

public interface IRomanConverter
{
    static Dictionary<char, int> Map = new()
    {
        { 'I', 1 },
        { 'V', 5 },
        { 'X', 10 },
        { 'L', 50 },
        { 'C', 100 },
        { 'D', 500 },
        { 'M', 1000 },
    };
    int Convert(string roman);
}

public abstract class RomanToIntegerTests
{
    protected abstract IRomanConverter CreateConverter();

    [Theory]
    [ClassData(typeof(RomanToIntegerTestData))]
    public void Test(string roman, int expected)
    {
        var converter = CreateConverter();
        int actual = converter.Convert(roman);
        Assert.Equal(expected, actual);
    }
}

public class RomanToIntegerTestData : TheoryDataContainer.TwoArg<string, int>
{
    public RomanToIntegerTestData()
    {
        Add("I", 1);
        Add("V", 5);
        Add("X", 10);
        Add("L", 50);
        Add("C", 100);
        Add("D", 500);
        Add("M", 1000);
        Add("II", 2);
        Add("VI", 6);
        Add("XV", 15);
        Add("CL", 150);
        Add("MD", 1500);
        Add("MMM", 3000);
        Add("IV", 4);
        Add("IX", 9);
        Add("XL", 40);
        Add("XC", 90);
        Add("CD", 400);
        Add("CM", 900);
        Add("MCMXCIV", 1994);
        Add("LVIII", 58);
        Add("III", 3);
        Add("XIV", 14);
        Add("MCDXLIX", 1449);
        Add("MMMCMXCIX", 3999);
        Add("", 0);
    }
}

public class BackToFrontRomanToInteger : IRomanConverter
{
    public int Convert(string roman)
    {
        if (roman.Length == 0)
            return 0;
        if (roman.Length == 1)
            return IRomanConverter.Map[roman[0]];

        int sum = 0;

        int i = roman.Length - 1;
        while (i > 0)
        {
            int curr = IRomanConverter.Map[roman[i]];
            int prev = IRomanConverter.Map[roman[i - 1]];
            if (curr <= prev)
            {
                sum += curr;
                i--;
            }
            else
            {
                sum += curr - prev;
                i -= 2;
            }
        }
        if (i == 0)
            sum += IRomanConverter.Map[roman[0]];

        return sum;
    }
}

public class BackToFrontRomanToIntegerTests : RomanToIntegerTests
{
    protected override IRomanConverter CreateConverter() => new BackToFrontRomanToInteger();
}

public class SimpleRomanToInteger : IRomanConverter
{
    public int Convert(string roman)
    {
        int sum = 0;

        for (int i = 0; i < roman.Length; i++)
        {
            int curr = IRomanConverter.Map[roman[i]];
            int next = (i + 1) < roman.Length ? IRomanConverter.Map[roman[i + 1]] : 0;

            if (curr >= next)
                sum += curr;
            else
                sum -= curr;
        }
        return sum;
    }
}

public class SimpleRomanToIntegerTests : RomanToIntegerTests
{
    protected override IRomanConverter CreateConverter() => new SimpleRomanToInteger();
}
