using System.Collections;
using HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree;

public class AvlTree<T>() : IBinarySearchTree<T>
    where T : IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node? Left;
        public Node? Right;
        public int Height;

        // Больше 0 - перегруз справа. Меньше 0 - перегруз слева
        private int GetOverload() => (Right?.Height ?? -1) - (Left?.Height ?? -1);

        public void UpdateHeight() => Height = Math.Max(Left?.Height ?? -1, Right?.Height ?? -1) + 1;

        public bool Add(T value)
        {
            if (value.Equals(Value)) return false;
            ref var child = ref value.CompareTo(Value) >= 0 ? ref Right : ref Left;
            if (child == null)
                child = new Node { Value = value };
            else
                child.Add(value);
            UpdateHeight();
            Balance();
            return true;
        }

        private void RotateToLeft()
        {
            if (Right == null) throw new InvalidOperationException();
            (Value, Right.Value) = (Right.Value, Value);
            var tmp = Right;
            Right = Right.Right;
            tmp.Right = tmp.Left;
            tmp.Left = Left;
            Left = tmp;

            Left.UpdateHeight();
            UpdateHeight();
        }
        

        private void RotateToRight()
        {
            if (Left == null) throw new InvalidOperationException();
            (Value, Left.Value) = (Left.Value, Value);
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

        public bool Contains(T target)
        {
            if (Value.Equals(target))
                return true;

            ref var child = ref target.CompareTo(Value) >= 0 ? ref Right : ref Left;
            if (child == null)
                return false;
            return child.Contains(target);
        }

        public void InOrderTraverse(Action<T> handler)
        {
            Left?.InOrderTraverse(handler);
            handler(Value);
            Right?.InOrderTraverse(handler);
        }

        public void PreOrderTraverse(Action<T> handler)
        {
            handler(Value);
            Left?.PreOrderTraverse(handler);
            Right?.PreOrderTraverse(handler);
        }

        public bool TryRemove(T target, ref Node? childLink)
        {
            bool hasRemoved;

            if (!Value.Equals(target))
            {
                if (target.CompareTo(Value) < 0)
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
                Left.TryRemove(maxNode.Value, ref Left);
                Value = maxNode.Value;
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

    public AvlTree(IEnumerable<T> values)
        : this()
    {
        foreach (var val in values)
            Add(val);
    }

    public bool Add(T value) {
        if (_root is null)
        {
            _root = new Node { Value = value };
            return true;
        }
        else
            return _root.Add(value);
    }


    public bool Contains(T value) => _root?.Contains(value) ?? false;

    public IEnumerator<T> GetEnumerator()
    {
        var list = new List<T>();
        _root?.InOrderTraverse(list.Add);
        return list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> Backup()
    {
        var list = new List<T>();
        _root?.PreOrderTraverse(list.Add);
        return list;
    }

    public bool TryRemove(T target) => _root?.TryRemove(target, ref _root) ?? false;
}

public class AvlTreeTests
{
    [Fact]
    public void Usage()
    {   
        int[] initialValues = [-5, 0, 1, 2, 4, 6, 7, 10, 15, 20];
        var bst = new AvlTree<int>(initialValues);
        Assert.Equal([-5, 0, 1, 2, 4, 6, 7, 10, 15, 20], bst);

        var backup = bst.Backup();

        Assert.Equal([2, 0, -5, 1, 10, 6, 4, 7, 15 , 20], backup);
    }
}
