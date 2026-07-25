using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;


[ApiController]
[Route("articles")]
public class ArticlesController(ArticleService articleService) : ControllerBase
{
    private readonly ArticleService _articleService = articleService;

    
    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await _articleService.Get();
        return Ok(articles);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(Guid id)
    { 
        var article = await _articleService.Get(id);
        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> PostArticle(CreateArticleDto newArticle)
    { 
        var createdArticle = await _articleService.Create(newArticle);
        
        return CreatedAtAction(
            nameof(GetArticle), 
            new {id = createdArticle.Id},
            createdArticle
        );
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArticle(Guid id, UpdateArticleDto updateArticle)
    {
        bool updated = await _articleService.Update(id, updateArticle);
    
        if (updated)
        {
            return NoContent();
        }
        return NotFound();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        bool result = await _articleService.Delete(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
        
    }
    
    
}