using Telegram.Bot;
using Telegram.Bot.Types;

namespace WebApplication1.Services;

public class TelegramBotService
{
    private readonly TelegramBotClient _client;

    public TelegramBotService(IConfiguration configuration)
    {
        var token = configuration["Telegram:BotToken"];
        _client = new TelegramBotClient(token);
    }

    public void Start()
    {
        _client.StartReceiving(
            HandleUpdate,
            HandleError
        );
    }


    private async Task HandleUpdate(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message?.Text != null)
        {
            var chatId = update.Message.Chat.Id;

            await botClient.SendMessage(
                chatId,
                "Привет! Я работаю 🤖",
                cancellationToken: cancellationToken
            );
        }
    }


    private Task HandleError(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(exception.Message);

        return Task.CompletedTask;
    }
}