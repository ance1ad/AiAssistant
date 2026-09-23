namespace WebApplication1.Dtos.Gemini;

public class GeminiResponse
{
    public Candidate[] Candidates { get; set; } = [];

    public class Candidate
    {
        public Content Content { get; set; } = null!;
    }

    public class Content
    {
        public Part[] Parts { get; set; } = [];
    }

    public class Part
    {
        public string Text { get; set; } = "";
    }
} 