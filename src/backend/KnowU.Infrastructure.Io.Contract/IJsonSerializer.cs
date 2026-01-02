namespace KnowU.Infrastructure.Io.Contract;

/// <summary>
/// Used to serialize / deserialize json objects
/// Encapsulates the path selection
/// </summary>
public interface IJsonSerializer
{
    void Serialize<T>(T obj);
}
