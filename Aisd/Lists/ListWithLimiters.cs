using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public class ListWithLimiters<T> : IList<T>
    {
        private class Node
        {
            public required T Value { get; set; }
            public Node? Next { get; set; }
        }

        private readonly Node _head;
        private Node _tail;

        public ListWithLimiters()
        {
            var limiter = new Node() { Value = default! };
            _head = _tail = limiter;
        }

        public ListWithLimiters(IEnumerable<T> values)
            : this()
        {
            foreach (var val in values)
                AddLast(val);
        }

        public void AddFirst(T value)
        {
            var node = new Node { Value = value };
            node.Next = _head.Next;
            _head.Next = node;
        }

        public void AddLast(T value)
        {
            var node = new Node { Value = value };
            _tail.Next = node;
            _tail = node;
        }

        public void Clear() => _head.Next = null;

        public ListValue<T>? RemoveFirst()
        {
            var node = _head.Next;
            if (node == null)
                return null;

            _head.Next = node.Next;
            return ListValue<T>.Of(node.Value);
        }

        public ListValue<T>? RemoveLast()
        {
            if (_head == _tail)
                return null;

            var tmp = _head;
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

        public IEnumerator<T> GetEnumerator()
        {
            var tmp = _head.Next;
            while (tmp != null)
            {
                yield return tmp.Value;
                tmp = tmp.Next;
            }
        }
    }
}
