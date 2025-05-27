using System.Collections;
using System.Runtime.CompilerServices;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public class ListWithLimiter<T> : IList<T>
    {
        private class Node
        {
            public required T Value { get; set; }
            public Node? Next { get; set; }
        }

        private readonly Node _head;
        private Node _tail;

        public ListWithLimiter()
        {
            var limiter = new Node() { Value = default! };
            _head = _tail = limiter;
        }

        public ListWithLimiter(IEnumerable<T> values)
            : this()
        {
            foreach (var val in values)
                AddLast(val);
        }

        // Ignoring top element
        private Node? FindBefore(Node top, T value) =>
            Traverse(top).FirstOrDefault(n => n.Next != null && n.Next.Value!.Equals(value));

        public bool InsertBefore(T target, T value)
        {
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
            if (_head.Next == null)
                _tail = node;
            node.Next = _head.Next;
            _head.Next = node;
        }

        public void AddLast(T value)
        {
            var node = new Node { Value = value };
            _tail.Next = node;
            _tail = node;
        }

        public void Clear()
        {
            _head.Next = null;
            _tail = _head;
        }

        public ListValue<T>? RemoveFirst()
        {
            var node = _head.Next;
            if (node == null)
                return null;

            _head.Next = node.Next;
            if (_head.Next == null)
                _tail = _head;

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

        public IEnumerator<ListValue<T>> GetEnumerator()
        {
            foreach (var node in Traverse(_head.Next))
                yield return new(node.Value);
        }

        private IEnumerable<Node> Traverse(Node? node)
        {
            var tmp = node;
            while (tmp != null)
            {
                yield return tmp;
                tmp = tmp.Next;
            }
        }

        public bool Contains(T value) => Traverse(_head.Next).Any(n => n.Value!.Equals(value));

        public void ForEach(Action<T> action)
        {
            foreach (var node in Traverse(_head.Next))
                action(node.Value);
        }

        public ListValue<T>? Remove(T target)
        {
            var beforeNode = FindBefore(_head, target);
            if (beforeNode is null)
                return null;

            if (beforeNode.Next == _tail)
                return RemoveLast();

            var removedNode = beforeNode.Next;
            beforeNode.Next = removedNode!.Next;
            removedNode.Next = null;
            return new(removedNode.Value);
        }

        public void ShiftLeft(T target, int count)
        {
            throw new NotImplementedException();
        }

        public void ShiftRight(T target, int count)
        {
            throw new NotImplementedException();
        }
    }
}
