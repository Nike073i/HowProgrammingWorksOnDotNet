namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SumOfPathsToLeaf;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 113 https://leetcode.com/problems/path-sum-ii/description/
    memory: O(n)
    time: O(n)
*/
public class Solution
{
    public static IList<IList<int>> PathSum(TreeNode root, int targetSum)
    {
        IList<IList<int>> result = [];
        List<int> path = [];
        Dfs(root, 0);
        return result;

        void Dfs(TreeNode? node, int acc)
        {
            if (node == null)
                return;

            int sum = node.val + acc;

            if (node.left == null && node.right == null)
            {
                if (sum == targetSum)
                {
                    result.Add([.. path, node.val]);
                }
            }
            else
            {
                path.Add(node.val);
                Dfs(node.left, sum);
                Dfs(node.right, sum);
                path.RemoveAt(path.Count - 1);
            }
        }
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestPathSum(TreeNode root, int targetSum, IList<IList<int>> expected)
    {
        var actual = Solution.PathSum(root, targetSum);

        var sortedExpected = expected
            .Select(list => list.OrderBy(x => x).ToList())
            .OrderBy(list => string.Join(",", list));

        var sortedActual = actual
            .Select(list => list.OrderBy(x => x).ToList())
            .OrderBy(list => string.Join(",", list));

        Assert.True(sortedExpected.Zip(sortedActual).All(t => t.First.SequenceEqual(t.Second)));
    }
}

public class SolutionTestData : TheoryData<TreeNode, int, IList<IList<int>>>
{
    public SolutionTestData()
    {
        Add(
            new TreeNode(
                5,
                new TreeNode(4, new TreeNode(11, new TreeNode(7), new TreeNode(2)), null),
                new TreeNode(8, new TreeNode(13), new TreeNode(4, new TreeNode(5), new TreeNode(1)))
            ),
            22,
            [
                [5, 4, 11, 2],
                [5, 8, 4, 5],
            ]
        );

        Add(new TreeNode(1, new TreeNode(2), new TreeNode(3)), 5, []);

        Add(new TreeNode(1, new TreeNode(2), new TreeNode(3)), 0, []);

        Add(null, 0, []);

        Add(
            new TreeNode(5),
            5,
            [
                [5],
            ]
        );

        Add(new TreeNode(5), 3, []);

        Add(
            new TreeNode(-2, null, new TreeNode(-3)),
            -5,
            [
                [-2, -3],
            ]
        );

        Add(
            new TreeNode(0, new TreeNode(0), new TreeNode(0)),
            0,
            [
                [0, 0],
                [0, 0],
            ]
        );

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(1), new TreeNode(1)),
                new TreeNode(3, null, new TreeNode(2))
            ),
            4,
            [
                [1, 2, 1],
                [1, 2, 1],
                [1, 3],
            ]
        );

        Add(
            new TreeNode(
                5,
                new TreeNode(4, new TreeNode(11, new TreeNode(7), new TreeNode(2)), null),
                new TreeNode(8)
            ),
            22,
            [
                [5, 4, 11, 2],
            ]
        );

        Add(
            new TreeNode(
                5,
                null,
                new TreeNode(8, new TreeNode(13), new TreeNode(4, null, new TreeNode(1)))
            ),
            18,
            [
                [5, 8, 4, 1],
            ]
        );

        Add(
            new TreeNode(1, null, new TreeNode(2, null, new TreeNode(3, null, new TreeNode(4)))),
            10,
            [
                [1, 2, 3, 4],
            ]
        );

        Add(
            new TreeNode(
                1,
                new TreeNode(2, new TreeNode(4), new TreeNode(5)),
                new TreeNode(3, null, new TreeNode(6, new TreeNode(7), new TreeNode(8)))
            ),
            8,
            [
                [1, 2, 5],
            ]
        );

        Add(
            new TreeNode(
                100,
                new TreeNode(50, new TreeNode(25), new TreeNode(75)),
                new TreeNode(150)
            ),
            225,
            [
                [100, 50, 75],
            ]
        );

        Add(
            new TreeNode(
                -1,
                new TreeNode(-2, new TreeNode(-4), new TreeNode(-5)),
                new TreeNode(-3, new TreeNode(-6), new TreeNode(-7))
            ),
            -8,
            [
                [-1, -2, -5],
            ]
        );
    }
}
