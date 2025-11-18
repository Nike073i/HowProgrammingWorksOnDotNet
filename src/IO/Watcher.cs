namespace HowProgrammingWorksOnDotNet.IO;

public class Watcher
{
    [Fact]
    public void Watch()
    {
        var info = Directory.CreateDirectory("tmp_" + DateTime.Now);
        var watcher = new FileSystemWatcher
        {
            Path = info.Name,
            Filter = "*.txt",
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime | NotifyFilters.Size,
        };

        static void OnChanged(object sender, FileSystemEventArgs args) =>
            Console.WriteLine($"Type - {args.ChangeType}. File/Dir - {args.Name}");

        watcher.Created += OnChanged;
        watcher.Renamed += (sender, args) =>
            Console.WriteLine($"Renamed. File/Dir: {args.OldName} to {args.Name}");
        watcher.Changed += OnChanged;
        watcher.Deleted += OnChanged;

        // begin
        watcher.EnableRaisingEvents = true;

        Console.ReadLine();
    }
}
