namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.ArrayFromBinaryTree;

public class TreeNode(int val)
{
    public int Val => val;
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
}

/*
    task: Массив из бинарного дерева. LevelOrderTraversal
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static List<int?> FromTree(TreeNode? root)
    {
        var result = new List<int?>();

        var queue = new Queue<TreeNode?>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            result.Add(node?.Val);

            if (node != null)
            {
                queue.Enqueue(node.Left);
                queue.Enqueue(node.Right);
            }
        }

        int elements = result.Count;
        while (elements > 0 && result[elements - 1] == null)
            elements--;

        return result[..elements];
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestFromTree(TreeNode root, List<int?> expected)
    {
        var actual = Solution.FromTree(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, List<int?>>
{
    public SolutionTestData()
    {
        Add(new TreeNode(1) { Left = new TreeNode(2), Right = new TreeNode(3) }, [1, 2, 3]);
        Add(
            new TreeNode(1) { Right = new TreeNode(2) { Left = new TreeNode(3) } },
            [1, null, 2, 3]
        );
        Add(new TreeNode(1), [1]);
        Add(
            new TreeNode(5)
            {
                Left = new TreeNode(4)
                {
                    Left = new TreeNode(11) { Left = new TreeNode(7), Right = new TreeNode(2) },
                },
                Right = new TreeNode(8)
                {
                    Left = new TreeNode(13),
                    Right = new TreeNode(4) { Right = new TreeNode(1) },
                },
            },
            [5, 4, 8, 11, null, 13, 4, 7, 2, null, null, null, 1]
        );

        Add(new TreeNode(1) { Left = new TreeNode(2), Right = new TreeNode(3) }, [1, 2, 3]);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4), Right = new TreeNode(5) },
                Right = new TreeNode(3) { Left = new TreeNode(6), Right = new TreeNode(7) },
            },
            [1, 2, 3, 4, 5, 6, 7]
        );

        Add(
            new TreeNode(1) { Right = new TreeNode(2) { Right = new TreeNode(3) } },
            [1, null, 2, null, 3]
        );
        Add(new TreeNode(1) { Left = new TreeNode(2) { Left = new TreeNode(3) } }, [1, 2, null, 3]);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4) },
                Right = new TreeNode(3) { Right = new TreeNode(5) },
            },
            [1, 2, 3, 4, null, null, 5]
        );

        Add(new TreeNode(0) { Left = new TreeNode(1), Right = new TreeNode(2) }, [0, 1, 2]);
        Add(new TreeNode(-1) { Left = new TreeNode(-2), Right = new TreeNode(-3) }, [-1, -2, -3]);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Right = new TreeNode(4) },
                Right = new TreeNode(3) { Right = new TreeNode(5) },
            },
            [1, 2, 3, null, 4, null, 5]
        );
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4) { Left = new TreeNode(7) } },
                Right = new TreeNode(3)
                {
                    Left = new TreeNode(5),
                    Right = new TreeNode(6) { Left = new TreeNode(8) },
                },
            },
            [1, 2, 3, 4, null, 5, 6, 7, null, null, null, 8]
        );
    }
}
