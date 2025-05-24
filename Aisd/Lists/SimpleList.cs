using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public class SimpleList<T> : IList<T>
    {
        private class Node
        {
            public required T Value { get; set; }
            public Node? Next { get; set; }
        }

        private Node? _head;
        private Node? _tail;

        public SimpleList() { }

        public SimpleList(IEnumerable<T> values)
        {
            foreach (var val in values)
                AddLast(val);
        }

        // Ignoring top elements
        private Node? FindBefore(Node top, T value) =>
            Traverse(top).FirstOrDefault(n => n.Next != null && n.Next.Value!.Equals(value));

        public bool InsertBefore(T target, T value)
        {
            if (_head == null)
                return false;

            if (_head.Value!.Equals(target))
            {
                AddFirst(value);
                return true;
            }

            var beforeNode = FindBefore(_head, target);
            if (beforeNode is null)
                return false;
            var node = new Node { Value = value };
            node.Next = beforeNode.Next;
            beforeNode.Next = node;
            return true;
        }

        public void AddFirst(T value)
        {
            var node = new Node { Value = value };
            if (_head is null)
            {
                _head = _tail = node;
                return;
            }
            node.Next = _head;
            _head = node;
        }

        public void AddLast(T value)
        {
            var node = new Node { Value = value };
            if (_head is null)
            {
                _head = _tail = node;
                return;
            }

            _tail!.Next = node;
            _tail = node;
        }

        public void Clear()
        {
            _head = _tail = null;
        }

        public ListValue<T>? RemoveFirst()
        {
            if (_head is null)
                return null;

            var node = _head;

            if (_head == _tail)
                _head = _tail = null;
            else
                _head = _head.Next;

            return ListValue<T>.Of(node.Value);
        }

        public ListValue<T>? RemoveLast()
        {
            if (_head is null)
                return null;

            var tmp = _head;
            if (_head == _tail)
            {
                _head = _tail = null;
                return ListValue<T>.Of(tmp.Value);
            }

            while (tmp!.Next != _tail)
            {
                tmp = tmp.Next;
            }
            tmp.Next = null;
            var value = _tail!.Value;
            _tail = tmp;
            return ListValue<T>.Of(value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<ListValue<T>> GetEnumerator()
        {
            foreach (var node in Traverse(_head))
                yield return new(node.Value);
        }

        private IEnumerable<Node> Traverse(Node? top)
        {
            var tmp = top;
            while (tmp != null)
            {
                yield return tmp;
                tmp = tmp.Next;
            }
        }

        public bool Contains(T value) => Traverse(_head).Any(n => n.Value!.Equals(value));
    }
}
