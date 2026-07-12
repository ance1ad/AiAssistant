using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record CreateArticleDto(
    [Required][StringLength(30)] string Title, 
    [Required] string Content
);