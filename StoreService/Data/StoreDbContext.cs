using Microsoft.EntityFrameworkCore;
using StoreService.Entities;

namespace StoreService.Data;

public class StoreDbContext : DbContext
{
    public StoreDbContext(DbContextOptions<StoreDbContext> options)
        : base(options) { }

    public DbSet<Match> Matches => Set<Match>();
    public DbSet<MatchUser> MatchUsers => Set<MatchUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.HasMany(m => m.Players)
                .WithOne(mu => mu.Match)
                .HasForeignKey(mu => mu.MatchId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MatchUser>(entity =>
        {
            entity.HasKey(mu => mu.Id);
        });
    }
}