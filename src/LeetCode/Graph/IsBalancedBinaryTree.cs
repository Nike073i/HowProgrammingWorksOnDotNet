namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.IsBalancedBinaryTree;

public class TreeNode(int val)
{
    public int Val => val;
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
}

/*
    leetcode: 110 https://leetcode.com/problems/balanced-binary-tree/
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static bool IsBalanced(TreeNode? root) => Height(root) != -1;

    public static int Height(TreeNode? node)
    {
        if (node == null)
            return 0;

        int lH = Height(node.Left);
        if (lH == -1)
            return -1;

        int rH = Height(node.Right);
        if (rH == -1)
            return -1;

        int d = Math.Abs(lH - rH);

        return d > 1 ? -1 : Math.Max(lH, rH) + 1;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsBalanced(TreeNode root, bool expected)
    {
        bool actual = Solution.IsBalanced(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, bool>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(3)
            {
                Left = new TreeNode(9),
                Right = new TreeNode(20) { Left = new TreeNode(15), Right = new TreeNode(7) },
            },
            true
        );
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2)
                {
                    Left = new TreeNode(3) { Left = new TreeNode(4), Right = new TreeNode(4) },
                    Right = new TreeNode(3),
                },
                Right = new TreeNode(2),
            },
            false
        );
        Add(null, true);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(3) { Left = new TreeNode(4) } },
                Right = new TreeNode(2) { Right = new TreeNode(3) { Right = new TreeNode(4) } },
            },
            false
        );
        Add(new TreeNode(1), true);
        Add(new TreeNode(1) { Left = new TreeNode(2), Right = new TreeNode(3) }, true);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4), Right = new TreeNode(5) },
                Right = new TreeNode(3) { Left = new TreeNode(6), Right = new TreeNode(7) },
            },
            true
        );
        Add(new TreeNode(1) { Right = new TreeNode(2) { Right = new TreeNode(3) } }, false);
        Add(new TreeNode(1) { Left = new TreeNode(2) { Left = new TreeNode(3) } }, false);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4) },
                Right = new TreeNode(3) { Right = new TreeNode(5) },
            },
            true
        );
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2)
                {
                    Left = new TreeNode(4) { Left = new TreeNode(6) },
                    Right = new TreeNode(5),
                },
                Right = new TreeNode(3),
            },
            false
        );
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2),
                Right = new TreeNode(3)
                {
                    Left = new TreeNode(4),
                    Right = new TreeNode(5) { Right = new TreeNode(6) },
                },
            },
            false
        );
        Add(new TreeNode(-1) { Left = new TreeNode(-2), Right = new TreeNode(-3) }, true);
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2)
                {
                    Left = new TreeNode(4) { Left = new TreeNode(8) },
                    Right = new TreeNode(5),
                },
                Right = new TreeNode(3)
                {
                    Left = new TreeNode(6),
                    Right = new TreeNode(7) { Left = new TreeNode(9) },
                },
            },
            true
        );
        Add(
            new TreeNode(1)
            {
                Left = new TreeNode(2) { Left = new TreeNode(4) },
                Right = new TreeNode(3) { Left = new TreeNode(5), Right = new TreeNode(6) },
            },
            true
        );
    }
}
