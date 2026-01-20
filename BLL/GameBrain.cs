using System;
using System.Linq;
using System.Threading;

namespace BLL;

public class GameBrain(GameState gameState)
{
    public Action? OnBoardUpdated { get; set; }
    public void PlayerMove(string? playerMove, bool animate)
    {
        var col = int.Parse(playerMove!) - 1;

        if (gameState.Board[0][col] != ECellState.E) return;
        
        MakeAMove(col, animate);

        if (CheckForWin(gameState.Board ) && !gameState.GameOver) {
            gameState.GameOver = true;
            gameState.GameWinner = gameState.CurrentPlayer;
        }
        
        ChangePlayer();
    }

    private void ChangePlayer() {
        gameState.CurrentPlayer = gameState.CurrentPlayer == 1 ? 2 : 1;
    }
    
     public bool CheckForWin(ECellState[][] board) {
        var winSize = gameState.CheckersWinningSize;
        var height = gameState.BoardHeight;
        var width = gameState.BoardWidth;

        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                var cell = board[row][col];
                if (cell == ECellState.E) continue;

                //Horizontal
                if (gameState.BoardShape == "rectangle") {
                    if (col + winSize <= width)
                    {
                        if (GetWinInRectangleShape(board, winSize, row, col, cell, "horizontal")) return true;
                    }
                }
                
                else
                {
                    if (GetWinInCylinderShape(board, winSize, col, width, row, cell, "horizontal", height)) return true;
                }

                //Vertical
                if (row + winSize <= height) 
                {
                    if (GetWinInRectangleShape(board, winSize, row, col, cell, "vertical")) return true;
                }

                // Diagonal down-right
                if (gameState.BoardShape == "rectangle")
                {
                    if (row + winSize <= height && col + winSize <= width)
                    {
                        if (GetWinInRectangleShape(board, winSize, row, col, cell, "diagonalDownRight")) return true;
                    }
                }
                else
                {
                    if (GetWinInCylinderShape(board, winSize, col, width, row, cell, "diagonalDownRight", height)) return true;
                }

                // Diagonal up-right
                if (gameState.BoardShape == "rectangle")
                {
                    if (row - winSize + 1 < 0 || col + winSize > width) continue;
                    {
                        if (GetWinInRectangleShape(board, winSize, row, col, cell, "diagonalUpRight")) return true;
                    }
                }
                else
                {
                    if (GetWinInCylinderShape(board, winSize, col, width, row, cell, "diagonalUpRight", height)) return true;
                }
            }
        }

        return false;
    }
     
     public bool CheckForDraw(ECellState[][] board) {
        return !CheckForWin(board) && board.All(row => row.All(cell => cell != ECellState.E));
     }
     
    private static bool GetWinInRectangleShape(ECellState[][] board, int winSize, int row, int col, ECellState cell, string direction)
    {
        var win = true;
        for (var i = 1; i < winSize; i++) {
            
            switch (direction)
            {
                case "horizontal" when board[row][col + i] == cell:
                case "vertical" when board[row + i][col] == cell:
                case "diagonalDownRight" when board[row + i][col + i] == cell:
                case "diagonalUpRight" when board[row - i][col + i] == cell:
                    continue;
            }
            
            win = false;
            break;
        }

        return win;
    }

    private static bool GetWinInCylinderShape(ECellState[][] board, int winSize, int col, int width, int row,
        ECellState cell, string direction, int height) {
        for (var i = 1; i < winSize; i++) {
            var wrappedCol = (col + i) % width;

            var wrappedRow = direction switch
            {
                "diagonalDownRight" => row + i,
                "diagonalUpRight" => row - i,
                _ => row
            };

            if (wrappedRow < 0 || wrappedRow >= height)
                return false;

            if (board[wrappedRow][wrappedCol] != cell)
                return false;
        }

        return true;
    }

    public void AiBehavior(bool animate)
    {
        switch (gameState.AiDifficulty)
        {
            case "easy":
                AiRandomMove(animate);
                break;
            case "medium" when AiWinMove(animate):
            case "medium" when AiBlockMove(animate):
                break;
            case "medium": 
                AiRandomMove(animate);
                break;
            case "hard":
                var bestColumn = FindBestMove();
                MakeAMove(bestColumn, animate);
                break;
        }
        
        if (CheckForWin(gameState.Board) && !gameState.GameOver) {
            gameState.GameOver = true;  
            gameState.GameWinner = gameState.CurrentPlayer;
        }
        
        ChangePlayer();
    }

    private void AiRandomMove(bool animate)
    {
        var rnd = new Random();
        var randomMove = rnd.Next(0, gameState.BoardWidth);
        
        MakeAMove(randomMove, animate);
    }

    private bool AiBlockMove(bool animate)
    {
        ChangePlayer();

        for (var i = 0; i < gameState.BoardHeight; i++) {
            
            for (var j = 0; j < gameState.BoardWidth; j++)
            {
                if (gameState.Board[i][j] == ECellState.E) {
                    gameState.Board[i][j] = gameState.CurrentPlayer == 1 ? ECellState.X : ECellState.O;
                    if (CheckForWin(gameState.Board)) {
                        gameState.Board[i][j] = ECellState.E;
                        ChangePlayer();
                        MakeAMove(j, animate);
                        return true;
                    }

                    gameState.Board[i][j] = ECellState.E;
                }
            }
        }
        ChangePlayer();
        return false;
    }
    
    private bool AiWinMove(bool animate)
    {

        for (var i = 0; i < gameState.BoardHeight; i++)
        {
            for (var j = 0; j < gameState.BoardWidth; j++)
            {
                if (gameState.Board[i][j] == ECellState.E)
                {
                    gameState.Board[i][j] = gameState.CurrentPlayer == 1 ? ECellState.X : ECellState.O;
                    if (CheckForWin(gameState.Board))
                    {
                        gameState.Board[i][j] = ECellState.E;
                        MakeAMove(j, animate);
                        return true;
                    }

                    gameState.Board[i][j] = ECellState.E;
                }
            }
        }
        return false;
    }

    private int AiMinimax(ECellState[][] board, int depth, int maximizingPlayer, int alpha, int beta) {
        
        if (CheckForWin(board))
        {
            var winner = maximizingPlayer == 1 ? 2 : 1;
            return winner == 2 ? 10 - depth : -10 + depth;
        }

        if (CheckForDraw(board)) return 0;
        
        if (depth >= 6) return EvaluateBoard(board);

        if (maximizingPlayer == 2) {
            var bestScore = int.MinValue;
                for (var j = 0; j < gameState.BoardWidth; j++) {
                    var lastEmptyCell = GetEmptyCellInTheColumn(board, j);
                    if (lastEmptyCell == -1) continue;
                    board[lastEmptyCell][j] = ECellState.O;
                    var score = AiMinimax(board, depth + 1, 1, alpha, beta);
                    board[lastEmptyCell][j] = ECellState.E;
                    bestScore = Math.Max(score, bestScore);
                    alpha = Math.Max(alpha, score);
                    if (alpha >= beta) {
                        break;
                    }
                }
            return bestScore;
        }
        
        else {
            var bestScore = int.MaxValue;
                for (var j = 0; j < gameState.BoardWidth; j++) {
                    var lastEmptyCell = GetEmptyCellInTheColumn(board, j);
                    if (lastEmptyCell == -1) continue;
                    board[lastEmptyCell][j] = ECellState.X;
                    var score = AiMinimax(board, depth + 1, 2, alpha, beta);
                    board[lastEmptyCell][j] = ECellState.E;
                    bestScore = Math.Min(score, bestScore);
                    beta = Math.Min(beta, score);
                    if (beta <= alpha) {
                        break;
                    }
                }
                
            return bestScore;
        }
        
    }

    private int FindBestMove() {
        var bestScore = int.MinValue;
        var bestColumn = 0;

        for (var j = 0; j < gameState.BoardWidth; j++)
        {
            var lastEmptyCell = GetEmptyCellInTheColumn(gameState.Board, j);
            if (lastEmptyCell == -1) continue;
            
            gameState.Board[lastEmptyCell][j] = ECellState.O;
            var score = AiMinimax(gameState.Board, 0, 1, int.MinValue, int.MaxValue);
            gameState.Board[lastEmptyCell][j] = ECellState.E;
            
            if (score > bestScore) {
                bestScore = score;
                bestColumn = j;
            }
        }
        return bestColumn;
    }
    
    private int EvaluateBoard(ECellState[][] board)
    {
        var score = 0;
        var center = gameState.BoardWidth / 2;

        for (var i = 0; i < gameState.BoardHeight; i++)
        {
            if (board[i][center] == ECellState.O) score += 2;
            if (board[i][center] == ECellState.X) score -= 2;
        }

        return score;
    }
    
    private void MakeAMove(int row, bool animate)
    {
        var lastEmptyCell = GetEmptyCellInTheColumn(gameState.Board, row);

        if (lastEmptyCell == -1) return;

        if (animate) {
            for (var animRow = 0; animRow <= lastEmptyCell; animRow++)
            {
                gameState.Board[animRow][row] =  gameState.CurrentPlayer == 1 ? ECellState.X : ECellState.O;
                OnBoardUpdated?.Invoke(); 
                Thread.Sleep(100);
                gameState.Board[animRow][row] = ECellState.E;
            }
        }
        
        gameState.Board[lastEmptyCell][row] = gameState.CurrentPlayer == 1 ? ECellState.X : ECellState.O;
    }

    private int GetEmptyCellInTheColumn(ECellState[][] board, int row) {
        for (var column = gameState.BoardHeight - 1; column >= 0; column--) {
            if (board[column][row] == ECellState.E) {
                return column;
            }
        }
        
        return -1;
    }
}
