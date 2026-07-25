namespace WebApplication1.Models;

public class UserEntity
{
    public Guid Id { get; set; }
    public long TelegramId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<TicketEntity> Tickets { get; set; } = [];
}