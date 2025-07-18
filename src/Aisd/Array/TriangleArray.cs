using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Array;

public class TriangleArray<T> : IEnumerable<T>
{
    private readonly T[] _values;
    private readonly int _dimension;

    public TriangleArray(int dimension)
    {
        if (dimension < 2)
            throw new ArgumentException();
        int size = TriangleSum(dimension);
        _values = new T[size];
        _dimension = dimension;
    }

    private int GetIndex(int i, int j)
    {
        if (i < 0 || j < 0 || i >= _dimension || j >= _dimension)
            throw new IndexOutOfRangeException();
        if (j > i)
            (i, j) = (j, i);
        return TriangleSum(i) + j;
    }

    public T this[int i, int j]
    {
        get
        {
            int index = GetIndex(i, j);
            return _values[index];
        }
        set
        {
            int index = GetIndex(i, j);
            _values[index] = value;
        }
    }

    private static int TriangleSum(int n) => n * (n + 1) / 2;

    public IEnumerator<T> GetEnumerator() => _values.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> GetDiag()
    {
        for (int i = 0, ind = 0; i < _dimension; i++, ind += i + 1)
            yield return _values[ind];
    }
}

public class TriangleArrayTests()
{
    [Fact]
    public void Usage()
    {
        int dim = 5;
        var triangle = new TriangleArray<int>(dim);

        int count = 0;
        for (int i = 0; i < dim; i++)
        {
            for (int j = 0; j <= i; j++)
                triangle[i, j] = ++count;
        }

        triangle.ToList().ForEach(Console.WriteLine);
        Assert.Equal(8, triangle[3, 1]);
        Assert.Equal(8, triangle[1, 3]);

        Assert.Equal([1, 3, 6, 10, 15], triangle.GetDiag());
    }
}
