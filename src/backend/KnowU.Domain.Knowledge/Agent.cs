using System.Text.Json;
using System.Text.Json.Serialization;
using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge;

internal class Agent : IAgent, IDisposable
{
    private readonly IAiCore _aiCore;
    private readonly IOntologyProvider _ontologyProvider;
    private readonly IStorage _storage;

    public Agent(string systemPrompt, IOntologyProvider ontologyProvider, IStorage storage, IAiCore aiCore)
    {
        _ontologyProvider = ontologyProvider;
        _storage = storage;
        _aiCore = aiCore;
        
        // Build complete system prompt with ontology
        var completeSystemPrompt = systemPrompt
                                   + "\n\n" + _ontologyProvider.GetOntologyPromptSection()
                                   + "\n\n" + _ontologyProvider.GetJsonSchemaExample();

        // Initialize the AI core with the complete system prompt
        _aiCore.Initialize(completeSystemPrompt);
    }

    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public async Task<IList<Claim>> ProcessAsync(Document document)
    {
        var respondJson = await _aiCore.ProcessAsync(document);
        var jsonContent = respondJson.ExtractJson();

        // Parse JSON response into Claims
        try
        {
            var wrapper = JsonSerializer.Deserialize<ClaimsWrapper>(jsonContent);

            if (wrapper?.Claims == null)
            {
                Console.WriteLine("Warning: No claims extracted from response");
                return new List<Claim>();
            }

            var claims = GenerateClaims(document, wrapper);

            return claims;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error parsing LLM response as JSON: {ex.Message}");
            Console.WriteLine($"Response was: {jsonContent}");
            return new List<Claim>();
        }
    }

    public void Dispose()
    {
        if (_aiCore is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    private List<Claim> GenerateClaims(Document document, ClaimsWrapper wrapper)
    {
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
                    Description = claim.Subject.Description,
                    Properties = claim.Subject.Properties,
                    Type = subjectType
                },
                Predicate = predicate,
                Object = new Entity
                {
                    Id = claim.Object.Id,
                    Name = claim.Object.Name,
                    Description = claim.Object.Description,
                    Properties = claim.Object.Properties,
                    Type = objectType
                }
            };

            claims.Add(validatedClaim);
            
            _storage.StoreClaim(validatedClaim, document.Id);
        }

        return claims;
    }

    private class ClaimsWrapper
    {
        [JsonPropertyName("claims")] public List<Claim> Claims { get; set; } = new();
    }
}