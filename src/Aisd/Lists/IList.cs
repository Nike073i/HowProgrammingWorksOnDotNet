namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public record struct ListValue<T>(T Value)
    {
        public static ListValue<T> Of(T value) => new(value);
    }

    public interface IList<T> : IEnumerable<ListValue<T>>
    {
        void AddFirst(T value);
        void AddLast(T value);
        void Clear();
        bool Contains(T value);
        bool InsertBefore(T target, T value);
        ListValue<T>? Remove(T target);
        ListValue<T>? RemoveLast();
        ListValue<T>? RemoveFirst();
        void ShiftLeft(T target, int count);
        void ShiftRight(T target, int count);
    }
}
