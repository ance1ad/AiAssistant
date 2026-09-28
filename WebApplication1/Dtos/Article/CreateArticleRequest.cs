using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record CreateArticleRequest(
    [Required]
    [StringLength(128)] 
    string Title, 
    
    [Required] 
    string Keywords,
    
    [Required] 
    string Content
);