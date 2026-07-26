using Telegram.Bot;
using Telegram.Bot.Types;
using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Telegram;

public class TelegramUpdateHandler
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public TelegramUpdateHandler(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    
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
        
        long chatId = update.Message.Chat.Id;
        var fromInfo = update.Message.From;

        using var scope = _scopeFactory.CreateScope();
        
        var userService = GetService<UserService>(scope);
        var ticketService = GetService<TicketService>(scope);
        var articleService = GetService<ArticleService>(scope);
        
        // Создадим и получим пользователя
        UserDto user = await userService
            .GetOrCreate(fromInfo.Id, fromInfo.Username);
        
        // Добавим его вопрос в базу
        var ticket = await ticketService
            .Create(user.Id, update.Message.Text, TicketStatus.New);

        var answer = await articleService.SearchArticle(ticket);
        if (answer != null)
        {
            await botClient.SendMessage(chatId, 
                $"{answer.Content}", 
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendMessage(chatId, 
                $"Не смог найти ответ, формирую запрос...", 
                cancellationToken: cancellationToken);
        }
        
        
        
    }

    private static T GetService<T>(IServiceScope scope)
    {
        var service = scope.ServiceProvider
            .GetRequiredService<T>();
        return service;
    }
    
    
}