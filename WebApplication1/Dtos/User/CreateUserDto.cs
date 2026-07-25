using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record CreateUserDto(
    [Required][StringLength(50)] string Name, 
    [Required] string Email
);