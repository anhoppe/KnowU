using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

internal class AgentFactory : IAgentFactory
{
    public IAgent CreateAgent(string systemPrompt, string id, string name, IOntologyProvider ontologyProvider)
    {
        return new Agent(systemPrompt, ontologyProvider)
        {
            Id = id,
            Name = name
        };
    }
}
