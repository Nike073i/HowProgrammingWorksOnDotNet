// Некорректная реализация. PositveMoney создает базовый объект Money минуя все проверки

// namespace HowProgrammingWorksOnDotNet.ObjectOrientedProgramming.ValueObjects;

// public record Money
// {
//     public decimal Amount { get; }
//     public string Currency { get; }

//     // Internal защищает от несанкционированного доступа к конструктору вне метода Create через наследование.
//     // Унаследовать в обход Create можно только в том-же проекте. В другом проекте для наследования доступен лишь конструктор копирования от Record
//     internal Money(decimal amount, string currency)
//     {
//         Amount = amount;
//         Currency = currency;
//     }

//     public static Money Create(decimal amount, string currency = "Default")
//     {
//         // Something check...
//         return new Money(amount, currency);
//     }

//     public static Money operator +(Money first, Money second)
//     {
//         if (first.Currency != second.Currency)
//             throw new InvalidOperationException("Currencies have to be equal");
//         // or return first with { Amount = first.Amount + second.Amount };
//         return Create(first.Amount + second.Amount, first.Currency);
//     }

//     public static implicit operator Money(decimal amount) => Create(amount);
// }

// public record PositiveMoney : Money
// {
//     private PositiveMoney(decimal Amount, string Currency)
//         : base(Amount, Currency) { }

//     public static new PositiveMoney Create(decimal Amount, string Currency = "Default")
//     {
//         if (Amount < 0)
//             throw new ArgumentException("Amount must be positive");
//         return new PositiveMoney(Amount, Currency);
//     }

//     public static implicit operator PositiveMoney(decimal amount) => Create(amount);
// }

// public class MoneyTests
// {
//     [Fact]
//     public void MoneyOperations()
//     {
//         Money vova = -15;
//         Assert.Throws<ArgumentException>(() =>
//         {
//             PositiveMoney error = -15;
//         });
//         PositiveMoney nikita = 15;

//         Money balance = nikita + vova;
//         Assert.Equal(0, balance.Amount);
//     }
// }
