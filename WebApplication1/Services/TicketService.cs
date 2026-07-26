using WebApplication1.Configurations;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class TicketService(TicketsRepository ticketsRepository)
{
    private readonly TicketsRepository _ticketsRepository = ticketsRepository;

    public async Task<TicketDto> Create(Guid userId, string message, TicketStatus status)
    {
        var ticketEntity = new TicketEntity()
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Message = message,
            Status = status
        };
        
        await _ticketsRepository.Add(ticketEntity);

        return new TicketDto(ticketEntity.Id, message, status);
    }
    
}