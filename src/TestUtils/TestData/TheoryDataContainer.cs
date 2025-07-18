using System.Collections;

namespace HowProgrammingWorksOnDotNet.TestUtils.TheoryData
{
    /*
        Author Idea - Andrew Lock.
        Modified by Skuld
        Link - https://andrewlock.net/creating-strongly-typed-xunit-theory-test-data-with-theorydata/
    */
    public abstract class TheoryDataContainer : IEnumerable<object?[]>
    {
        private TheoryDataContainer() { }

        private readonly List<object?[]> _dataset = [];

        private void AddRow(params object?[] args) => _dataset.Add(args);

        public IEnumerator<object?[]> GetEnumerator() => _dataset.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public abstract class OneArg<T> : TheoryDataContainer
        {
            protected void Add(T arg) => AddRow(arg);
        }

        public abstract class TwoArg<T1, T2> : TheoryDataContainer
        {
            protected void Add(T1 t1, T2 t2) => AddRow(t1, t2);
        }

        public abstract class ThreeArg<T1, T2, T3> : TheoryDataContainer
        {
            protected void Add(T1 t1, T2 t2, T3 t3) => AddRow(t1, t2, t3);
        }

        public abstract class FourArg<T1, T2, T3, T4> : TheoryDataContainer
        {
            protected void Add(T1 t1, T2 t2, T3 t3, T4 t4) => AddRow(t1, t2, t3, t4);
        }
    }
}
