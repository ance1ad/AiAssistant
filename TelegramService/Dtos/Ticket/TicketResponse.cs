using TelegramService.Models;

namespace TelegramService.Dtos.Ticket;

public record TicketResponse(
    Guid Id, 
    string Message, 
    TicketStatus Status
);