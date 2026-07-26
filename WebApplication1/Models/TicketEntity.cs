namespace WebApplication1.Models;

public class TicketEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;
}

public enum TicketStatus
{
    New,
    Processing,
    Answered,
    NeedsHuman,
    Closed
}