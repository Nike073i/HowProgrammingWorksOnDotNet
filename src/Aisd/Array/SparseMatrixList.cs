using System.Collections;
using System.Reflection.Metadata;

namespace HowProgrammingWorksOnDotNet.Aisd.Array;

# region Contracts

public interface ISparseMatrix<T> : IEnumerable<T>
{
    public record Element(int I, int J, T Value);

    T this[int i, int j] { get; set; }
    T DefaultVal { get; set; }
    IEnumerable<Element> GetElements();
    T Extract(int i, int j);
}

#endregion

#region tests

public abstract class SparseMatrixTests
{
    protected abstract ISparseMatrix<int> CreateMatrix(int nrows, int ncols, int defaultVal);

    [Fact]
    public void Usage()
    {
        var matrix = CreateMatrix(4, 4, -1);
        Assert.Equal(Enumerable.Repeat(-1, 16), matrix);
        Assert.Equal([], matrix.GetElements());

        matrix[0, 1] = 2;
        matrix[2, 1] = 10;
        matrix[2, 2] = 11;
        matrix[3, 1] = 14;
        matrix[3, 3] = 16;

        Assert.Equal([-1, 2, -1, -1, -1, -1, -1, -1, -1, 10, 11, -1, -1, 14, -1, 16], matrix);
        Assert.Equal(
            [new(0, 1, 2), new(2, 1, 10), new(2, 2, 11), new(3, 1, 14), new(3, 3, 16)],
            matrix.GetElements()
        );

        Assert.Equal(-1, matrix.Extract(1, 1));
        Assert.Equal(10, matrix.Extract(2, 1));
        Assert.Equal(-1, matrix.Extract(2, 1));

        Assert.Equal([-1, 2, -1, -1, -1, -1, -1, -1, -1, -1, 11, -1, -1, 14, -1, 16], matrix);
        Assert.Equal(
            [new(0, 1, 2), new(2, 2, 11), new(3, 1, 14), new(3, 3, 16)],
            matrix.GetElements()
        );

        matrix.Extract(0, 1);
        matrix.Extract(2, 2);
        matrix.Extract(3, 1);
        matrix.Extract(3, 3);

        Assert.Equal([], matrix.GetElements());
        Assert.Equal(Enumerable.Repeat(-1, 16), matrix);

        matrix[2, 2] = 100;
        Assert.Equal([new(2, 2, 100)], matrix.GetElements());
        var excepted = Enumerable.Repeat(-1, 16).ToArray();
        excepted[10] = 100;
        Assert.Equal(excepted, matrix);
    }
}

#endregion

#region List of Lists

public class SparseMatrixList<T>(int nrows, int ncols, T defaultVal) : ISparseMatrix<T>
{
    private class ArrayEntry
    {
        public int Column { get; set; }
        public T Value { get; set; }
        public ArrayEntry? Next { get; set; }
    }

    private class ArrayRow
    {
        public int Row { get; set; }
        public ArrayEntry Entry { get; set; }
        public ArrayRow? Next { get; set; }
    }

    private readonly ArrayRow _head = new() { Row = -1 };

    public T DefaultVal { get; set; } = defaultVal;

    private ArrayRow AddRow(ArrayRow rowBefore, int i)
    {
        var newRow = new ArrayRow
        {
            Entry = new ArrayEntry() { Column = -1 },
            Row = i,
        };
        rowBefore.Next = newRow;
        return newRow;
    }

    private ArrayEntry AddEntry(ArrayEntry entryBefore, int j)
    {
        var newEntry = new ArrayEntry { Column = j };
        entryBefore.Next = newEntry;
        return newEntry;
    }

    private ArrayRow FindBefore(ArrayRow start, int i)
    {
        var tmp = start;
        while (tmp.Next != null && tmp.Next.Row < i)
            tmp = tmp.Next;

        return tmp;
    }

    private ArrayEntry FindBefore(ArrayEntry start, int j)
    {
        var tmp = start;
        while (tmp.Next != null && tmp.Next.Column < j)
            tmp = tmp.Next;

        return tmp;
    }

    private void Set(int i, int j, T value)
    {
        var rowBefore = FindBefore(_head, i);
        var row = rowBefore.Next;
        row ??= AddRow(rowBefore, i);

        var columnBefore = FindBefore(row.Entry, j);
        var column = columnBefore.Next;
        column ??= AddEntry(columnBefore, j);

        column.Value = value;
    }

