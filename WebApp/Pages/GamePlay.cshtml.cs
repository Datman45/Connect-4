using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Helpers; 

namespace WebApp.Pages;

public class GamePlay(IRepository<GameState> configRepository) : PageModel
{
    public string GameId { get; set; } = null!;
    public GameBrain GameBrain { get; set; } = null!;
    [BindProperty]
    public GameState GameState { get; set; } = null!;
    
    public void OnGet(string id, string? x) {
        var playerMadeAMove = false;
        var sessionState = HttpContext.Session.Get<GameState>("GameState");
        
        if (sessionState != null) {
            GameState = sessionState;
        }
        else if (!string.IsNullOrEmpty(id))
        {
            GameId = id;
            GameState = configRepository.Load(id)
                        ?? throw new InvalidOperationException("Saved game not found");

            HttpContext.Session.Set("GameState", GameState);
        } else { 
            GameState = new GameState
            {
                BoardHeight = HttpContext.Session.GetInt32("BoardHeight") ?? 6,
                BoardWidth = HttpContext.Session.GetInt32("BoardWidth") ?? 6,
                CheckersWinningSize = HttpContext.Session.GetInt32("CheckersWinningSize") ?? 4,
                BoardShape = HttpContext.Session.GetString("BoardShape") ?? "rectangle",
                GameMode = HttpContext.Session.GetString("GameMode") ?? "",
                AiDifficulty = HttpContext.Session.GetString("AiDifficulty") ?? ""
            }; 
            
            GameState.Resize();
            HttpContext.Session.Set("GameState", GameState);
        }
        
        GameBrain = new GameBrain(GameState);
        
        if (GameState.GameOver)
        {
            return;
        }

        if (GameState.GameMode is "AiVsAi")
        {
            GameBrain.AiBehavior(false);
            GameWin(playerMadeAMove);
        } else {
            if (x != null)
            {
                x = (int.Parse(x) + 1).ToString();
                GameBrain.PlayerMove(x, false);
                GameWin(playerMadeAMove);
                playerMadeAMove = true;
            }

            if (GameState.GameOver) {
                return;
            }
            
            if (playerMadeAMove && GameState.GameMode is "HumanVsAi")
            {
                GameBrain.AiBehavior(false);
                GameWin(playerMadeAMove);
            }
        }

        HttpContext.Session.Set("GameState", GameState);

        if (!string.IsNullOrEmpty(id)) {
            configRepository.Save(GameState);
        } 
    }

    public Task<RedirectToPageResult> OnPostSave() {
        if (!ModelState.IsValid)
        {
            return Task.FromResult(RedirectToPage());
        }

        string? jsonPath;
        string? fileName;
        if (HttpContext.Session.Get<GameState>("GameState") != null) {
            var state = HttpContext.Session.Get<GameState>("GameState")!;
            
            jsonPath = configRepository.Save(state);
            
            if (!Path.GetFileName(jsonPath).Contains(".json"))
                return Task.FromResult(RedirectToPage(new { id = state.Id }));
            
            fileName = Path.GetFileName(jsonPath);
            return Task.FromResult(RedirectToPage(new { id = fileName.Replace(".json", "") }));

        }

        jsonPath = configRepository.Save(GameState);
        if (!Path.GetFileName(jsonPath).Contains(".json"))
            return Task.FromResult(RedirectToPage(new { id = GameState.Id }));
        
        fileName = Path.GetFileName(jsonPath);
        return Task.FromResult(RedirectToPage(new { id = fileName.Replace(".json", "") }));
    }
    
    public Task<RedirectToPageResult> OnPostReturnToMenu() {
        HttpContext.Session.Remove("GameState");
        return Task.FromResult(RedirectToPage("./Index"));
    }

    private void GameWin(bool playerMadeAMove)
    {
        if (GameState.GameMode == "HumanVsAi" && !playerMadeAMove) {
            GameState.GameWinner = GameState.CurrentPlayer == 1 ? 2 : 1;
        }
        
        if (GameBrain.CheckForWin(GameState.Board))
        {
            GameState.GameOver = true;
            GameState.GameWinner = GameState.GameWinner;
        }
    }
}
