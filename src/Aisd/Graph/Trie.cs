namespace HowProgrammingWorksOnDotNet.Aisd.Graph;

public class Trie
{
    private class Node
    {
        public Dictionary<char, Node> Children = [];

        public bool IsEnd;
    }

    private readonly Node _root = new();

    public void Add(string word)
    {
        var node = _root;
        foreach (var c in word)
        {
            if (!node.Children.TryGetValue(c, out var next))
            {
                next = new Node();
                node.Children[c] = next;
            }
            node = next;
        }
        node.IsEnd = true;
    }

    private Node? GetNode(string path)
    {
        var node = _root;
        foreach (var c in path)
        {
            if (!node.Children.TryGetValue(c, out var next))
                return null;
            node = next;
        }
        return node;
    }

    public bool Contains(string word)
    {
        var node = GetNode(word);
        return node != null && node.IsEnd;
    }

    public bool StartsWith(string prefix)
    {
        var node = GetNode(prefix);
        return node != null;
    }

    public IEnumerable<string> GetByPrefix(string prefix)
    {
        var node = GetNode(prefix);
        if (node == null)
            yield break;

        var queue = new Queue<(Node, string)>();
        queue.Enqueue((node, prefix));
        while (queue.Any())
        {
            (node, string pref) = queue.Dequeue();
            if (node.IsEnd)
                yield return pref;
            foreach ((char c, var child) in node.Children)
                queue.Enqueue((child, pref + c));
        }
    }
}

public class TrieTest
{
    [Fact]
    public void Usage()
    {
        var trie = new Trie();
        trie.Add("apple");
        trie.Add("app");
        trie.Add("banana");
        trie.Add("band");
        trie.Add("application");
        trie.Add("apt");
        trie.Add("ban");

        Assert.True(trie.Contains("app"));
        Assert.True(trie.Contains("apple"));
        Assert.True(trie.Contains("ban"));
        Assert.False(trie.Contains("ap"));
        Assert.False(trie.Contains("bandit"));
        Assert.False(trie.Contains("applicationa"));

        Assert.True(trie.StartsWith("a"));
        Assert.True(trie.StartsWith("app"));
        Assert.True(trie.StartsWith("ban"));
        Assert.True(trie.StartsWith("band"));
        Assert.False(trie.StartsWith("c"));
        Assert.False(trie.StartsWith("banned"));
        Assert.False(trie.StartsWith("applepie"));

        Assert.Equal(["app", "apple", "application"], trie.GetByPrefix("app").Order());
        Assert.Equal(["app", "apple", "application", "apt"], trie.GetByPrefix("ap").Order());
        Assert.Equal(["ban", "banana", "band"], trie.GetByPrefix("ba").Order());
        Assert.Equal(["ban", "banana", "band"], trie.GetByPrefix("ban").Order());
        Assert.Equal(
            ["app", "apple", "application", "apt", "ban", "banana", "band"],
            trie.GetByPrefix("").Order()
        );
        Assert.Empty(trie.GetByPrefix("xyz"));
    }
}
