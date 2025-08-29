using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.RemoveDuplicatesFromSortedList;

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
    public static ListNode? DeleteDuplicates(ListNode head)
    {
        var dummy = new ListNode(0, head);

        var prev = dummy;
        var tmp = head;
        while (tmp != null)
        {
            bool dupl = false;

            while (tmp.next != null && tmp.val == tmp.next.val)
            {
                dupl = true;
                tmp = tmp.next;
            }

            if (dupl)
                prev.next = tmp.next;
            else
            {
                prev = tmp;
            }
            tmp = tmp.next;
        }

        return dummy.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void Test(int[] original, int[] expected)
    {
        ListNode? originalList = CreateList(original);
        ListNode? expectedList = CreateList(expected);

        ListNode? actual = Solution.DeleteDuplicates(originalList);

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

public class SolutionTestData : TheoryDataContainer.TwoArg<int[], int[]>
{
    public SolutionTestData()
    {
        Add([1, 2, 3, 4, 5], [1, 2, 3, 4, 5]);
        Add([1], [1]);
        Add([1, 2], [1, 2]);
        Add([-5, -3, 0, 2, 4], [-5, -3, 0, 2, 4]);
        Add([1, 1, 2, 3, 4], [2, 3, 4]);
        Add([1, 2, 2, 3, 4], [1, 3, 4]);
        Add([1, 2, 3, 3, 4], [1, 2, 4]);
        Add([1, 2, 3, 4, 4], [1, 2, 3]);
        Add([1, 1, 1, 2, 3], [2, 3]);
        Add([1, 2, 2, 2, 3], [1, 3]);
        Add([1, 2, 3, 3, 3], [1, 2]);
        Add([1, 1, 2, 2, 3], [3]);
        Add([1, 1], []);
        Add([1, 1, 2, 3, 3, 4, 5, 5], [2, 4]);
        Add([1, 2, 2, 3, 4, 4, 5], [1, 3, 5]);
        Add([1, 1, 2, 2, 3, 3], []);
        Add([1, 1, 2, 3, 3, 4, 4, 5], [2, 5]);
        Add([1, 1, 2, 3, 4, 4, 5, 6, 6, 7], [2, 3, 5, 7]);
        Add([1, 2, 2, 3, 4, 5, 5, 6, 7, 7], [1, 3, 4, 6]);
        Add([1, 1, 2, 2, 3, 3, 4, 4, 5, 5], []);
        Add([], []);
        Add([1, 1, 2, 2, 3, 4, 5, 5], [3, 4]);
        Add([1, 2, 2, 3, 4, 4, 5, 5], [1, 3]);
        Add([1, 1, 2, 3, 4, 4, 5, 6], [2, 3, 5, 6]);
    }
}
