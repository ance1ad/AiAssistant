using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class UsersRepository(AssistentDbContext dbContext)
{
    private readonly AssistentDbContext _dbContext = dbContext;

    public async Task Add(ArticleEntity article)
    {
        await _dbContext.AddAsync(article);
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
        return await _dbContext.Articles.Where(a => a.Id == id).FirstOrDefaultAsync();
    }
    
    public async Task Update(Guid id, ArticleEntity newArticle)
    {
        await _dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteUpdateAsync(a => a
                .SetProperty(article => article.Title, newArticle.Title)
                .SetProperty(article => article.Content, newArticle.Content)
            );
    }
    
    public async Task Delete(Guid id)
    {
        await _dbContext.Articles
            .Where(a => a.Id == id)
            .ExecuteDeleteAsync();
    }

}