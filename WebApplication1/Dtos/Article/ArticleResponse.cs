using Shared.Contracts.Events;
using Shared.Contracts.Models;

namespace WebApplication1.Dtos;

public record ArticleResponse(
    Guid Id, 
    string Title, 
    string Keywords, 
    string Content,
    ProcessingStatus ProcessingStatus
);