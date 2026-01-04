using System.Diagnostics;
using KnowU.Domain.Knowledge.Contract;
using LLama;
using LLama.Common;
using LLama.Native;

namespace KnowU.Domain.Knowledge;

/// <summary>
/// Encapsulates the LLM chat session for AI-based document processing
/// </summary>
internal class AiCore : IAiCore, IDisposable
{
    private const string ModelPath = @"c:\models\google_gemma-3-4b-it-Q6_K.gguf";

    private ChatSession _chatSession = null!;
    private LLamaContext _context = null!;

    private readonly InferenceParams _inferenceParams = new()
    {
        AntiPrompts = ["<end_of_turn>"]
    };

    private LLamaWeights _model = null!;

    public void Initialize(string systemPrompt)
    {
        // Configure model parameters
        var parameters = new ModelParams(ModelPath)
        {
            ContextSize = 4096,
            GpuLayerCount = -1
        };

        NativeLogConfig.llama_log_set((d, msg) => { Debug.WriteLine($"[{d}] - {msg.Trim()} "); });

        Console.WriteLine("Loading model...");
        _model = LLamaWeights.LoadFromFile(parameters);
        _context = _model.CreateContext(parameters);
        var executor = new InteractiveExecutor(_context);

        // Start a fresh history with the system prompt
        var history = new ChatHistory();
        history.AddMessage(AuthorRole.System, systemPrompt);

        _chatSession = new ChatSession(executor, history);
    }

    public async Task<AgentRespondJson> ProcessAsync(IDocument document)
    {
        var respondJson = new AgentRespondJson();

        await foreach (var text in _chatSession.ChatAsync(new ChatHistory.Message(AuthorRole.User, document.Content),
                           _inferenceParams))
        {
            respondJson.AppendText(text);
        }

        return respondJson;
    }

    public void Dispose()
    {
        _context.Dispose();
        _model.Dispose();
    }
}
