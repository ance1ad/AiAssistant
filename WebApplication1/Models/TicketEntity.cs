namespace WebApplication1.Models;

public class TicketEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
    public UserEntity? User { get; set; }
    public Guid UserId { get; set; }
}