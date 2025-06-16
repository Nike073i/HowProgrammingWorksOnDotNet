using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Exercises.Recursion;

// Это расчет чисел Фибоначчи. Сумма текущего равна сумме двух предыдущих
// LeetCode 70
public class ClimbStairs
{
    [ClassData(typeof(ClimbStairsData))]
    [Theory]
    public void Iterative(int n, int expected)
    {
        static int Solution(int n)
        {
            if (n < 3)
                return n;

            int first = 2;
            int second = 3;

            for (int i = 3; i < n; i++)
            {
                int tmp = first + second;
                first = second;
                second = tmp;
            }
            return second;
        }

        Assert.Equal(expected, Solution(n));
    }

    [ClassData(typeof(ClimbStairsData))]
    [Theory]
    public void Recursion(int n, int expected)
    {
        var memory = new Dictionary<int, int>();
        int Solution(int n)
        {
            if (n <= 3)
                return n;
            if (!memory.TryGetValue(n, out int value))
            {
                value = Solution(n - 1) + Solution(n - 2);
                memory[n] = value;
            }
            return value;
        }

        Assert.Equal(expected, Solution(n));
    }

    private class ClimbStairsData : TheoryDataContainer.TwoArg<int, int>
    {
        public ClimbStairsData()
        {
            Add(1, 1);
            Add(2, 2);
            Add(3, 3);
            Add(4, 5);
            Add(5, 8);
            Add(6, 13);
            Add(7, 21);
            Add(8, 34);
            Add(9, 55);
            Add(10, 89);
            Add(11, 144);
            Add(12, 233);
            Add(13, 377);
            Add(14, 610);
            Add(15, 987);
            Add(16, 1597);
            Add(17, 2584);
            Add(18, 4181);
            Add(19, 6765);
            Add(20, 10946);
            Add(21, 17711);
            Add(22, 28657);
            Add(23, 46368);
            Add(24, 75025);
            Add(25, 121393);
        }
    }
}
