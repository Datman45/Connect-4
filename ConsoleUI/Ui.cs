using BLL;

namespace ConsoleUI;

public class Ui(GameState gameState)
{
    public void DrawBoard()
    {
        var board = gameState.Board;
        
        for (var i = 0; i < gameState.BoardHeight; i++)
        {
            for (var j = 0; j < gameState.BoardWidth; j++)
            {
                Console.Write("|" + board[i][j] + "|");
            }

            Console.WriteLine();
        }
    }
}
