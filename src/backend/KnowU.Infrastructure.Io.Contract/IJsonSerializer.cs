namespace KnowU.Infrastructure.Io.Contract;

/// <summary>
/// Used to serialize / deserialize json objects
/// Encapsulates the path selection
/// </summary>
public interface IJsonSerializer
{
    /// <summary>
    /// Serializes an object to JSON file with auto-generated ID
    /// </summary>
    /// <returns>The GUID used as the filename (without extension)</returns>
    void Serialize<T>(T obj);
    
    /// <summary>
    /// Serializes an object to JSON file using the specified ID
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <param name="id">ID to use for the filename</param>
    void Serialize<T>(T obj, Guid id);
}
