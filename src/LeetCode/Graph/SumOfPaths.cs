namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SumOfPaths;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 129 https://leetcode.com/problems/sum-root-to-leaf-numbers/
    memory: O(1 + recursive stack) ~ O(n)
    time: O(n)
*/
public class Solution
{
    public static int SumNumbers(TreeNode? root) => Sum(root, 0);

    private static int Sum(TreeNode? node, int sum)
    {
        if (node == null)
            return 0;

        int acc = sum * 10 + node.val;

        if (node.left == null && node.right == null)
            return acc;

        return Sum(node.left, acc) + Sum(node.right, acc);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSumNumbers(TreeNode root, int expected)
    {
        int actual = Solution.SumNumbers(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, int>
{
    public SolutionTestData()
    {
        Add(new TreeNode(1, new TreeNode(2), new TreeNode(3)), 25);
        Add(
            new TreeNode(4, new TreeNode(9, new TreeNode(5), new TreeNode(1)), new TreeNode(0)),
            1026
        );
        Add(new TreeNode(1), 1);
        Add(new TreeNode(9), 9);
        Add(null, 0);
        Add(new TreeNode(0, new TreeNode(0), new TreeNode(0)), 0);
        Add(new TreeNode(0, new TreeNode(1), new TreeNode(2)), 3);
        Add(new TreeNode(9, new TreeNode(9), new TreeNode(9)), 198);
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3, new TreeNode(4), null), null), null),
            1234
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), null),
                new TreeNode(3, null, new TreeNode(5, null, new TreeNode(7)))
            ),
            2603
        );
        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), -25);
        Add(
            new TreeNode(
                5,
                new TreeNode(3, new TreeNode(7, new TreeNode(9), null), new TreeNode(4)),
                new TreeNode(2, null, new TreeNode(8, new TreeNode(6), new TreeNode(1)))
            ),
            16480
        );
        Add(new TreeNode(1, new TreeNode(1), new TreeNode(1)), 22);
    }
}
