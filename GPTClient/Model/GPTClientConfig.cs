namespace GPT.Model;

public class GPTClientConfig
{
    public string Key { get; set; }
    public string ChatModel { get; set; } = ChatModels.GPT_35_TURBO_0613;
    public string Url { get; set; } = "https://api.openai.com/v1/chat/completions";
    public uint MaxTokens { get; set; } = 300;

    public string EmbeddingsModel { get; set; } = EmbedModels.TEXT_EMBEDDING_ADA_002;
    public string EmbeddingsUrl { get; set; } = "https://api.openai.com/v1/embeddings";
}

public static class ChatModels
{
    public static string GPT_35_TURBO = "gpt-3.5-turbo";
    public static string GPT_35_TURBO_0613 = "gpt-3.5-turbo-0613";
    public static string GPT_4 = "gpt-4";
    public static string GPT_4_0613 = "gpt-4-0613";
    public static string GPT_35_TURBO_INSTRUCT = "gpt-3.5-turbo-instruct";
}

public static class EmbedModels
{
    public static string TEXT_EMBEDDING_ADA_002 = "text-embedding-ada-002";
}