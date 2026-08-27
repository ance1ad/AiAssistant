using Microsoft.EntityFrameworkCore;
using WebApplication1.Configurations;
using WebApplication1.Models;

namespace WebApplication1.Application;

public class AssistentDbContext : DbContext
{
    public AssistentDbContext(DbContextOptions<AssistentDbContext> options) 
        : base(options)
    { }
    
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<KnowledgeDocument> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

}
