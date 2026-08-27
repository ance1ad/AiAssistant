namespace WebApplication1.Dtos;

public record ArticleResponse(
    Guid Id, 
    string Title, 
    string Keywords, 
    string Content
);