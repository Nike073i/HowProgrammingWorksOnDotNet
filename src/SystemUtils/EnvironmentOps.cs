namespace HowProgrammingWorksOnDotNet.SystemUtils;

public class EnvironmentOps
{
    private readonly Action<string> Display = Console.WriteLine;

    private string CreateLine(string element = "=", int length = 80) =>
        string.Concat(Enumerable.Repeat(element, length));

    [Fact]
    public void ShowEnvironmentDetails()
    {
        Display(CreateLine());
        var args = Environment.GetCommandLineArgs();
        Display(args.Length > 0 ? string.Join(", ", args) : "without args");

        Display(CreateLine());
        // В Windows возвращает массив строк, представляющих имена всех доступных томов (диски, приводы, флешки) вида "C:\\")
        // В Linux возвращает массив строк представляющих пути ко всем точкам монтирования файловых систем, смонтированных на машине (например, "/home/user", "/media/usb")
        var logicalVolumes = Environment.GetLogicalDrives();
        Display(string.Join(", ", logicalVolumes));

        Display(CreateLine());
        Display("Host info:");
        Display(Environment.MachineName);
        Display(Environment.OSVersion.VersionString);
        Display(Environment.CurrentDirectory);
        Display(Environment.SystemDirectory);
        Display(Environment.ProcessorCount.ToString());
        Display(".net version - " + Environment.Version.ToString());
    }
}
