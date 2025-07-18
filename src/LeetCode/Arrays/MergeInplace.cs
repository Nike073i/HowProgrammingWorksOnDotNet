using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.Arrays;

public class MergeInplace
{
    public static void Merge(int[] nums1, int m, int[] nums2, int n)
    {
        int p = m + n - 1;
        int i = m - 1;
        int j = n - 1;
        while (p >= 0)
        {
            if (i >= 0 && j >= 0)
                nums1[p--] = nums1[i] > nums2[j] ? nums1[i--] : nums2[j--];
            else
                nums1[p--] = i >= 0 ? nums1[i--] : nums2[j--];
        }
    }

    [Theory]
    [ClassData(typeof(MergeInplaceTestData))]
    public void MergeTests(int[] a, int[] b)
    {
        var extendedA = new int[a.Length + b.Length];
        Array.Fill(extendedA, 0);
        for (int i = 0; i < a.Length; i++)
            extendedA[i] = a[i];

        var expected = a.Concat(b).Order();
        Merge(extendedA, a.Length, b, b.Length);

        Assert.Equal(expected, extendedA);
    }

    public class MergeInplaceTestData : TheoryDataContainer.TwoArg<int[], int[]>
    {
        public MergeInplaceTestData()
        {
            Add([], []);

            Add([], [1]);
            Add([], [1, 2, 3]);

            Add([1], []);
            Add([1, 2, 3], []);

            Add([1], [2]);
            Add([2], [1]);

            Add([1, 3, 5], [2, 4]);
            Add([2, 4], [1, 3, 5]);

            Add([1, 2, 2], [2, 3, 5]);
            Add([1, 1, 1], [1, 1, 1]);

            Add(
                Enumerable.Range(1, 100).Where(x => x % 2 == 1).ToArray(),
                Enumerable.Range(1, 100).Where(x => x % 2 == 0).ToArray()
            );

            Add([-5, -3, -1], [-4, -2, 0]);

            Add([1, 2, 3], [4, 5, 6]);
            Add([4, 5, 6], [1, 2, 3]);

            Add([0], [0]);
            Add([int.MaxValue], [int.MinValue]);
        }
    }
}
