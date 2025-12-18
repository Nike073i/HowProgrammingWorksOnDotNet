namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.MinDepthOfBinaryTree;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 111 https://leetcode.com/problems/minimum-depth-of-binary-tree/
    time: O(n)
    memory: O(n)
*/

public class Solution
{
    public static int MinDepth(TreeNode root)
    {
        if (root == null)
            return 0;

        var queue = new Queue<(TreeNode, int)>();
        queue.Enqueue((root, 1));

        while (queue.Count != 0)
        {
            (var node, int depth) = queue.Dequeue();
            if (node.left == null && node.right == null)
                return depth;

            if (node.left != null)
                queue.Enqueue((node.left, depth + 1));

            if (node.right != null)
                queue.Enqueue((node.right, depth + 1));
        }
        return 0;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMinDepth(TreeNode root, int expected)
    {
        int actual = Solution.MinDepth(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, int>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))),
            2
        );
        Add(
            new TreeNode(
                2,
                null,
                new TreeNode(3, null, new TreeNode(4, null, new TreeNode(5, null, new TreeNode(6))))
            ),
            5
        );
        Add(new TreeNode(1), 1);
        Add(null, 0);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6))
            ),
            3
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2),
                new TreeNode(3, null, new TreeNode(4, null, new TreeNode(5)))
            ),
            2
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(5), null), null),
                new TreeNode(3)
            ),
            2
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            3
        );

        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3, new TreeNode(4), null), null), null),
            4
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), null),
                new TreeNode(3, null, new TreeNode(5, null, new TreeNode(7)))
            ),
            4
        );
    }
}
