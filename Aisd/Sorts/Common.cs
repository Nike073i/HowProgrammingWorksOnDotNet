using System.Numerics;

namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class Common
{
    public static IEnumerable<int> GetRandomValues(int count = 100, int max = 100)
    {
        var random = new Random();
        return Enumerable.Range(0, count).Select(i => random.Next(max));
    }

    public record struct Pair<T>(T First, T Second)
        where T : INumber<T>
    {
        public int Direction() => Second.CompareTo(First);
    };

    public static bool IsSorted<T>(IEnumerable<T> values)
        where T : INumber<T>
    {
        int length = values.Count();
        if (length < 2)
            return true;

        var pairs = values.Zip(values.Skip(1)).Select(p => new Pair<T>(p.First, p.Second));
        var directionPair = pairs.FirstOrDefault(p => p.Direction() != 0);

        if (directionPair == default)
            return true;

        int direction = directionPair.Direction();

        bool isSorted = pairs.All(p => p.Direction() == direction || p.Direction() == 0);
        return isSorted;
    }
}
