using EmbeddingService.Abstractions;
using EmbeddingService.Application;
using EmbeddingService.Grpc;
using EmbeddingService.Messaging;
using EmbeddingService.Repositories;
using EmbeddingService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<EmbeddingDbContext>(options =>
{
    options.UseNpgsql(
        configuration.GetConnectionString("AssistentDbContext"),
        npgsqlOptions =>
        {
            npgsqlOptions.UseVector();
        });
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5025, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.AddControllers();
builder.Services.AddGrpc();


builder.Services.AddHostedService<EmbeddingConsumer>();
builder.Services.AddSingleton<RabbitMqConsumerInitializer>();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

// Embedding logic
builder.Services.AddScoped<EmbeddingProcessor>();
builder.Services.AddScoped<EmbeddingRepository>();

// Broker
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<RabbitMqConnectionProvider>();

builder.Services.AddHttpClient<IEmbeddingService, GeminiEmbeddingService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    
    var apiKey = configuration["Gemini:ApiKey"];

    client.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);
});

var app = builder.Build();

app.MapControllers();

app.MapGrpcService<EmbeddingGrpcEndpoint>();

app.Run();
