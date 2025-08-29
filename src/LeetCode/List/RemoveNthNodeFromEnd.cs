using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.RemoveNthNodeFromEnd;

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
    public static ListNode? RemoveNthFromEnd(ListNode head, int n)
    {
        var f = head;
        for (int i = 0; i < n; i++)
            f = f!.next;

        if (f == null)
            return head.next;

        var s = head;
        while (f.next != null)
        {
            f = f.next;
            s = s!.next;
        }
        s!.next = s.next!.next;
        return head;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] original, int n, int[] expected)
    {
        ListNode originalList = CreateList(original);
        ListNode? expectedList = CreateList(expected);

        ListNode? actual = Solution.RemoveNthFromEnd(originalList, n);

        Assert.Equal(expectedList.ToArray(), actual?.ToArray());
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

public class SolutionTestData : TheoryDataContainer.ThreeArg<int[], int, int[]>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], 1, [1, 2, 3, 4]);
        Add([1, 2, 3, 4, 5], 2, [1, 2, 3, 5]);
        Add([1, 2, 3, 4, 5], 3, [1, 2, 4, 5]);
        Add([1, 2, 3, 4, 5], 5, [2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5, 6], 3, [1, 2, 3, 5, 6]);
        Add([1, 2, 3, 4, 5, 6, 7], 4, [1, 2, 3, 5, 6, 7]);
        Add([1, 2], 1, [1]);
        Add([1, 2], 2, [2]);
        Add([1, 2, 3], 1, [1, 2]);
        Add([1, 2, 3], 2, [1, 3]);
        Add([1, 2, 3], 3, [2, 3]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 1, [1, 2, 3, 4, 5, 6, 7, 8, 9]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 2, [1, 2, 3, 4, 5, 6, 7, 8, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 3, [1, 2, 3, 4, 5, 6, 7, 9, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 5, [1, 2, 3, 4, 6, 7, 8, 9, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 10, [2, 3, 4, 5, 6, 7, 8, 9, 10]);
        Add([-5, -4, -3, -2, -1], 2, [-5, -4, -3, -1]);
        Add([-10, -5, 0, 5, 10], 3, [-10, -5, 5, 10]);
        Add([1, 2, 2, 3, 3], 2, [1, 2, 2, 3]);
        Add([1, 2, 3, 4, 5], 4, [1, 3, 4, 5]);
        Add([1, 2, 3, 4], 3, [1, 3, 4]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9], 5, [1, 2, 3, 4, 6, 7, 8, 9]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12], 6, [1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12]);
    }
}
