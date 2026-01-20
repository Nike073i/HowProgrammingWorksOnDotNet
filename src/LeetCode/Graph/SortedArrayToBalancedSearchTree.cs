namespace HowProgrammingWorksOnDotNet.LeetCode.Graph.SortedArrayToBalancedSearchTree;

public class TreeNode(int val = 0, TreeNode? left = null, TreeNode? right = null)
{
    public int val = val;
    public TreeNode? left = left;
    public TreeNode? right = right;
}

/*
    leetcode: 108 https://leetcode.com/problems/convert-sorted-array-to-binary-search-tree
    memory: O(1 + recursive stack) ~ O(log n)
    time: O(n)
*/
public class Solution
{
    public static TreeNode? SortedArrayToBST(int[] nums)
    {
        return GetNode(0, nums.Length - 1);

        TreeNode? GetNode(int l, int r)
        {
            if (l > r)
                return null;
            int middle = l + (r - l) / 2;
            var node = new TreeNode(nums[middle]);
            if (l == r)
                return node;
            node.left = GetNode(l, middle - 1);
            node.right = GetNode(middle + 1, r);
            return node;
        }
    }
}
