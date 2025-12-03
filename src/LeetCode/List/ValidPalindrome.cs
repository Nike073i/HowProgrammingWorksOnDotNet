namespace HowProgrammingWorksOnDotNet.LeetCode.List.ValidPalindrome;

/*
    leetcode: 234 https://leetcode.com/problems/palindrome-linked-list/
    time: O(n)
    memory: O(1)
*/
public class ListNode
{
    public int val;
    public ListNode next;

    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }
}

public class Solution
{
    public static bool IsPalindrome(ListNode head)
    {
        ListNode f = head,
            s = f;

        while (f.next != null && f.next.next != null)
        {
            s = s.next;
            f = f.next.next;
        }

        var reverseTmp = Reverse(s.next);
        var tmp = head;
        while (reverseTmp != null)
        {
            if (reverseTmp.val != tmp.val)
                return false;
            reverseTmp = reverseTmp.next;
            tmp = tmp.next;
        }
        return true;
    }

    public static ListNode Reverse(ListNode start)
    {
        ListNode prev = null;
        ListNode curr = start;
        while (curr != null)
        {
            var next = curr.next;
            curr.next = prev;
            prev = curr;
            curr = next;
        }
        return prev;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestIsPalindrome(int[] values, bool expected)
    {
        ListNode head = CreateList(values);
        bool actual = Solution.IsPalindrome(head);
        Assert.Equal(expected, actual);
    }

    private static ListNode CreateList(int[] values)
    {
        if (values == null || values.Length == 0)
            return null;

        ListNode head = new ListNode(values[0]);
        ListNode current = head;
        for (int i = 1; i < values.Length; i++)
        {
            current.next = new ListNode(values[i]);
            current = current.next;
        }
        return head;
    }
}

public class SolutionTestData : TheoryData<int[], bool>
{
    public SolutionTestData()
    {
        Add([1, 2, 2, 1], true);
        Add([1, 2, 3, 2, 1], true);
        Add([1], true);
        Add([1, 1], true);
        Add([1, 2, 3, 3, 2, 1], true);
        Add([5, 4, 3, 4, 5], true);
        Add([1, 2], false);
        Add([1, 2, 3], false);
        Add([1, 2, 3, 4], false);
        Add([1, 2, 3, 2, 2], false);
        Add([1, 1, 2, 1], false);
        Add([9, 9, 9, 9, 9], true);
        Add([1, 0, 1], true);
        Add([1, 0, 0], false);
        Add([1, 2, 3, 4, 5, 4, 3, 2, 1], true);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9], false);
    }
}
