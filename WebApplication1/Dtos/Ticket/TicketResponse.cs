using WebApplication1.Models;

namespace WebApplication1.Dtos;

public record TicketResponse(
    Guid Id, 
    string Message, 
    TicketStatus Status
);