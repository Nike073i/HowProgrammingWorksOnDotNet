using System.Runtime.InteropServices;

namespace HowProgrammingWorksOnDotNet.Language.Unsafe;

public unsafe class Node(int data)
{
    public int Data = data;
    public Node* Left;
    public Node* Right;

    public void Insert(int value)
    {
        ref var child = ref value < Data ? ref Left : ref Right;

        if (child == null)
            child = CreateNode(value);
        else
            child->Insert(value);
    }

    public static Node* CreateNode(int data)
    {
        var newNode = (Node*)NativeMemory.Alloc((nuint)sizeof(Node));
        *newNode = new Node(data);
        return newNode;
    }

    public bool Contains(int target)
    {
        if (Data == target)
            return true;

        ref var child = ref target < Data ? ref Left : ref Right;

        if (child == null)
            return false;
        return child->Contains(target);
    }
}

public unsafe class Tree : IDisposable
{
    private Node* _root;

    public Tree()
    {
        _root = null;
    }

    public void Insert(int data)
    {
        if (_root == null)
            _root = Node.CreateNode(data);
        else
            _root->Insert(data);
    }

    public bool Contains(int data) => _root != null && _root->Contains(data);

    public void Dispose()
    {
        DisposeRecursive(_root);
        _root = null;
    }

    private void DisposeRecursive(Node* node)
    {
        if (node != null)
        {
            DisposeRecursive(node->Left);
            DisposeRecursive(node->Right);
            NativeMemory.Free(node);
        }
    }
}

public class Usage()
{
    [Fact]
    public void Example()
    {
        using var tree = new Tree();

        int[] values = [50, 30, 70, 20, 40, 60, 80];
        foreach (int value in values)
            tree.Insert(value);

        Console.WriteLine($"Содержит 40: {tree.Contains(40)}");
        Console.WriteLine($"Содержит 45: {tree.Contains(45)}");
    }
}
