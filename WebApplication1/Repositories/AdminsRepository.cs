using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repositories;

public class AdminsRepository(AssistentDbContext dbContext)
{

    public async Task<Admin> Add(Admin admin)
    {
        DbSet<Admin> admins = dbContext.Admins;
        dbContext.Admins.Add(admin);
        await dbContext.SaveChangesAsync();
        return admin;
    }
    
    public async Task<List<Admin>> Get()
    {
        return await dbContext.Admins
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<Admin?> Get(Guid id)
    {
        return await dbContext.Admins.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await dbContext.Admins
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }

    public async Task<Admin?> GetByUsername(string username)
    {
        var admin = await dbContext.Admins
            .Where(a => a.Username == username)
            .AsNoTracking()
            .FirstOrDefaultAsync();
        return admin;
    }

}