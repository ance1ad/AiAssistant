using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Dtos;

public record LoginAdminRequest(
    [Required]
    string Username,
    [Required]
    string Password
);