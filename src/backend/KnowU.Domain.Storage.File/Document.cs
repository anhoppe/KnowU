using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Storage.File;


internal class Document : IDocument
{
    /// <summary>
    ///     Unique identifier for the document
    /// </summary>
    public string Id { get; init; } = string.Empty;
    
    /// <summary>
    ///     The note added by the user
    /// </summary>
    public string Content { get; init; } = string.Empty;

    /// <summary>
    ///     Creation date of the note
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    ///     Metadata version of the note
    /// </summary>
    public int Version { get; init; }
}