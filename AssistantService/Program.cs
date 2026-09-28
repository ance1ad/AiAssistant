using AssistantService.Abstractions;
using AssistantService.Grpc;
using AssistantService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IAiService, GeminiService>();

builder.Services.AddGrpc();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5215, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

// Связь с Embedding сервисом
builder.Services.AddGrpcClient<EmbeddingService.Grpc.Embedding.EmbeddingClient>(options =>
{
    options.Address = new Uri("http://embedding-service:5025");
});

// Связь с Article сервисом
builder.Services.AddGrpcClient<ArticleService.Grpc.Article.ArticleClient>(options =>
{
    options.Address = new Uri("http://web-application:5020");
});

// Связь с Document сервисом
builder.Services.AddGrpcClient<DocumentService.Grpc.Document.DocumentClient>(options =>
{
    options.Address = new Uri("http://document-service:5243");
});

builder.Services.AddScoped<EmbeddingGrpcClient>();
builder.Services.AddScoped<TextDataGrpcClient>();

builder.Services.AddScoped<AssistantProcessor>();

var app = builder.Build();

app.MapGrpcService<AssistantGrpcEndpoint>();

app.Run();

