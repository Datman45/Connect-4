using BLL;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages;

public class LoadGame(IRepository<GameState> configRepository) : PageModel
{
    [BindProperty]
    public string GameId { get; set; } = null!;
    public SelectList ListOfGames { get; set; } = null!;
    public async Task OnGetAsync() {
        await LoadDataAsync();
    }
    
    private async Task LoadDataAsync() {
        var data = await configRepository.ListAsync();
        var data2 = data.Select(i => new
        {
            i.id,
            value = i.description
        }).ToList();
        
        ListOfGames = new SelectList(data2, "id", "value");
    }
    
    public async Task<IActionResult> OnPostLoadAsync() {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }
        
        if (!string.IsNullOrEmpty(GameId) && GameId.Contains(".json")) {
            GameId = GameId.Replace(".json", "");
        }
        
        return RedirectToPage("./GamePlay", new { id = GameId});
    }

    public async Task<IActionResult> OnPostDelete() {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        if (!string.IsNullOrEmpty(GameId) && GameId.Contains(".json")) {
            GameId = GameId.Replace(".json", "");
        }
        
        configRepository.Delete(GameId);
        
        await LoadDataAsync();
        return Page();
    }
}
