namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.BinaryTreeFromArray;

public class TreeNode(int val)
{
    public int Val => val;
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
}

/*
    task: Создать бинарное дерево из массива значений
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    private static int GetLeftInd(int parent) => (parent + 1) * 2 - 1;

    private static int GetRightInd(int parent) => (parent + 1) * 2;

    private static TreeNode? InternalRecursiveCreateTree(int?[] values, int i)
    {
        if (i >= values.Length || !values[i].HasValue)
            return null;

        return new TreeNode(values[i]!.Value)
        {
            Left = InternalRecursiveCreateTree(values, GetLeftInd(i)),
            Right = InternalRecursiveCreateTree(values, GetRightInd(i)),
        };
    }

    public static TreeNode? RecursiveCreateTree(int?[] values) =>
        InternalRecursiveCreateTree(values, 0);

    public static TreeNode? CreateTree(int?[] values)
    {
        if (values.Length == 0 || !values[0].HasValue)
            return null;

        var nodes = new TreeNode?[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].HasValue)
                nodes[i] = new TreeNode(values[i].Value);
        }
        for (int i = 0; i < values.Length; i++)
        {
            if (nodes[i] == null)
                continue;

            int leftInd = GetLeftInd(i);
            int rightInd = GetRightInd(i);

            if (leftInd < values.Length)
                nodes[i].Left = nodes[leftInd];

            if (rightInd < values.Length)
                nodes[i].Right = nodes[rightInd];
        }
        return nodes[0];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestCreateTree(int?[] values, int?[] expectedInOrder)
    {
        TreeNode? root = Solution.CreateTree(values);
        var actualInOrder = InOrderTraversal(root);
        Assert.Equal(expectedInOrder, actualInOrder);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestRecursiveCreateTree(int?[] values, int?[] expectedInOrder)
    {
        TreeNode? root = Solution.RecursiveCreateTree(values);
        var actualInOrder = InOrderTraversal(root);
        Assert.Equal(expectedInOrder, actualInOrder);
    }

    private static int?[] InOrderTraversal(TreeNode? root)
    {
        var result = new List<int?>();
        InOrder(root, result);
        return [.. result];
    }

    private static void InOrder(TreeNode? node, List<int?> result)
    {
        if (node == null)
            return;

        InOrder(node.Left, result);
        result.Add(node.Val);
        InOrder(node.Right, result);
    }
}

public class SolutionTestData : TheoryData<int?[], int?[]>
{
    public SolutionTestData()
    {
        Add([1, 2, 3], [2, 1, 3]);
        Add([], []);
        Add([5, 4, 8, 11, null, 13, 4, 7, 2, null, null, null, 1], [7, 11, 2, 4, 5, 13, 1, 8, 4]);
        Add([1, 2, 3], [2, 1, 3]);
        Add([1, 2, 3, 4, 5, 6, 7], [4, 2, 5, 1, 6, 3, 7]);
        Add([1, null, 2, null, null, null, 3], [1, 2, 3]);
        Add([1, 2, null, 3], [3, 2, 1]);
        Add([null], []);
        Add([0, 1, 2], [1, 0, 2]);
        Add([-1, 2, -3], [2, -1, -3]);
        Add([int.MaxValue, int.MinValue, 0], [int.MinValue, int.MaxValue, 0]);
        Add([1, 2, 3, 4, null, null, 5], [4, 2, 1, 3, 5]);
        Add([1, 2, 3, 4, null, 5, 6, 7, null, null, null, 8], [7, 4, 2, 1, 8, 5, 3, 6]);
        Add([1], [1]);
        Add([1, null, null, null, null], [1]);
    }
}
