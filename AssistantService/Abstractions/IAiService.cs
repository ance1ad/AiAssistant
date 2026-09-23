namespace AssistantService.Abstractions;

public interface IAiService
{
    Task<string> GenerateAnswer(
        string question,
        List<string> dataForAnswer);
}