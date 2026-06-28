namespace HowProgrammingWorksOnDotNet.Patterns.Memento.Draft3;

public class Originator
{
    private int state_int = 5;
    private string state_text = "text";

    public Snapshot CreateSnapshot() => new(this);

    public void Restore(Snapshot snapshot) => snapshot.Restore(this);

    public class Snapshot
    {
        private int state_int;
        private string state_text;

        internal Snapshot(Originator originator)
        {
            state_int = originator.state_int;
            state_text = originator.state_text;
        }

        internal void Restore(Originator originator)
        {
            originator.state_int = state_int;
            originator.state_text = state_text;
        }
    }
}

public class Caretaker(Originator originator)
{
    public Stack<Originator.Snapshot> Snapshots { get; } = new();

    public void Save() => Snapshots.Push(originator.CreateSnapshot());

    public void Undo() => originator.Restore(Snapshots.Pop());
}

public class MementoPatternTest
{
    [Fact]
    public void Usage()
    {
        var originator = new Originator();
        var caretaker = new Caretaker(originator);
        caretaker.Save();
        caretaker.Undo();
    }
}
