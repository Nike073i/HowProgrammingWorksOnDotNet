namespace HowProgrammingWorksOnDotNet.Language.Strings;

public class Format
{
    private void DisplayByFormat(string format) => Console.WriteLine(format, 99999);
    [Fact]
    public void Usage()
    {
        DisplayByFormat("{0} - Без всякого формата");
        DisplayByFormat("Валюта - {0:c}");
        DisplayByFormat("Десятичное число шириной в 9 знаков с заполнением 0-ми слева - {0:d9}");
        DisplayByFormat("Дробное число с 3 знаками после запятой - {0:f3}");
        DisplayByFormat("Число с выделением групп - {0:n}");
        DisplayByFormat("Exp запись - {0:e}");
        DisplayByFormat("16 cc - {0:x}");
        DisplayByFormat(Convert.ToString(99999, 8)); // Только так. Здесь никаких форматов нет
        DisplayByFormat("2 cc - {0:b}");
    }
}
