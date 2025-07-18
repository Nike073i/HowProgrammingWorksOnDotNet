namespace HowProgrammingWorksOnDotNet.Language.Arrays;

public class Ranges
{
    [Fact]
    public void Usage()
    {
        Range startIncludeEndExclude = 1..10; // 0 [ 1, 2, ... 9 ] 10, ...;
        Range twoLastElements = ^2..; // .. , 10, 11
        Range skip2AndTakeBefore2LastElements = 2..^2; // 0, 1, [2, 3, .. 9] , 10, 11
        Range firstFiveElements = ..5; // [ 0, 1, 2, 3, 4 ], 5, 6...;
        (var start, var length) = skip2AndTakeBefore2LastElements.GetOffsetAndLength(
            (new int[] { 1, 2, 3, 4, 5, 6, 8 }).Length
        );

        Assert.Equal((2, 3), (start, length));

        int[] values = Enumerable.Range(1, 25).ToArray(); // Or ToList();
        var slice = values[skip2AndTakeBefore2LastElements];
        Assert.Equal(Enumerable.Range(3, 21), slice);
    }
}
