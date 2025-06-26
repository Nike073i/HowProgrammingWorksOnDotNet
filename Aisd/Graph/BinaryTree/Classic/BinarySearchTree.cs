using System.Collections;
using System.Numerics;
using Xunit.Sdk;

namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

#region Contracts

public interface IBinarySearchTree<T> : IEnumerable<T>
{
    void Add(T value);
    bool Contains(T target);
    bool TryRemove(T target);
}

#endregion

#region Tests

public abstract class BinarySearchTreeTests
{
    protected abstract IBinarySearchTree<int> CreateTree(IEnumerable<int> values);

    [Fact]
    public void Usage()
    {
        int[] initialValues = [7, 1, 4, 0, 10, 15, -5, 20, 6, 4, 2];
        var bst = CreateTree(initialValues);

        Assert.Equal([-5, 0, 1, 2, 4, 4, 6, 7, 10, 15, 20], bst);
        Assert.True(bst.Contains(0));
        Assert.True(bst.Contains(-5));
        Assert.True(bst.Contains(20));
        Assert.True(bst.Contains(15));
        Assert.True(bst.Contains(4));
        Assert.True(bst.Contains(6));
        Assert.False(bst.Contains(3));
        Assert.False(bst.Contains(5));
        Assert.False(bst.Contains(-4));
        Assert.False(bst.Contains(19));
        Assert.False(bst.Contains(16));

        Assert.False(bst.TryRemove(5));
        Assert.False(bst.TryRemove(-2));

        Assert.True(bst.TryRemove(15));
        Assert.Equal([-5, 0, 1, 2, 4, 4, 6, 7, 10, 20], bst);

        Assert.True(bst.TryRemove(7));
        Assert.Equal([-5, 0, 1, 2, 4, 4, 6, 10, 20], bst);

        bst.Add(5);
        Assert.Equal([-5, 0, 1, 2, 4, 4, 5, 6, 10, 20], bst);

        Assert.True(bst.TryRemove(4));
        Assert.Equal([-5, 0, 1, 2, 4, 5, 6, 10, 20], bst);

        Assert.True(bst.TryRemove(4));
        Assert.Equal([-5, 0, 1, 2, 5, 6, 10, 20], bst);

        bst.Add(-4);
        bst.Add(3);
        bst.Add(5);
        bst.Add(7);
        bst.Add(14);
        Assert.Equal([-5, -4, 0, 1, 2, 3, 5, 5, 6, 7, 10, 14, 20], bst);
    }

    [Fact]
    public void SaveDataWithPreOrderTraversal()
    {
        int[] initialValues = [7, 1, 4, 0, 10, 15, -5, 20, 6, 4, 2];
        var tree = new BinarySearchTreeIterative<int>(initialValues);

        int[] inOrder = [-5, 0, 1, 2, 4, 4, 6, 7, 10, 15, 20];

        Assert.Equal(inOrder, tree);

        var backup = tree.Backup();
        Assert.Equal([7, 1, 0, -5, 4, 2, 6, 4, 10, 15, 20], tree);

        var fromBackup = new BinarySearchTreeIterative<int>(backup);

        Assert.Equal(inOrder, fromBackup);
    }
}

#endregion

#region Iterative

public class BinarySearchTreeIterativeTests : BinarySearchTreeTests
{
    protected override IBinarySearchTree<int> CreateTree(IEnumerable<int> values) =>
        new BinarySearchTreeIterative<int>(values);
}

