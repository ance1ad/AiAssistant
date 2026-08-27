using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class TicketsRepository(AssistentDbContext dbContext)
{
    
    public async Task<List<Ticket>> Get()
    {
        return await dbContext.Tickets
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task Add(Ticket ticket)
    {
        dbContext.Add(ticket);
        await dbContext.SaveChangesAsync();
    }
    
    
    public async Task<Ticket?> Get(Guid id)
    {
        return await dbContext.Tickets.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    
    public async Task<bool> Update(Guid id, Ticket newTicket)
    {
        var rows = await dbContext.Tickets
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(article => article.Message, newTicket.Message)
                .SetProperty(article => article.Status, newTicket.Status)
            );
        return rows > 0;
    }
    
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await dbContext.Tickets
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }
}