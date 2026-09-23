using Microsoft.EntityFrameworkCore;
using TelegramService.Application;
using TelegramService.Repositories;
using TelegramService.Services;
using TelegramService.Telegram;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<TelegramDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(TelegramDbContext)));
});

builder.Services.AddGrpcClient<AssistantService.Grpc.Assistant.AssistantClient>(options =>
{
    options.Address = new Uri("http://localhost:5215");
});

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TicketsRepository>();

builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton<TelegramUpdateHandler>();

var app = builder.Build();
var bot =  app.Services.GetRequiredService<TelegramBotService>();
bot.Start();

app.Run();
