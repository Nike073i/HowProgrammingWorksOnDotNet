namespace HowProgrammingWorksOnDotNet.Language.PatternMatching.Type;

public record User(int Id, string Name);

public class TypePattern
{
    [Fact]
    public void CaseByType()
    {
        string DefineCollType<T>(IEnumerable<T> coll) =>
            coll switch
            {
                HashSet<T> set => "set",
                List<T> list => "list",
                Array arr => "array",
                _ => "undefined",
            };

        IEnumerable<int> coll = new int[5];
        Console.WriteLine(DefineCollType(coll));
        coll = new List<int>() { 5, 3, 2 };
        Console.WriteLine(DefineCollType(coll));
        coll = new HashSet<int> { 5, 3, 3 };
        Console.WriteLine(DefineCollType(coll));
    }

    [Fact]
    public void WhenThen()
    {
        List<User> users = [new User(1, "Nikita"), new User(2, "Nikolya")];

        var mappedUsers = users.Select(user =>
            user switch
            {
                User nik when nik.Name == "Nikita" => "SuperUser", // Можно использовать любые выражения, возврающие bool
                User { Name: "Nikolya", Id: >= 2 } => "Kolya", // Проверка на равенство конкретных полей
                _ => "Prostak",
            }
        );
        Console.WriteLine(string.Join(", ", mappedUsers));
    }
}
