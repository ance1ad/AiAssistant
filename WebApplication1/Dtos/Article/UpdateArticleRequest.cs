using System.ComponentModel.DataAnnotations;
using Shared.Contracts.Events;

namespace WebApplication1.Dtos;

public record UpdateArticleRequest(
    [Required]
    [StringLength(30)] 
    string Title, 
    
    [Required]
    string  Keywords, 
    
    [Required]
    string  Content
);