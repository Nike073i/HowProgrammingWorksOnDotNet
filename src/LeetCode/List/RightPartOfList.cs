using System.Collections;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.RightPartOfList;

public class ListNode : IEnumerable<int>
{
    public int val;
    public ListNode next;

    public ListNode(int val = 0, ListNode next = null)
    {
        this.val = val;
        this.next = next;
    }

    public IEnumerator<int> GetEnumerator()
    {
        var tmp = this;
        while (tmp != null)
        {
            yield return val;
            tmp = tmp.next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

/*
    leetcode: 876 https://leetcode.com/problems/middle-of-the-linked-list/description/
    time: O(n)
    memory: O(n)
*/
public class Solution
{
    public static ListNode RightPart(ListNode head)
    {
        ListNode slow = head,
            fast = head;

        while (fast != null && fast.next != null)
        {
            slow = slow.next;
            fast = fast.next.next;
        }

        return slow;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestRightPart(int[] values, int[] expectedRightPart)
    {
        ListNode head = CreateList(values);
        ListNode result = Solution.RightPart(head);

        ListNode expectedHead = CreateList(expectedRightPart);
        Assert.True(expectedHead.SequenceEqual(result));
    }

    private static ListNode CreateList(int[] values)
    {
        ListNode head = new(values[0]);
        ListNode current = head;
        for (int i = 1; i < values.Length; i++)
        {
            current.next = new ListNode(values[i]);
            current = current.next;
        }
        return head;
    }
}

public class SolutionTestData : TheoryData<int[], int[]>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], [3, 4, 5]);
        Add([1, 2, 3], [2, 3]);
        Add([1], [1]);
        Add([1, 2, 3, 4], [3, 4]);
        Add([1, 2], [2]);
        Add([1, 2, 3, 4, 5, 6], [4, 5, 6]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9], [5, 6, 7, 8, 9]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], [6, 7, 8, 9, 10]);
        Add([1, 1, 1, 1, 1], [1, 1, 1]);
        Add([2, 2, 2, 2], [2, 2]);
        Add([int.MinValue, 0, int.MaxValue], [0, int.MaxValue]);
    }
}
