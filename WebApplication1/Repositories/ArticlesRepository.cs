using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ArticlesRepository
{
    private readonly AssistentDbContext _dbContext;

    public ArticlesRepository(AssistentDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<List<ArticleEntity>> Get()
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task Add(ArticleEntity article)
    {
        await _dbContext.AddAsync(article);
        await _dbContext.SaveChangesAsync();
    }
}