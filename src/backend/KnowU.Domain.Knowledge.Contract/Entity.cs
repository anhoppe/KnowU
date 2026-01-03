namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
/// Represents an entity in a claim, can either be Subject or an Object in the Claim
/// </summary>
public class Entity
{
    public string Id { get; set; } = string.Empty;
    
    public EntityClass? Type { get; set; }
}
