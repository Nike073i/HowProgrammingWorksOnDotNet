namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SameTree;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 100 https://leetcode.com/problems/same-tree/
    time: O(n)
    memory: O(h), где h - высота дерева. Худший случай - h = n
*/
public class Solution
{
    public static bool IsSameTree(TreeNode p, TreeNode q)
    {
        if (p == null && q == null)
            return true;
        if (p == null || q == null)
            return false;

        return p.val == q.val && IsSameTree(p.left, q.left) && IsSameTree(p.right, q.right);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsSameTree(TreeNode p, TreeNode q, bool expected)
    {
        bool actual = Solution.IsSameTree(p, q);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, TreeNode, bool>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(1, new TreeNode(2), new TreeNode(3)),
            new TreeNode(1, new TreeNode(2), new TreeNode(3)),
            true
        );
        Add(
            new TreeNode(1, new TreeNode(2), new TreeNode(3)),
            new TreeNode(1, new TreeNode(2), new TreeNode(4)),
            false
        );
        Add(new TreeNode(1, new TreeNode(2), null), new TreeNode(1, null, new TreeNode(2)), false);
        Add(null, null, true);
        Add(null, new TreeNode(1), false);
        Add(new TreeNode(1), null, false);
        Add(new TreeNode(1), new TreeNode(1), true);
        Add(new TreeNode(1), new TreeNode(2), false);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6))
            ),
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6))
            ),
            true
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6))
            ),
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(7))
            ),
            false
        );
        Add(
            new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)),
            new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)),
            true
        );
        Add(
            new TreeNode(0, new TreeNode(0), new TreeNode(0)),
            new TreeNode(0, new TreeNode(0), new TreeNode(0)),
            true
        );
        Add(
            new TreeNode(int.MaxValue, new TreeNode(int.MinValue), null),
            new TreeNode(int.MaxValue, new TreeNode(int.MinValue), null),
            true
        );
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3), null), null),
            new TreeNode(1, new TreeNode(2), null),
            false
        );
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3), null), null),
            new TreeNode(1, null, new TreeNode(2, new TreeNode(3), null)),
            false
        );
    }
}
