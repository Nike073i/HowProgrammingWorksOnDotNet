namespace HowProgrammingWorksOnDotNet.LeetCode.List.Reverse;

/*
    leetcode: 206 https://leetcode.com/problems/reverse-linked-list/description/
    time: O(n)
    memory: O(1)
*/
public class ListNode(int val = 0, ListNode next = null)
{
    public int val = val;
    public ListNode next = next;
}

public class Solution
{
    public static ListNode ReverseList(ListNode head)
    {
        ListNode? prev = null;
        ListNode curr = head;
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
