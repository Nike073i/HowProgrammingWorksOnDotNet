using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public class LinkedList<T> : IList<T>
    {
        private class Node
        {
            public required T Value { get; set; }
            public Node? Next { get; set; }
            public Node? Prev { get; set; }
        }

        private readonly Node _head;
        private readonly Node _tail;

        public LinkedList()
        {
            _head = new Node() { Value = default! };
            _tail = new Node() { Value = default! };
            _head.Next = _tail;
            _tail.Prev = _head;
        }

        public LinkedList(IEnumerable<T> values)
            : this()
        {
            foreach (var val in values)
                AddLast(val);
        }

        public bool InsertBefore(T target, T value)
        {
            var targetNode = DirectTraverse(_head.Next!)
                .FirstOrDefault(n => n.Value!.Equals(target));
            if (targetNode is null)
                return false;

            var node = new Node { Value = value };
            InternalInsertNode(targetNode!.Prev!, targetNode, node);
            return true;
        }

        public void AddFirst(T value)
        {
            var node = new Node { Value = value };
            InternalInsertNode(_head, _head.Next!, node);
        }

        public void AddLast(T value)
        {
            var node = new Node { Value = value };
            InternalInsertNode(_tail.Prev!, _tail, node);
        }

        private void InternalInsertNode(Node left, Node right, Node node)
        {
            node.Next = left.Next;
            node.Prev = right.Prev;
            left.Next = node;
            right.Prev = node;
        }

        private void InternalRemoveNode(Node left, Node right, Node node)
        {
            left.Next = right;
            right.Prev = left;
            node.Next = null;
            node.Prev = null;
        }

        public void Clear()
        {
            _head.Next = _tail;
            _tail.Prev = _head;
        }

        public ListValue<T>? RemoveFirst()
        {
            var node = _head.Next!;
            if (node == _tail)
                return null;

            InternalRemoveNode(_head, node!.Next!, node);
            return ListValue<T>.Of(node.Value);
        }

        public ListValue<T>? RemoveLast()
        {
            var node = _tail.Prev;
            if (node == _head)
                return null;

            InternalRemoveNode(node!.Prev!, _tail, node);
            return ListValue<T>.Of(node.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<ListValue<T>> GetEnumerator()
        {
            foreach (var node in DirectTraverse(_head.Next!))
                yield return new(node.Value);
        }

        private IEnumerable<Node> Traverse(Node startNode, Node endNode, bool direct)
        {
            var tmp = startNode;
            while (tmp != endNode)
            {
                yield return tmp!;
                tmp = direct ? tmp!.Next : tmp!.Prev;
            }
        }

        private IEnumerable<Node> DirectTraverse(Node node) => Traverse(node, _tail, true);

        private IEnumerable<Node> ReverseTraverse(Node node) => Traverse(node, _head, false);

        public bool Contains(T value) =>
            DirectTraverse(_head.Next!).Any(n => n.Value!.Equals(value));

        public void ForEach(Action<T> action)
        {
            foreach (var node in DirectTraverse(_head.Next!))
                action(node.Value);
        }

        public ListValue<T>? Remove(T target)
        {
            var removedNode = DirectTraverse(_head.Next!)
                .FirstOrDefault(n => n.Value!.Equals(target));
            if (removedNode is null)
                return null;
            InternalRemoveNode(removedNode.Prev!, removedNode.Next!, removedNode);
            return new(removedNode.Value);
        }
    }
}
