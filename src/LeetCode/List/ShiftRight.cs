using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.ShiftRight;

public class ListNode(int val = 0, ListNode? next = null)
{
    public int val = val;
    public ListNode? next = next;

    public IEnumerable<int> ToArray()
    {
        var tmp = this;
        while (tmp != null)
        {
            yield return val;
            tmp = tmp.next;
        }
    }

    public static ListNode CreateList(int[] values)
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

public class Solution
{
    public static ListNode? RotateRight(ListNode head, int k)
    {
        if (head == null || k == 0)
            return head;

        var dummy = new ListNode(0, head);
        int length = 0;
        var tail = dummy;
        while (tail.next != null)
        {
            tail = tail.next;
            length++;
        }

        k %= length;

        if (k == 0)
            return head;

        var tmp = dummy;
        for (int i = 0; i < length - k; i++)
        {
            tmp = tmp!.next;
        }
        tail.next = dummy.next;
        dummy.next = tmp!.next;
        tmp.next = null;

        return dummy.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] l1Values, int k, int[] expectedValues)
    {
        ListNode l1 = ListNode.CreateList(l1Values);
        ListNode? expected = ListNode.CreateList(expectedValues);

        ListNode? actual = Solution.RotateRight(l1, k);

        Assert.Equal(expected?.ToArray(), actual?.ToArray());
    }
}

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public SolutionTestData()
    {
        Add([1], 0, [1]);
        Add([1, 2, 3, 4, 5], 1, [5, 1, 2, 3, 4]);
        Add([1, 2, 3, 4, 5], 2, [4, 5, 1, 2, 3]);
        Add([1, 2, 3, 4, 5], 3, [3, 4, 5, 1, 2]);
        Add([1, 2, 3, 4], 3, [2, 3, 4, 1]);
        Add([1, 2, 3, 4, 5], 5, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 6, [5, 1, 2, 3, 4]);
        Add([1], 1, [1]);
        Add([1], 5, [1]);
        Add([1, 2], 1, [2, 1]);
        Add([1, 2], 2, [1, 2]);
        Add([1, 2], 3, [2, 1]);
        Add([1, 2], 4, [1, 2]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 3, [8, 9, 10, 1, 2, 3, 4, 5, 6, 7]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 7, [4, 5, 6, 7, 8, 9, 10, 1, 2, 3]);
        Add([1, 2, 3, 4, 5, 6, 7], 3, [5, 6, 7, 1, 2, 3, 4]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 5, [4, 5, 6, 7, 8, 1, 2, 3]);
    }
}
