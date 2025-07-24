using UnityEngine;
using Microsoft.EntityFrameworkCore;

public class SQLiteContext : DbContext
{
    public DbSet<LevelData> Levels
    {
        get; 
        set;
    }
    
    public SQLiteContext() : base()
    {
        
    }
    
    public SQLiteContext(DbContextOptions<SQLiteContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<LevelData>();
    }
}
