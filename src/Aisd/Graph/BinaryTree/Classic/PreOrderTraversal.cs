namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

public class PreOrderTraversal
{
    [Fact]
    public void RecursiveTraversalTest() => Traversal.Traverse(RecursiveTraversal);

    private void RecursiveTraversal<T>(Node<T>? node, Action<T> handler)
    {
        if (node == null)
            return;

        handler(node.Value);
        RecursiveTraversal(node.Left, handler);
        RecursiveTraversal(node.Right, handler);
    }

    public void IterativeTraversal<T>(Node<T>? root, Action<T> handler)
    {
        var stack = new Stack<Node<T>?>();
        stack.Push(root);
        while (stack.Any())
        {
            var node = stack.Pop();
            if (node == null)
                continue;

            handler(node.Value);

            // Обратный порядок добавления!
            stack.Push(node.Right);
            stack.Push(node.Left);
        }
    }

    [Fact]
    public void IterativeTraversalTest() => Traversal.Traverse(IterativeTraversal);
}
