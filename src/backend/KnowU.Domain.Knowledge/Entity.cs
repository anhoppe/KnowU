namespace KnowU.Domain.Knowledge;

public class Entity
{
    public string Id { get; set; } = string.Empty;
    public EntityClass? Type { get; set; }
}