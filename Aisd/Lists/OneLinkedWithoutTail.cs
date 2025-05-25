using System.Collections;

namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public class OneLinkedWithoutTail<T> : IList<T>
    {
        private class Node
        {
            public required T Value { get; set; }
            public Node? Next { get; set; }
        }

        private readonly Node _head;

        public OneLinkedWithoutTail()
        {
            var limiter = new Node() { Value = default! };
            _head = limiter;
        }

        public OneLinkedWithoutTail(IEnumerable<T> values)
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
            node.Next = _head.Next;
            _head.Next = node;
        }

        public void AddLast(T value)
        {
            var node = new Node { Value = value };
            var afterMe = Traverse(_head).TakeLast(1).First();
            afterMe.Next = node;
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
            if (_head.Next == null)
                return null;

            var newLast = Traverse(_head).TakeLast(2).First();
            var value = newLast.Next!.Value;
            newLast.Next = null;
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

            if (beforeNode.Next!.Next == null)
                return RemoveLast();

            var removedNode = beforeNode.Next;
            beforeNode.Next = removedNode!.Next;
            removedNode.Next = null;
            return new(removedNode.Value);
        }

        public void ShiftRight(T target, int count)
        {
            throw new NotImplementedException();
        }

        public void ShiftLeft(T target, int count)
        {
            throw new NotImplementedException();
        }
    }
}
