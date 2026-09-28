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
        var telegramId = update.Message.From.Id;

        logger.LogInformation(
            "Пришел вопрос от пользователя {User} - {Question}",  
            update.Message.From.Username,
            update.Message.Text);
        
        await botClient.SendMessage(telegramId, 
            "Спасибо за вопрос! Постараемся ответить как можно скорее", 
            cancellationToken: cancellationToken);

        string answer = await GetAnswer(update, telegramId);

        await botClient.SendMessage(telegramId, 
            $"{answer}", 
            cancellationToken: cancellationToken);
    }

    private async Task<string> GetAnswer(Update update, long telegramId)
    {
        using var scope = scopeFactory.CreateScope();
        
        var userService = scope.ServiceProvider
            .GetRequiredService<UserService>();

        UserResponse user = await userService
            .GetOrCreate(telegramId, update.Message.From.Username);
        
        var ticketService = scope.ServiceProvider
            .GetRequiredService<TicketService>();
        
        var answer = await AskAssistant(update.Message.Text);
        
        logger.LogInformation("Пришел ответ от сервиса: {Answer}",  answer);

        if (!string.IsNullOrEmpty(answer))
        {
            await ticketService
                .Create(user.Id, update.Message.Text, TicketStatus.Processing);
        }
        else
        {
            await ticketService
                .Create(user.Id, update.Message.Text, TicketStatus.Error);
            answer = "Прошу прошения, не смог найти ответ на ваш вопрос, формирую запрос в систему...";
        }

        return answer;
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