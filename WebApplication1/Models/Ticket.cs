namespace WebApplication1.Models;

public class Ticket
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public enum TicketStatus
{
    New,
    Processing,
    Answered,
    NeedsHuman,
    Closed
}