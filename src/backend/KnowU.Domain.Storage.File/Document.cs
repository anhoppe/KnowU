using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Storage.File;


internal class Document : IDocument
{
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