namespace KnowU.Domain.Storage.Contract;

/// <summary>
///     Represents a single document (note) that is submitted to the storage
/// </summary>
public interface IDocument
{
    /// <summary>
    /// Content of the document
    /// </summary>
    string Content { get; }
}
