using AssistantService.Grpc;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramService.Models;
using TelegramService.Services;
using WebApplication1.Dtos;

namespace TelegramService.Telegram;

public class TelegramUpdateHandler(
    IServiceScopeFactory scopeFactory,
    Assistant.AssistantClient assistantClient,
    ILogger<TelegramUpdateHandler> logger)
{
    public async Task HandleUpdate(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message?.Text == null ||
            update.Message.From == null)
        {
            return;
        }

        logger.LogInformation(
            "Пришел вопрос от пользователя {User} - {Question}",  
            update.Message.From.Username,
            update.Message.Text);
        
        var telegramId = update.Message.From.Id;
        
        using var scope = scopeFactory.CreateScope();
        
        var userService = scope.ServiceProvider
            .GetRequiredService<UserService>();

        UserResponse user = await userService
            .GetOrCreate(telegramId, update.Message.From.Username);
        
        var ticketService = scope.ServiceProvider
            .GetRequiredService<TicketService>();
        
        var ticket = await ticketService
            .Create(user.Id, update.Message.Text, TicketStatus.New);

        var answer = await AskAssistant(update.Message.Text);
        
        logger.LogInformation("Пришел ответ от сервиса: {Answer}",  answer);
        
        if (answer == string.Empty) answer = "Не смог найти ответ, формирую запрос...";

        await botClient.SendMessage(telegramId, 
            $"{answer}", 
            cancellationToken: cancellationToken);
    }

    private async Task<string> AskAssistant(string question)
    {
        AskResponse? response = await assistantClient.AskAsync(new AskRequest
        {
            Question = question
        });

        return response.Answer;
    }
}