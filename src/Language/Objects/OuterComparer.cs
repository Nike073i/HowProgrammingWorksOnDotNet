namespace HowProgrammingWorksOnDotNet.Language.Objects;

public class Machine
{
    public int Id { get; set; }
    public int Weight { get; set; }

    public override string ToString() => $"[Id = {Id}; Weight = {Weight}]";
}

// Внешний компаратор
public class MachineComparer : IComparer<Machine>
{
    public int Compare(Machine? x, Machine? y)
    {
        if (ReferenceEquals(x, y))
            return 0;
        if (x is null)
            return -1;
        if (y is null)
            return 1;

        return x.Weight.CompareTo(y.Weight);
    }
}

public class OuterComparer
{
    [Fact]
    public void Usage()
    {
        var random = new Random();
        var machines = Enumerable
            .Range(1, 10)
            .Select(i => new Machine { Id = i, Weight = random.Next() })
            .ToArray();

        var comparer = new MachineComparer();
        var sortedMachines = machines.Order(comparer);
        Console.WriteLine(string.Join(", ", sortedMachines));
        Array.Sort(machines, comparer);
        Console.WriteLine(string.Join(", ", [.. machines]));
    }
}
