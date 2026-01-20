using System.Text.Json;
using BLL;

namespace DAL;

public class ConfigRepositoryJson : IRepository<GameState>
{
    public List<(string id, string description)> List()
    {
        var dir = FileSystemHelpers.GetGameDirectory();
        var res = new List<(string id, string description)>();

        foreach (var fullFileName in  Directory.EnumerateFiles(dir))
        {
            var fileName = Path.GetFileName(fullFileName);
            if (!fileName.EndsWith(".json")) continue;
            res.Add((Path.GetFileName(fileName), 
                Path.GetFileNameWithoutExtension(fileName)));
        }
        return res;
    }
    
    public async Task<List<(string id, string description)>> ListAsync()
    {
        return List();
    }

    public string Save(GameState data)
    {
        var jsonStr = JsonSerializer.Serialize(data);
        
        var fileName = $"id_{data.Id}_board_{data.BoardWidth}x{data.BoardHeight}_win{data.CheckersWinningSize}" +
                       $"_game-mode.{data.GameMode.ToLower()}.json";
        var fullFileName = FileSystemHelpers.GetGameDirectory() + Path.DirectorySeparatorChar + fileName;
        File.WriteAllText(fullFileName, jsonStr);
        
        return fullFileName;
    }

    public GameState Load(string id)
    {
        var jsonFileName = FileSystemHelpers.GetGameDirectory() + Path.DirectorySeparatorChar + id + ".json";
        var jsonText = File.ReadAllText(jsonFileName);
        var conf = JsonSerializer.Deserialize<GameState>(jsonText);
        
        return conf ?? throw new Exception("Could not load game state from file. " + jsonText);
    }

    public void Delete(string id)
    {
        var jsonFilename = FileSystemHelpers.GetGameDirectory() + Path.DirectorySeparatorChar + id + ".json";
        if (File.Exists(jsonFilename))
        {
            File.Delete(jsonFilename); 
        }
    }
}
