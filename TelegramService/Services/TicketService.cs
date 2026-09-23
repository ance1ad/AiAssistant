using TelegramService.Dtos.Ticket;
using TelegramService.Models;
using TelegramService.Repositories;

namespace TelegramService.Services;

public class TicketService(TicketsRepository ticketsRepository)
{
    private readonly TicketsRepository _ticketsRepository = ticketsRepository;

    public async Task<TicketResponse> Create(Guid userId, string message, TicketStatus status)
    {
        var ticketEntity = new Ticket()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            Status = status
        };
        
        await _ticketsRepository.Add(ticketEntity);

        return new TicketResponse(ticketEntity.Id, message, status);
    }
    
}