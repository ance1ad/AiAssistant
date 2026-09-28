FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TelegramService/TelegramService.csproj"
RUN dotnet publish "TelegramService/TelegramService.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore
    
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TelegramService.dll"]