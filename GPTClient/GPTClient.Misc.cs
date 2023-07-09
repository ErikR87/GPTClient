using GPT.Model;
using System.Net.Http.Json;

namespace GPT;

public partial class GPTClient
{
    /// <summary>
    /// Get list of available open ai models
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<Model.Model>> GetModels()
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Config.Key}");
        var response = await httpClient.GetAsync("https://api.openai.com/v1/models");
        response.EnsureSuccessStatusCode();
        var content = response.Content;
        var result = await content.ReadFromJsonAsync<ModelResult>();

        return result.data;
    }
}
