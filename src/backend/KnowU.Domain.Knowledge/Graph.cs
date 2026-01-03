using System.Text.Json.Serialization;
using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

public class Ontology
{
    [JsonPropertyName("@context")]
    public Dictionary<string, object> Context { get; set; } = new();

    [JsonPropertyName("classes")]
    public List<EntityClass> Classes { get; set; } = new();

    [JsonPropertyName("properties")]
    public List<PredicateProperty> Properties { get; set; } = new();
}
