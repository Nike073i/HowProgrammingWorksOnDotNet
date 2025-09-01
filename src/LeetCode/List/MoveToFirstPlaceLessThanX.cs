using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.MoveToFirstPlaceLessThanX;

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
    public static ListNode? Partition(ListNode head, int x)
    {
        var dummy = new ListNode(0, head);

        var newHead = new ListNode(0);
        var tail = newHead;

        var tmp = dummy;
        while (tmp.next != null)
        {
            var node = tmp.next;
            if (node.val < x)
            {
                tmp.next = node.next;
                tail.next = node;
                tail = node;
            }
            else
                tmp = node;
        }

        tail.next = dummy.next;

        return newHead.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] original, int x, int[] expected)
    {
        ListNode? originalList = CreateList(original);
        ListNode? expectedList = CreateList(expected);

        ListNode? actual = Solution.Partition(originalList!, x);

        Assert.Equal(expectedList?.ToArray(), actual?.ToArray());
    }

    private static ListNode? CreateList(int[] values)
    {
        if (values == null || values.Length == 0)
            return null;

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
        Add([-5, -3, -1], 0, [-5, -3, -1]);
        Add([1, 1, 1], 2, [1, 1, 1]);
        Add([5, 6, 7, 8], 5, [5, 6, 7, 8]);
        Add([10, 20, 30], 5, [10, 20, 30]);
        Add([0, 1, 2], 0, [0, 1, 2]);
        Add([1, 2, 3, 4, 5], 4, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 5, 6], 4, [1, 2, 3, 5, 6]);
        Add([-3, -2, -1, 0, 1], -1, [-3, -2, -1, 0, 1]);
        Add([5, 6, 7, 1, 2], 4, [1, 2, 5, 6, 7]);
        Add([10, 20, 3, 4], 5, [3, 4, 10, 20]);
        Add([100, 200, 5, 10], 50, [5, 10, 100, 200]);
        Add([10, 1, 20, 2, 30], 5, [1, 2, 10, 20, 30]);
        Add([5, 1, 6, 2, 7], 3, [1, 2, 5, 6, 7]);
        Add([100, 50, 200, 25, 300], 75, [50, 25, 100, 200, 300]);
        Add([1, 4, 3, 2, 5], 3, [1, 2, 4, 3, 5]);
        Add([5, 1, 8, 2, 9], 4, [1, 2, 5, 8, 9]);
        Add([10, 5, 15, 3, 20], 7, [5, 3, 10, 15, 20]);
        Add([1, 3, 2, 3, 4], 3, [1, 2, 3, 3, 4]);
        Add([5, 5, 1, 5, 2], 5, [1, 2, 5, 5, 5]);
        Add([0, 0, -1, 0, 1], 0, [-1, 0, 0, 0, 1]);
        Add([10, 20, 1, 30], 5, [1, 10, 20, 30]);
        Add([5, 6, 2, 7], 3, [2, 5, 6, 7]);
        Add([100, 50, 200], 75, [50, 100, 200]);
        Add([10, 1, 20, 2, 30, 3], 5, [1, 2, 3, 10, 20, 30]);
        Add([5, 1, 6, 2, 7, 3], 4, [1, 2, 3, 5, 6, 7]);
        Add([1, 2], 3, [1, 2]);
        Add([3, 1], 2, [1, 3]);
        Add([2, 1], 2, [1, 2]);
        Add([5, 3], 4, [3, 5]);
        Add([-5, -3, -1, 1, 3], 0, [-5, -3, -1, 1, 3]);
        Add([-10, -5, 0, 5, 10], -3, [-10, -5, 0, 5, 10]);
        Add([1, -1, 2, -2], 0, [-1, -2, 1, 2]);
        Add([5, 5, 5, 5], 5, [5, 5, 5, 5]);
        Add([0, 0, 0], 0, [0, 0, 0]);
        Add([5, 15, 25, 35, 45, 1, 2, 3], 10, [5, 1, 2, 3, 15, 25, 35, 45]);
        Add([3, 1, 4, 2, 5], 3, [1, 2, 3, 4, 5]);
        Add([5, 2, 8, 1, 9], 4, [2, 1, 5, 8, 9]);
        Add([10, 30, 20, 5, 15], 25, [10, 20, 5, 15, 30]);
        Add([5, 6, 7, 8], 4, [5, 6, 7, 8]);
        Add([0, 1, 2], -1, [0, 1, 2]);
        Add([1, 2, 3, 4], 5, [1, 2, 3, 4]);
        Add([1, 4, 3, 2, 5, 2], 3, [1, 2, 2, 4, 3, 5]);
        Add([3, 5, 8, 5, 10, 2, 1], 5, [3, 2, 1, 5, 8, 5, 10]);
    }
}
