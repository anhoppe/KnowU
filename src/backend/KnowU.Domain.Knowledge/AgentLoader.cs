using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnowU.Domain.Knowledge;

internal class AgentLoader
{
    private readonly IAgentFactory _agentFactory;
    private readonly IOntologyProvider _ontologyProvider;
    private readonly IStorage _storage;

    public AgentLoader(string yamlFilePath, IAgentFactory agentFactory, IOntologyProvider ontologyProvider, IStorage storage)
    {
        _agentFactory = agentFactory;
        _ontologyProvider = ontologyProvider;
        _storage = storage;

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var yamlContent = File.ReadAllText(yamlFilePath);
        var wrapper = deserializer.Deserialize<PersonasWrapper>(yamlContent);

        foreach (var persona in wrapper.Roles)
        {
            var aiCore = new AiCore();
            Agents.Add(_agentFactory.CreateAgent(persona.SystemPrompt, persona.Id, persona.Name, _ontologyProvider, _storage, aiCore));
        }
    }

    public IList<IAgent> Agents { get; } = new List<IAgent>();

    private class PersonasWrapper
    {
        public List<Persona> Roles { get; set; } = new();
    }
}