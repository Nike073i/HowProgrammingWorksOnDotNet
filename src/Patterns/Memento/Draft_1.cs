using HowProgrammingWorksOnDotNet.Patterns.Memento.Draft2;

namespace HowProgrammingWorksOnDotNet.Patterns.Memento.Draft1;

public class Originator
{
    private int state_int = 5;
    private string state_text = "text";

    public class Snapshot(Originator originator)
    {
        private int snap_state_int = originator.state_int;
        private string snap_state_text = originator.state_text;

        public void Restore(Originator originator)
        {
            originator.state_int = snap_state_int;
            originator.state_text = snap_state_text;
        }
    }
}

public class Caretaker(Originator originator)
{
    public Stack<Originator.Snapshot> Snapshots { get; } = new();

    public void Save() => Snapshots.Push(new Originator.Snapshot(originator));

    public void Undo() => Snapshots.Pop().Restore(originator);
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
