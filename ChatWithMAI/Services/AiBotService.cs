using System.Diagnostics.CodeAnalysis;
using ChatWithMAI.Shared;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace ChatWithMAI.Services;

public interface IAiBotService
{
    public Task<ChatMessageContent> SendChatPromptAsync(string userInput);
}
public class AiBotService : IAiBotService
{
    private readonly Kernel? kernel;
    private readonly IChatCompletionService? chatCompletionService;
    private readonly OpenAIPromptExecutionSettings? openAiPromptExecutionSettings;
    private ChatHistory history = new ();

    [Experimental("SKEXP0001")]
    public AiBotService(IConfiguration config)
    {
        if (!Settings.AiInUse)
            return;

        var builder = Kernel.CreateBuilder().AddOpenAIChatCompletion("gpt-3.5-turbo", config["OpenAiKey"] ?? string.Empty);

        // Add enterprise components
        builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace));

        // Build the kernel
        kernel = builder.Build();
        chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // Enable planning
        openAiPromptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };
    }

    public async Task<ChatMessageContent> SendChatPromptAsync(string userInput)
    {
        if (!Settings.AiInUse)
        {
            var standardMessage = new ChatMessageContent
            {
                Content = "Ai bot is currently unavailable."
            };
            return standardMessage;
        }

        // Add user input
        history.AddUserMessage(userInput);
        var result = new ChatMessageContent();
        try
        {
            // Get the response from the AI
            result = await chatCompletionService!.GetChatMessageContentAsync(
                history,
                executionSettings: openAiPromptExecutionSettings,
                kernel: kernel);

            // Add the message from the agent to the chat history
            history.AddMessage(result.Role, result.Content ?? string.Empty);
        }
        catch (Exception e)
        {
            result.Content = $"Error: {e.Message}";
        }

        return result;
    }
}

public static class AIBotServiceExtensions
{
    public static IServiceCollection AddAIBotService(this IServiceCollection services)
    {
        return services.AddScoped<IAiBotService, AiBotService>();
    }
}
