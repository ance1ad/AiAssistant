using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers;


[ApiController]
[Route("articles")]
public class ArticlesController(ArticlesRepository articlesRepository) : ControllerBase
{
    private readonly ArticlesRepository _articlesRepository = articlesRepository;

    [HttpGet]
    public async Task<IActionResult> GetArticles()
    {
        var articles = await _articlesRepository.Get();
        return Ok(articles);
    }
    
    
    [HttpGet("{id}")]
    public IActionResult GetArticle(int id)
    { 
        return Ok();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> PostArticle(CreateArticleDto newArticle)
    { 
        
        var articleEntity = new ArticleEntity
        {
            Id = Guid.NewGuid(),
            Title = newArticle.Title,
            Content = newArticle.Content
        };
        
        await _articlesRepository.Add(articleEntity);
        
        var articleDto = new ArticleDto(
            articleEntity.Id,
            newArticle.Title,
            newArticle.Content
        );
        
        
        return CreatedAtAction(
            nameof(GetArticle), 
            new {id = articleDto.Id},
            articleDto
        );

    }

    
    // [HttpPut]
    // public IActionResult PutArticle(int id, UpdateArticleDto updateArticle)
    // {
    //     var index = articleDtos.FindIndex(a => a.Id == id);
    //
    //     if (index == -1)
    //     {
    //         return NotFound();
    //     }
    //     
    //     articleDtos[index] = new (
    //         articleDtos[index].Id,
    //         updateArticle.Title,
    //         updateArticle.Content
    //     );
    //     return NoContent();
    // }
    //
    //
    // [HttpDelete("{id}")]
    // public IActionResult DeleteArticle(int id)
    // {
    //     var article = articleDtos.FirstOrDefault(p => p.Id == id);
    //     if (article == null)
    //     {
    //         return NotFound();
    //     }
    //     articleDtos.Remove(article);
    //     return NoContent();
    // }
    
    
}