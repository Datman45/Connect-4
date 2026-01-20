using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class Options : PageModel
{
    [BindProperty]
    public string BoardHeight { get; set; } = "6";
    [BindProperty]
    public string BoardWidth { get; set; } = "6";
    [BindProperty]
    public string CheckersWinningSize { get; set; } = "4";
    [BindProperty]
    public string BoardShape { get; set; } = "rectangle";
    
    public void OnGet() {
        BoardHeight = HttpContext.Session.GetInt32("BoardHeight")?.ToString() ?? BoardHeight;
        BoardWidth = HttpContext.Session.GetInt32("BoardWidth")?.ToString() ?? BoardWidth;
        CheckersWinningSize = HttpContext.Session.GetInt32("CheckersWinningSize")?.ToString() ?? CheckersWinningSize;
        BoardShape = HttpContext.Session.GetString("BoardShape") ?? BoardShape;
    }
    
    public void OnPost() {
        HttpContext.Session.SetInt32("BoardHeight", int.Parse(BoardHeight));
        HttpContext.Session.SetInt32("BoardWidth", int.Parse(BoardWidth));
        HttpContext.Session.SetInt32("CheckersWinningSize", int.Parse(CheckersWinningSize));
        HttpContext.Session.SetString("BoardShape", BoardShape);
    }
}
