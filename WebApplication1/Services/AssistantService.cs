using WebApplication1.Dtos;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class AssistantService(
    UserService userService,
    TicketService ticketService,
    ArticleService articleService,
    IAiService aiService)
{
    public async Task<string> HandleQuestion(
        long telegramId,
        string? username,
        string message)
    {
        UserResponse user = await userService
            .GetOrCreate(telegramId, username);
        
        var ticket = await ticketService
            .Create(user.Id, message, TicketStatus.New);

        var relevantArticles = await articleService.FindRelevantArticles(ticket.Message);
        
        string generateAnswer = string.Empty;
        if (relevantArticles.Count > 0)
        {
            generateAnswer = await aiService.GenerateAnswer(message, relevantArticles);
        }

        return generateAnswer;
    }
}