using System;

namespace BLL;

public class GameState : BaseEntity
{
    public int BoardWidth { get; set; } = 6;
    public int BoardHeight { get; set; } = 6;
    public int CheckersWinningSize { get; set; } = 4;
    public int CurrentPlayer { get; set; } = 1;
    public ECellState[][] Board { get; set; }
    public string BoardShape { get; set; } = "rectangle";
    public string GameMode { get; set; } = "";
    public string AiDifficulty { get; set; } = "";
    public bool GameOver { get; set; } = false;
    public int GameWinner { get; set; } = 0;
    public Guid? Player1Id { get; set; }
    public Guid? Player2Id { get; set; }

    public GameState()
    {
        Board = CreateBoard(BoardHeight, BoardWidth);
    }

    private static ECellState[][] CreateBoard(int height, int width)
    {
        var board = new ECellState[height][];
        for (var i = 0; i < height; i++)
        {
            board[i] = new ECellState[width];
        }
        return board;
    }

    public void Resize()
    {
        Board = CreateBoard(BoardHeight, BoardWidth);
    }

    public void Reset() {
        CurrentPlayer = 1;
        Board = CreateBoard(BoardHeight, BoardWidth);
        GameOver = false; 
        GameWinner = 0;
    }
}
