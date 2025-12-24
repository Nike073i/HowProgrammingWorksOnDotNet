namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SumOfLeaves;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    task: Вернуть сумму листовых узлов
    memory: O(1 + recursive stack) ~ O(n)
    time: O(n)
*/
public class Solution
{
    public static int Sum(TreeNode? root)
    {
        if (root == null)
            return 0;
        if (root.left == null && root.right == null)
            return root.val;
        else
            return Sum(root.left) + Sum(root.right);
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSum(TreeNode root, int expected)
    {
        int actual = Solution.Sum(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, int>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6))
            ),
            15
        );
        Add(new TreeNode(5), 5);
        Add(null, 0);
        Add(new TreeNode(1, new TreeNode(2), null), 2);
        Add(new TreeNode(1, null, new TreeNode(2)), 2);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            22
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), new TreeNode(7)), null),
                new TreeNode(3, null, new TreeNode(5, new TreeNode(8), new TreeNode(9)))
            ),
            30
        );

        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), -5);
        Add(new TreeNode(0, new TreeNode(0), new TreeNode(0)), 0);
        Add(
            new TreeNode(
                1,
                new TreeNode(-2, new TreeNode(4), null),
                new TreeNode(3, new TreeNode(-5), new TreeNode(6))
            ),
            5
        );
        Add(
            new TreeNode(1, null, new TreeNode(2, null, new TreeNode(3, null, new TreeNode(4)))),
            4
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(
                    2,
                    new TreeNode(4, new TreeNode(6, new TreeNode(8), null), null),
                    null
                ),
                new TreeNode(3, null, new TreeNode(5, null, new TreeNode(7, null, new TreeNode(9))))
            ),
            17
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), null),
                new TreeNode(3, new TreeNode(5), null)
            ),
            11
        );
    }
}
