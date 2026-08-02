using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using WebApplication1.Application;
using WebApplication1.Interfaces;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.Telegram;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AssistentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(AssistentDbContext)));
});

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


builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<ArticlesRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UsersRepository>();

builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AdminsRepository>();

builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<TicketsRepository>();

builder.Services.AddScoped<AssistantService>();
builder.Services.AddHttpClient<IAiService, GeminiService>();
builder.Services.AddSingleton<JwtService>();

builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton<TelegramUpdateHandler>();


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

var bot =  app.Services.GetRequiredService<TelegramBotService>();
bot.Start();


app.Run();
