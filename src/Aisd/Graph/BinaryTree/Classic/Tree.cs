namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Left { get; set; }
    public Node<T>? Right { get; set; }
}

public class Traversal
{
    public static void Traverse(Action<Node<int>, Action<int>> impl)
    {
        var root = new Node<int>
        {
            Value = 4,
            Left = new()
            {
                Value = 2,
                Left = new() { Value = 1 },
                Right = new() { Value = 3 },
            },
            Right = new()
            {
                Value = 6,
                Left = new() { Value = 5 },
                Right = new() { Value = 7 },
            },
        };

        impl(root, Console.WriteLine);
    }
}
