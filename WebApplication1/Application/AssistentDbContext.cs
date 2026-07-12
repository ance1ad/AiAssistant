using Microsoft.EntityFrameworkCore;
using WebApplication1.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Application;

public class AssistentDbContext : DbContext
{
    public AssistentDbContext(DbContextOptions<AssistentDbContext> options) 
        : base(options)
    { }
    
    public DbSet<TicketEntity> Tickets { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ArticleEntity> Articles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

}
