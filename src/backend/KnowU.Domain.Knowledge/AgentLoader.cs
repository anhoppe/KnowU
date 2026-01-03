using KnowU.Domain.Knowledge.Contract;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace KnowU.Domain.Knowledge;

public class AgentLoader
{
    private readonly IAgentFactory _agentFactory;

    public AgentLoader(string yamlFilePath, IAgentFactory agentFactory)
    {
        _agentFactory = agentFactory;
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        var yamlContent = File.ReadAllText(yamlFilePath);
        var wrapper = deserializer.Deserialize<PersonasWrapper>(yamlContent);

        foreach (var persona in wrapper.Roles)
        {
            Agents.Add(_agentFactory.CreateAgent(persona.SystemPrompt, persona.Id, persona.Name));
        }
    }

    public IList<IAgent> Agents { get; } = new List<IAgent>();

    private class PersonasWrapper
    {
        public List<Persona> Roles { get; set; } = new();
    }
}