using System.Diagnostics;

namespace HowProgrammingWorksOnDotNet.SystemUtils;

public class Processes
{
    [Fact]
    public void GetInfo()
    {
        var processes = Process.GetProcesses();
        foreach (var p in processes)
            Console.WriteLine($"[{p.Id}] - {p.ProcessName}");

        int sampleProcInd = new Random().Next(0, processes.Length);
        var sampleProc = processes[sampleProcInd];

        Console.WriteLine("Process info");
        Console.WriteLine($"[{sampleProc.Id}] - {sampleProc.ProcessName}:");
        foreach (ProcessThread t in sampleProc.Threads)
        {
            Console.WriteLine(
                $"- [{t.Id}] - Started At: {t.StartTime}, Priority: {t.PriorityLevel}"
            );
        }
        Console.WriteLine("Modules:");
        foreach (ProcessModule m in sampleProc.Modules)
        {
            Console.WriteLine(m.ModuleName);
        }
    }

    /* Only for Windows */
    [Fact]
    public void OperateByVerb()
    {
        const string filePath = "./example.txt";

        var startInfo = new ProcessStartInfo(filePath);
        foreach (var verb in startInfo.Verbs)
        {
            Console.WriteLine(verb);
        }
        startInfo.Verb = "Edit";
        Process.Start(startInfo);
    }
}
