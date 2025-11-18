using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

/*
    name: Найти общий префикс в двух равных массивах
    leetcode: 2657 - https://leetcode.com/problems/find-the-prefix-common-array-of-two-arrays/description/
    description: Оба массива - множества целых чисел от 1 до N. Нужно вернуть массив, где каждый элемент массива будет отвечать за кол-во пар однозначных элементов в префиксе длинной i.
    time: O(n)
    memory: O(n)
    notes:
    - common это grow-only счетчик, который отвечает за кол-во пар, увиденных на текущий момент
    - Сложная конструкция с if/else нужна для того, чтобы дважды не посчитать одинаковые значения. Например, [1], [1] без этой проверки дадут common = 2
*/
public class Solution
{
    public static int[] FindThePrefixCommonArray(int[] A, int[] B)
    {
        int common = 0;
        int[] counts = new int[A.Length + 1];
        int[] result = new int[A.Length];

        for (int i = 0; i < A.Length; i++)
        {
            counts[A[i]]++;
            counts[B[i]]++;
            if (A[i] == B[i])
            {
                common++;
            }
            else
            {
                if (counts[A[i]] == 2)
                    common++;

                if (counts[B[i]] == 2)
                    common++;
            }
            result[i] = common;
        }

        return result;
    }
}

public class FindThePrefixCommonArrayTests
{
    [Theory]
    [ClassData(typeof(FindThePrefixCommonArrayTestData))]
    public void Test(int[] A, int[] B, int[] expected)
    {
        int[] actual = Solution.FindThePrefixCommonArray(A, B);
        Assert.Equal(expected, actual);
    }
}

public class FindThePrefixCommonArrayTestData : TheoryDataContainer.ThreeArg<int[], int[], int[]>
{
    public FindThePrefixCommonArrayTestData()
    {
        Add([1, 2, 3], [1, 2, 3], [1, 2, 3]);
        Add([1, 3, 2], [3, 1, 2], [0, 2, 3]);
        Add([1, 3, 2, 4], [3, 1, 2, 4], [0, 2, 3, 4]);
        Add([1], [1], [1]);
        Add([1, 2], [2, 1], [0, 2]);
        Add([1, 2], [1, 2], [1, 2]);
        Add([1, 2, 3, 4, 5], [1, 2, 3, 4, 5], [1, 2, 3, 4, 5]);
        Add([5, 4, 3, 2, 1], [1, 2, 3, 4, 5], [0, 0, 1, 3, 5]);
        Add([1, 2, 3, 4], [4, 3, 2, 1], [0, 0, 2, 4]);
        Add([1, 2, 3, 4], [1, 3, 4, 2], [1, 1, 2, 4]);
    }
}
