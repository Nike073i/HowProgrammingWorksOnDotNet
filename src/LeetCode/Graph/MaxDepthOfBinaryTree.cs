namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.MaxDepthOfBinaryTree;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 104 https://leetcode.com/problems/maximum-depth-of-binary-tree
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static int MaxDepthBfs(TreeNode root)
    {
        if (root == null)
            return 0;
        var queue = new Queue<(TreeNode, int)>();
        queue.Enqueue((root, 1));

        int maxLevel = 0;

        while (queue.Count > 0)
        {
            (var node, int level) = queue.Dequeue();
            maxLevel = Math.Max(maxLevel, level);
            if (node.left != null)
                queue.Enqueue((node.left, level + 1));
            if (node.right != null)
                queue.Enqueue((node.right, level + 1));
        }
        return maxLevel;
    }

    public static int MaxDepthDfs(TreeNode root)
    {
        if (root == null)
            return 0;

        int left = MaxDepthDfs(root.left);
        int right = MaxDepthDfs(root.right);
        return Math.Max(left, right) + 1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMaxDepthBfs(TreeNode root, int expected)
    {
        int actual = Solution.MaxDepthBfs(root);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestMaxDepthDfs(TreeNode root, int expected)
    {
        int actual = Solution.MaxDepthDfs(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, int>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))),
            3
        );
        Add(new TreeNode(1, null, new TreeNode(2)), 2);
        Add(null, 0);
        Add(new TreeNode(1), 1);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            3
        );
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3, null, new TreeNode(4)), null), null),
            4
        );
        Add(
            new TreeNode(
                1,
                null,
                new TreeNode(2, null, new TreeNode(3, null, new TreeNode(4, null, new TreeNode(5))))
            ),
            5
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), null),
                new TreeNode(3, null, new TreeNode(5, new TreeNode(6), new TreeNode(7)))
            ),
            4
        );
        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), 2);
        Add(new TreeNode(0, new TreeNode(0), new TreeNode(0, new TreeNode(0), null)), 3);
        Add(
            new TreeNode(
                1,
                new TreeNode(
                    2,
                    new TreeNode(3, new TreeNode(4, new TreeNode(5), null), null),
                    null
                ),
                new TreeNode(6, null, new TreeNode(7, null, new TreeNode(8)))
            ),
            5
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), new TreeNode(5)),
                new TreeNode(3)
            ),
            4
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2),
                new TreeNode(3, new TreeNode(4), new TreeNode(5, null, new TreeNode(6)))
            ),
            4
        );
    }
}
