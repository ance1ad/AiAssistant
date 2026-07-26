using WebApplication1.Models;

namespace WebApplication1.Dtos;

public record TicketDto(Guid Id, string Message, TicketStatus Status);