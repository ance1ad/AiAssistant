using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class TicketsRepository(AssistentDbContext dbContext)
{
    private readonly AssistentDbContext _dbContext = dbContext;
    
    
    public async Task<List<TicketEntity>> Get()
    {
        return await _dbContext.Tickets
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task Add(TicketEntity ticket)
    {
        _dbContext.Add(ticket);
        await _dbContext.SaveChangesAsync();
    }
    
    
    public async Task<TicketEntity?> Get(Guid id)
    {
        return await _dbContext.Tickets.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    
    public async Task<bool> Update(Guid id, TicketEntity newTicket)
    {
        var rows = await _dbContext.Tickets
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(article => article.Message, newTicket.Message)
                .SetProperty(article => article.Status, newTicket.Status)
            );
        return rows > 0;
    }
    
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await _dbContext.Tickets
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }
}