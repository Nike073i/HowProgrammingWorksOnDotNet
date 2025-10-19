namespace HowProgrammingWorksOnDotNet.Language.Enums;

using static Console;

public enum Color
{
    Red = 100,
    Green,
    Blue,
}

public class Ops
{
    private void PrintColl<T>(IEnumerable<T> coll) => WriteLine(string.Join(", ", coll));

    [Fact]
    public void Usage()
    {
        var constantNames = Enum.GetNames<Color>();
        PrintColl(constantNames);

        var allColors = Enum.GetValues<Color>();
        PrintColl(allColors.Select(color => $"Name - {color}, Value - {color:D};"));

        // Тип кодового значения
        WriteLine(Enum.GetUnderlyingType(typeof(Color)));
    }
}
