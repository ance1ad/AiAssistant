using WebApplication1.Dtos;

namespace WebApplication1.Interfaces;

public interface IAiService
{
    Task<string> GenerateAnswer(
        string question,
        List<ArticleResponse> articles);
}