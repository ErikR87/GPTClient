namespace GPT.Model;

[Serializable]
public class ChatMessage
{
    public string Role { get; set; }
    public string Content { get; set; }
}

public class ChatMessageResponse : ChatMessage
{
    public FunctionCall? FunctionCall { get; set; }
}

public static class GPTRoles
{
    public static readonly string System = "system";
    public static readonly string User = "user";
    public static readonly string Function = "function";
}

public struct FunctionCall
{
    public string Name { get; set; }
    public string Arguments { get; set; }
}