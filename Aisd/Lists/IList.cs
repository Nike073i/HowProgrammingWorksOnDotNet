namespace HowProgrammingWorksOnDotNet.Aisd.Lists
{
    public record struct ListValue<T>(T Value)
    {
        public static ListValue<T> Of(T value) => new(value);
    }

    public interface IList<T> : IEnumerable<T>
    {
        void AddFirst(T value);
        void AddLast(T value);
        void Clear();
        ListValue<T>? RemoveLast();
        ListValue<T>? RemoveFirst();
    }
}
