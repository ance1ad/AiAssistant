namespace WebApplication1.Dtos;

public record UserResponse(
    Guid Id, 
    string? Name,
    string? Email
);