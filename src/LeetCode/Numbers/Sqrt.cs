namespace HowProgrammingWorksOnDotNet.LeetCode.Numbers.Sqrt;

/*
    leetcode: 69 https://leetcode.com/problems/sqrtx/description/
    time: O(logn)
    memory: O(1)
    notes:
    - Середина высчитывается относительно диапазона l_r. Поэтому сначала высчитываем середину, а затем - сдвигаем на l
    - Перемножение 2-х int храним в long во избежание переполнения
    - Если x имеет дробный корень, то округляем до меньшего (возвращаем r, поскольку l будет на 1 больше)
*/
public class Solution
{
    public static int MySqrt(int x)
    {
        int l = 1,
            r = x;
        while (l <= r)
        {
            int middle = l + (r - l) / 2;
            long pow2 = (long)middle * middle;
            if (pow2 == x)
                return middle;
            else if (pow2 > x)
                r = middle - 1;
            else
                l = middle + 1;
        }
        return r;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMySqrt(int x, int expected)
    {
        int actual = Solution.MySqrt(x);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<int, int>
{
    public SolutionTestData()
    {
        Add(4, 2);
        Add(8, 2);
        Add(9, 3);
        Add(16, 4);
        Add(0, 0);
        Add(1, 1);
        Add(2, 1);
        Add(3, 1);
        Add(100, 10);
        Add(99, 9);
        Add(101, 10);
        Add(2147395599, 46339);
        Add(2147483647, 46340);
        Add(25, 5);
        Add(24, 4);
        Add(26, 5);
        Add(10, 3);
        Add(15, 3);
        Add(20, 4);
    }
}
