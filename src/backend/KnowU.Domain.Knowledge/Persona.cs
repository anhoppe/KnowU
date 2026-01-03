using YamlDotNet.Serialization;

namespace KnowU.Domain.Knowledge;

public class Persona
{
    public string Id { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;

    [YamlMember(Alias = "system_prompt")]
    public string SystemPrompt { get; set; } = string.Empty;
}
