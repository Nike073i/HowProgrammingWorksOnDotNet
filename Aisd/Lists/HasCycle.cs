using System.Linq.Expressions;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists;

public class HasCycle
{
    private class Node
    {
        public int Value { get; set; }
        public Node? Next;
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
}
