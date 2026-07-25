namespace WebApplication1.Dtos;

public record UserDto(
    Guid Id, 
    string Name, 
    string Email
);