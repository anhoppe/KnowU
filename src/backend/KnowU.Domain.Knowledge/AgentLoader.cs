using KnowU.Domain.Knowledge.Contract;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnowU.Domain.Knowledge;

internal class AgentLoader
{
    private readonly IAgentFactory _agentFactory;
    private readonly IOntologyProvider _ontologyProvider;

    public AgentLoader(string yamlFilePath, IAgentFactory agentFactory, IOntologyProvider ontologyProvider)
    {
        _agentFactory = agentFactory;
        _ontologyProvider = ontologyProvider;

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var yamlContent = File.ReadAllText(yamlFilePath);
        var wrapper = deserializer.Deserialize<PersonasWrapper>(yamlContent);

        foreach (var persona in wrapper.Roles)
        {
            Agents.Add(_agentFactory.CreateAgent(persona.SystemPrompt, persona.Id, persona.Name, _ontologyProvider));
        }
    }

    public IList<IAgent> Agents { get; } = new List<IAgent>();

    private class PersonasWrapper
    {
        public List<Persona> Roles { get; set; } = new();
    }
}