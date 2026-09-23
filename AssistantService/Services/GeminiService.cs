using AssistantService.Abstractions;
using AssistantService.Dtos.Gemini;
using WebApplication1.Dtos.Gemini;

namespace AssistantService.Services;

public class GeminiService(IConfiguration configuration, HttpClient httpClient) : IAiService
{
     public async Task<string> GenerateAnswer(string question, List<string> dataForAnswer)
     {

         var context = string.Join(" ", dataForAnswer);
         
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
         // Console.WriteLine(url.Replace(apiKey, "HIDDEN"));
         
         var request = new GeminiRequest
         {
             Contents = 
             [
                 new GeminiRequest.Content
                 {
                     Parts = 
                     [
                         new GeminiRequest.Part
                         {
                             Text = prompt,
                         }
                     ]
                 }
             ]
         };
         
         var response = await httpClient.PostAsJsonAsync(url, request);


         if (!response.IsSuccessStatusCode)
         {
             var error = await response.Content.ReadAsStringAsync();
             throw new Exception(error);
         }


         var result = await response.Content
                 .ReadFromJsonAsync<GeminiResponse>();

         if (result?.Candidates == null ||
             result.Candidates.Length == 0)
         {
             return "Не удалось сформировать ответ";
         }
         
         
         var text = result?
             .Candidates?
             .FirstOrDefault()?
             .Content?
             .Parts?
             .FirstOrDefault()?
             .Text;

         return text ?? "Не удалось сформировать ответ";
         
     }
}


  