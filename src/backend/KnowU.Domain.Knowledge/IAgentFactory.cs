using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

public interface IAgentFactory
{
    IAgent CreateAgent(string systemPrompt, string id, string name);
}
