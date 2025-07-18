namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

public class SymmetricTraversal
{
    [Fact]
    public void RecursiveTraversalTest() => Traversal.Traverse(RecursiveTraversal);

    private void RecursiveTraversal<T>(Node<T>? node, Action<T> handler)
    {
        if (node == null)
            return;

        RecursiveTraversal(node.Left, handler);
        handler(node.Value);
        RecursiveTraversal(node.Right, handler);
    }

    private void ManualRecursiveTraversal<T>(Node<T>? root, Action<T> handler)
    {
        var stack = new Stack<(Node<T>?, int)>();
        stack.Push((root, 0));
        while (stack.Any())
        {
            (var node, int section) = stack.Pop();
            if (node == null)
                continue;
            if (section == 0)
            {
                stack.Push((node, 1));
                stack.Push((node.Left, 0));
            }
            if (section == 1)
            {
                handler(node.Value);
                stack.Push((node.Right, 0));
            }
        }
    }

    [Fact]
    public void ManualRecursiveTraversalTest() => Traversal.Traverse(ManualRecursiveTraversal);

    public void IterativeTraversal<T>(Node<T>? root, Action<T> handler)
    {
        var depth = new Stack<Node<T>>();
        Node<T>? current = root;
        while (depth.Any() || current != null)
        {
            while (current != null)
            {
                depth.Push(current);
                current = current.Left;
            }
            current = depth.Pop();
            handler(current.Value);
            current = current.Right;
        }
    }

    [Fact]
    public void IterativeTraversalTest() => Traversal.Traverse(IterativeTraversal);
}
