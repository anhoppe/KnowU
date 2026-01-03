using System.Text;
using System.Text.Json;
using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

public class OntologyProvider : IOntologyProvider
{
    private readonly Ontology _ontology;

    public OntologyProvider(string ontologyJsonPath)
    {
        var jsonContent = File.ReadAllText(ontologyJsonPath);
        _ontology = JsonSerializer.Deserialize<Ontology>(jsonContent) 
            ?? throw new InvalidOperationException($"Could not load ontology from {ontologyJsonPath}");
    }

    public Ontology GetOntology() => _ontology;

    public string GetOntologyPromptSection()
    {
        var sb = new StringBuilder();
        sb.AppendLine("\n## Available Entity Classes:");
        foreach (var cls in _ontology.Classes)
        {
            sb.AppendLine($"- {cls.Id}: {cls.Label} - {cls.Description}");
        }
        
        sb.AppendLine("\n## Available Predicates:");
        foreach (var pred in _ontology.Properties)
        {
            sb.AppendLine($"- {pred.Id}: {pred.Label} - {pred.Description}");
            sb.AppendLine($"  Domain: {pred.Domain}, Range: {pred.Range}");
        }
        
        return sb.ToString();
    }

    public string GetJsonSchemaExample()
    {
        return @"
## Output Format:
You MUST respond with ONLY valid JSON. No markdown code blocks, no explanations, no additional text.

Example structure:
{
  ""claims"": [
    {
      ""subject"": {
        ""id"": ""entity_identifier"",
        ""name"": ""Entity Display Name"",
        ""typeId"": ""http://example.org/class/ClassName""
      },
      ""predicate"": {
        ""id"": ""http://example.org/predicate/predicateName""
      },
      ""object"": {
        ""id"": ""another_entity_id"",
        ""name"": ""Another Entity Name"",
        ""typeId"": ""http://example.org/class/ClassName""
      }
    }
  ]
}

CRITICAL RULES:
1. Use ONLY predicate IDs from the Available Predicates list above
2. Use ONLY class IDs from the Available Entity Classes list above
3. Entity IDs should be unique identifiers (e.g., slugified names)
4. Entity names should be human-readable labels extracted from the text
5. Output valid JSON only - no code fences, no extra text
";
    }

    public EntityClass? FindClass(string classId)
    {
        return _ontology.Classes.FirstOrDefault(c => c.Id == classId);
    }

    public PredicateProperty? FindPredicate(string predicateId)
    {
        return _ontology.Properties.FirstOrDefault(p => p.Id == predicateId);
    }
}
