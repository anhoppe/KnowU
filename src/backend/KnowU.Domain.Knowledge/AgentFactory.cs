using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge;

internal class AgentFactory : IAgentFactory
{
    public IAgent CreateAgent(string systemPrompt, string id, string name, IOntologyProvider ontologyProvider, IStorage storage)
    {
        return new Agent(systemPrompt, ontologyProvider, storage)
        {
            Id = id,
            Name = name
        };
    }
}
