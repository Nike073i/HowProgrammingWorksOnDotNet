using HowProgrammingWorksOnDotNet.TestUtils.TheoryData;

namespace HowProgrammingWorksOnDotNet.Documents.JSON.Newtonsoft.Data;

public class JsonDataFixture() : FileContentFixture("Documents/JSON/data.json");

public class Root
{
    public string SquadName { get; set; } = null!;
    public string HomeTown { get; set; } = null!;
    public int Formed { get; set; }

    // Skipped
    // public string SecretBase { get; set; }

    public bool Active { get; set; }
    public string? UnnecessaryField { get; set; }

    public Member[] Members { get; set; } = null!;

    public record Member(
        string Name,
        int Age,
        string SecretIdentity,
        string[] Powers,
        // Unnecessary object
        Security? Security
    );

    public record Security(bool IsActive, string AnotherField);

    public override string ToString() =>
        $"""
            SN - {SquadName}, HT - {HomeTown}, F - {Formed}, A - {Active}, U = {UnnecessaryField
            ?? "empty"}, 
            Member = {string.Join(", ", Members.ToList())}
        """;
}
