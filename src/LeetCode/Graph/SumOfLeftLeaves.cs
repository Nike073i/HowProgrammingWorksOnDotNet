namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SumOfLeftLeaves;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 404 https://leetcode.com/problems/sum-of-left-leaves/description/
    memory: O(n)
    time: O(n)
*/
public class Solution
{
    public static int SumOfLeftLeaves(TreeNode? root) => Recursive(root, false);

    public static int Recursive(TreeNode node, bool isLeft)
    {
        if (node == null)
            return 0;

        if (node.left == null && node.right == null && isLeft)
            return node.val;

        return Recursive(node.left, true) + Recursive(node.right, false);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSumOfLeftLeaves(TreeNode root, int expected)
    {
        int actual = Solution.SumOfLeftLeaves(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, int>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))),
            24
        );
        Add(new TreeNode(1), 0);
        Add(null, 0);
        Add(new TreeNode(1, new TreeNode(2, new TreeNode(3), null), null), 3);
        Add(new TreeNode(1, null, new TreeNode(2, null, new TreeNode(3))), 0);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            10
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), new TreeNode(7)), null),
                new TreeNode(3, null, new TreeNode(5, new TreeNode(8), new TreeNode(9)))
            ),
            14
        );
        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), -2);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), null),
                new TreeNode(3, null, new TreeNode(5, null, new TreeNode(7)))
            ),
            6
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(3, new TreeNode(5), new TreeNode(6)), new TreeNode(4)),
                null
            ),
            5
        );
        Add(new TreeNode(1, null, new TreeNode(2, new TreeNode(3), new TreeNode(4))), 3);
        Add(new TreeNode(1, new TreeNode(2), new TreeNode(3, new TreeNode(4), null)), 6);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(8), null), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6, null, new TreeNode(9)), new TreeNode(7))
            ),
            8
        );
    }
}
