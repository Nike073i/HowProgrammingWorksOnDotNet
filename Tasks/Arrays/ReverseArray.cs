namespace HowProgrammingWorksOnDotNet.Tasks.Arrays;

public class ReverseArray
{
    [Fact]
    public void ReverseWithStack()
    {
        int[] array = [1, 2, 3, 4, 5];
        var stack = new Stack<int>();
        foreach (var val in array)
            stack.Push(val);

        Assert.Equal(array.Reverse(), stack);
    }

    [Fact]
    public void ReverseInPlace()
    {
        int[] array = [1, 2, 3, 4, 5];
        int length = array.Length;
        for (int i = 0; i < length / 2; i++)
            (array[i], array[^(i + 1)]) = (array[^(i + 1)], array[i]);

        Assert.Equal([5, 4, 3, 2, 1], array);
    }
}
