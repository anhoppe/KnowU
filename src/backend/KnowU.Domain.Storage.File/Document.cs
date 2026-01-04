using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Storage.File;

/// <summary>
/// Internal storage document with persistence metadata
/// </summary>
internal class StorageDocument : Document
{
    /// <summary>
    ///     Creation date of the note
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    ///     Metadata version of the note
    /// </summary>
    public int Version { get; init; }
}