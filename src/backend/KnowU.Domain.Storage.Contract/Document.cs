namespace KnowU.Domain.Storage.Contract;

/// <summary>
///     Represents a single document (note) that is submitted to the storage
/// </summary>
public class Document
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