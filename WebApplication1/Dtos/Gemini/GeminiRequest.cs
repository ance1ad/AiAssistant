namespace WebApplication1.Dtos.Gemini;

public class GeminiRequest
{
    public Content[] Contents { get; set; } = [];

    public class Content
    {
        public Part[] Parts { get; set; } = [];
    }

    public class Part
    {
        public string Text { get; set; } = "";
    }
}