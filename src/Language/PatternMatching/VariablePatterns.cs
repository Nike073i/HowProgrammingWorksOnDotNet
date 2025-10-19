namespace HowProgrammingWorksOnDotNet.Language.PatternMatching.Variable;

public record User(int Id, string Name);

public record Machine(string CreatorName, User? Maintainer = null);

public class VariablePatterns
{
    [Fact]
    public void ComplexCheck()
    {
        Console.WriteLine(Describe(new("Nik")));
        Console.WriteLine(Describe(new("Nik")));
        Console.WriteLine(Describe(new("Nik", new(1, "Nikita"))));
        Console.WriteLine(Describe(new("Nikita", new(1, "Nikita"))));
        Console.WriteLine(Describe(new("Nik", new(2, "Kolya"))));
        Console.WriteLine(Describe(null));

        string Describe(Machine? machine) =>
            machine switch
            {
                // Если machine != null && Maintainer != null && Maintainer.Name == CreatorName
                { Maintainer: User m, CreatorName: var c } when m.Name == c =>
                    $"Сопровод есть и он же - создатель - {c}",
                { Maintainer: not null } => "Сопровод есть",
                _ => "Сопровод отсутствует",
            };
    }
}
