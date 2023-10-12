using GPT;
using GPT.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

// get appsettings.json

var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

#if DEBUG
    configBuilder.AddJsonFile("appsettings.development.json", optional: true);
#endif

IConfiguration config = configBuilder.Build();

// initialize gpt configuration
var gptConfig = new GPTClientConfig
{
    Key = config["OpenAIKey"],
    ChatModel = ChatModels.GPT_4,
};

// optional: use function calling -> add all assemblies with functions (look at SpecialFunctions.cs)
FunctionResolver.AddAssemby(Assembly.GetExecutingAssembly().FullName);

// initialize GPTClient
var client = new GPT.GPTClient(gptConfig, functions: FunctionResolver.ToGPTFunction());

Console.WriteLine("Stelle deine Frage:");

while(true)
{
    var input = Console.ReadLine();

    // send message to gpt
    var response = await client.SendMessage(input);
    
    // optional: use this for function calling
    if(response.FunctionCall.HasValue)
    {
        var functionResult = await FunctionResolver.ExecuteFunction(response.FunctionCall.Value);
        response.Content = functionResult;
    }
    
    Console.WriteLine(response.Content);
}