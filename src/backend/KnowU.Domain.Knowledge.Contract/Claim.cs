using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge.Contract;

/// <summary>
/// A claim is directly derived from text corpus
/// </summary>
public class Claim
{
    public Entity Object { get; init; } = new();

    public PredicateProperty Predicate { get; init; } = new();
   
    public IDocument ReferenceDocument { get; init; } = null!;

    public Entity Subject { get; init; } = new();
}
