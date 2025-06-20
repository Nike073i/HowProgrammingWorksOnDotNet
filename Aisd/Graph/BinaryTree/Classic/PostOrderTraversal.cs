namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

public class PostOrderTraversal
{
    [Fact]
    public void RecursiveTraversalTest() => Traversal.Traverse(RecursiveTraversal);

    private void RecursiveTraversal<T>(Node<T>? node, Action<T> handler)
    {
        if (node == null)
            return;

        RecursiveTraversal(node.Left, handler);
        RecursiveTraversal(node.Right, handler);
        handler(node.Value);
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
                stack.Push((node, 2));
                stack.Push((node.Right, 0));
            }
            if (section == 2)
                handler(node.Value);
        }
    }

    [Fact]
    public void ManualRecursiveTraversalTest() => Traversal.Traverse(ManualRecursiveTraversal);

    public void IterativeTraversal<T>(Node<T>? root, Action<T> handler)
    {
        var depth = new Stack<Node<T>>();
        Node<T>? lastVisited = null;
        var current = root;
        while (depth.Any() || current != null)
        {
            while (current != null)
            {
                depth.Push(current);
                current = current.Left;
            }

            var order = depth.Peek();
            if (order.Right == null || order.Right == lastVisited)
            {
                lastVisited = depth.Pop();
                handler(order.Value);
            }
            else
                current = order.Right;
        }
    }

    [Fact]
    public void IterativeTraversalTest() => Traversal.Traverse(IterativeTraversal);
}
