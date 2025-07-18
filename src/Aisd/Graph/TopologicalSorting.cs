namespace HowProgrammingWorksOnDotNet.Aisd.Graph;

public class TopologicalSorting
{
    private class Node(string name)
    {
        public readonly string Name = name;
        public int BeforeMe;
        public HashSet<Node> Next = [];
    }

    private readonly Dictionary<string, Node> _nodes = [];

    public void Add(string name, params string[] dependencies)
    {
        if (!_nodes.TryGetValue(name, out var node))
            node = _nodes[name] = new Node(name);

        foreach (string dep in dependencies)
        {
            if (!_nodes.TryGetValue(dep, out var depNode))
                depNode = _nodes[dep] = new Node(dep);

            depNode.Next.Add(node);
        }
    }

    public List<string> Sort()
    {
        foreach (var node in _nodes.Values)
            node.BeforeMe = 0;

        foreach (var node in _nodes.Values)
            foreach (var next in node.Next)
                next.BeforeMe++;

        var result = new List<string>(_nodes.Count);

        var ready = new Queue<Node>();
        foreach (var node in _nodes.Values)
            if (node.BeforeMe == 0)
                ready.Enqueue(node);

        while (ready.Count > 0)
        {
            var node = ready.Dequeue();
            result.Add(node.Name);

            foreach (var next in node.Next)
            {
                next.BeforeMe--;
                if (next.BeforeMe == 0)
                    ready.Enqueue(next);
            }
        }

        if (result.Count < _nodes.Count)
            throw new InvalidOperationException("Обнаружен цикл в графе.");            

        return result;
    }
}

public class TopologicalSortingTests
{
    [Fact]
    public void Usage()
    {
        string[] works = [
        /* [0] */ "Начальная проверка",
        /* [1] */ "Покраска стен",
        /* [2] */ "Установка бытовой техники",
        /* [3] */ "Покраска потолка",
        /* [4] */ "Отделка пола",
        /* [5] */ "Установка освещения",
        /* [6] */ "Установка шкафов",
        /* [7] */ "Итоговая проверка"
        ];
        var topSort = new TopologicalSorting();
        topSort.Add(works[4], works[1]);
        // Проверка дублирования зависимостей
        topSort.Add(works[4], works[1], works[3]);
        topSort.Add(works[0]);
        topSort.Add(works[1], works[0]);
        topSort.Add(works[7], works[6], works[5], works[2], works[4]);
        topSort.Add(works[6], works[4]);
        topSort.Add(works[2], works[4]);
        topSort.Add(works[5], works[3]);

        topSort.Sort().ForEach(Console.WriteLine);
    }
}
