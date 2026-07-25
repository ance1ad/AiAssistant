using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class UsersRepository(AssistentDbContext dbContext)
{
    private readonly AssistentDbContext _dbContext = dbContext;

    public async Task Add(UserEntity user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<UserEntity>> Get()
    {
        return await _dbContext.Users
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task<UserEntity?> Get(Guid id)
    {
        return await _dbContext.Users.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> Update(Guid id, UserEntity newUser)
    {
        var rows = await _dbContext.Users
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(user => user.Name, newUser.Name)
                .SetProperty(user => user.Email, newUser.Email)
            );
        return rows > 0;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await _dbContext.Users
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }

}