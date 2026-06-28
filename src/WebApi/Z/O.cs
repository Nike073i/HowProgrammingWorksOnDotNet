namespace HowProgrammingWorksOnDotNet.WebApi.O;

public class CounterUpdatedEvent;

public class IncrementState;

public class IncrementCommand;

public delegate CounterUpdatedEvent IncrementCounterDecider(
    IncrementState state,
    IncrementCommand command
);


// public interface IEvent;

// public interface IStore<TState>
// {
//     TState GetState();
//     void Evolve(IEvent @event);
// }

// public interface IIntention;

// public interface IDecider<TState, TIntention>
//     where TIntention : IIntention
// {
//     IEvent Increment(TIntention intention);
// };

// public class UseCase<TStore, TState, TParam, TDecider, TIntention>(
//     TStore store,
//     TDecider decider,
//     Func<TParam, TIntention> intentionFactory
// )
//     where TStore : IStore<TState>
//     where TDecider : IDecider<TState, TIntention>
//     where TIntention : IIntention
// {
//     public IEvent Execute(TParam param)
//     {
//         var intention = intentionFactory(param);
//         var state = store.GetState();
//         var @event = decider.Decide(state, intention);
//         store.Evolve(@event);
//         return @event;
//     }
// }
