using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class ArticleService(ArticlesRepository articlesRepository)
{
    private readonly ArticlesRepository _articlesRepository = articlesRepository;

    public async Task<List<ArticleDto>> Get()
    {
        var list = await _articlesRepository.Get();
        return list
            .Select(article => new ArticleDto (
                article.Id, 
                article.Title,  
                article.Content)
            )
            .ToList();
    }
    
    
    public async Task<ArticleDto?> Get(Guid id)
    {
        var article = await _articlesRepository.Get(id);
        if (article != null)
        {
            return new ArticleDto(article.Id, article.Title, article.Content);
        }
        return null;
    }
    
    
    public async Task<ArticleDto> Create(CreateArticleDto articleDto)
    {
        var articleEntity = new ArticleEntity
        {
            Id = Guid.NewGuid(),
            Title = articleDto.Title,
            Content = articleDto.Content
        };
        await _articlesRepository.Add(articleEntity);
        
        return new ArticleDto
        (
            articleEntity.Id,
            articleEntity.Title,
            articleEntity.Content
        );
    }
    
    
    public Task<bool> Update(Guid id, UpdateArticleDto articleDto)
    {
        var articleEntity = new ArticleEntity { 
            Title = articleDto.Title,
            Content = articleDto.Content
        };
        return _articlesRepository.Update(id, articleEntity);
    }
    
    public Task<bool> Delete(Guid id)
    {
        return _articlesRepository.Delete(id);
    }
        
}