    private T Get(int i, int j)
    {
        var rowBefore = FindBefore(_head, i);
        var row = rowBefore.Next;
        if (row is null)
            return DefaultVal;

        var columnBefore = FindBefore(rowBefore.Entry, j);
        var column = columnBefore.Next;
        if (column is null)
            return DefaultVal;
        return column.Value;
    }

    private void CheckIndex(int i, int j)
    {
        if (i < 0 || j < 0 || i >= nrows || j >= ncols)
            throw new IndexOutOfRangeException();
    }

    private int GetFlatIndex(int i, int j) => i * ncols + j;

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        int ind = 0;
        int count = nrows * ncols;

        foreach (var elem in GetElements())
        {
            int flatIndex = GetFlatIndex(elem.I, elem.J);
            for (; ind < flatIndex; ind++)
                yield return DefaultVal;
            ind = flatIndex + 1;
            yield return elem.Value;
        }

        while (ind < count)
        {
            ind++;
            yield return DefaultVal;
        }
    }

    public IEnumerator GetEnumerator() => GetEnumerator();

    public IEnumerable<ISparseMatrix<T>.Element> GetElements()
    {
        var row = _head.Next;
        while (row != null)
        {
            var col = row.Entry.Next;
            while (col != null)
            {
                yield return new ISparseMatrix<T>.Element(row.Row, col.Column, col.Value);
                col = col.Next;
            }
            row = row.Next;
        }
    }

    public T Extract(int i, int j)
    {
        var rowBefore = FindBefore(_head, i);
        var row = rowBefore.Next;

        if (row is null || row.Row != i)
            return DefaultVal;

        var columnBefore = FindBefore(row.Entry, j);
        var column = columnBefore.Next;
        if (column is null || column.Column != j)
            return DefaultVal;

        columnBefore.Next = column.Next;

        if (row.Entry.Next == null)
            rowBefore.Next = row.Next;

        return column.Value;
    }

    public T this[int i, int j]
    {
        get
        {
            CheckIndex(i, j);
            return Get(i, j);
        }
        set
        {
            CheckIndex(i, j);
            Set(i, j, value);
        }
    }
}

public class SparseMatrixListTests : SparseMatrixTests
{
    protected override ISparseMatrix<int> CreateMatrix(int nrows, int ncols, int defaultVal) =>
        new SparseMatrixList<int>(nrows, ncols, defaultVal);
}

#endregion

#region Dictionary of keys

public class SparseMatrixDictionary<T>(int nrows, int ncols, T defaultVal) : ISparseMatrix<T>
{
    private readonly Dictionary<(int i, int j), T> _values = [];

    public T DefaultVal { get; set; } = defaultVal;

    public T this[int i, int j]
    {
        get => _values.TryGetValue((i, j), out var value) ? value : DefaultVal;
        set => _values[(i, j)] = value;
    }

    public IEnumerable<ISparseMatrix<T>.Element> GetElements() =>
        _values
            .OrderBy(kv => kv.Key.i)
            .ThenBy(kv => kv.Key.j)
            .Select(kv => new ISparseMatrix<T>.Element(kv.Key.i, kv.Key.j, kv.Value));

    public T Extract(int i, int j)
    {
        bool contains = _values.TryGetValue((i, j), out var value);
        if (contains)
            _values.Remove((i, j));

        return contains ? value! : DefaultVal;
    }

    private int GetFlatIndex(int i, int j) => i * ncols + j;

    public IEnumerator<T> GetEnumerator()
    {
        int ind = 0;
        int count = nrows * ncols;

        foreach (var elem in GetElements())
        {
            int flatIndex = GetFlatIndex(elem.I, elem.J);
            for (; ind < flatIndex; ind++)
                yield return DefaultVal;
            ind = flatIndex + 1;
            yield return elem.Value;
        }

        while (ind < count)
        {
            ind++;
            yield return DefaultVal;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class SparseMatrixDictionaryTests : SparseMatrixTests
{
    protected override ISparseMatrix<int> CreateMatrix(int nrows, int ncols, int defaultVal) =>
        new SparseMatrixDictionary<int>(nrows, ncols, defaultVal);
}

#endregion
