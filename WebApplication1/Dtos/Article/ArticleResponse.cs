using Shared.Contracts.Events;

namespace WebApplication1.Dtos;

public record ArticleResponse(
    Guid Id, 
    string Title, 
    string Keywords, 
    string Content,
    ProcessingStatus ProcessingStatus
);