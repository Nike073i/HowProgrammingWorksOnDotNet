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

        public IEnumerator<T> GetEnumerator()
        {
            var tmp = _head;
            while (tmp != null)
            {
                yield return tmp.Value;
                tmp = tmp.Next;
            }
        }
    }
}
