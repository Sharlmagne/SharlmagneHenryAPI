using Microsoft.EntityFrameworkCore;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Data;

public class DataContextEf(DbContextOptions<DataContextEf> options) : DbContext(options)
{
    public virtual DbSet<Project> Projects => Set<Project>();
    public virtual DbSet<Media> Media => Set<Media>();
    public virtual DbSet<Skill> Skills => Set<Skill>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PortfolioSchema");

        modelBuilder
            .Entity<Project>()
            .HasMany(p => p.Skills)
            .WithMany(s => s.Projects)
            .UsingEntity(j => j.ToTable("ProjectSkills"));
    }
}
