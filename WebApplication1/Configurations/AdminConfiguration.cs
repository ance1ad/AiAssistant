using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Models;

namespace WebApplication1.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
{
    public void Configure(EntityTypeBuilder<AdminEntity> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.PasswordHash)
            .IsRequired()
            .HasMaxLength(128);
        
        builder.Property(a => a.Username)
            .IsRequired()
            .HasMaxLength(128);
    }
}