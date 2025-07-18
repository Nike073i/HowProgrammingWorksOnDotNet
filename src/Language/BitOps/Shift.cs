namespace HowProgrammingWorksOnDotNet.Language.BitOps;

public class Shift
{
    [Fact]
    public void Usage()
    {
        // >> и << - арифметический сдвиг. Знак учитывается. При сдвиге вправо у отрицательного - 1, у положительного - 0
        int x = 0b10;
        x <<= 1;
        Assert.Equal(0b100, x);
        x >>= 2;
        Assert.Equal(0b01, x);
        x >>= 1;
        Assert.Equal(0b0, x);

        int y = ~3 + 1;
        Assert.Equal(0b11111111_11111111_11111111_11111101, (uint)y);
        y <<= 1;
        Assert.Equal(0b11111111_11111111_11111111_11111010, (uint)y);
        y >>= 1;
        Assert.Equal(0b11111111_11111111_11111111_11111101, (uint)y);

        // Логический сдвиг право всегда добавляет 0
        x >>>= 1;
        Assert.Equal(0b0, x);
        y >>>= 1;
        Assert.Equal(0b01111111_11111111_11111111_11111110, y);
    }
}
