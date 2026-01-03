using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

public class AgentFactory : IAgentFactory
{
    public IAgent CreateAgent(string systemPrompt, string id, string name)
    {
        return new Agent(systemPrompt)
        {
            Id = id,
            Name = name
        };
    }
}
