using System.Collections;

namespace HowProgrammingWorksOnDotNet.LeetCode.List.MergeTwoSortedLists;

public class ListNode(int val = 0, ListNode? next = null) : IEnumerable<int>
{
    public int val = val;
    public ListNode? next = next;

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
    leetcode: 21 https://leetcode.com/problems/merge-two-sorted-lists/description/
    time: O(n + m)
    memory: O(1)
*/

public class Solution
{
    public static ListNode MergeTwoListsShortWay(ListNode list1, ListNode list2)
    {
        var head = new ListNode(0);
        var tail = head;
        var p1 = list1;
        var p2 = list2;

        while (p1 != null && p2 != null)
        {
            if (p1.val < p2.val)
            {
                tail.next = p1;
                p1 = p1.next;
            }
            else
            {
                tail.next = p2;
                p2 = p2.next;
            }
            tail = tail.next;
        }
        tail.next = p1 ?? p2;

        return head.next!;
    }

    public static ListNode MergeTwoListsFullWay(ListNode list1, ListNode list2)
    {
        var outputHead = new ListNode();
        var outputTail = outputHead;

        static int GetVal(ListNode node) => node == null ? int.MaxValue : node.val;

        while (list1 != null || list2 != null)
        {
            var nextNode = GetVal(list1) < GetVal(list2) ? list1 : list2;
            outputTail.next = nextNode;
            outputTail = nextNode;
            if (nextNode == list1)
                list1 = list1.next;
            else
                list2 = list2.next;
        }
        return outputHead.next;
    }
}

public class SolutionTests
{
    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestFirst(int[] l1Values, int[] l2Values, int[] expectedValues)
    {
        ListNode l1 = CreateList(l1Values);
        ListNode l2 = CreateList(l2Values);
        ListNode expected = CreateList(expectedValues);

        ListNode actual = Solution.MergeTwoListsShortWay(l1, l2);

        Assert.True(expected.SequenceEqual(actual));
    }

    [Theory]
    [ClassData(typeof(SolutionTestData))]
    public void TestSecond(int[] l1Values, int[] l2Values, int[] expectedValues)
    {
        ListNode l1 = CreateList(l1Values);
        ListNode l2 = CreateList(l2Values);
        ListNode expected = CreateList(expectedValues);

        ListNode actual = Solution.MergeTwoListsFullWay(l1, l2);

        Assert.True(expected.SequenceEqual(actual));
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

public class SolutionTestData : TheoryData<int[], int[], int[]>
{
    public SolutionTestData()
    {
        Add([1, 3, 5], [2, 4, 6], [1, 2, 3, 4, 5, 6]);
        Add([2, 3, 4], [5, 6, 7], [2, 3, 4, 5, 6, 7]);
        Add([1, 1, 1], [2, 2, 2], [1, 1, 1, 2, 2, 2]);
        Add([1, 3, 5, 7], [2, 4], [1, 2, 3, 4, 5, 7]);
        Add([1, 2], [3, 4, 5, 6], [1, 2, 3, 4, 5, 6]);
        Add([1, 5, 9], [2, 3, 4, 6, 7, 8], [1, 2, 3, 4, 5, 6, 7, 8, 9]);
        Add([1], [2], [1, 2]);
        Add([5], [3], [3, 5]);
        Add([0], [0], [0, 0]);
        Add([-5, -3, -1], [-4, -2, 0], [-5, -4, -3, -2, -1, 0]);
        Add([-10, 0, 10], [-5, 5], [-10, -5, 0, 5, 10]);
        Add([1, 1, 2, 2], [1, 1, 2, 2], [1, 1, 1, 1, 2, 2, 2, 2]);
        Add([3, 3, 3], [1, 1, 1], [1, 1, 1, 3, 3, 3]);
        Add([10, 30, 50], [20, 40, 60], [10, 20, 30, 40, 50, 60]);
        Add([100, 200, 300], [150, 250, 350], [100, 150, 200, 250, 300, 350]);
        Add(
            [1, 3, 5, 7, 9, 11, 13],
            [2, 4, 6, 8, 10, 12, 14],
            [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
        );
        Add([1, 2, 3], [4, 5, 6], [1, 2, 3, 4, 5, 6]);
        Add([10, 20, 30], [5, 15, 25], [5, 10, 15, 20, 25, 30]);
        Add([1, 4, 7], [2, 3, 5, 6], [1, 2, 3, 4, 5, 6, 7]);
        Add([2, 5, 8], [1, 3, 4, 6, 7], [1, 2, 3, 4, 5, 6, 7, 8]);
        Add([5, 5, 5], [5, 5, 5], [5, 5, 5, 5, 5, 5]);
        Add([0, 0, 0], [0, 0], [0, 0, 0, 0, 0]);
        Add([0, 1, 2], [-1, 0, 1], [-1, 0, 0, 1, 1, 2]);
        Add([-2, 0, 2], [-1, 1], [-2, -1, 0, 1, 2]);
        Add(
            [1, 5, 9, 13],
            [2, 3, 4, 6, 7, 8, 10, 11, 12, 14],
            [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
        );
        Add([1, 10, 100], [5, 50, 500], [1, 5, 10, 50, 100, 500]);
        Add([2, 20, 200], [1, 10, 100], [1, 2, 10, 20, 100, 200]);
    }
}
