namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
///     Represents an agent that extracts data from the document layer
/// </summary>
public interface IAgent
{
    /// <summary>
    ///     ID of the persona (short version of the name w/o space)
    /// </summary>
    string Id { get; }

    /// <summary>
    ///     Name of the persona the agent represents
    ///     The persona is the name of the role in the loaded configuration
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     Processes a single document and extracts claims
    /// </summary>
    /// <param name="document"></param>
    Task<IList<Claim>> ProcessAsync(Document document);
}