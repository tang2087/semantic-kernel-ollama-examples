#pragma warning disable SKEXP0010
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.TemplateEngine;
using math_function_example.Plugins;
using static System.Console;


// We use Mistral for this tutorial 
var modelId = "mistral";
// local Ollama endpoint
var endpoint = new Uri("http://localhost:11434");

var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.Plugins.AddFromType<MyMathPlugin>();
var kernel = kernelBuilder
    .AddOpenAIChatCompletion(
        modelId,
        endpoint,
        apiKey: null)
    .Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

var executionSettings = new OpenAIPromptExecutionSettings
{
    MaxTokens = 2000,
    Temperature = 0,
    // Enable automatic invocation of kernel functions
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

// Create chat history
ChatHistory history = [];

// Start the conversation
Console.BackgroundColor = ConsoleColor.Green;
Console.ForegroundColor = ConsoleColor.White; 
Write("User > ");
Console.ResetColor();
string? userInput;
while ((userInput = Console.ReadLine()) != null)
{
    history.AddUserMessage(userInput);
    var result = chatService.GetStreamingChatMessageContentsAsync(
                                history,
                                executionSettings: executionSettings,
                                kernel: kernel);

    // Stream the results
    string fullMessage = "";
    var first = true;
    await foreach (var content in result)
    {
        if (content.Role.HasValue && first)
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White; 
            Write("Assistant > ");
            Console.ResetColor();
            first = false;
        }
        Write(content.Content);
        fullMessage += content.Content;
    }

    WriteLine();

    // Add the message from the agent to the chat history
    history.AddAssistantMessage(fullMessage);

    // Get user input again
    Console.BackgroundColor = ConsoleColor.Green;
    Console.ForegroundColor = ConsoleColor.White; 
    Write("User > ");
    Console.ResetColor();
}
