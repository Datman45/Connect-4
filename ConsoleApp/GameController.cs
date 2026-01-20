using BLL;
using ConsoleUI;
using DAL;

namespace ConsoleApp;

public class GameController
{
  private IRepository<GameState> _configRepository;
  private GameBrain _gameBrain;
  private Ui _ui;
  private GameState _gameState;

  public GameController(GameState gameState, IRepository<GameState> configRepository)
  {
    _gameState = gameState;
    _configRepository = configRepository;

    _ui = new Ui(gameState);
    _gameBrain = new GameBrain(gameState)
    {
      OnBoardUpdated = () =>
      {
        Console.Clear();
        _ui.DrawBoard();
      }
    };
  }

  public bool GameStart()
  {
    while (true)
    {
      Console.Clear();
      _ui.DrawBoard();
      
      if (_gameState.GameWinner != 0 || _gameBrain.CheckForDraw(_gameState.Board)) {
        return PlayerChoiceAfterGameEnd();
      }

      if (_gameState.GameMode == "AiVsAi") {
        Thread.Sleep(1000);
        _gameBrain.AiBehavior(true);
      }
      
      else {
        Console.Write("\nPlayer " + _gameState.CurrentPlayer + " Enter your move (example: 1 = col), save or rm(return to menu): ");
        var playerMove = Console.ReadLine();
        
        switch (playerMove!.ToLower()) {
          case "save":
            _configRepository.Save(_gameState);
            Console.WriteLine("Configuration saved!");
            break;
          
          case "rm":
            _gameState.Reset();
            return true;
          
          default: {
            if (ProcessMove(playerMove, out var playerChoiceAfterGameEnd)) return playerChoiceAfterGameEnd;
            break;
          }
        }
      }
    }
  }

  private bool PlayerChoiceAfterGameEnd()
  {
    while (true)
    {
      if (_gameState.GameWinner != 0) {
        Console.WriteLine("\nPlayer " + _gameState.GameWinner + " wins!");
      }else {
        Console.WriteLine("\nIt`s a draw!");
      }
      
      Console.Write("Do you want to return to game menu? (y/n): ");
      var playerChoice = Console.ReadLine();
      switch (playerChoice!.ToLower())
      {
        case "y":
          _gameState.Reset();
          return true;
        case "n":
          return false;
      }
    }
  }
  
  private bool ProcessMove(string playerMove, out bool playerChoiceAfterGameEnd)
  {
    var currentPlayer = _gameState.CurrentPlayer;
    if (ValidatePlayerMove(playerMove!))
    {
      _gameBrain.PlayerMove(playerMove, true);
      if (currentPlayer != _gameState.CurrentPlayer) 
      {
        if (_gameState.GameMode == "HumanVsAi") {
          if (_gameBrain.CheckForWin(_gameState.Board))
          {
            playerChoiceAfterGameEnd = PlayerChoiceAfterGameEnd();
            return true;
          }
          Console.WriteLine("AI thinking...");
          Console.Write("Press any key to change player...");
          Console.ReadLine();
          _gameBrain.AiBehavior(true);
        }
      }
    }
    
    playerChoiceAfterGameEnd = false; 
    return false;
  }

  public void SetGameState(GameState newState)
  {
    _gameState.Id = newState.Id;
    _gameState.BoardWidth = newState.BoardWidth;
    _gameState.BoardHeight = newState.BoardHeight;
    _gameState.CheckersWinningSize = newState.CheckersWinningSize;
    _gameState.BoardShape = newState.BoardShape;
    
    _gameState.GameMode = newState.GameMode;
    _gameState.AiDifficulty = newState.AiDifficulty;
    
    _gameState.CurrentPlayer = newState.CurrentPlayer;
    _gameState.GameOver = newState.GameOver;
    _gameState.GameWinner = newState.GameWinner;
    _gameState.Player1Id = newState.Player1Id;
    _gameState.Player2Id = newState.Player2Id;
    
    _gameState.Board = newState.Board;
    
    _ui = new Ui(_gameState);
    _gameBrain = new GameBrain(_gameState)
    {
      OnBoardUpdated = () =>
      {
        Console.Clear();
        _ui.DrawBoard();
      }
    };
  }
  
  public void SetConfigRepository(IRepository<GameState> configRepository) {
    _configRepository = configRepository;
  }
  
  private bool ValidatePlayerMove(string playerMove)
     {
    return int.TryParse(playerMove, out var move) && move >= 1 && move <= _gameState.BoardWidth;
  }
}
