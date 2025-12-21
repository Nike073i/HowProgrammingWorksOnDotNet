namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.LevelOrderTraverse;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 102 https://leetcode.com/problems/binary-tree-level-order-traversal/
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static IList<IList<int>> LevelOrder(TreeNode root)
    {
        var output = new List<IList<int>>();
        if (root == null)
            return output;

        int currentLevel = 0;
        var queue = new Queue<(TreeNode, int)>();
        queue.Enqueue((root, 1));

        while (queue.Count > 0)
        {
            (var node, int level) = queue.Dequeue();
            if (currentLevel != level)
            {
                output.Add([]);
                currentLevel = level;
            }
            output[^1].Add(node.val);
            if (node.left != null)
                queue.Enqueue((node.left, level + 1));
            if (node.right != null)
                queue.Enqueue((node.right, level + 1));
        }
        return output;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestLevelOrder(TreeNode root, IList<IList<int>> expected)
    {
        var actual = Solution.LevelOrder(root);

        Assert.Equal(expected.Count, actual.Count);

        Assert.True(expected.Zip(actual).All(tuple => tuple.First.SequenceEqual(tuple.Second)));
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
                [9, 20],
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
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, new TreeNode(6), new TreeNode(7))
            ),
            [
                [1],
                [2, 3],
                [4, 5, 6, 7],
            ]
        ); 
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
                new TreeNode(2, new TreeNode(4), null),
                new TreeNode(3, null, new TreeNode(5))
            ),
            [
                [1],
                [2, 3],
                [4, 5],
            ]
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2),
                new TreeNode(3, new TreeNode(4), new TreeNode(5, new TreeNode(6), new TreeNode(7)))
            ),
            [
                [1],
                [2, 3],
                [4, 5],
                [6, 7],
            ]
        );

        Add(
            new TreeNode(-1, new TreeNode(-2), new TreeNode(-3)),
            [
                [-1],
                [-2, -3],
            ]
        );

        Add(
            new TreeNode(0, new TreeNode(0), new TreeNode(0)),
            [
                [0],
                [0, 0],
            ]
        );

        Add(
            new TreeNode(int.MaxValue, new TreeNode(int.MinValue), null),
            [
                [int.MaxValue],
                [int.MinValue],
            ]
        );
        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4, new TreeNode(7), null), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6, new TreeNode(8), new TreeNode(9)))
            ),
            [
                [1],
                [2, 3],
                [4, 5, 6],
                [7, 8, 9],
            ]
        );
    }
}
