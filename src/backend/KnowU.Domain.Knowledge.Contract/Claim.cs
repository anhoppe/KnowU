using System.Text.Json.Serialization;

namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
///     A claim is directly derived from text corpus, It represents a subject-predicate-object triple
/// </summary>
public class Claim
{
    [JsonPropertyName("object")]
    public Entity Object { get; init; } = new();

    [JsonPropertyName("predicate")]
    public PredicateProperty Predicate { get; init; } = new();

    [JsonPropertyName("subject")]
    public Entity Subject { get; init; } = new();
}