using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class UsersRepository(AssistentDbContext dbContext)
{

    public async Task Add(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<List<User>> Get()
    {
        return await dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task<User?> Get(long id)
    {
        return await dbContext.Users.Where(a => a.TelegramId == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> Update(Guid id, User newUser)
    {
        var rows = await dbContext.Users
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(user => user.Name, newUser.Name)
                .SetProperty(user => user.TelegramId, newUser.TelegramId)
            );
        return rows > 0;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await dbContext.Users
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }

}