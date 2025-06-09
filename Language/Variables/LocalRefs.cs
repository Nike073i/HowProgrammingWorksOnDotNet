namespace HowProgrammingWorksOnDotNet.Language.Variables;

public class LocalRefs
{
    [Fact]
    public void AccessingValueTypeObjectInCollection()
    {
        // Может быть сложной коллекцией - матрицей, деревом и т.д.
        var values = new int[] { 1, 2, 3, 4, 5 };

        int defaultVal = -1;
        ref int element = ref FindValueRef(val => val == 4, ref defaultVal);
        element = 40;

        Assert.Equal([1, 2, 3, 40, 5], values);

        element = ref FindValueRef(val => val == 6, ref defaultVal);
        Assert.Equal(element, defaultVal);

        ref int FindValueRef(Func<int, bool> predicate, ref int defaultVal)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (predicate(values[i]))
                    return ref values[i];
            }
            return ref defaultVal;
        }
    }

    public ref struct MinMaxRef
    {
        public ref int Max;
        public ref int Min;

        public MinMaxRef(ref int max, ref int min)
        {
            Max = ref max;
            Min = ref min;
        }
    }

    [Fact]
    public void PointerToMinMaxValueTypeObject()
    {
        int a = 10;
        int b = 20;

        var minmax = FindMinMax(ref a, ref b);
        Assert.Equal(minmax.Max, b);
        Assert.Equal(minmax.Min, a);

        a = 30;
        Assert.Equal(a, minmax.Min);
        minmax.Min = 40;
        Assert.Equal(minmax.Min, a);

        static MinMaxRef FindMinMax(ref int a, ref int b) =>
            a > b ? new MinMaxRef(ref a, ref b) : new MinMaxRef(ref b, ref a);
    }

    [Fact]
    public void PointerToMinMaxValueTypeObjectOldStyle()
    {
        int a = 10;
        int b = 20;

        FindMinMax(a, b, out int max, out int min);
        Assert.Equal(max, b);
        Assert.Equal(min, a);

        a = 30;
        // min не ссылается на a. Это другая переменная. А потому обновляется только a. Min = 10;
        Assert.NotEqual(30, min);
        Assert.Equal(10, min);
        Assert.Equal(30, a);

        static void FindMinMax(int a, int b, out int max, out int min)
        {
            if (a > b)
            {
                max = a;
                min = b;
            }
            else
            {
                max = b;
                min = a;
            }
        }
    }
}
