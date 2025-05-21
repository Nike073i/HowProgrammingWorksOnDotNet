using System.ComponentModel.DataAnnotations;

namespace HowProgrammingWorksOnDotNet.Aisd.NumericalAlgorithms.ShuffleArray;

public static class ShuffleArray
{
    public static void Shuffle(this int[] array)
    {
        var random = new Random();
        int length = array.Length;
        for (int i = 0; i < array.Length; i++)
        {
            int j = random.Next(0, length);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }

    public static bool IsSorted(this int[] array)
    {
        if (array.Length < 2)
            return true;

        int direction = 0;
        for (int i = 1; i < array.Length && direction == 0; i++)
            direction = Math.Sign(array[i] - array[i - 1]);
        if (direction == 0)
            return true;

        for (int i = 1; i < array.Length; i++)
        {
            if (Math.Sign(array[i] - array[i - 1]) != direction)
                return false;
        }
        return true;
    }
}

public class ShuffleArrayTests
{
    [Fact]
    public void Usage()
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7];
        array.Shuffle();

        Assert.False(array.IsSorted());
    }
}
