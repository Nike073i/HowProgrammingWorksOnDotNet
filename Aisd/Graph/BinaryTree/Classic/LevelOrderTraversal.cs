namespace HowProgrammingWorksOnDotNet.Aisd.Graph.BinaryTree.Classic;

public class LevelOrderTraversal
{
    public void IterativeTraversal<T>(Node<T>? root, Action<T> handler)
    {
        var queue = new Queue<Node<T>?>();
        queue.Enqueue(root);
        while (queue.Any())
        {
            var node = queue.Dequeue();
            if (node == null)
                continue;

            handler(node.Value);
            queue.Enqueue(node.Left);
            queue.Enqueue(node.Right);
        }
    }

    [Fact]
    public void IterativeTraversalTest() => Traversal.Traverse(IterativeTraversal);
}
