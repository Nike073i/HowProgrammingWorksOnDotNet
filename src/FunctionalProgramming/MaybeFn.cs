using HowProgrammingWorksOnDotNet.FunctionalProgramming.Maybe;

namespace HowProgrammingWorksOnDotNet.FunctionalProgramming;

public static class MaybeFn
{
    public static Maybe<R> Apply<T, R>(this Maybe<Func<T, R>> maybeFn, Maybe<T> maybeT) =>
        maybeFn.Match(fn => maybeT.Match(t => fn(t), Maybe<R>.None), Maybe<R>.None);

    public static Maybe<Func<T2, R>> Apply<T1, T2, R>(
        this Maybe<Func<T1, T2, R>> maybeFn,
        Maybe<T1> maybeT1
    ) => Apply(maybeFn.Map(f => f.Curry()), maybeT1);

    public static Func<Maybe<R>> Map<T, R>(this Func<Maybe<T>> f, Func<T, R> g) => () => f().Map(g);

    public static Func<Maybe<R>> Bind<T, R>(this Func<Maybe<T>> f, Func<T, Func<Maybe<R>>> g) =>
        () => f().Match(v => g(v)(), Maybe<R>.None);
}

public class MaybeFnTests
{
    private record Address(string City, int Flat);

    [Fact]
    public void Usage()
    {
        var createAddress = (string city, int flat) => new Address(city, flat);
        var validCity = (string city) => city == "Ulsk" ? city : Maybe<string>.None();
        var validFlat = (int flat) => flat == 100 ? flat : Maybe<int>.None();

        var createAddressMaybe = (string city, int flat) =>
            Maybe<Func<string, int, Address>>
                .Of(createAddress)
                .Apply(validCity(city))
                .Apply(validFlat(flat));

        var mapAddress = (Maybe<Address> address) =>
            address.Match(v => v.ToString(), () => "Плохой адрес");

        var badAddress = createAddressMaybe("moscow", 15);
        var goodAddress = createAddressMaybe("Ulsk", 100);

        Console.WriteLine(mapAddress(badAddress));
        Console.WriteLine(mapAddress(goodAddress));
    }
}
