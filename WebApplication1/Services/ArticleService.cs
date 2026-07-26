using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class ArticleService(ArticlesRepository articlesRepository)
{
    private readonly ArticlesRepository _articlesRepository = articlesRepository;

    private readonly string[] _stopWords =
    [
        "привет",
        "помоги",
        "мне",
        "пожалуйста",
        "как",
        "что"
    ];
    
    
    public async Task<List<ArticleDto>> Get()
    {
        var list = await _articlesRepository.Get();
        return list
            .Select(article => new ArticleDto (
                article.Id, 
                article.Title,  
                article.Keywords,  
                article.Content)
            )
            .ToList();
    }
    
    
    public async Task<ArticleDto?> Get(Guid id)
    {
        var article = await _articlesRepository.Get(id);
        if (article != null)
        {
            return new ArticleDto(article.Id, article.Title, article.Keywords, article.Content);
        }
        return null;
    }
    
    
    public async Task<ArticleDto> Create(CreateArticleDto articleDto)
    {
        var articleEntity = new ArticleEntity
        {
            Id = Guid.NewGuid(),
            Title = articleDto.Title,
            Keywords = articleDto.Keywords,
            Content = articleDto.Content
        };
        await _articlesRepository.Add(articleEntity);
        
        return new ArticleDto
        (
            articleEntity.Id,
            articleEntity.Title,
            articleEntity.Keywords,
            articleEntity.Content
        );
    }
    
    
    public async Task<List<ArticleDto>> CreateMany(List<CreateArticleDto> articles)
    {
       var articleEntitys =  articles.Select(articleDto => new ArticleEntity{
           Id = Guid.NewGuid(),
           Title = articleDto.Title,
           Keywords = articleDto.Keywords,
           Content = articleDto.Content
       }).ToList();
       
       await _articlesRepository.AddRange(articleEntitys);
       
       return articleEntitys.Select(a => new ArticleDto(
           a.Id, 
           a.Title, 
           a.Keywords, 
           a.Content
        )).ToList();
    }
    
    
    public Task<bool> Update(Guid id, UpdateArticleDto articleDto)
    {
        var articleEntity = new ArticleEntity { 
            Title = articleDto.Title,
            Keywords = articleDto.Keywords,
            Content = articleDto.Content
        };
        return _articlesRepository.Update(id, articleEntity);
    }
    
    
    public Task<bool> Delete(Guid id)
    {
        return _articlesRepository.Delete(id);
    }

    
    public async Task<ArticleDto?> SearchArticle(TicketDto ticketDto)
    {
        var words = ticketDto.Message
            .ToLower()
            .Split(' ',  StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !_stopWords.Contains(word))
            .ToArray();
        
        var articles = await _articlesRepository.Get();

        var result = articles
            .Select(article => new
            {
                Article = article,

                Score = words.Count(word =>
                    article.Title
                        .ToLower()
                        .Contains(word)
                    ||
                    article.Keywords
                        .ToLower()
                        .Contains(word))
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .FirstOrDefault();

        
        if (result == null)
            return null;


        return new ArticleDto(
            result.Article.Id,
            result.Article.Title,
            result.Article.Keywords,
            result.Article.Content
        );
    }
        
}