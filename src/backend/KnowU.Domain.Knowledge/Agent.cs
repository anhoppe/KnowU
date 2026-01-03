using System.Diagnostics;
using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using LLama;
using LLama.Common;
using LLama.Native;

namespace KnowU.Domain.Knowledge;

internal class Agent : IAgent
{
    const string ModelPath = @"c:\models\google_gemma-3-4b-it-Q6_K.gguf";

    private readonly ChatSession _chatSession;
    
    private readonly InferenceParams _interferenceParams = new()
    {

        AntiPrompts = new[] { "<end_of_turn>" }
    };
    
    public Agent(string systemPrompt)
    {
        // Configure model parameters
        var parameters = new ModelParams(ModelPath)
        {
            ContextSize = 4096,
            GpuLayerCount = -1,
        };

        NativeLogConfig.llama_log_set((d, msg) =>
        {
            Debug.WriteLine($"[{d}] - {msg.Trim()} ");
        });
        
        Console.WriteLine("Loading model...");
        using var model = LLamaWeights.LoadFromFile(parameters);

        using var context = model.CreateContext(parameters);
        var executor = new InteractiveExecutor(context);

        // Start a fresh history with the role's System Prompt
        var history = new ChatHistory();
        history.AddMessage(AuthorRole.System, systemPrompt);

        _chatSession = new ChatSession(executor, history);
    }

    public string Id { get; init; } = string.Empty;
    
    public string Name { get; init; } = string.Empty;

    public async Task<IList<Claim>> ProcessAsync(IDocument document)
    {
        var responseBuilder = new System.Text.StringBuilder();
        
        await foreach (var text in _chatSession.ChatAsync(new ChatHistory.Message(AuthorRole.User, document.Content), _interferenceParams))
        {
            responseBuilder.Append(text);
        }
        
        var fullResponse = responseBuilder.ToString();
        
        // TODO: Parse fullResponse into Claims
        return new List<Claim>();
    }
}