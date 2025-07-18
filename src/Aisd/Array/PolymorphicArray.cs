using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Array;

file static class LinqExtensions
{
    public static int Mult(this IEnumerable<int> values) =>
        values.Aggregate((acc, size) => acc * size);
}

public class PolymorphicArray<T>(params int[] sizes) : IEnumerable<T>
{
    private int[] _shape = sizes;
    private readonly T[] _values = new T[sizes.Mult()];

    public int[] Shape => [.. _shape];

    private int GetPhysicalIndex(params int[] indexes)
    {
        int dim = _shape.Length;
        if (indexes.Length != dim)
            throw new IndexOutOfRangeException();
        for (int i = 0; i < dim; i++)
            if (indexes[i] < 0 || indexes[i] >= _shape[i])
                throw new IndexOutOfRangeException();

        int factor = 1;
        int index = 0;

        for (int i = 0; i < dim; i++)
        {
            index += factor * indexes[i];
            factor *= _shape[i];
        }
        return index;
    }

    public void Reshape(params int[] shape)
    {
        var elements = shape.Mult();
        if (_values.Length != elements)
            throw new ArgumentException("Новая форма не совпадает по количеству элементов");

        _shape = shape;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => _values.AsEnumerable().GetEnumerator();

    public T this[params int[] indexes]
    {
        get
        {
            int index = GetPhysicalIndex(indexes);
            return _values[index];
        }
        set
        {
            int index = GetPhysicalIndex(indexes);
            _values[index] = value;
        }
    }
}

public class PolymorphicArrayTests()
{
    [Fact]
    public void Usage()
    {
        var array = new PolymorphicArray<int>(5, 4, 3);

        Assert.Throws<IndexOutOfRangeException>(() => array[5]);
        Assert.Throws<IndexOutOfRangeException>(() => array[5, 4, 3]);

        array[0, 0, 0] = 1;
        array[1, 1, 1] = 27;
        array[2, 2, 2] = 53;

        Assert.Throws<ArgumentException>(() => array.Reshape(5, 5, 5));
        array.Reshape(10, 2, 3);

        Assert.Equal(27, array[6, 0, 1]);

        array.Reshape(60);
        Assert.Equal(27, array[26]);

        var shape = array.Shape;
        shape[0] = 1000;
        Assert.Equal(60, array.Shape[0]);
    }
}
