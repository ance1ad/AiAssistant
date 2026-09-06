using DocumentService.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Configurations;

namespace DocumentService.Application;

public class DocumentDbContext : DbContext
{
    public DocumentDbContext(DbContextOptions<DocumentDbContext> options) 
        : base(options)
    { }

    public DbSet<KnowledgeDocument> Documents => Set<KnowledgeDocument>();
    public DbSet<DocumentChunk> Chunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DocumentConfiguration());
        modelBuilder.ApplyConfiguration(new ChunkConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}