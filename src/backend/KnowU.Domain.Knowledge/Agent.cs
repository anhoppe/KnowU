using System.Diagnostics;
using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using LLama;
using LLama.Common;
using LLama.Native;

namespace KnowU.Domain.Knowledge;

internal class Agent : IAgent, IDisposable
{
    const string ModelPath = @"c:\models\google_gemma-3-4b-it-Q6_K.gguf";

    private readonly ChatSession _chatSession;
    private readonly IOntologyProvider _ontologyProvider;
    private readonly LLamaWeights _model;
    private readonly LLamaContext _context;
    
    private readonly InferenceParams _interferenceParams = new()
    {
        AntiPrompts = ["<end_of_turn>"]
    };
    
    public Agent(string systemPrompt, IOntologyProvider ontologyProvider)
    {
        _ontologyProvider = ontologyProvider;
        
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
        _model = LLamaWeights.LoadFromFile(parameters);
        _context = _model.CreateContext(parameters);
        var executor = new InteractiveExecutor(_context);

        // Build complete system prompt with ontology
        var completeSystemPrompt = systemPrompt 
            + "\n\n" + _ontologyProvider.GetOntologyPromptSection() 
            + "\n\n" + _ontologyProvider.GetJsonSchemaExample();
        
        // Start a fresh history with the role's System Prompt
        var history = new ChatHistory();
        history.AddMessage(AuthorRole.System, completeSystemPrompt);

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
        
        // Strip markdown code fences if present (LLMs sometimes add them despite instructions)
        var jsonContent = fullResponse.Trim();
        if (jsonContent.StartsWith("```json"))
        {
            jsonContent = jsonContent.Substring(7); // Remove ```json
        }
        else if (jsonContent.StartsWith("```"))
        {
            jsonContent = jsonContent.Substring(3); // Remove ```
        }
        
        if (jsonContent.EndsWith("```"))
        {
            jsonContent = jsonContent.Substring(0, jsonContent.Length - 3); // Remove trailing ```
        }
        
        jsonContent = jsonContent.Trim();
        
        // Parse JSON response into Claims
        try
        {
            var wrapper = System.Text.Json.JsonSerializer.Deserialize<ClaimsWrapper>(jsonContent);
            
            if (wrapper?.Claims == null)
            {
                Console.WriteLine($"Warning: No claims extracted from response");
                return new List<Claim>();
            }
            
            var claims = new List<Claim>();
            
            foreach (var claim in wrapper.Claims)
            {
                // Validate and resolve predicate
                var predicate = _ontologyProvider.FindPredicate(claim.Predicate.Id);
                if (predicate == null)
                {
                    Console.WriteLine($"Warning: Unknown predicate {claim.Predicate.Id}");
                    continue;
                }
                
                // Resolve entity types
                var subjectType = claim.Subject.TypeId != null 
                    ? _ontologyProvider.FindClass(claim.Subject.TypeId) 
                    : null;
                var objectType = claim.Object.TypeId != null 
                    ? _ontologyProvider.FindClass(claim.Object.TypeId) 
                    : null;
                
                var validatedClaim = new Claim
                {
                    Subject = new Entity
                    {
                        Id = claim.Subject.Id,
                        Name = claim.Subject.Name,
                        Type = subjectType
                    },
                    Predicate = predicate,
                    Object = new Entity
                    {
                        Id = claim.Object.Id,
                        Name = claim.Object.Name,
                        Type = objectType
                    },
                    ReferenceDocument = document
                };
                
                claims.Add(validatedClaim);
            }
            
            return claims;
        }
        catch (System.Text.Json.JsonException ex)
        {
            Console.WriteLine($"Error parsing LLM response as JSON: {ex.Message}");
            Console.WriteLine($"Response was: {fullResponse}");
            return new List<Claim>();
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
        _model?.Dispose();
    }

    private class ClaimsWrapper
    {
        [System.Text.Json.Serialization.JsonPropertyName("claims")]
        public List<Claim> Claims { get; set; } = new();
    }
}