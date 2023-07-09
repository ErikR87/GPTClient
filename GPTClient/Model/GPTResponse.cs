namespace GPT.Model;

public class GPTResponse
{
    public string Id { get; set; }
    public string _object { get; set; }
    public int Created { get; set; }
    public string Model { get; set; }
    public Choice[] Choices { get; set; }
    public Usage Usage { get; set; }
    public ErrorMessage Error { get; set; }
}

public class Usage
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}

public class Choice
{
    public int Index { get; set; }
    public ChatMessageResponse Message { get; set; }
    public string FinishReason { get; set; }
}

public struct ErrorMessage
{
    public string Message { get; set; }
}