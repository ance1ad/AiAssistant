using EmbeddingService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmbeddingService.Application;

public class EmbeddingDbContext : DbContext
{
    public EmbeddingDbContext(DbContextOptions<EmbeddingDbContext> options)
    : base(options)
    { }   
    
    public DbSet<DocumentChunk> Chunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");

        // Указываем что это не новая таблица
        modelBuilder.Entity<DocumentChunk>()
            .ToTable("Chunks");
        
        modelBuilder.Entity<DocumentChunk>()
            .Property(x => x.Embedding)
            .HasColumnType("vector(768)");
    }
}