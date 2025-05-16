namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.ValueObjects.StaticFactoryMethod;

// Этот подход имеет смысл в следующих ситуациях:
// 1. Создание объекта является асинхронным. `Task<Money> Create(...)`
// 2. Объект должен вернуть не себя, а косвенную сущность (`ErrorOr<Money> Create(...)`)
// Этот подход применим только в том случае, когда у класса не будет наследников, поскольку наследники требуют конструктора и создают родительский объект через конструктор
public record Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency = "Default")
    {
        // Something check...
        return new Money(amount, currency);
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

public class MoneyTests
{
    [Fact]
    public void MoneyOperations()
    {
        Money vova = -15;
        Money nikita = 15;
        Money balance = nikita + vova;
        Assert.Equal(0, balance.Amount);
    }
}

// Некорректная реализация. PositveMoney создает базовый объект Money минуя все проверки
// Можно решить эту проблему через костыль - в Positive.Create вызывается Money.Create, затем создается Positive. Таким образом будет вызвана проверка, но будет 2 объекта создано
public class IncorrectImplements
{
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        protected Money(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }

        public static Money Create(decimal amount, string currency = "Default")
        {
            // Something check...
            return new Money(amount, currency);
        }
    }

    public record PositiveMoney : Money
    {
        // !!! Никакой Money.Create не вызывается !!! Проверки не отработают
        private PositiveMoney(decimal Amount, string Currency)
            : base(Amount, Currency) { }

        // Скрытие базового метода. Но не страшно, поскольку это статический метод
        public static new PositiveMoney Create(decimal Amount, string Currency = "Default")
        {
            if (Amount < 0)
                throw new ArgumentException("Amount must be positive");
            return new PositiveMoney(Amount, Currency);
        }
    }
}
