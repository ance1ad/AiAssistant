using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record RegisterAdminDto(
    [Required]
    [StringLength(50)]
    string Username,

    [Required]
    [MinLength(6)]
    string Password
);