using WebApplication1.Dtos;
using WebApplication1.Interfaces;

namespace WebApplication1.Services;

public class GeminiService(IConfiguration configuration, HttpClient httpClient) : IAiService
{
    public async Task<string> GenerateAnswer(string question, List<ArticleDto> articles)
    {

        var context = string.Join("\n\n",
            articles.Select(a =>
                $"""
                 Название:
                 {a.Title}

                 Ключевые слова:
                 {a.Keywords}

                 Информация:
                 {a.Content}
                 """));
        
        var prompt = $"""
                      Ты помощник службы поддержки.

                      Правила:
                      - Используй только информацию из базы знаний.
                      - Не придумывай ответы.
                      - Если информации недостаточно, скажи что не знаешь.

                      База знаний:
                      {context}

                      Вопрос пользователя:
                      {question}

                      Ответ:
                      """;
        
        var apiKey = configuration["Gemini:ApiKey"];
        var model = "gemini-3.1-flash-lite";

        var url =
            $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";
        Console.WriteLine(url.Replace(apiKey, "HIDDEN"));
        
        var request = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = prompt
                        }
                    }
                }
            }
        };
        var response =
            await httpClient.PostAsJsonAsync(
                url,
                request);


        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception(error);
        }


        var result =
            await response.Content
                .ReadFromJsonAsync<GeminiResponse>();

        if (result?.Candidates == null ||
            result.Candidates.Length == 0)
        {
            return "Не удалось сформировать ответ";
        }
        
        
        return result
            .Candidates[0]
            .Content
            .Parts[0]
            .Text;
        
    }
}

public class GeminiResponse
{
    public Candidate[] Candidates { get; set; } = [];
}


public class Candidate
{
    public GeminiContent Content { get; set; } = null!;
}


public class GeminiContent
{
    public GeminiPart[] Parts { get; set; } = [];
}


public class GeminiPart
{
    public string Text { get; set; } = "";
}