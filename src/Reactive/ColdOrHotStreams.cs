using System.Reactive.Linq;

namespace HowProgrammingWorksOnDotNet.Reactive;

public class ColdOrHotStreams
{
    private record StockTick(double Price, DateTime Time);

    [Fact]
    public async Task CreateColdStream()
    {
        var rand = new Random();
        var data = Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Select(_ => new StockTick(
                Math.Round(100 + (rand.NextDouble() * 1000), 2),
                DateTime.Now
            ));

        Action<StockTick> PrintHandler(string prefix) =>
            tick => Console.WriteLine(prefix + tick.ToString());

        var printSub1 = data.Subscribe(PrintHandler("first: "));
        var printSub2 = data.Subscribe(PrintHandler("second: "));

        await Task.Delay(10000);
    }

    [Fact]
    public async Task ConvertColdToHot()
    {
        var recipe = Observable.Interval(TimeSpan.FromSeconds(1));
        var hot = recipe.Publish(); // Превращаем в горячий

        hot.Connect();

        Console.WriteLine("Генерация данных началась, но подписчиков пока нет");

        await Task.Delay(2500);
        var subscription = hot.Subscribe(x =>
            Console.WriteLine($"Получено значение: {x} (время: {DateTime.Now:ss.fff})")
        );

        await Task.Delay(3000);
        subscription.Dispose();
        Console.WriteLine("Отписались в " + DateTime.Now.ToString("ss.fff"));

        await Task.Delay(2000);

        subscription = hot.Subscribe(x =>
            Console.WriteLine($"Получено значение: {x} (время: {DateTime.Now:ss.fff})")
        );

        await Task.Delay(2000);
    }
}
