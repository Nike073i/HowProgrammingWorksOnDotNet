using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays.TopKFrequent;

/*
    task: Топ K самых частых элементов
    leetcode: 347 https:    time: O(n)    memory: O(n)    notes:    - Результат формируется в 3 прохода:
        1. На первом проходе считается частота каждого элемента.
        2. На втором формируется словарь, где ключ - частота, а значение - коллекция элементов с этой частотой.
        3. На третьем формируем выходной массив, проходя от самых частотных к менее частотным, пока не наберем K
*/
public class Solution
{
    public static int[] TopKFrequent(int[] nums, int k)
    {
        var counts = new Dictionary<int, int>();

        foreach (var v in nums)
        {
            counts.TryGetValue(v, out int count);
            counts[v] = count + 1;
        }

        var freqs = new Dictionary<int, List<int>>();
        foreach (var kvp in counts)
        {
            int freq = kvp.Value;
            if (!freqs.ContainsKey(freq))
                freqs[freq] = [];

            freqs[freq].Add(kvp.Key);
        }

        int c = 0;
        int[] output = new int[k];
        for (int i = nums.Length; i > 0 && c < k; i--)
        {
            if (!freqs.ContainsKey(i))
                continue;

            for (int j = 0; c < k && j < freqs[i].Count; j++)
            {
                output[c++] = freqs[i][j];
            }
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestTopKFrequent(int[] nums, int k, int[] expected)
    {
        var actual = Solution.TopKFrequent(nums, k);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public SolutionTestData()
    {
        Add([1, 1, 1, 2, 2, 3], 2, [1, 2]);
        Add([1], 1, [1]);
        Add([1, 2, 3, 4, 5], 3, [1, 2, 3]);
        Add([1, 1, 1, 1], 1, [1]);
        Add([-1, -1, -2, -2, -2, -3], 2, [-2, -1]);
        Add([1, 1, 1, 2, 2, 3, 3, 3, 3, 4], 3, [3, 1, 2]);
        Add([1, 2, 2, 3, 3, 3], 3, [3, 2, 1]);
        Add([1000, 1000, 999, 999, 999, 888], 2, [999, 1000]);
        Add([0, 0, 0, 1, 1, 2], 2, [0, 1]);
        Add([1, 2, 3, 4, 5, 6], 3, [1, 2, 3]);
        Add([1, 1, 2, 2, 3, 3, 4, 4, 5, 5], 5, [1, 2, 3, 4, 5]);
        Add([1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3], 2, [3, 2]);
        Add([], 0, []);
        Add([5], 1, [5]);
        Add([1, 1, 1, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 4], 3, [4, 1, 3]);
    }
}
