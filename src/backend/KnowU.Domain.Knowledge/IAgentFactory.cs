using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge;

internal interface IAgentFactory
{
    IAgent CreateAgent(string systemPrompt, string id, string name, IOntologyProvider ontologyProvider, IStorage storage, IAiCore aiCore);
}
