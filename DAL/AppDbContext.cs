using System.Text.Json;
using BLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL;

public class AppDbContext : DbContext
{
   public DbSet<GameState> GameState { get; set; }
   
   public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
   {
   }
   
   protected override void OnModelCreating(ModelBuilder modelBuilder){
      var boardConverter = new ValueConverter<ECellState[][], string>(
         v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
         v => JsonSerializer.Deserialize<ECellState[][]>(v, (JsonSerializerOptions?)null)!
      );

      modelBuilder.Entity<GameState>()
         .Property(g => g.Board)
         .HasConversion(boardConverter);
      base.OnModelCreating(modelBuilder);

      // remove all cascade deletion stuff
      foreach (var relationship in modelBuilder.Model
                  .GetEntityTypes()
                  .Where(e => !e.IsOwned())
                  .SelectMany(e => e.GetForeignKeys()))
      {
         relationship.DeleteBehavior = DeleteBehavior.Restrict;
      }
   }
   
}
