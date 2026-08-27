namespace WebApplication1.Dtos;

public record UpdateUserRequest(
    string Name, 
    long TelegramId
);