using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

// [Authorize(Roles = "Admin")]
[ApiController]
[Route("articles")]
public class ArticlesController(Services.ArticleService articleService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await articleService.Get();
        return Ok(articles);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArticle(Guid id)
    { 
        var article = await articleService.Get(id);
        
        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
    }

    
    [HttpPost]
    public async Task<IActionResult> PostArticle(CreateArticleRequest newArticle)
    { 
        var createdArticle = await articleService.Create(newArticle);
        
        return CreatedAtAction(
            nameof(GetArticle), 
            new {id = createdArticle.Id},
            createdArticle
        );
    }
    
    
    [HttpPost("bulk")]
    public async Task<IActionResult> PostArticles(List<CreateArticleRequest> newArticles)
    {
        var actions = await articleService.CreateMany(newArticles);
        return Ok(actions);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArticle(Guid id, UpdateArticleRequest updateArticle)
    {
        bool updated = await articleService.Update(id, updateArticle);
    
        if (updated)
        {
            return NoContent();
        }
        return NotFound();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(Guid id)
    {
        bool result = await articleService.Delete(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
    }
}