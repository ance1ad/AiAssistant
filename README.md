# AiAssistant

AI Support Assistant - backend-проект на ASP.NET Core,
использующий RAG для поиска информации в базе знаний
и генерации ответов пользователям через Telegram.

## Возможности

- управление статьями базы знаний
- загрузка PDF/TXT документов
- автоматический парсинг и разбиение документов на chunks
- генерация embeddings
- семантический поиск через pgvector
- генерация ответа через Gemini
- Telegram-бот
- создание обращений пользователей
- JWT-аутентификация администратора
- RabbitMQ для фоновой обработки
- gRPC между микросервисами
- Docker Compose для запуска всей системы

## Архитектура

...

## Стек

### Backend
- C#
- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- pgvector
- RabbitMQ
- gRPC

### AI
- Google Gemini
- Gemini Embeddings

### Infrastructure
- Docker
- Docker Compose

## Микросервисы

### WebApplication
REST API для:
- Articles
- Users
- Tickets
- Admin authentication

HTTP: 5010
gRPC: 5020

### DocumentService
Работа с документами:
- upload
- processing
- parsing
- chunking

HTTP: 5233
gRPC: 5243

### EmbeddingService
Создание embeddings и semantic search.

gRPC: 5025

### AssistantService
Основная RAG-логика:
1. получает вопрос
2. отправляет его в EmbeddingService
3. получает наиболее похожие источники
4. получает текст источников
5. отправляет контекст в Gemini
6. возвращает ответ

gRPC: 5215

### TelegramService
Получает сообщения Telegram и передаёт вопросы в AssistantService.

## Схема взаимодействия

Telegram
   ↓
TelegramService
   ↓ gRPC
AssistantService
   ├──→ EmbeddingService
   ├──→ WebApplication
   ├──→ DocumentService
   └──→ Gemini

DocumentService
   ↓
RabbitMQ
   ↓
EmbeddingService

## Запуск

### Требования

- Docker
- Docker Compose
- Telegram Bot Token
- Gemini API Key

### Environment variables

Создать `.env` в корне проекта.

Пример находится в `.env.example`.

...

### Запуск

```bash
docker compose up --build
