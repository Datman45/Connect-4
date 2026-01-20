using BLL;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ConfigRepositoryEF(AppDbContext dbContext) : IRepository<GameState>
{
    public List<(string id, string description)> List()
    {
        var res = new List<(string id, string description)>();
        foreach (var dbConf in dbContext.GameState.ToList())
        {
            res.Add((dbConf.Id.ToString(),
                $"board_{dbConf.BoardWidth}x{dbConf.BoardHeight}_win{dbConf.CheckersWinningSize}" +
                $"_player{dbConf.CurrentPlayer}"));
        }
        
        return res;
    }
    
    public async Task<List<(string id, string description)>> ListAsync()
    {
        var res = new List<(string id, string description)>();
        foreach (var dbConf in await dbContext.GameState.ToListAsync())
        {
            res.Add((dbConf.Id.ToString(),
                $"board_{dbConf.BoardWidth}x{dbConf.BoardHeight}_win{dbConf.CheckersWinningSize}" +
                $"_player{dbConf.CurrentPlayer}" + $"_mode:{dbConf.GameMode.ToLower()}"));
        }
        
        return res;
    }

    public string Save(GameState data)
    {
        var entity = dbContext.GameState.Find(Guid.Parse(data.Id.ToString()));
        if (entity != null && data.Id == entity.Id)
        { 
            dbContext.Entry(entity).CurrentValues.SetValues(data);
            dbContext.SaveChanges();
            return data.Id.ToString();
        }

        dbContext.GameState.Add(data);
        dbContext.SaveChanges();
        return data.Id.ToString();
    }

    public GameState Load(string id)
    {
        var entity = dbContext.GameState.Find(Guid.Parse(id));
        var gameState = dbContext.GameState.AsNoTracking().FirstOrDefault(g => g.Id == Guid.Parse(id));
        return gameState ?? throw new Exception($"Game state with ID {id} was not found.");
    }

    public void Delete(string id)
    {
        var entity = dbContext.GameState.Find(Guid.Parse(id));
        if (entity != null) dbContext.GameState.Remove(entity);
        dbContext.SaveChanges();
    }
}
