namespace HowProgrammingWorksOnDotNet.Language.Math;

public class CheckOverflow
{
    [Fact]
    public void Usage()
    {
        int x = int.MaxValue;
        Assert.Equal(int.MinValue, x + 1);

        Assert.Throws<OverflowException>(() => checked(x++));
        Assert.Throws<OverflowException>(() =>
        {
            checked
            {
                x++;
            }
        });

        Assert.Equal(double.MaxValue, double.MaxValue + 1);
        Assert.Equal(double.MinValue, double.MinValue - 1);
    }
}
