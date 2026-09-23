using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TelegramService.Models;

namespace TelegramService.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        
        builder.Property(u => u.Name)
            .HasMaxLength(60);

        
        builder.HasIndex(u => u.TelegramId)
            .IsUnique();
        
        
        builder
            .HasMany(u => u.Tickets)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId);
    }
}