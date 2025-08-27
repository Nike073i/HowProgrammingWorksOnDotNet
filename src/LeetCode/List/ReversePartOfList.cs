using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.ReversePartOfList;

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
    public static ListNode ReverseBetween(ListNode head, int left, int right)
    {
        var dummy = new ListNode(0, head);
        var queue = new Queue<ListNode>();

        var leftPartTail = dummy;
        for (int i = 0; i < left - 1; i++)
            leftPartTail = leftPartTail.next!;

        var reversedHead = leftPartTail.next;
        var reversedTail = reversedHead!;

        for (int i = 0; i < right - left; i++)
        {
            var newElement = reversedTail.next;
            reversedTail.next = newElement!.next;
            newElement.next = reversedHead;
            reversedHead = newElement;
        }

        leftPartTail.next = reversedHead;

        return dummy.next!;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] original, int left, int right, int[] expected)
    {
        ListNode originalList = CreateList(original);
        ListNode? expectedList = CreateList(expected);

        ListNode actual = Solution.ReverseBetween(originalList, left, right);

        Assert.Equal(expectedList.ToArray(), actual.ToArray());
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

public class SolutionTestData : TheoryDataContainer.FourArg<int[], int, int, int[]>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], 2, 4, [1, 4, 3, 2, 5]);
        Add([1, 2, 3, 4, 5], 1, 3, [3, 2, 1, 4, 5]);
        Add([1, 2, 3, 4, 5, 6], 2, 5, [1, 5, 4, 3, 2, 6]);
        Add([1, 2, 3, 4, 5], 1, 1, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 1, 2, [2, 1, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 5, 5, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 3, 5, [1, 2, 5, 4, 3]);
        Add([1, 2, 3, 4, 5], 2, 2, [1, 2, 3, 4, 5]);
        Add([1, 2, 3, 4, 5], 3, 3, [1, 2, 3, 4, 5]);
        Add([1], 1, 1, [1]);
        Add([1, 2, 3, 4, 5], 1, 5, [5, 4, 3, 2, 1]);
        Add([1, 2, 3], 1, 3, [3, 2, 1]);
        Add([1, 2], 1, 2, [2, 1]);
        Add([42], 1, 1, [42]);
        Add([1, 2], 1, 1, [1, 2]);
        Add([1, 2], 2, 2, [1, 2]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 3, 8, [1, 2, 8, 7, 6, 5, 4, 3, 9, 10]);
        Add([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], 1, 10, [10, 9, 8, 7, 6, 5, 4, 3, 2, 1]);
        Add([1, 2, 3, 4, 5], 2, 3, [1, 3, 2, 4, 5]);
        Add([1, 2, 3, 4, 5], 3, 4, [1, 2, 4, 3, 5]);
        Add([1, 2, 3, 4, 5], 4, 5, [1, 2, 3, 5, 4]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 2, 3, [1, 3, 2, 4, 5, 6, 7, 8]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 2, 4, [1, 4, 3, 2, 5, 6, 7, 8]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 2, 5, [1, 5, 4, 3, 2, 6, 7, 8]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 2, 6, [1, 6, 5, 4, 3, 2, 7, 8]);
        Add([1, 2, 3, 4, 5, 6, 7, 8], 2, 7, [1, 7, 6, 5, 4, 3, 2, 8]);
    }
}
