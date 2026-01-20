namespace MenuSystem;

public class MenuItem
{
    public string Name { get; set; } = default!;
    public string Hotkey { get; set; } = default!;
    
    public Func<string>? Label { get; set; }
    
    public Func<string>? Action { get; set; } = default!;

    public override string ToString() {
        var label = Label?.Invoke();
        return string.IsNullOrEmpty(label) ? $"{Name}) {Hotkey}" : $"{Name}: {Label!()}) {Hotkey}";
    }
}
