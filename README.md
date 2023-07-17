# GPTClient

## Description
This project includes functions for processing and executing GPT models, and a sample project that demonstrates how to use these functions.

## Installation
`NuGet\Install-Package ErikR87.AI.GPTClient -Version 1.0.0`

## Sample Project
The sample project located in the `SampleConsoleApp` directory is a simple console application that demonstrates how to use the functions in this project.

## GPT-Client
Simple client:
`var client = new GPT.GPTClient("insert-openai-key-here");`

Custom behavior:
`var client = new GPT.GPTClient("key", "you are a smart assistant...")`

## Function Calling
Please have a look at `SampleConsoleApp`.

## Embeddings
Use `gptClient.GetEmbeddings("...")` on your gptClient-instance to get vector-data.

## GPTClientConfig
Use `GPTClientConfig` class for more configuration settings.
Here you can set: `chat-model`, `max-tokens`, `embedding-model` and the endpoint urls.
```
var config = new GPTClientConfig {
  Key = "...",
  ChatModel = ChatModels.GPT_4_0613
}

var client = new GPT.GPTClient(config);
```
