using Microsoft.EntityFrameworkCore;
using WebApplication1.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Application;

public class AssistentDbContext : DbContext
{
    public AssistentDbContext(DbContextOptions<AssistentDbContext> options) 
        : base(options)
    { }
    
    
    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Admin> Admins => Set<Admin>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

}
