using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repositories;

public class AdminsRepository(AssistentDbContext dbContext)
{
    private readonly AssistentDbContext _dbContext = dbContext;

    public async Task<AdminEntity> Add(AdminEntity admin)
    {
        DbSet<AdminEntity> admins = _dbContext.Admins;
        _dbContext.Admins.Add(admin);
        await _dbContext.SaveChangesAsync();
        return admin;
    }

    
    
    public async Task<List<AdminEntity>> Get()
    {
        return await _dbContext.Admins
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task<AdminEntity?> Get(Guid id)
    {
        return await _dbContext.Admins.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await _dbContext.Admins
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }


    public async Task<AdminEntity?> GetByUsername(string username)
    {
        var admin = await _dbContext.Admins
            .Where(a => a.Username == username)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return admin;
    }

}