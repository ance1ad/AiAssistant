using EmbeddingService.Application;
using EmbeddingService.Messaging;
using EmbeddingService.Repositories;
using EmbeddingService.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;


builder.Services.AddDbContext<EmbeddingDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("AssistentDbContext"),
        npgsqlOptions =>
        {
            npgsqlOptions.UseVector();
        });
});

builder.Services.AddControllers();

builder.Services.AddHostedService<RabbitMqConsumer>();

builder.Services.AddSingleton<RabbitMqConnectionProvider>();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

// Embedding logic
builder.Services.AddScoped<EmbeddingCreator>();
builder.Services.AddScoped<EmbeddingRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