public class BinarySearchTreeIterative<T>() : IBinarySearchTree<T>
    where T : IComparable<T>, IMinMaxValue<T>
{
    private class Node
    {
        public T Value;
        public Node? Left;
        public Node? Right;
    }

    private readonly Node _root = new Node { Value = T.MinValue };

    public BinarySearchTreeIterative(IEnumerable<T> values)
        : this()
    {
        foreach (var val in values)
            Add(val);
    }

    public void Add(T value)
    {
        var current = _root;
        var node = new Node { Value = value };
        while (true)
        {
            if (value.CompareTo(current.Value) < 0)
            {
                if (current.Left == null)
                {
                    current.Left = node;
                    break;
                }
                else
                    current = current.Left;
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = node;
                    break;
                }
                current = current.Right;
            }
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var depth = new Stack<Node>();
        Node? current = _root.Right;
        while (depth.Any() || current != null)
        {
            while (current != null)
            {
                depth.Push(current);
                current = current.Left;
            }
            current = depth.Pop();
            yield return current.Value;
            current = current.Right;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool Contains(T target)
    {
        var tmp = _root.Right;
        while (tmp != null)
        {
            if (tmp.Value.Equals(target))
                return true;

            if (tmp.Value.CompareTo(target) > 0)
                tmp = tmp.Left;
            else
                tmp = tmp.Right;
        }
        return false;
    }

    public bool TryRemove(T target)
    {
        var parent = _root;
        var removable = _root.Right;

        while (removable != null && !removable.Value.Equals(target))
        {
            parent = removable;
            removable = target.CompareTo(removable.Value) < 0 ? removable.Left : removable.Right;
        }
        if (removable == null)
            return false;

        Node? replacement;

        if (removable.Right == null)
            replacement = removable.Left;
        else if (removable.Left == null)
            replacement = removable.Right;
        else
        {
            var tmp = removable.Left;

            if (tmp.Right == null)
            {
                replacement = tmp;
                replacement.Right = removable.Right;
            }
            else
            {
                while (tmp.Right!.Right != null)
                    tmp = tmp.Right;

                var maxNode = tmp.Right;
                tmp.Right = tmp.Right.Left;

                maxNode.Left = removable.Left;
                maxNode.Right = removable.Right;

                replacement = maxNode;
            }
        }

        removable.Left = null;
        removable.Right = null;

        if (parent.Left == removable)
            parent.Left = replacement;
        else
            parent.Right = replacement;

        return true;
    }

    public IEnumerable<T> Backup()
    {
        var stack = new Stack<Node?>();
        stack.Push(_root.Right);
        while (stack.Any())
        {
            var node = stack.Pop();
            if (node == null)
                continue;

            yield return node.Value;

            stack.Push(node.Right);
            stack.Push(node.Left);
        }
    }
}

#endregion

#region Recursive

public class BinarySearchTreeRecursiveTests : BinarySearchTreeTests
{
    protected override IBinarySearchTree<int> CreateTree(IEnumerable<int> values) =>
        new BinarySearchTreeRecursive<int>(values);
}

public class BinarySearchTreeRecursive<T>() : IBinarySearchTree<T>
    where T : IMinMaxValue<T>, IComparable<T>
{
    private class Node
    {
        public T Value;
        public Node? Left;
        public Node? Right;

        public void Add(T value)
        {
            ref var child = ref value.CompareTo(Value) >= 0 ? ref Right : ref Left;
            if (child == null)
                child = new Node { Value = value };
            else
                child.Add(value);
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

        public bool TryRemove(T target, ref Node? parentLink)
        {
            if (!Value.Equals(target))
            {
                if (target.CompareTo(Value) < 0)
                    return Left?.TryRemove(target, ref Left) ?? false;
                else
                    return Right?.TryRemove(target, ref Right) ?? false;
            }

            if (Right == null)
                parentLink = Left;
            else if (Left == null)
                parentLink = Right;
            else
            {
                var maxNode = Left.Max();
                Left.TryRemove(maxNode.Value, ref Left);
                Value = maxNode.Value;
            }
            return true;
        }

        public Node Min() => Left == null ? this : Left.Min();

        public Node Max() => Right == null ? this : Right.Max();
    }

    private readonly Node _root = new() { Value = T.MinValue };

    public BinarySearchTreeRecursive(IEnumerable<T> values)
        : this()
    {
        foreach (var val in values)
            Add(val);
    }

    public void Add(T value) => _root.Add(value);

    public bool Contains(T value) => _root.Contains(value);

    public IEnumerator<T> GetEnumerator()
    {
        if (IsEmpty)
            return Enumerable.Empty<T>().GetEnumerator();

        var list = new List<T>();
        _root.Right!.InOrderTraverse(list.Add);
        return list.GetEnumerator();
    }

    public IEnumerable<T> Backup()
    {
        if (IsEmpty)
            return [];

        var list = new List<T>();
        _root.Right!.PreOrderTraverse(list.Add);
        return list;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public bool IsEmpty => _root.Right == null;

    public bool TryRemove(T target)
    {
        if (IsEmpty)
            return false;
        return _root.Right!.TryRemove(target, ref _root.Right);
    }
}

#endregion
