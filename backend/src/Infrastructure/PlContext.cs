using Microsoft.EntityFrameworkCore;
using PremierLeagueWebApi.Infrastructure.Models;

namespace PremierLeagueWebApi.Infrastructure;

public class PlContext(IConfiguration _configuration, DbContextOptions<PlContext> options) : DbContext(options)
{
    public DbSet<Team> Teams { get; set; }

    public DbSet<Season> Seasons { get; set; }

    public DbSet<Match> Matches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.FullName)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(t => t.UsedName)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(t => t.Abbreviation)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.HasIndex(t => t.FullName).IsUnique();
            entity.HasIndex(t => t.UsedName).IsUnique();
            entity.HasIndex(t => t.Abbreviation).IsUnique();
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.Property(s => s.Label)
                  .IsRequired()
                  .HasMaxLength(7); // "yyyy/yy"

            entity.HasIndex(s => s.Label).IsUnique();
        });

        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasOne(m => m.Season)
                  .WithMany()
                  .HasForeignKey(m => m.SeasonId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.HomeTeam)
                  .WithMany()
                  .HasForeignKey(m => m.HomeTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.AwayTeam)
                  .WithMany()
                  .HasForeignKey(m => m.AwayTeamId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
