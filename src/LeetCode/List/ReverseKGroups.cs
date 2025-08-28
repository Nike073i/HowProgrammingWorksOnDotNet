using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.ReverseKGroups;

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
    private static ListNode Reverse(ListNode start, ListNode? end)
    {
        var dummy = new ListNode(0, end);

        var tmp = start;

        while (tmp != end)
        {
            var next = tmp!.next;
            tmp.next = dummy.next;
            dummy.next = tmp;
            tmp = next;
        }

        return dummy.next!;
    }

    public static ListNode? ReverseKGroup(ListNode head, int k)
    {
        var dummy = new ListNode(0);
        var tmpHead = dummy;

        var tmp = head;
        var first = head;
        int i = 0;
        while (tmp != null)
        {
            if (i != 0 && i % k == 0)
            {
                tmpHead.next = Reverse(first, tmp);
                tmpHead = first;
                first = tmp;
            }
            tmp = tmp.next;
            i++;
        }
        if (i % k == 0)
            tmpHead.next = Reverse(first, null);

        return dummy.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] original, int k, int[] expected)
    {
        ListNode originalList = CreateList(original);
        ListNode? expectedList = CreateList(expected);

        ListNode? actual = Solution.ReverseKGroup(originalList, k);

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
        Add([1, 2, 3, 4, 5, 6], 3, [3, 2, 1, 6, 5, 4]);
        Add([1, 2, 3, 4, 5, 6, 7], 3, [3, 2, 1, 6, 5, 4, 7]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 4, [4, 3, 2, 1, 8, 7, 6, 5]);
        Add([1, 2], 2, [2, 1]);
        Add([1, 2, 3], 3, [3, 2, 1]);
        Add([1, 2, 3, 4], 2, [2, 1, 4, 3]);
        Add([1, 2, 3, 4, 5], 1, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 2, [2, 1, 4, 3, 5]);
        Add([1, 2, 3, 4, 5], 3, [3, 2, 1, 4, 5]);
        Add([1, 2, 3, 4, 5], 4, [4, 3, 2, 1, 5]);
        Add([1, 2, 3, 4, 5], 5, [5, 4, 3, 2, 1]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 10, [10, 9, 8, 7, 6, 5, 4, 3, 2, 1]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 7, [7, 6, 5, 4, 3, 2, 1, 8, 9, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 2, [2, 1, 4, 3, 6, 5, 8, 7, 10, 9]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 3, [3, 2, 1, 6, 5, 4, 9, 8, 7, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 4, [4, 3, 2, 1, 8, 7, 6, 5, 9, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9], 4, [4, 3, 2, 1, 8, 7, 6, 5, 9]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9], 3, [3, 2, 1, 6, 5, 4, 9, 8, 7]);
    }
}
