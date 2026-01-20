using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class AiDifficulty : PageModel
{
    public RedirectToPageResult OnGetEasy() {
        HttpContext.Session.SetString("AiDifficulty", "easy");
        HttpContext.Session.SetString("GameMode", "HumanVsAi");
        
        return RedirectToPage("./GamePlay");
    }
    
    public RedirectToPageResult OnGetMedium() {
        HttpContext.Session.SetString("AiDifficulty", "medium");
        HttpContext.Session.SetString("GameMode", "HumanVsAi");
        
        return RedirectToPage("./GamePlay");
    }
    
    public RedirectToPageResult OnGetHard() {
        HttpContext.Session.SetString("AiDifficulty", "hard");
        HttpContext.Session.SetString("GameMode", "HumanVsAi");
        
        return RedirectToPage("./GamePlay");
    }
}
