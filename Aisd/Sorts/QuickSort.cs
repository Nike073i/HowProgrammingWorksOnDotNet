namespace HowProgrammingWorksOnDotNet.Aisd.Sorts;

public class QuickSort
{
    [Fact]
    public void RecursiveSortArray()
    {
        int[] values = Common.GetRandomValues(10000).ToArray();

        RecursiveQuickSort(values);

        Assert.True(Common.IsSorted(values));
    }

    [Fact]
    public void QueueSortArray()
    {
        int[] values = Common.GetRandomValues(10000).ToArray();

        QueueQuickSort(values);

        Assert.True(Common.IsSorted(values));
    }

    private void QueueQuickSort(int[] values)
    {
        if (values.Length < 2)
            return;

        var queue = new Queue<(int, int)>();
        queue.Enqueue((0, values.Length - 1));

        while (queue.Any())
        {
            (int start, int end) = queue.Dequeue();
            if (start >= end)
                continue;

            int pivot = SplitByPivot(values, start, end);

            queue.Enqueue((start, pivot - 1));
            queue.Enqueue((pivot + 1, end));
        }

        // Опорный элемент - первый
        static int SplitByPivot(int[] values, int start, int end)
        {
            int pivot = start;

            for (int i = start + 1; i <= end; i++)
            {
                if (values[i] < values[pivot])
                {
                    (values[pivot], values[pivot + 1], values[i]) = (
                        values[i],
                        values[pivot],
                        values[pivot + 1]
                    );
                    pivot++;
                }
            }
            return pivot;
        }
    }

    private void RecursiveQuickSort(Span<int> values)
    {
        if (values.Length < 2)
            return;

        int pivot = SplitByPivot(values);
        RecursiveQuickSort(values[..pivot]);
        RecursiveQuickSort(values[(pivot + 1)..]);

        // Опорный элемент - первый
        static int SplitByPivot(Span<int> values)
        {
            int pivot = 0;

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < values[pivot])
                {
                    (values[pivot], values[pivot + 1], values[i]) = (
                        values[i],
                        values[pivot],
                        values[pivot + 1]
                    );
                    pivot++;
                }
            }
            return pivot;
        }

        /* Или традиционным образом:
        static int SplitByPivot(Span<int> values)
        {
            int pivotInd = 0;
            int pivotVal = values[0];

            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < pivotVal)
                {
                    values[pivotInd] = values[i];
                    values[i] = values[++pivotInd];
                }
            }
            values[pivotInd] = pivotVal;
            return pivotInd;
        }
        */
    }
}
