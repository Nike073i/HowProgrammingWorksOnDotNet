namespace HowProgrammingWorksOnDotNet.Aisd.Graph;

public class Graph
{
    private class Node(string name)
    {
        public string Name = name;
        public HashSet<Link> Links = [];
        public void LinkWith(Node another, int cost)
        {
            Links.Add(new Link(another, cost));
            another.Links.Add(new Link(this, cost));
        }
    }

    private record Link(Node ToNode, int Cost);

    private readonly Dictionary<string, Node> _nodes = [];

    public Graph() { }
    private Graph(Dictionary<string, Node> nodes) => _nodes = nodes;

    private Node GetOrCreate(string name)
    {
        _nodes.TryGetValue(name, out var node);
        node ??= _nodes[name] = new Node(name);
        return node;
    }

    public void AddLink(string first, string second, int cost)
    {
        var firstNode = GetOrCreate(first);
        var secondNode = GetOrCreate(second);
        firstNode.LinkWith(secondNode, cost);
    }

    public void BreadthFirstTtraverse(string start, Action<string> handler)
    {
        if (!_nodes.TryGetValue(start, out var node)) return;

        var queue = new Queue<Node>();
        queue.Enqueue(node);

        var visited = new HashSet<Node>();

        while (queue.Any())
        {
            node = queue.Dequeue();
            if (visited.Add(node))
            {
                handler(node.Name);
                foreach (var link in node.Links)
                    queue.Enqueue(link.ToNode);
            }
        }
    }

    private record struct Edge(Node First, Node Second, int Cost);

    public Graph CreatePrimMst(string start)
    {
        if (!_nodes.TryGetValue(start, out var startNode)) throw new InvalidOperationException();

        var mst = new Dictionary<string, Node>();
        var queue = new PriorityQueue<Edge, int>();

        AddToMst(startNode);
        while (queue.Count > 0 && mst.Count < _nodes.Count)
        {
            var edge = queue.Dequeue();
            (var first, var second, int cost) = edge;

            if (mst.ContainsKey(second.Name)) continue;

            var secondClone = AddToMst(second);

            mst[first.Name].LinkWith(secondClone, cost);
        }
        return new Graph(mst);

        Node AddToMst(Node orig)
        {
            var clone = new Node(orig.Name);
            mst[clone.Name] = clone;

            foreach (var link in orig.Links)
                queue.Enqueue(new Edge(orig, link.ToNode, link.Cost), link.Cost);

            return clone;
        }
    }
}

public class GraphCsvFactory
{
    private static readonly string LineSeparator = Environment.NewLine;
    private static readonly string DataSeparator = ",";

    public static Graph Create(string data)
    {
        var graph = new Graph();
        foreach (string edge in data.Split(LineSeparator))
        {
            var values = edge.Split(DataSeparator);

            string fromName = values[0].Trim();
            string toName = values[1].Trim();
            int cost = int.Parse(values[2].Trim());

            graph.AddLink(fromName, toName, cost);
        }

        return graph;
    }
}

public class GraphTests
{
    [Fact]
    public void Usage()
    {
        string data =
        """
        A, B, 5
        B, C, 1
        C, E, 2
        F, E, 10
        B, D, 4
        D, C, 3
        A, F, 6
        E, A, 15
        """;
        var graph = GraphCsvFactory.Create(data);
        graph.BreadthFirstTtraverse("E", Console.WriteLine);

        var mst = graph.CreatePrimMst("A");
        int x = 5;
    }
}

