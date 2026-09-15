using EmbeddingService.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EmbeddingService.Controllers;

[ApiController]
[Route("Test")]
public class TestController(IEmbeddingService embeddingService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Get(TestDto dto)
    {
        Console.WriteLine($"пришел запрос {dto.Text}");
        var result = await embeddingService.CreateEmbedding(dto.Text);
        return Ok(result);
    }
}

public class TestDto
{
    public string Text { get; set; }
}