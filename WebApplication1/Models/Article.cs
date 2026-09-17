using Shared.Contracts.Events;

namespace WebApplication1.Models;

public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ProcessingStatus ProcessingStatus { get; set; }
}


