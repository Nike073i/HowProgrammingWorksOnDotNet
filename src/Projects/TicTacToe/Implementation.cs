using HowProgrammingWorksOnDotNet.TicTacToe.Domain;

namespace HowProgrammingWorksOnDotNet.TicTacToe.Implementation
{
    public class Game : ICloneable
    {
        private readonly Dictionary<CellPosition, Cell> _cells;

        public static Game NewGame()
        {
            var allPositions = HorizPosition.All.SelectMany(h =>
                VertPosition.All.Select(v => new CellPosition(h, v))
            );

            return new Game(allPositions.Select(pos => new Cell(pos, new Empty())));
        }

        private Game(IEnumerable<Cell> cells) =>
            _cells = cells.ToDictionary(keySelector: c => c.Position, elementSelector: c => c);

        public DisplayInfo DisplayInfo => new([.. _cells.Values]);

        public bool IsGameWonBy(Player player)
        {
            bool CellWasPlayedByPlayer(Cell cell) =>
                cell.State is Played played && played.Player == player;

            bool LineIsAllSamePlayer(Line line) =>
                line.Cells.Select(pos => _cells[pos]).All(CellWasPlayedByPlayer);

            return Line.All.Any(LineIsAllSamePlayer);
        }

        public bool IsGameTied() => _cells.Values.All(cell => cell.State is Played);

        private void UpdateCell(CellPosition position, Cell cell) => _cells[position] = cell;

        public void MakeMove(CellPosition position, Player player) =>
            UpdateCell(position, new Cell(position, new Played(player)));

        public object Clone() => new Game([.. _cells.Values]);

        public Game CloneGame() => (Game)Clone();

        public List<CellPosition> RemainingMoves =>
            [.. _cells.Values.Where(cell => cell.State is Empty).Select(cell => cell.Position)];
    }

    public record Line
    {
        public static readonly Line MainDiagonal = new(
            [
                new(HorizPosition.Left, VertPosition.Top),
                new(HorizPosition.HCenter, VertPosition.VCenter),
                new(HorizPosition.Right, VertPosition.Bottom),
            ]
        );

        public static readonly Line SecondaryDiagonal = new(
            [
                new(HorizPosition.Right, VertPosition.Top),
                new(HorizPosition.HCenter, VertPosition.VCenter),
                new(HorizPosition.Left, VertPosition.Bottom),
            ]
        );

        public static readonly List<Line> HLines = VertPosition.All.Select(MakeHLine).ToList();
        public static readonly List<Line> VLines = HorizPosition.All.Select(MakeVLine).ToList();

        public static List<Line> All => [.. HLines, .. VLines, MainDiagonal, SecondaryDiagonal];

        public IReadOnlyCollection<CellPosition> Cells { get; }

        private Line(List<CellPosition> cells) => Cells = cells;

        private static Line MakeHLine(VertPosition vertPosition) =>
            new(HorizPosition.All.Select(h => new CellPosition(h, vertPosition)).ToList());

        private static Line MakeVLine(HorizPosition horizPosition) =>
            new(VertPosition.All.Select(v => new CellPosition(horizPosition, v)).ToList());
    }

    public class Api : IApi
    {
        private static Player OtherPlayer(Player player) =>
            player switch
            {
                Player.PlayerX => Player.PlayerO,
                Player.PlayerO => Player.PlayerX,
                _ => throw new ArgumentOutOfRangeException(nameof(player)),
            };

        private static NextMoveInfo MakeNextMoveInfo(
            Func<Player, CellPosition, Game, IMoveResult> f,
            Player player,
            Game game,
            CellPosition cellPos
        )
        {
            IMoveResult Capability() => f(player, cellPos, game);
            return new NextMoveInfo(cellPos, Capability);
        }

        private static IMoveResult MoveResultFor(
            Player player,
            DisplayInfo displayInfo,
            List<NextMoveInfo> nextMoves
        ) =>
            player switch
            {
                Player.PlayerX => new PlayerXToMove(displayInfo, nextMoves),
                Player.PlayerO => new PlayerOToMove(displayInfo, nextMoves),
                _ => throw new ArgumentOutOfRangeException(nameof(player)),
            };

        private static IMoveResult MakeMoveResultWithCapabilities(
            Func<Player, CellPosition, Game, IMoveResult> f,
            Player player,
            Game game
        )
        {
            var displayInfo = game.DisplayInfo;
            var nextMoves = game
                .RemainingMoves.Select(pos => MakeNextMoveInfo(f, player, game, pos))
                .ToList();

            return MoveResultFor(player, displayInfo, nextMoves);
        }

        private IMoveResult PlayerMove(Player player, CellPosition cellPos, Game game)
        {
            var newGameStage = game.CloneGame();
            newGameStage.MakeMove(cellPos, player);

            var displayInfo = newGameStage.DisplayInfo;

            if (newGameStage.IsGameWonBy(player))
                return new GameWon(displayInfo, player);

            if (newGameStage.IsGameTied())
                return new GameTied(displayInfo);

            var otherPlayer = OtherPlayer(player);
            var moveResult = MakeMoveResultWithCapabilities(PlayerMove, otherPlayer, newGameStage);

            return moveResult;
        }

        public IMoveResult NewGame()
        {
            var gameState = Game.NewGame();
            var moveResult = MakeMoveResultWithCapabilities(PlayerMove, Player.PlayerX, gameState);
            return moveResult;
        }
    }
}
