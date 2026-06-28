namespace HowProgrammingWorksOnDotNet.FunctionalProgramming;

using System.Reflection.Metadata;
using HowProgrammingWorksOnDotNet.FunctionalProgramming.Maybe;
using Newtonsoft.Json.Linq;
using Unit = ValueTuple;

public delegate dynamic Middleware<T>(Func<T, dynamic> fn);

public static class Utils
{
    public static Func<Unit> ToFunc(this Action action) =>
        () =>
        {
            action();
            return Unit.Create();
        };

    public static Func<T, Unit> ToFunc<T>(this Action<T> action) =>
        t =>
        {
            action(t);
            return Unit.Create();
        };

    public static Func<T2, R> Apply<T1, T2, R>(this Func<T1, T2, R> fn, T1 t1) => t2 => fn(t1, t2);

    public static Func<T2, T3, R> Apply<T1, T2, T3, R>(this Func<T1, T2, T3, R> fn, T1 t1) =>
        (t2, t3) => fn(t1, t2, t3);

    public static Func<T1, Func<T2, R>> Curry<T1, T2, R>(this Func<T1, T2, R> fn) =>
        t1 => t2 => fn(t1, t2);

    public static Func<T1, Func<T2, Func<T3, R>>> Curry<T1, T2, T3, R>(
        this Func<T1, T2, T3, R> fn
    ) => t1 => t2 => t3 => fn(t1, t2, t3);

    public static Maybe<T> Head<T>(this IEnumerable<T> values)
    {
        var val = values.FirstOrDefault();
        return val == null ? Maybe<T>.None() : val;
    }

    public static R Match<T, R>(
        this IEnumerable<T> values,
        Func<R> onEmpty,
        Func<T, IEnumerable<T>, R> onOtherwise
    ) => values.Head().Match(v => onOtherwise(v, values.Skip(1)), onEmpty);

    public static Func<R> Map<T, R>(this Func<T> f, Func<T, R> g) => () => g(f());

    public static Func<TEnv, R> Map<T, R, TEnv>(this Func<TEnv, T> f, Func<T, R> g) =>
        env => g(f(env));

    public static Func<R> Bind<T, R>(this Func<T> f, Func<T, Func<R>> g) => g(f());

    // Позволяет "передать" исходное окружение в функцию g, на основе которого будет получен R
    public static Func<TEnv, R> Bind<TEnv, T, R>(this Func<TEnv, T> f, Func<T, Func<TEnv, R>> g) =>
        env => g(f(env))(env);

    public static Middleware<R> Bind<T, R>(this Middleware<T> mw, Func<T, Middleware<R>> g) =>
        cont => mw(t => g(t)(cont));

    public static Middleware<R> Map<T, R>(this Middleware<T> mw, Func<T, R> g) =>
        cont => mw(t => cont(g(t)));

    public static T Run<T>(this Middleware<T> mw) => (T)mw(t => t!);
}

public class PomogiTe
{
    private record Context
    {
        public string LoadString() => Guid.NewGuid().ToString();

        public int LoadInt(string key) => key.Length * 5;
    }

    [Fact]
    public void Usage()
    {
        var GreeterMethod = (string gr, string name) => $"{gr}, {name}";
        Console.WriteLine(GreeterMethod.Apply("hello")("Nikita"));

        Console.WriteLine(GreeterMethod.Curry()("hello")("nikita"));

        Func<Context, string> loadData = ctx => ctx.LoadString();

        var chain = loadData.Bind<Context, string, int>(data => ctx => ctx.LoadInt(data));

        Func<Maybe<JObject>> Parse(string s) => () => JObject.Parse(s);
        Func<Maybe<Uri>> CreateUri(string uri) => () => new Uri(uri);

        Func<Maybe<Uri>> ExtractUri(string json) =>
            Parse(json).Bind(jObj => CreateUri((string)jObj["Uri"]!));

        string data = """
            {
                "Uri": "https://github.com/Nike073i"
            }
            """;

        //TODO реальный тест-кейс не сработает, поскольку плохой вход - исключение при парсинге. Здесь изложена концепция Связки ленивых функций, возвращающих монады
        Console.WriteLine(ExtractUri(data)().Match(u => u.Host, () => "Увы"));
    }

    // Нужно рассматривать Middleware не как "обертку", а как "Источник данных" - HOC. Например withWidth, withStyles, Connect, ReadFile
    // Map - это "преобразование источника данных". Был текст - стал SomethingDomainText. Map(t => new ...);
    // Bind - это "конкатенация" источников данных. Читаем файл -> по файлу читаем ConnectionString -> Создаем соединение к БД. Соединение к БД - новый источник

    [Fact]
    public void Usage_2()
    {
        Middleware<string> ReadFile = cont =>
        {
            var content = "FileContent";
            var r = cont(content);
            return r;
        };

        Middleware<User> GetUser(string name) => cont => new User(name);

        Func<string, dynamic> GetLength = s => s.Length;

        Console.WriteLine(ReadFile(GetLength));
        Console.WriteLine(ReadFile.Run());
        Console.WriteLine(ReadFile.Bind(GetUser).Run());
    }

    private record User(string Name);

    private record Counter(int Value)
    {
        public static Counter Zero => new(0);
    }

    [Fact]
    public void OpWithoutMutation() => Console.WriteLine(CountSumUntil10(1, Counter.Zero));

    private static Counter CountSumUntil10(int iter, Counter state) =>
        iter < 10 ? CountSumUntil10(iter + 1, Increment(iter, state)) : state;

    private static Counter Increment(int value, Counter state) =>
        state with
        {
            Value = state.Value + value,
        };
}


// Никаких Async для Task. Только Sync для IO-не Task