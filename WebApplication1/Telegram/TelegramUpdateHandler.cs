using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using WebApplication1.Interfaces;
using WebApplication1.Services;

namespace WebApplication1.Telegram;

public class TelegramUpdateHandler(IServiceScopeFactory scopeFactory)
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
        
        using var scope = scopeFactory.CreateScope();
        var assistantService = scope.ServiceProvider
            .GetRequiredService<AssistantService>();

        var answer = await assistantService.HandleQuestion(
            telegramId,
            update.Message.From.Username,
            update.Message.Text
        );
        
        if (answer == string.Empty) answer = "Не смог найти ответ, формирую запрос...";
        

        await botClient.SendMessage(telegramId, 
            $"{answer}", 
            cancellationToken: cancellationToken);
    }
}