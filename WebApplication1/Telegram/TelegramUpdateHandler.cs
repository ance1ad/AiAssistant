using Telegram.Bot;
using Telegram.Bot.Types;
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
        var userService = GetUserService(scope);
        
        var user = await userService.GetOrCreate(fromInfo.Id, fromInfo.Username);

    }

    private static UserService GetUserService(IServiceScope scope)
    {
        var userService = scope.ServiceProvider
            .GetRequiredService<UserService>();
        return userService;
    }
}