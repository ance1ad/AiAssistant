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
    
    
    public async Task<List<ArticleResponse>> Get()
    {
        var list = await _articlesRepository.Get();
        return list
            .Select(article => new ArticleResponse (
                article.Id, 
                article.Title,  
                article.Keywords,  
                article.Content)
            )
            .ToList();
    }
    
    
    public async Task<ArticleResponse?> Get(Guid id)
    {
        var article = await _articlesRepository.Get(id);
        if (article != null)
        {
            return new ArticleResponse(article.Id, article.Title, article.Keywords, article.Content);
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
            Content = articleRequest.Content
        };
        await _articlesRepository.Add(articleEntity);
        
        return new ArticleResponse
        (
            articleEntity.Id,
            articleEntity.Title,
            articleEntity.Keywords,
            articleEntity.Content
        );
    }
    
    
    public async Task<List<ArticleResponse>> CreateMany(List<CreateArticleRequest> articles)
    {
       var articleEntitys =  articles.Select(articleDto => new Article{
           Id = Guid.NewGuid(),
           Title = articleDto.Title,
           Keywords = articleDto.Keywords,
           Content = articleDto.Content
       }).ToList();
       
       await _articlesRepository.AddRange(articleEntitys);
       
       return articleEntitys.Select(a => new ArticleResponse(
           a.Id, 
           a.Title, 
           a.Keywords, 
           a.Content
        )).ToList();
    }
    
    
    public Task<bool> Update(Guid id, UpdateArticleRequest articleRequest)
    {
        var articleEntity = new Article { 
            Title = articleRequest.Title,
            Keywords = articleRequest.Keywords,
            Content = articleRequest.Content
        };
        return _articlesRepository.Update(id, articleEntity);
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
            a.Article.Content
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