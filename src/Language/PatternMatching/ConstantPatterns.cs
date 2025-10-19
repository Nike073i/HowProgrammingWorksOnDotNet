namespace HowProgrammingWorksOnDotNet.Language.PatternMatching.Constant;

public record User(int Id, string Name);

public enum State
{
    A,
    B,
}

public record Machine(State State, string CreatorName, User? Maintainer = null);

public class ConstantPatterns
{
    [Fact]
    public void ComplexCheck()
    {
        Console.WriteLine(Describe(new(State.B, "Nik")));
        Console.WriteLine(Describe(new(State.A, "Nik")));
        Console.WriteLine(Describe(new(State.A, "Nik", new(1, "Nikita"))));
        Console.WriteLine(Describe(new(State.A, "Nikita", new(1, "Nikita"))));
        Console.WriteLine(Describe(new(State.A, "Nik", new(2, "Kolya"))));
        Console.WriteLine(Describe(null));

        string Describe(Machine? machine) =>
            machine switch
            {
                // Если machine != null && State == State.A && Maintainer != null && Maintainer.Name == "Nikita"
                { State: State.A, Maintainer.Name: "Nikita" } or { State: State.B } =>
                    "Машина в А, сопровод - Никита ИЛИ Машина в Б",
                _ => "машина == NULL, или Maintainer == NULL, или Maintainer.Name != Nikita",
            };
    }
}
