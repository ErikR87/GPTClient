using GPT.Model.Embeddings;
using GPTClient.Helper;
using Newtonsoft.Json;

namespace GPT;

public partial class GPTClient 
{
   public async Task<EmbeddingResponse> GetEmbeddings(string input)
    {
        if (input == null)
        {
            throw new ArgumentException(nameof(input) + " is null");
        }

        // Create the request for the API sending the
        // latest collection of chat messages
        var request = new
        {
            model = Config.EmbeddingsModel,
            input,
        };

        // Send the request and capture the response
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Config.Key}");
        var requestJson = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings
        {
            ContractResolver = new LowercaseContractResolver()
        });
        var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PostAsync(Config.EmbeddingsUrl, requestContent);
        var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<EmbeddingResponse>(jsonString);

        return responseObject;
    }
}