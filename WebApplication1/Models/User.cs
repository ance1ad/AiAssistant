namespace WebApplication1.Models;

public class User
{
    public Guid Id { get; set; }
    public long TelegramId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<Ticket> Tickets { get; set; } = [];
}