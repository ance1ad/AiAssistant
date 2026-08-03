# AI Assistant

> Full Stack AI-ассистент для работы с базой знаний компании с использованием RAG.

## Описание

Проект представляет собой веб-приложение, которое позволяет пользователям задавать вопросы и получать ответы от AI-ассистента.

Система использует подход **RAG (Retrieval-Augmented Generation)**, который позволяет искать релевантную информацию в базе знаний и использовать её при генерации ответа.

## Возможности

- AI-ассистент для ответов на вопросы
- Поиск по базе знаний
- REST API
- React клиент
- PostgreSQL
- Entity Framework Core
- Swagger документация

## Технологический стек

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL

### Frontend

- React
- Next.js
- TypeScript

## Архитектура

```text
Frontend (React / Next.js)

        ↓

ASP.NET Core Web API

        ↓

Application Layer

        ↓

Infrastructure Layer

        ↓

PostgreSQL
```

## Как работает RAG

1. Пользователь отправляет вопрос

2. Система ищет релевантную информацию в базе знаний

3. Найденный контекст добавляется к запросу

4. LLM формирует ответ

## Ссылки

GitHub:
- [AI Assistant](https://github.com/ance1ad/AiAssistant)
