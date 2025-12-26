namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.ZigZagLevelTraverse;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 103 https://leetcode.com/problems/binary-tree-zigzag-level-order-traversal
    time: O(n)
    memory: O(h), где h - высота дерева. Худший случай - h = n
*/
public class Solution
{
    public static IList<IList<int>> ZigzagLevelOrder(TreeNode root)
    {
        if (root == null)
            return [];

        var traversal = new List<IList<int>>();
        var queue = new Queue<(TreeNode, int)>();
        queue.Enqueue((root, 0));

        while (queue.Count > 0)
        {
            (var node, int level) = queue.Dequeue();
            if (traversal.Count <= level)
                traversal.Add([]);
            traversal[level].Add(node.val);

            if (node.left != null)
                queue.Enqueue((node.left, level + 1));

            if (node.right != null)
                queue.Enqueue((node.right, level + 1));
        }

        for (int i = 0; i < traversal.Count; i++)
        {
            if (i % 2 == 0)
                continue;
            for (int j = 0; j < traversal[i].Count / 2; j++)
            {
                (traversal[i][j], traversal[i][^(j + 1)]) = (
                    traversal[i][^(j + 1)],
                    traversal[i][j]
                );
            }
        }
        return traversal;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestZigzagLevelOrder(TreeNode root, IList<IList<int>> expected)
    {
        var actual = Solution.ZigzagLevelOrder(root);

        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }
}

public class SolutionTestData : TheoryData<TreeNode, IList<IList<int>>>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(3, new TreeNode(9), new TreeNode(20, new TreeNode(15), new TreeNode(7))),
            [
                [3],
                [20, 9],
                [15, 7],
            ]
        );
        Add(
            new TreeNode(1),
            [
                [1],
            ]
        );

        Add(null, []);
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), null),
                new TreeNode(3, null, new TreeNode(5))
            ),
            [
                [1],
                [3, 2],
                [4, 5],
            ]
        );

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            [
                [1],
                [3, 2],
                [4, 5, 6, 7],
            ]
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(7), null), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6, null, new TreeNode(8)))
            ),
            [
                [1],
                [3, 2],
                [4, 5, 6],
                [8, 7],
            ]
        ); //3
        Add(
            new TreeNode(1, new TreeNode(2, new TreeNode(3), null), null),
            [
                [1],
                [2],
                [3],
            ]
        );
        Add(
            new TreeNode(1, null, new TreeNode(2, null, new TreeNode(3))),
            [
                [1],
                [2],
                [3],
            ]
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2),
                new TreeNode(3, new TreeNode(4, null, new TreeNode(5)), null)
            ),
            [
                [1],
                [3, 2],
                [4],
                [5],
            ]
        );

        Add(
            new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)),
            [
                [-1],
                [-3, -2],
            ]
        );
        Add(
            new TreeNode(0, new TreeNode(0), new TreeNode(0, new TreeNode(0), new TreeNode(0))),
            [
                [0],
                [0, 0],
                [0, 0],
            ]
        );
    }
}
