using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;

namespace KnowU.Domain.Knowledge;

internal class AgentFactory : IAgentFactory
{
    public IAgent CreateAgent(string systemPrompt, string id, string name, IOntologyProvider ontologyProvider, IStorage storage, IAiCore aiCore)
    {
        return new Agent(systemPrompt, ontologyProvider, storage, aiCore)
        {
            Id = id,
            Name = name
        };
    }
}
