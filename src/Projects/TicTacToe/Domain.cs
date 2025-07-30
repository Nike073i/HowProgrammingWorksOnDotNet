namespace HowProgrammingWorksOnDotNet.TicTacToe.Domain
{
    public record HorizPosition
    {
        public static readonly HorizPosition Left = new(1, "Left");
        public static readonly HorizPosition HCenter = new(2, "HCenter");
        public static readonly HorizPosition Right = new(3, "Right");

        public static List<HorizPosition> All => [Left, HCenter, Right];

        public string Name { get; }
        public int Code { get; }

        private HorizPosition(int code, string name)
        {
            Name = name;
            Code = code;
        }
    }

    public record VertPosition
    {
        public static readonly VertPosition Top = new(1, "Top");
        public static readonly VertPosition VCenter = new(2, "VCenter");
        public static readonly VertPosition Bottom = new(3, "Bottom");

        public static List<VertPosition> All => [Top, VCenter, Bottom];

        public string Name { get; }
        public int Code { get; }

        private VertPosition(int code, string name)
        {
            Name = name;
            Code = code;
        }
    }

    public record CellPosition(HorizPosition HorizPosition, VertPosition VertPosition);

    public interface ICellState;

    public enum Player
    {
        PlayerX,
        PlayerO,
    }

    public record Played(Player Player) : ICellState;

    public record Empty : ICellState;

    public record Cell(CellPosition Position, ICellState State);

    public interface IMoveResult
    {
        DisplayInfo Info { get; }
    };

    public record PlayerXToMove(DisplayInfo Info, List<NextMoveInfo> Moves) : IMoveResult;

    public record PlayerOToMove(DisplayInfo Info, List<NextMoveInfo> Moves) : IMoveResult;

    public record GameWon(DisplayInfo Info, Player Player) : IMoveResult;

    public record GameTied(DisplayInfo Info) : IMoveResult;

    public interface IApi
    {
        IMoveResult NewGame();
    }

    public record NextMoveInfo(CellPosition PosToPlay, MoveCapability MoveCapability);

    public record DisplayInfo(List<Cell> Cells);

    public delegate IMoveResult MoveCapability();
}
