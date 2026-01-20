namespace DAL;

public class FileSystemHelpers
{
    private const string AppName = "ConnectX";
    
    public static string GetConfigDirectory()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var finalDirectory = homeDirectory + Path.DirectorySeparatorChar 
                                           + AppName + Path.DirectorySeparatorChar + "configs";
        
        Directory.CreateDirectory(finalDirectory);
        
        return finalDirectory;
    }
    
    public static string GetGameDirectory()
    {
        var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        var finalDirectory = homeDirectory + Path.DirectorySeparatorChar 
                             + AppName + Path.DirectorySeparatorChar + "savegames";
        
        Directory.CreateDirectory(finalDirectory);
        
        return finalDirectory;
    }
}
