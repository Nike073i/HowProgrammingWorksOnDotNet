namespace HowProgrammingWorksOnDotNet.Patterns.Memento.Draft2;

public class Snapshot
{
    private int snap_state_int;
    private string snap_state_text;

    public class Originator
    {
        private int state_int = 5;
        private string state_text = "text";

        public Snapshot CreateSnapshot() =>
            new() { snap_state_int = state_int, snap_state_text = state_text };

        public void Restore(Snapshot snapshot)
        {
            state_int = snapshot.snap_state_int;
            state_text = snapshot.snap_state_text;
        }
    }
}

public class Caretaker(Snapshot.Originator originator)
{
    public Stack<Snapshot> Snapshots { get; } = new();

    public void Save() => Snapshots.Push(originator.CreateSnapshot());

    public void Undo() => originator.Restore(Snapshots.Pop());
}

public class MementoPatternTest
{
    [Fact]
    public void Usage()
    {
        var originator = new Snapshot.Originator();
        var caretaker = new Caretaker(originator);
        caretaker.Save();
        caretaker.Undo();
    }
}
