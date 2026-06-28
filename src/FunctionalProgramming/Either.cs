namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.EitherFirst
{
    public interface IEither<L, R>
    {
        IEither<L, CR> Map<CR>(Func<R, CR> fn);
        IEither<L, CR> Bind<CR>(Func<R, IEither<L, CR>> _);

        void Switch(Action<R> onRight, Action<L>? onLeft = null);

        G Match<G>(Func<R, G> onRight, Func<L, G> onLeft);

        IEither<L, R> Filter(Func<R, bool> predicate, L left);
    }

    public class Left<L, R> : IEither<L, R>
    {
        private readonly L _value;

        private Left(L value) => _value = value;

        public static Left<L, R> Create(L value) => new(value);

        public static implicit operator Left<L, R>(L value) => Left<L, R>.Create(value);

        public IEither<L, RR> Map<RR>(Func<R, RR> _) => Left<L, RR>.Create(_value);

        public IEither<L, RR> Bind<RR>(Func<R, IEither<L, RR>> _) => Left<L, RR>.Create(_value);

        public void Switch(Action<R> _, Action<L>? onLeft = null) => onLeft?.Invoke(_value);

        public G Match<G>(Func<R, G> onRight, Func<L, G> onLeft) => onLeft(_value);

        public IEither<L, R> Filter(Func<R, bool> _, L __) => this;
    }

    public class Right<L, R> : IEither<L, R>
    {
        private readonly R _value;

        private Right(R value) => _value = value;

        public static Right<L, R> Create(R value) => new(value);

        public IEither<L, RR> Map<RR>(Func<R, RR> fn) => Right<L, RR>.Create(fn(_value));

        public IEither<L, RR> Bind<RR>(Func<R, IEither<L, RR>> fn) => fn(_value);

        public static implicit operator Right<L, R>(R value) => Right<L, R>.Create(value);

        public void Switch(Action<R> onRight, Action<L>? _ = null) => onRight(_value);

        public G Match<G>(Func<R, G> onRight, Func<L, G> _) => onRight(_value);

        public IEither<L, R> Filter(Func<R, bool> predicate, L left) =>
            predicate(_value) ? this : Left<L, R>.Create(left);
    }

    public class EitherTests
    {
        [Fact]
        public void Usage()
        {
            static IEither<string, int> Workflow(Right<string, int> input) =>
                input
                    .Map(v => v * 2)
                    .Bind<int>(v =>
                        v < 10
                            ? Left<string, int>.Create("Недобор")
                            : Right<string, int>.Create(v / 3)
                    )
                    .Filter(v => v < 5, "Другая ошибка");

            foreach (var i in Enumerable.Range(1, 10))
                Workflow(i).Switch(Console.WriteLine, Console.WriteLine);
        }
    }
}

namespace HowProgrammingWorksOnDotNet.FunctionalProgramming.EitherSecond
{
    public class Either<L, R>
    {
        private readonly L? _left;
        private readonly R? _right;
        private readonly bool _itsLeft;

        private Either(L left)
        {
            _left = left;
            _itsLeft = true;
        }

        private Either(R right)
        {
            _right = right;
            _itsLeft = false;
        }

        public static implicit operator Either<L, R>(L value) => new(value);

        public static implicit operator Either<L, R>(R value) => new(value);

        public Either<L, CR> Map<CR>(Func<R, CR> fn) =>
            Match(right => fn(right), left => new Either<L, CR>(left));

        public Either<L, CR> Bind<CR>(Func<R, Either<L, CR>> fn) =>
            Match(right => fn(right), left => new Either<L, CR>(left));

        public void Switch(Action<R> onRight, Action<L>? onLeft = null)
        {
            if (_itsLeft)
                onLeft?.Invoke(_left!);
            else
                onRight(_right!);
        }

        public G Match<G>(Func<R, G> onRight, Func<L, G> onLeft)
        {
            if (_itsLeft)
                return onLeft(_left!);
            return onRight(_right!);
        }

        public Either<L, R> Filter(Func<R, bool> predicate, L left)
        {
            if (_itsLeft)
                return this;
            return predicate(_right!) ? this : left;
        }
    }

    public class EitherTests
    {
        [Fact]
        public void Usage()
        {
            static Either<string, int> Workflow(Either<string, int> input) =>
                input
                    .Map(v => v * 2)
                    .Bind<int>(v => v < 10 ? "Недобор" : v / 3)
                    .Filter(v => v < 5, "Другая ошибка");

            foreach (var i in Enumerable.Range(1, 10))
                Workflow(i).Switch(Console.WriteLine, Console.WriteLine);
        }
    }
}
