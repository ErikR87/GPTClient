namespace GPTClient.Model.Embeddings;

public struct EmbeddingValue<T>
{
    public T Info;
    public float[] Data;
    public double Similarity;
}