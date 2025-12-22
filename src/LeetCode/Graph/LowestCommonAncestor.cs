namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.LowestCommonAncestor;

public class TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
{
    public int val = val;
    public TreeNode left = left;
    public TreeNode right = right;
}

/*
    leetcode: 236 https://leetcode.com/problems/lowest-common-ancestor-of-a-binary-tree
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static TreeNode? LowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q)
    {
        if (root == null || root == p || root == q)
            return root;

        var left = LowestCommonAncestor(root.left, p, q);
        var right = LowestCommonAncestor(root.right, p, q);
        if (left != null && right != null)
            return root;

        return left ?? right;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestLowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q, TreeNode? expected)
    {
        TreeNode? actual = Solution.LowestCommonAncestor(root, p, q);
        Assert.Equal(expected, actual);
    }
}

public class SolutionTestData : TheoryData<TreeNode, TreeNode, TreeNode, TreeNode?>
{
    public SolutionTestData()
    {
        TreeNode node3 = new(3);
        TreeNode node5 = new(5);
        TreeNode node1 = new(1);
        TreeNode node6 = new(6);
        TreeNode node2 = new(2);
        TreeNode node0 = new(0);
        TreeNode node8 = new(8);
        TreeNode node7 = new(7);
        TreeNode node4 = new(4);

        node3.left = node5;
        node3.right = node1;
        node5.left = node6;
        node5.right = node2;
        node1.left = node0;
        node1.right = node8;
        node2.left = node7;
        node2.right = node4;

        Add(node3, node5, node1, node3);
        Add(node3, node5, node4, node5);

        TreeNode single = new(1);
        Add(single, single, single, single);

        TreeNode linear1 = new(1);
        TreeNode linear2 = new(2);
        TreeNode linear3 = new(3);
        linear1.right = linear2;
        linear2.right = linear3;

        Add(linear1, linear1, linear3, linear1);
        Add(linear1, linear2, linear3, linear2);

        TreeNode left1 = new(1);
        TreeNode left2 = new(2);
        TreeNode left3 = new(3);
        left3.left = left2;
        left2.left = left1;

        Add(left3, left1, left3, left3);
        Add(left3, left1, left2, left2);

        TreeNode bal1 = new(1);
        TreeNode bal2 = new(2);
        TreeNode bal3 = new(3);
        TreeNode bal4 = new(4);
        TreeNode bal5 = new(5);
        TreeNode bal6 = new(6);
        TreeNode bal7 = new(7);

        bal1.left = bal2;
        bal1.right = bal3;
        bal2.left = bal4;
        bal2.right = bal5;
        bal3.left = bal6;
        bal3.right = bal7;

        Add(bal1, bal4, bal5, bal2);
        Add(bal1, bal4, bal6, bal1);
        Add(bal1, bal4, bal7, bal1);
        Add(bal1, bal6, bal7, bal3);

        TreeNode anc1 = new(1);
        TreeNode anc2 = new(2);
        TreeNode anc3 = new(3);
        TreeNode anc4 = new(4);
        TreeNode anc5 = new(5);

        anc1.left = anc2;
        anc1.right = anc3;
        anc2.left = anc4;
        anc2.right = anc5;

        Add(anc1, anc2, anc5, anc2);
        Add(anc1, anc4, anc2, anc2);

        TreeNode negRoot = new(-1);
        TreeNode negLeft = new(-2);
        TreeNode negRight = new(-3);
        negRoot.left = negLeft;
        negRoot.right = negRight;

        Add(negRoot, negLeft, negRight, negRoot);

        TreeNode bigRoot = new(int.MaxValue);
        TreeNode bigLeft = new(int.MaxValue - 1);
        TreeNode bigRight = new(int.MaxValue - 2);
        bigRoot.left = bigLeft;
        bigRoot.right = bigRight;

        Add(bigRoot, bigLeft, bigRight, bigRoot);

        TreeNode dup1 = new(1);
        TreeNode dup2 = new(1);
        TreeNode dup3 = new(2);
        dup1.left = dup2;
        dup1.right = dup3;

        Add(dup1, dup1, dup3, dup1);
        Add(dup1, dup2, dup3, dup1);

        TreeNode comp1 = new(1);
        TreeNode comp2 = new(2);
        TreeNode comp3 = new(3);
        TreeNode comp4 = new(4);
        TreeNode comp5 = new(5);
        TreeNode comp6 = new(6);
        TreeNode comp7 = new(7);
        TreeNode comp8 = new(8);
        TreeNode comp9 = new(9);
        TreeNode comp10 = new(10);
        TreeNode comp11 = new(11);

        comp1.left = comp2;
        comp1.right = comp3;
        comp2.left = comp4;
        comp2.right = comp5;
        comp3.left = comp6;
        comp3.right = comp7;
        comp4.left = comp8;
        comp4.right = comp9;
        comp7.left = comp10;
        comp7.right = comp11;

        Add(comp1, comp8, comp9, comp4);
        Add(comp1, comp8, comp5, comp2);
        Add(comp1, comp8, comp10, comp1);
        Add(comp1, comp6, comp11, comp3);
    }
}
