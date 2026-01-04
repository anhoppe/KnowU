namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
///     Represents a single document (note) that is submitted to the storage
/// </summary>
public interface IDocument
{
    /// <summary>
    /// Unique identifier for the document
    /// </summary>
    string Id { get; }
    
    /// <summary>
    /// Content of the document
    /// </summary>
    string Content { get; }
}
