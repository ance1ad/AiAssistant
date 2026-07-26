using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ArticlesRepository(AssistentDbContext dbContext)
{
    private readonly AssistentDbContext _dbContext = dbContext;

    public async Task Add(ArticleEntity article)
    {
        _dbContext.Add(article);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task AddRange(List<ArticleEntity> articles)
    {
        await _dbContext.AddRangeAsync(articles);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<ArticleEntity>> Get()
    {
        return await _dbContext.Articles
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task<ArticleEntity?> Get(Guid id)
    {
        return await _dbContext.Articles.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> Update(Guid id, ArticleEntity newArticle)
    {
        var rows = await _dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(article => article.Title, newArticle.Title)
                .SetProperty(article => article.Content, newArticle.Content)
            );
        return rows > 0;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await _dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }

}