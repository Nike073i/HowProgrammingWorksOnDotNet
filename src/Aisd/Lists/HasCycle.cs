using System.Linq.Expressions;
using HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft.Data;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists;

public class HasCycle
{
    private class Node
    {
        public int Value { get; set; }
        public Node? Next { get; set; }
    }

    private Node CreateListWithCycle()
    {
        var nodes = Enumerable.Range(1, 12).Select(i => new Node { Value = i }).ToList();
        for (int i = 0; i < nodes.Count - 1; i++)
            nodes[i].Next = nodes[i + 1];
        nodes.Last().Next = nodes[5];
        return nodes.First();
    }

    [Fact]
    public void HasCycle_Mark()
    {
        var root = CreateListWithCycle();

        var visited = new HashSet<Node>();
        var tmp = root;

        while (tmp != null && visited.Add(tmp))
            tmp = tmp.Next;

        Assert.NotNull(tmp);
    }

    [Fact]
    public void HasCycle_Reverse()
    {
        var root = CreateListWithCycle();

        var prev = root;
        var tmp = root.Next;
        while (tmp != null && tmp != root)
        {
            var next = tmp.Next;
            tmp.Next = prev;
            prev = tmp;
            tmp = next;
        }
        Assert.NotNull(tmp);
    }

    [Fact]
    public void HasCycle_RabbitAndTurtle()
    {
        var root = CreateListWithCycle();

        var turtle = root;
        var rabbit = root.Next;

        while (rabbit != null && rabbit != turtle)
        {
            rabbit = rabbit.Next?.Next;
            turtle = turtle.Next!;
        }
        Assert.NotNull(rabbit);
    }

    private class DoubleNode
    {
        public int Value { get; set; }
        public DoubleNode? Next { get; set; }
        public DoubleNode? Prev { get; set; }
    }

    [Fact]
    public void HasCycle_DoubleLinkList()
    {
        var nodes = Enumerable.Range(0, 12).Select(i => new DoubleNode { Value = i }).ToArray();
        for (int i = 1; i < nodes.Length; i++)
        {
            nodes[i].Prev = nodes[i - 1];
            nodes[i - 1].Next = nodes[i];
        }
        nodes.Last().Next = nodes[5];

        var root = nodes.First();
        var tmp = root;
        while (tmp.Next != null && tmp.Next.Prev == tmp)
            tmp = tmp.Next;

        Assert.NotNull(tmp.Next);
    }
}
