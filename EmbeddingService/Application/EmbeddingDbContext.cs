using EmbeddingService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmbeddingService.Application;

public class EmbeddingDbContext : DbContext
{
    public EmbeddingDbContext(DbContextOptions<EmbeddingDbContext> options)
    : base(options)
    { }   
    
    public DbSet<Embedding> Embeddings => Set<Embedding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        
        modelBuilder.Entity<Embedding>()
            .Property(x => x.Vector)
            .HasColumnType("vector(768)");
        
        modelBuilder.Entity<Embedding>()
            .Property(x => x.SourceType)
            .IsRequired()
            .HasConversion<string>();
    }
}