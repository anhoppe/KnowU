using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

internal interface IAgentFactory
{
    IAgent CreateAgent(string systemPrompt, string id, string name, IOntologyProvider ontologyProvider);
}
