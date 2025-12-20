namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SymmetricTree;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 101 https://leetcode.com/problems/symmetric-tree
    time: O(n)
    memory: O(h), где h - высота дерева. Худший случай - h = n
*/
public class Solution
{
    public static bool IsSymmetric(TreeNode root) => IsSymmetric(root.left, root.right);

    private static bool IsSymmetric(TreeNode l, TreeNode r)
    {
        if (l == null && r == null)
            return true;
        if (l == null || r == null)
            return false;
        return l.val == r.val && IsSymmetric(l.right, r.left) && IsSymmetric(l.left, r.right);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsSymmetric(TreeNode root, bool expected)
    {
        bool actual = Solution.IsSymmetric(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, bool>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(3), new TreeNode(4)),
                new TreeNode(2, new TreeNode(4), new TreeNode(3))
            ),
            true
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, null, new TreeNode(3)),
                new TreeNode(2, null, new TreeNode(3))
            ),
            false
        );
        Add(new TreeNode(1), true);
        Add(new TreeNode(1, new TreeNode(2), null), false);
        Add(new TreeNode(1, null, new TreeNode(2)), false);
        Add(new TreeNode(1, new TreeNode(2), new TreeNode(2)), true);
        Add(new TreeNode(1, new TreeNode(2), new TreeNode(3)), false);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(3, new TreeNode(5), null), new TreeNode(4)),
                new TreeNode(2, new TreeNode(4), new TreeNode(3, new TreeNode(5), null))
            ),
            false
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(3), null),
                new TreeNode(2, new TreeNode(3), null)
            ),
            false
        );
        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-2)), true);
        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), false);
        Add(new TreeNode(0, new TreeNode(0), new TreeNode(0)), true);
        Add(
            new TreeNode(int.MaxValue, new TreeNode(int.MinValue), new TreeNode(int.MinValue)),
            true
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(3, new TreeNode(4), null), null),
                new TreeNode(2, null, new TreeNode(3, null, new TreeNode(4)))
            ),
            true
        );
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3), new TreeNode(4)), new TreeNode(2)),
            false
        );
    }
}
