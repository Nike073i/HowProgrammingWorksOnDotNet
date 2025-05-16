namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.ValueObjects.Inheritance;

// Исключения в конструкторе могут привести к частично-созданному объекту.
// Этого следует опасаться в 1 случае - когда в классе есть финализатор. В таком случае финализатор отработает при частично-созданном объекте
// Если в иерархии не подразумевается определение финализатора - можно выбрасывать исключения в конструкторе не боясь
public record Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    public Money(decimal amount, string currency = "Default")
    {
        // Something check... throw new Exception("something went wrong...");
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
            throw new InvalidOperationException("Currencies have to be equal");

        return first with
        {
            Amount = first.Amount + second.Amount,
        };
    }

    public static implicit operator Money(decimal amount) => new(amount);
}

public record PositiveMoney : Money
{
    public PositiveMoney(decimal amount, string currency = "Default")
        : base(amount, currency)
    {
        if (Amount < 0)
            throw new ArgumentException("Amount must be positive");
    }

    public static implicit operator PositiveMoney(decimal amount) => new(amount);
}

public class MoneyTests
{
    [Fact]
    public void MoneyOperations()
    {
        Money vova = -15;
        Assert.Throws<ArgumentException>(() =>
        {
            PositiveMoney error = -15;
        });
        PositiveMoney nikita = 15;

        Money balance = nikita + vova;
        Assert.Equal(0, balance.Amount);
    }
}
