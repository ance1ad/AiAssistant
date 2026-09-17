using DocumentService.Dtos;
using Shared.Contracts.Events;
using Shared.Messaging;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class ArticleService(
    ArticlesRepository articlesRepository,
    RabbitMqPublisher publisher)
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
    
    
    public async Task<List<ArticleResponse>> Get()
    {
        var list = await _articlesRepository.Get();
        return list
            .Select(article => new ArticleResponse (
                article.Id, 
                article.Title,  
                article.Keywords,  
                article.Content,
                article.ProcessingStatus)
            )
            .ToList();
    }
    
    
    public async Task<ArticleResponse?> Get(Guid id)
    {
        var article = await _articlesRepository.Get(id);
        if (article != null)
        {
            return new ArticleResponse(
                article.Id, 
                article.Title,
                article.Keywords,
                article.Content,
                article.ProcessingStatus);
        }
        return null;
    }
    
    
    public async Task<ArticleResponse> Create(CreateArticleRequest articleRequest)
    {
        var articleEntity = new Article
        {
            Id = Guid.NewGuid(),
            Title = articleRequest.Title,
            Keywords = articleRequest.Keywords,
            Content = articleRequest.Content,
            ProcessingStatus = ProcessingStatus.Pending,
        };
        
        await _articlesRepository.Add(articleEntity);
        
        try
        {
            var chunkData = new List<TextChunkData>
            {
                new (articleEntity.Id, 0, articleEntity.Content)
            };

            await publisher.PublishAsync(new TextChunksPreparedEvent(
                articleEntity.Id, SourceType.Article, chunkData), "text.chunks.prepared");
            
            await _articlesRepository.SetArticleStatus(articleEntity.Id, ProcessingStatus.Processing);
            articleEntity.ProcessingStatus = ProcessingStatus.Processing;
        }
        catch
        {
            await _articlesRepository.SetArticleStatus(articleEntity.Id, ProcessingStatus.Error);
            throw;
        }
        
        return new ArticleResponse
        (
            articleEntity.Id,
            articleEntity.Title,
            articleEntity.Keywords,
            articleEntity.Content,
            articleEntity.ProcessingStatus
        );
    }
    
    
    public async Task<List<ArticleResponse>> CreateMany(
        List<CreateArticleRequest> articles)
    {
       var articleEntitys = articles.Select(article => new Article{
           Id = Guid.NewGuid(),
           Title = article.Title,
           Keywords = article.Keywords,
           Content = article.Content,
           ProcessingStatus = ProcessingStatus.Pending,
       }).ToList();
       
       await _articlesRepository.AddRange(articleEntitys);
       
       return articleEntitys.Select(a => new ArticleResponse(
           a.Id, 
           a.Title, 
           a.Keywords, 
           a.Content,
           a.ProcessingStatus
        )).ToList();
    }
    
    
    public Task<bool> Update(Guid id, UpdateArticleRequest articleRequest)
    {
        var articleEntity = new Article { 
            Title = articleRequest.Title,
            Keywords = articleRequest.Keywords,
            Content = articleRequest.Content,
            ProcessingStatus = ProcessingStatus.Pending
        };
        return _articlesRepository.Update(id, articleEntity);
    }


    public async Task SetArticleStatus(Guid id, ProcessingStatus status)
    {
        await _articlesRepository.SetArticleStatus(id, status);
    }
    
    public Task<bool> Delete(Guid id)
    {
        return _articlesRepository.Delete(id);
    }

    
    public async Task<List<ArticleResponse>> FindRelevantArticles(string message)
    {
        var words = ExtractWords(message);
        
        var articles = await _articlesRepository.Get();

        var result = articles
            .Select(article => new
            {
                Article = article,
                Score = CalculateScore(words, article)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(5);


        return result.Select(a => new ArticleResponse(
            a.Article.Id,
            a.Article.Title,
            a.Article.Keywords,
            a.Article.Content,
            a.Article.ProcessingStatus
        )).ToList();
    }

    private static int CalculateScore(string[] words, Article article)
    {
        return words.Sum(word =>
        {
            var score = 0;

            var title = Normalize(article.Title);
            var keyWords = Normalize(article.Keywords);
            var content = Normalize(article.Content);
            
            
            if(title.Contains(word))
                score += 3;

            if(keyWords.Contains(word))
                score += 2;

            if(content.Contains(word))
                score += 1;

            return score;
        });
    }

    private string[] ExtractWords(string text)
    {
        return text
            .ToLower()
            .Split([' ', ',', '.', '!', '?', ';', ':', '\n'],  StringSplitOptions.RemoveEmptyEntries)
            .Where(word => !_stopWords.Contains(word))
            .ToArray();
    }
    
    private static string Normalize(string text)
    {
        return text
            .ToLower()
            .Trim();
    }
}