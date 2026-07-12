namespace WebApplication1.Models;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<TicketEntity> Tickets { get; set; } = [];
}