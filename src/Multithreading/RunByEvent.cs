public class SimpleAutoResetEventExample
{
    // Запуск последовательных потоков. Поток 2 запуститься только тогда, когда закончится первый, сколько бы он не работал
    [Fact]
    public async Task SequentialThreads()
    {
        var autoResetEvent = new AutoResetEvent(false);

        var thread1 = Task.Run(() =>
        {
            Thread.Sleep(50);
            Console.WriteLine("Поток 1: работа завершена, сигнализируем потоку 2");
            autoResetEvent.Set();
        });

        var thread2 = Task.Run(() =>
        {
            Console.WriteLine("Поток 2: жду сигнала от потока 1...");
            autoResetEvent.WaitOne();
            Console.WriteLine("Поток 2: получил сигнал, работа завершена");
        });

        await Task.WhenAll(thread1, thread2);
    }

    [Fact]
    public async Task DidntWait()
    {
        var autoResetEvent = new AutoResetEvent(false);
        bool timeoutOccurred = false;

        var task = Task.Run(() =>
        {
            timeoutOccurred = !autoResetEvent.WaitOne(100);
        });

        await Task.WhenAny(task, Task.Delay(200));

        Assert.True(timeoutOccurred);
    }
}
