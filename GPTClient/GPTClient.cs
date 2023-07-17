using GPT.Helper;
using GPT.Model;
using GPT.Model.FunctionCalling;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GPT;

public partial class GPTClient
{
    private IEnumerable<Function>? functions;

    public GPTClientConfig Config;

    public List<ChatMessage> messages = new();

    public GPTClient(GPTClientConfig config,
                     string initialContent = "You are ChatGPT, a large language Answer as concisely as possible. Make a joke every few lines just to spice things up.",
                     IEnumerable<Function>? functions = null)
    {
        Config = config;

        GenerateInitialMessages(initialContent, functions);
    }

    public GPTClient(string key,
                     string initialContent = "You are ChatGPT, a large language Answer as concisely as possible. Make a joke every few lines just to spice things up.",
                     IEnumerable<Function>? functions = null)
    {
        Config = new GPTClientConfig
        {
            Key = key,
        };

        GenerateInitialMessages(initialContent, functions);
    }

    private void GenerateInitialMessages(string? initialContent = null, IEnumerable<Function>? functions = null)
    {
        if (!string.IsNullOrWhiteSpace(initialContent))
        {
            messages.Add(new ChatMessage
            {
                Role = GPTRoles.System,
                Content = initialContent
            });
        }

        this.functions = functions;
    }

    /// <summary>
    /// send message to chat gpt
    /// </summary>
    /// <param name="message">message to send</param>
    /// <returns>chat gpt response</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<ChatMessageResponse> SendMessage(string message)
    {
        if (message == null)
        {
            throw new ArgumentException(nameof(message) + " is null");
        }

        messages.Add(new ChatMessage { Role = "user", Content = message });

        // Create the request for the API sending the
        // latest collection of chat messages
        var request = new
        {
            messages,
            model = Config.ChatModel,
            max_tokens = Config.MaxTokens,
            functions = this.functions
        };

        // Send the request and capture the response
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Config.Key}");
        var requestJson = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings
        {
            ContractResolver = new LowercaseContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
        });
        var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
        var httpResponseMessage = await httpClient.PostAsync(Config.Url, requestContent);
        var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<GPTResponse>(jsonString, new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true
                }
            }
        });

        if (!string.IsNullOrEmpty(response.Error.Message))  // Check for errors
        {
            //throw new Exception(response.Error.Message);
            return new ChatMessageResponse
            {
                Content = response.Error.Message
            };
        }
        else  // Add the message object to the message collection
        {
            var responseMessage = response.Choices.First().Message;
            messages.Add(new ChatMessage
            {
                Content = responseMessage.Content ?? string.Empty,
                Role = responseMessage.Role
            });

            return responseMessage;
        }
    }

}