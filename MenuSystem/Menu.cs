namespace MenuSystem;

public class Menu
{
  public string Title { get; set; } = default!;
  public List<MenuItem> Items { get; set; } = new();

  public EMenuLevel Level { get; set; }
  
  public Menu(string title, EMenuLevel level)
  {
    Title = title;
    Level = level;
  }

  public void AddMenuItem(string tittle, string hotkey) {
    if (Items.Any(i => i.Hotkey == hotkey)) {
      throw new Exception($"Hotkey {hotkey} is already in use in menu {Title}!");
    }
    
    var item = new MenuItem {Name = tittle, Hotkey = hotkey};
    Items.Add(item);
  }
  
  //Menu Item with dynamic label
  public void AddMenuItem(string tittle, string hotkey, Func<string>? label) {
    if (Items.Any(i => i.Hotkey == hotkey)) {
      throw new Exception($"Hotkey {hotkey} is already in use in menu {Title}!");
    }
    
    var item = new MenuItem {Name = tittle, Hotkey = hotkey, Label = label};
    Items.Add(item);
  }
  
  private void ShowMenu() {
    Console.Clear();                 
    Console.WriteLine("Connect X!");  
    Console.WriteLine(Title);       
    Console.WriteLine("--------------------------");
    foreach (var item in Items)
    {
      Console.WriteLine(item);
    }
  }
  
  public string Run() {
    ShowMenu();
    Console.Write("\nEnter your choice: ");
    return Console.ReadLine()!;
  }
}
