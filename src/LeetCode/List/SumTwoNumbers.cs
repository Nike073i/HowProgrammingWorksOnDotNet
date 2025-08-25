using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.SumTwoNumbers;

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
}

public class Solution
{
    public static ListNode? AddTwoNumbers(ListNode l1, ListNode l2)
    {
        var p1 = l1;
        var p2 = l2;
        int r = 0;

        ListNode head = new(0);
        var tail = head;

        while (p1 != null || p2 != null || r > 0)
        {
            int s = (p1?.val ?? 0) + (p2?.val ?? 0) + r;
            r = s > 9 ? 1 : 0;
            s %= 10;
            var node = new ListNode(s);
            tail.next = node;
            tail = node;

            p1 = p1?.next;
            p2 = p2?.next;
        }

        return head.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] l1Values, int[] l2Values, int[] expectedValues)
    {
        ListNode l1 = CreateList(l1Values);
        ListNode l2 = CreateList(l2Values);
        ListNode? expected = CreateList(expectedValues);

        ListNode? actual = Solution.AddTwoNumbers(l1, l2);

        Assert.Equal(expected?.ToArray(), actual?.ToArray());
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

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int[], int[]>
{
    public SolutionTestData()
    {
        Add([2, 4, 3], [5, 6, 4], [7, 0, 8]);
        Add([0], [0], [0]);
        Add([1, 8], [0], [1, 8]);
        Add([9, 9, 9], [1], [0, 0, 0, 1]);
        Add([1], [9, 9, 9], [0, 0, 0, 1]);
        Add([5, 5], [5, 5], [0, 1, 1]);
        Add([8, 9], [2, 1], [0, 1, 1]);
        Add([1, 0, 0, 0, 0, 0], [1], [2, 0, 0, 0, 0, 0]);
        Add([9, 9, 9, 9, 9], [1], [0, 0, 0, 0, 0, 1]);
        Add([1, 0, 0], [2, 0, 0], [3, 0, 0]);
        Add([1, 0, 5], [2, 0, 3], [3, 0, 8]);
        Add([7], [8], [5, 1]);
        Add([9], [9], [8, 1]);
        Add([0], [5], [5]);
        Add([9, 9, 9, 9], [1], [0, 0, 0, 0, 1]);
        Add([1, 9, 9, 9], [1], [2, 9, 9, 9]);
        Add([2, 4, 6], [8, 5], [0, 0, 7]);
        Add([7, 2], [3, 5, 9], [0, 8, 9]);
        Add([9, 9, 9, 9, 9, 9, 9], [1], [0, 0, 0, 0, 0, 0, 0, 1]);
        Add([1, 0, 0, 0, 0, 0, 0], [9, 9, 9, 9, 9, 9], [0, 0, 0, 0, 0, 0, 1]);
        Add([1, 2, 3], [4, 5, 6], [5, 7, 9]);
        Add([2, 3, 4], [5, 6, 7], [7, 9, 1, 1]);
        Add([9, 8, 7, 6, 5], [4, 3, 2, 1], [3, 2, 0, 8, 5]);
        Add([1, 2, 3, 4, 5], [6, 7, 8, 9], [7, 9, 1, 4, 6]);
        Add([9], [1], [0, 1]);
        Add([9, 9], [1], [0, 0, 1]);
        Add([5], [5], [0, 1]);
        Add([2, 5], [2, 5], [4, 0, 1]);
        Add([1, 9, 1, 9, 1], [9, 1, 9, 1, 9], [0, 1, 1, 1, 1, 1]);
    }
}
