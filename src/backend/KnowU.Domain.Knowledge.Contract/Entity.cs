using System.Text.Json.Serialization;

namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
/// Represents an entity in a claim, can either be Subject or an Object in the Claim
/// </summary>
public class Entity
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }
    
    [JsonPropertyName("properties")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Properties { get; set; }
    
    [JsonPropertyName("typeId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TypeId { get; set; }
    
    [JsonIgnore]
    public EntityClass? Type { get; set; }
}
