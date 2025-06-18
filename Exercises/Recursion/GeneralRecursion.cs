namespace HowProgrammingWorksOnDotNet.Exercises.Recursion;

public class GeneralRecursion
{
    private class Node
    {
        public int Value;
        public Node Left;
        public Node Right;
    }

    private void TestInOrderTraversal(Action<Node, List<int>> impl)
    {
        var root = new Node
        {
            Value = 4,
            Left = new Node
            {
                Value = 2,
                Left = new Node { Value = 1 },
                Right = new Node { Value = 3 },
            },
            Right = new Node
            {
                Value = 6,
                Left = new Node { Value = 5 },
                Right = new Node { Value = 7 },
            },
        };
        var result = new List<int>();
        impl(root, result);

        Assert.Equal([1, 2, 3, 4, 5, 6, 7], result);
    }

    private void InOrderRecursive(Node node, List<int> result)
    {
        if (node == null)
            return;

        InOrderRecursive(node.Left, result);
        result.Add(node.Value);
        InOrderRecursive(node.Right, result);
    }

    private void InOrderIterative(Node root, List<int> result)
    {
        var stack = new Stack<(Node, int)>();
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
                result.Add(node.Value);
                stack.Push((node.Right, 0));
            }
        }
    }

    [Fact]
    public void TestRecursive() => TestInOrderTraversal(InOrderRecursive);

    [Fact]
    public void TestIterative() => TestInOrderTraversal(InOrderIterative);
}
