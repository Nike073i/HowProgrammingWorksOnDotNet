using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree;

public record struct Interval : IComparable<Interval>
{
    public int Start;
    public int End;

    public readonly int CompareTo(Interval other) => Start - other.Start;

    public readonly bool IsOverlaps(Interval another) =>
        another.End >= Start && another.Start <= End;
}

public record struct Slot<T>(T Value, Interval Interval);

public class IntervalTree<T> : IEnumerable<Slot<T>>
{
    private class Node
    {
        public Interval Interval;
        public Node? Left;
        public Node? Right;
        public T Value;
        public int Height;

        // Больше 0 - перегруз справа. Меньше 0 - перегруз слева
        private int GetOverload() => (Right?.Height ?? -1) - (Left?.Height ?? -1);

        public void UpdateHeight() =>
            Height = Math.Max(Left?.Height ?? -1, Right?.Height ?? -1) + 1;

        public bool Add(Interval interval, T value)
        {
            if (Interval.IsOverlaps(interval))
                return false;

            bool added;
            ref var child = ref interval.CompareTo(Interval) >= 0 ? ref Right : ref Left;
            if (child == null)
            {
                child = new Node { Value = value, Interval = interval };
                added = true;
            }
            else
                added = child.Add(interval, value);

            if (added)
            {
                UpdateHeight();
                Balance();
            }
            return added;
        }

        private void RotateToLeft()
        {
            if (Right == null)
                throw new InvalidOperationException();
            SwapValues(this, Right);
            var tmp = Right;
            Right = Right.Right;
            tmp.Right = tmp.Left;
            tmp.Left = Left;
            Left = tmp;

            Left.UpdateHeight();
            UpdateHeight();
        }

        private void SwapValues(Node a, Node b)
        {
            (a.Value, b.Value) = (b.Value, a.Value);
            (a.Interval, b.Interval) = (b.Interval, a.Interval);
        }

        private void RotateToRight()
        {
            if (Left == null)
                throw new InvalidOperationException();
            SwapValues(this, Left);
            var tmp = Left;
            Left = Left.Left;
            tmp.Left = tmp.Right;
            tmp.Right = Right;
            Right = tmp;

            Right.UpdateHeight();
            UpdateHeight();
        }

        private void Balance()
        {
            int overload = GetOverload();

            if (overload < -1)
            {
                if (Left?.GetOverload() > 0)
                    Left.RotateToLeft();

                RotateToRight();
            }
            else if (overload > 1)
            {
                if (Right?.GetOverload() < 0)
                    Right.RotateToRight();

                RotateToLeft();
            }
        }

        public bool Contains(Interval target)
        {
            if (Interval.IsOverlaps(target))
                return true;

            ref var child = ref target.CompareTo(Interval) >= 0 ? ref Right : ref Left;
            if (child == null)
                return false;
            return child.Contains(target);
        }

        public void InOrderTraverse(Action<T, Interval> handler)
        {
            Left?.InOrderTraverse(handler);
            handler(Value, Interval);
            Right?.InOrderTraverse(handler);
        }

        public void PreOrderTraverse(Action<T, Interval> handler)
        {
            handler(Value, Interval);
            Left?.PreOrderTraverse(handler);
            Right?.PreOrderTraverse(handler);
        }

        public bool TryRemove(Interval target, ref Node? childLink)
        {
            bool hasRemoved;

            if (!Interval.Equals(target))
            {
                if (target.CompareTo(Interval) < 0)
                    hasRemoved = Left?.TryRemove(target, ref Left) ?? false;
                else
                    hasRemoved = Right?.TryRemove(target, ref Right) ?? false;
            }
            else if (Right == null || Left == null)
            {
                childLink = Left ?? Right;
                Left = Right = null;
                hasRemoved = true;
            }
            else
            {
                var maxNode = Left.Max();
                Left.TryRemove(maxNode.Interval, ref Left);
                SwapValues(this, maxNode);
                hasRemoved = true;
            }

            childLink?.UpdateHeight();
            childLink?.Balance();
            return hasRemoved;
        }

        public Node Min() => Left == null ? this : Left.Min();

        public Node Max() => Right == null ? this : Right.Max();
    }

    private Node? _root;

    public bool Add(Interval interval, T value)
    {
        if (_root is null)
        {
            _root = new Node { Value = value, Interval = interval };
            return true;
        }
        else
            return _root.Add(interval, value);
    }

    public bool Contains(Interval interval) => _root?.Contains(interval) ?? false;

    public IEnumerator<Slot<T>> GetEnumerator()
    {
        var list = new List<Slot<T>>();
        _root?.InOrderTraverse((t, interval) => list.Add(new(t, interval)));
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<Slot<T>> Backup()
    {
        var list = new List<Slot<T>>();
        _root?.PreOrderTraverse((t, interval) => list.Add(new(t, interval)));
        return list;
    }

    public bool TryRemove(Interval interval) => _root?.TryRemove(interval, ref _root) ?? false;
}

public class IntervalTreeTests
{
    private enum Type
    {
        Phone,
        Email,
        Address,
        Idiom,
        // ...
    }

    [Fact]
    public void Usage()
    {
        var intervalTree = new IntervalTree<Type>();
        Assert.Empty(intervalTree);

        AddInterval(13, 16, Type.Phone);
        AddInterval(18, 23, Type.Email);
        AddInterval(6, 9, Type.Idiom);
        AddInterval(12, 20, Type.Address);
        AddInterval(12, 17, Type.Address);
        AddInterval(12, 16, Type.Address);
        AddInterval(13, 20, Type.Address);
        AddInterval(13, 17, Type.Address);
        AddInterval(14, 20, Type.Address);
        AddInterval(14, 14, Type.Address);
        AddInterval(17, 20, Type.Address);
        AddInterval(17, 17, Type.Address);

        Print();

        Assert.True(intervalTree.TryRemove(new Interval { Start = 13, End = 16 }));
        Print();

        Assert.True(intervalTree.TryRemove(new Interval { Start = 17, End = 17 }));
        Print();

        AddInterval(10, 17, Type.Idiom);
        Print();

        void Print()
        {
            Console.WriteLine(string.Join(", ", intervalTree));
        }

        bool AddInterval(int start, int end, Type type) =>
            intervalTree.Add(new Interval { Start = start, End = end }, type);
    }
}
