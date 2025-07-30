using HowProgrammingWorksOnDotNet.TicTacToe.ConsoleUi;
using HowProgrammingWorksOnDotNet.TicTacToe.Domain;
using HowProgrammingWorksOnDotNet.TicTacToe.Implementation;

namespace HowProgrammingWorksOnDotNet.TicTacToe.Tests
{
    public class TicTacToeTests
    {
        [Fact]
        public void Play()
        {
            var api = new Api();
            var game = new ConsoleGame(api);
            game.StartGame();
        }

        [Fact]
        public void TryHack_DoubleMove()
        {
            var api = new Api();

            var state1 = api.NewGame();
            PrintNotEmptyCells(state1);

            var state2 = ((PlayerXToMove)state1).Moves[0].MoveCapability();
            PrintNotEmptyCells(state2);

            // Двойной ход не случится, поскольку игра клонируется. Каждый ход возращается новая игра
            var doubleMove = ((PlayerXToMove)state1).Moves[1].MoveCapability();
            PrintNotEmptyCells(doubleMove);

            static void PrintNotEmptyCells(IMoveResult moveResult)
            {
                Console.WriteLine(
                    string.Join(", ", moveResult.Info.Cells.Where(c => c.State is not Empty))
                );
            }
        }
    }
}
