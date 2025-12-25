namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.RightSideView;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 199 https://leetcode.com/problems/binary-tree-right-side-view
    time: O(n)
    memory: O(h), где h - высота дерева. Худший случай - h = n
*/
public class Solution
{
    public static IList<int> RightSideView(TreeNode root)
    {
        var result = new List<int>();
        if (root == null)
            return result;

        var queue = new Queue<(TreeNode, int)>();
        queue.Enqueue((root, 0));
        while (queue.Count > 0)
        {
            (var node, int level) = queue.Dequeue();
            if (result.Count <= level)
            {
                result.Add(0);
            }
            result[level] = node.val;

            if (node.left != null)
                queue.Enqueue((node.left, level + 1));

            if (node.right != null)
                queue.Enqueue((node.right, level + 1));
        }
        return result;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestRightSideView(TreeNode root, IList<int> expected)
    {
        IList<int> actual = Solution.RightSideView(root);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, IList<int>>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(
                1,
                new TreeNode(2, null, new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(4))
            ),
            [1, 3, 4]
        );

        Add(new TreeNode(1, null, new TreeNode(3)), [1, 3]);

        Add(null, []);

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            [1, 3, 7]
        );

        Add(new TreeNode(1, new TreeNode(2, new TreeNode(4), null), new TreeNode(3)), [1, 3, 4]);

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(6), null), null),
                new TreeNode(3, null, new TreeNode(5))
            ),
            [1, 3, 5, 6]
        );

        Add(new TreeNode(1, new TreeNode(2, new TreeNode(3), null), null), [1, 2, 3]);

        Add(new TreeNode(1, null, new TreeNode(2, null, new TreeNode(3))), [1, 2, 3]);

        Add(new TreeNode(1), [1]);

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(7), null), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6, null, new TreeNode(8)))
            ),
            [1, 3, 6, 8]
        );

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(7), null), null),
                new TreeNode(3, new TreeNode(5, new TreeNode(8), null), new TreeNode(6))
            ),
            [1, 3, 6, 8]
        );

        Add(new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)), [-1, -3]);

        Add(new TreeNode(0, new TreeNode(0, new TreeNode(0), null), new TreeNode(0)), [0, 0, 0]);
    }
}
