namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
/// Represents a document (note) in the knowledge domain
/// </summary>
public class Document
{
    public string Id { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public IReadOnlyList<string> Tags { get; init; } = new List<string>();
}
