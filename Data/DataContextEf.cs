using Microsoft.EntityFrameworkCore;
using SharlmagneHenryAPI.Models;

namespace SharlmagneHenryAPI.Data;

public class DataContextEf : DbContext
{
    private readonly IConfiguration _config;

    public DataContextEf(IConfiguration config)
    {
        _config = config;
    }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Media> Media { get; set; }
    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                _config.GetConnectionString("AZURE_SQL_CONNECTION"),
                options => options.EnableRetryOnFailure()
            );
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("PortfolioSchema");
    }

}