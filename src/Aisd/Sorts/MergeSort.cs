namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class MergeSort
{
    [Fact]
    public void RecursiveMergeSortArray()
    {
        var values = Common.GetRandomValues().ToArray();

        RecursiveMergeSort(values);

        Assert.True(Common.IsSorted(values));
    }

    [Fact]
    public void IterativeMergeSortArray()
    {
        var values = Common.GetRandomValues().ToArray();

        IterativeMergeSort(values);

        Assert.True(Common.IsSorted(values));
    }

    private void IterativeMergeSort(int[] values)
    {
        int window = 1;
        int length = values.Length;
        while (window < length)
        {
            int left = 0;
            while (left < length)
            {
                int middle = left + window;
                int right = Math.Min(middle + window, length);

                Merge(left, middle, right);

                left = right;
            }
            window *= 2;
        }

        void Merge(int left, int middle, int right)
        {
            if (left >= right)
                return;
            int[] tmp = new int[right - left];

            int lPtr = left;
            int rPtr = middle;
            for (int i = 0; i < tmp.Length; i++)
            {
                if (lPtr < middle && rPtr < right)
                    tmp[i] = values[lPtr] < values[rPtr] ? values[lPtr++] : values[rPtr++];
                else
                    tmp[i] = lPtr < middle ? values[lPtr++] : values[rPtr++];
            }
            for (int i = 0; i < tmp.Length; i++)
                values[left + i] = tmp[i];
        }
    }

    private void RecursiveMergeSort(Span<int> values)
    {
        if (values.Length < 2)
            return;

        int length = values.Length;
        int middle = length / 2;

        RecursiveMergeSort(values[..middle]);
        RecursiveMergeSort(values[middle..]);

        var tmp = new int[length];
        int lPtr = 0;
        int rPtr = middle;
        for (int i = 0; i < length; i++)
        {
            if (lPtr < middle && rPtr < length)
                tmp[i] = values[lPtr] < values[rPtr] ? values[lPtr++] : values[rPtr++];
            else
                tmp[i] = lPtr < middle ? values[lPtr++] : values[rPtr++];
        }
        for (int i = 0; i < length; i++)
            values[i] = tmp[i];
    }
}
