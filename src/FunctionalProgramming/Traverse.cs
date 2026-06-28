using HowProgrammingWorksOnDotNet.FunctionalProgramming.Maybe;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace HowProgrammingWorksOnDotNet.FunctionalProgramming;

public static class TraverseExts
{
    public static Maybe<IEnumerable<T>> Append<T>(this Maybe<IEnumerable<T>> coll, Maybe<T> val) =>
        val.Match(v => coll.Map(i => i.Append(v)), Maybe<IEnumerable<T>>.None);

    public static Maybe<IEnumerable<R>> Traverse<T, R>(
        this IEnumerable<T> ts,
        Func<T, Maybe<R>> f
    ) => ts.Aggregate(seed: Maybe<IEnumerable<R>>.Of([]), func: (optRs, t) => optRs.Append(f(t)));
}

public class TraverseTests
{
    [Fact]
    public void Usage()
    {
        static Maybe<double> ParseValue(string value) =>
            double.TryParse(value, out var result) ? result : Maybe<double>.None();

        static string Seq2String<T>(IEnumerable<T> seq) => string.Join("\n", seq);

        static Maybe<IEnumerable<double>> ParseInput(string input) =>
            input.Split(" ").Traverse(ParseValue);

        static string workflow(Maybe<string> input) =>
            input.Bind(ParseInput).Match(Seq2String, () => "Некорректный ввод");

        Console.WriteLine(workflow("123 32,4 12"));
        Console.WriteLine(workflow("12a 32,4 12"));
    }
}
