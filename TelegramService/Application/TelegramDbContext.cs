using Microsoft.EntityFrameworkCore;
using TelegramService.Configuration;
using TelegramService.Models;

namespace TelegramService.Application;

public class TelegramDbContext : DbContext
{
    public TelegramDbContext(DbContextOptions<TelegramDbContext> options)
    : base(options)
    { }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<User> Users => Set<User>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}