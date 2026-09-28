FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "DocumentService/DocumentService.csproj"
RUN dotnet publish "DocumentService/DocumentService.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore
    
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5233
EXPOSE 5243

ENTRYPOINT ["dotnet", "DocumentService.dll"]