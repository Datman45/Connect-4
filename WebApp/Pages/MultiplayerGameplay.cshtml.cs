using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class MultiplayerGameplay(IRepository<GameState> configRepository) : PageModel
{
    public string GameId { get; set; } = null!;
    public GameBrain GameBrain { get; set; } = null!;
    [BindProperty]
    public GameState GameState { get; set; } = null!;
    
    public IActionResult OnGet(string id, Guid? player1Name, Guid? player2Name, string? x) {
        GameId = id;
        HttpContext.Session.SetString("GameId", GameId);
        try {
            GameState = configRepository.Load(id);
        }
        catch {
            return RedirectToPage("./Index");
        }
        
        GameBrain = new GameBrain(GameState);
        

        if (GameState.GameOver) {
            return Page();
        }
        
        if (x != null && GameState.Player2Id != null) {
            if (HttpContext.Session.GetString("Player1Id") == GameState.Player1Id.ToString() && GameState.CurrentPlayer == 1 
                || HttpContext.Session.GetString("Player2Id") == GameState.Player2Id.ToString() && GameState.CurrentPlayer == 2) {
                x = (int.Parse(x) + 1).ToString();
                GameBrain.PlayerMove(x, false);
                if (GameBrain.CheckForWin(GameState.Board)) {
                    GameState.GameOver = true;
                }
                configRepository.Save(GameState);
            }

            return RedirectToPage(new { id });
        }
        
        return Page();
    }
    
    public Task<RedirectToPageResult> OnPostReturnToMenu() {
        var game = configRepository.Load(HttpContext.Session.GetString("GameId")!);
        
        if (HttpContext.Session.GetString("Player1Id") == game.Player1Id.ToString()) {
            configRepository.Delete(HttpContext.Session.GetString("GameId")!);
        }
        return Task.FromResult(RedirectToPage("./Index"));
    }
}
