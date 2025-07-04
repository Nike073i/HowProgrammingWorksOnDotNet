using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Aisd.Strings;

public class Parenthesis
{
    [Theory]
    [ClassData(typeof(JustOneTypeClassData))]
    public void JustOneType(string input, bool expectedValid)
    {
        int counter = 0;
        foreach (char s in input)
        {
            if (s == '(')
                counter++;
            else if (s == ')')
            {
                counter--;
                if (counter < 0)
                    break;
            }
        }

        bool isValid = counter == 0;
        Assert.Equal(expectedValid, isValid);
    }

    private class JustOneTypeClassData : TheoryDataContainer.TwoArg<string, bool>
    {
        public JustOneTypeClassData()
        {
            Add("((5 + 4) * (2 + 7))* (-1) + 2 / (10 - (15 * (6)))", true);
            Add("((5 + 4) * (2 + 7))* (-1) + 2 / (10 - 15 * (6)))", false);
            Add("((5 + 4) * (2 + 7))* (-1) + 2 / (10 - (15 * ((6)))", false);
            Add("((5 + 4) * (2 + 7)* (-1) + 2 / (10 - (15 * (6)))", false);
            Add("((5 + 4) * 2 + 7))* (-1) + 2 / (10 - (15 * (6)))", false);
            Add("()()() ((()()()())) (()()((())))", true);
            Add("()()() (((()()())) (()()((())))", false);
            Add("()()()) ((()()()())) (()()((())))", false);
            Add("asdasda", true);
        }
    }
}
