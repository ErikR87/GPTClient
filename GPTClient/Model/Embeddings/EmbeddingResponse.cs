namespace GPT.Model.Embeddings;

public class EmbeddingResponse
{
    public Datum[] Data { get; set; }
    public string Model { get; set; }
    public string Object { get; set; }
    public Usage Usage { get; set; }
}

public class Usage
{
    public int Prompt_tokens { get; set; }
    public int Total_tokens { get; set; }
}

public class Datum
{
    public float[] Embedding { get; set; }
    public int Index { get; set; }
    public string Object { get; set; }
}
