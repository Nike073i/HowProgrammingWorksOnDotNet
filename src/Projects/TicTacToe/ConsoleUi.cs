using HowProgrammingWorksOnDotNet.TicTacToe.Domain;

namespace HowProgrammingWorksOnDotNet.TicTacToe.ConsoleUi
{
    public class ConsoleGame(IApi api)
    {
        public void StartGame()
        {
            Console.WriteLine("Игра началась!");
            var moveResult = api.NewGame();
            HandleResult(moveResult);
        }

        private void HandleResult(IMoveResult moveResult)
        {
            Console.WriteLine("\n------------------------------\n");

            switch (moveResult)
            {
                case GameTied tied:
                    DisplayCells(tied.Info);
                    Console.WriteLine("Игра завершена - Ничья\n");
                    AskToPlayAgain();
                    break;

                case GameWon won:
                    DisplayCells(won.Info);
                    Console.WriteLine($"Игра завершена. Победа {won.Player}\n");
                    AskToPlayAgain();
                    break;

                case PlayerOToMove oToMove:
                    DisplayCells(oToMove.Info);
                    Console.WriteLine("Ход игрока O");
                    ProcessInput(oToMove.Moves);
                    break;

                case PlayerXToMove xToMove:
                    DisplayCells(xToMove.Info);
                    Console.WriteLine("Ход игрока X");
                    ProcessInput(xToMove.Moves);
                    break;
            }
        }

        private void DisplayCells(DisplayInfo displayInfo)
        {
            var cells = displayInfo.Cells;

            string CellToStr(Cell cell)
            {
                return cell.State switch
                {
                    Empty _ => "-",
                    Played p => p.Player == Player.PlayerX ? "X" : "O",
                    _ => "?",
                };
            }

            void PrintLine(IEnumerable<Cell> cellsToPrint)
            {
                var line = "|" + string.Join("|", cellsToPrint.Select(CellToStr)) + "|";
                Console.WriteLine(line);
            }

            var sortedCells = cells.OrderBy(c =>
                (c.Position.VertPosition.Code, c.Position.HorizPosition.Code)
            );
            sortedCells.Chunk(3).ToList().ForEach(PrintLine);
            Console.WriteLine();
        }

        private void DisplayNextMoves(List<NextMoveInfo> nextMoves)
        {
            for (int i = 0; i < nextMoves.Count; i++)
            {
                var pos = nextMoves[i].PosToPlay;
                Console.WriteLine($"{i}) {pos.HorizPosition.Name} {pos.VertPosition.Name}");
            }
        }

        private void ProcessInput(List<NextMoveInfo> nextMoves)
        {
            DisplayNextMoves(nextMoves);

            Console.WriteLine("Введите номер хода или q для выхода:");
            var input = Console.ReadLine();

            if (input?.ToLower() == "q")
            {
                Console.WriteLine("Пока!");
                return;
            }

            if (int.TryParse(input, out int index) && index >= 0 && index < nextMoves.Count)
            {
                var moveResult = nextMoves[index].MoveCapability();
                HandleResult(moveResult);
            }
            else
            {
                Console.WriteLine("Вводи нормально!");
                ProcessInput(nextMoves);
            }
        }

        private void AskToPlayAgain()
        {
            Console.WriteLine("Заново играть будешь? (y/n)?");
            var input = Console.ReadLine()?.ToLower();

            switch (input)
            {
                case "y":
                    var newGame = api.NewGame();
                    HandleResult(newGame);
                    break;

                case "n":
                    Console.WriteLine("Exiting game.");
                    break;

                default:
                    AskToPlayAgain();
                    break;
            }
        }
    }
}
