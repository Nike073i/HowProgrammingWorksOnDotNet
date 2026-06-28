using HowProgrammingWorksOnDotNet.FunctionalProgramming.Maybe;

namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.EventSource;

public enum AccountStatus
{
    Requested,
    Active,
    Frozen,
    Dormant,
    Closed,
}

// Нет никаких ID и прочего инфраструктурного мусора
public sealed record AccountState(
    AccountStatus Status,
    string Currency,
    decimal Balance = 0,
    decimal AllowedOverdraft = 0
)
{
    public AccountState Debit(decimal amount) => Credit(-amount);

    public AccountState Credit(decimal amount) => this with { Balance = Balance + amount };

    public AccountState UpdateStatus(AccountStatus newStatus) => this with { Status = newStatus };
}

public abstract class Event
{
    public Guid EntityId { get; init; }
    public DateTime Timestamp { get; init; }
}

public abstract class Command
{
    public DateTime Timestamp { get; set; }
}

public class CreatedAccount : Event
{
    public required string Currency { get; init; }
}

public class FreezeAccount : Command
{
    public Guid AccountId { get; set; }
}

public class FrozeAccount : Event;

// Решение Enrico. Но лучше разбить метод на 2 части: Decide и Evolve (паттерн Decider)
public static class Account
{
    public static Maybe<(Event Event, AccountState NewState)> Freeze(
        this AccountState @this,
        FreezeAccount cmd
    )
    {
        if (@this.Status == AccountStatus.Frozen)
            return Maybe<(Event Event, AccountState NewState)>.None();

        var evt = new FrozeAccount { EntityId = cmd.AccountId, Timestamp = cmd.Timestamp };

        var newState = @this.Apply(evt);

        return (evt, newState);
    }

    public static AccountState Create(CreatedAccount evt) =>
        new(Currency: evt.Currency, Status: AccountStatus.Active);

    public static AccountState Apply(this AccountState @this, Event evt) =>
        evt switch
        {
            FrozeAccount => @this with { Status = AccountStatus.Frozen },
            _ => throw new NotImplementedException(),
        };

    public static Maybe<AccountState> From(IEnumerable<Event> history) =>
        history.Match(
            onEmpty: Maybe<AccountState>.None,
            onOtherwise: (createdEvent, otherEvents) =>
                otherEvents.Aggregate(
                    seed: Create((CreatedAccount)createdEvent),
                    func: (state, evt) => state.Apply(evt)
                )
        );
}

//TODO: В контроллер внедряются 2 метода: Func<Guid, AccountState> getAccount и Action<Event> saveAndPublish. Важно, что saveAndPublish выражен в качестве 1 атомарной функции