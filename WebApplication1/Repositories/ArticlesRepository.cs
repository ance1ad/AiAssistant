using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class ArticlesRepository(AssistentDbContext dbContext)
{
    public async Task Add(Article article)
    {
        dbContext.Add(article);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task AddRange(List<Article> articles)
    {
        await dbContext.AddRangeAsync(articles);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Article>> Get()
    {
        return await dbContext.Articles
            .AsNoTracking()
            .ToListAsync();
    }
    
    
    public async Task<Article?> Get(Guid id)
    {
        return await dbContext.Articles.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> Update(Guid id, Article newArticle)
    {
        var rows = await dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(article => article.Title, newArticle.Title)
                .SetProperty(article => article.Content, newArticle.Content)
            );
        return rows > 0;
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var deleteCount = await dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
        return deleteCount > 0;
    }

}