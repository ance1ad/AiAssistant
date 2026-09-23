using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Shared.Messaging;
using WebApplication1.Application;
using WebApplication1.Grpc;
using WebApplication1.Messaging;
using WebApplication1.Repositories;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AssistentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(AssistentDbContext)));
});

builder.Services.AddGrpc();

// Конфигурируем на http2 
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5010, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddControllers();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введите JWT токен."
    });


    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(
                "Bearer",
                document
            ),
            new List<string>(Array.Empty<string>())
        }
    });
});


builder.Services.AddScoped<WebApplication1.Services.ArticleService>();
builder.Services.AddScoped<ArticlesRepository>();



builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AdminsRepository>();

builder.Services.AddSingleton<JwtService>();


// Backround service / Consumer
builder.Services.AddHostedService<ArticleEmbeddingConsumer>();
builder.Services.AddSingleton<RabbitMqConsumerInitializer>();

builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<RabbitMqConnectionProvider>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("client",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowCredentials()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var jwtKey = configuration["Jwt:Key"]
             ?? throw new InvalidOperationException("JWT Key is missing");

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Events =
            new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token =
                        context.Request.Cookies["token"];

                    return Task.CompletedTask;
                }
            };
        
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            )
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("client");

app.MapGrpcService<ArticleGrpcEndpoint>();


app.Run();
