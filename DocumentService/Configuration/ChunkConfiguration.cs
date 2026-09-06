using DocumentService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Configurations;

public class ChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
{
    public void Configure(EntityTypeBuilder<DocumentChunk> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.HasIndex(c => new
        {
            c.ChunkIndex,
            c.KnowledgeDocumentId,
        })
        .IsUnique();

        builder
            .HasOne(c => c.KnowledgeDocument)
            .WithMany(d => d.Chunks)
            .HasForeignKey(d => d.KnowledgeDocumentId);
    }
}