using System.Text;
using System.Text.Json;
using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

public class OntologyProvider : IOntologyProvider
{
    private readonly Ontology _ontology;
    private readonly string _jsonSchemaPrompt;

    public OntologyProvider(string ontologyJsonPath)
    {
        var jsonContent = File.ReadAllText(ontologyJsonPath);
        _ontology = JsonSerializer.Deserialize<Ontology>(jsonContent) 
            ?? throw new InvalidOperationException($"Could not load ontology from {ontologyJsonPath}");
        
        // Load JSON schema prompt from data directory
        var dataDirectory = Path.GetDirectoryName(ontologyJsonPath) 
            ?? throw new InvalidOperationException("Could not determine data directory");
        var jsonSchemaPath = Path.Combine(dataDirectory, "jsonSchema.txt");
        _jsonSchemaPrompt = File.ReadAllText(jsonSchemaPath);
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
        return _jsonSchemaPrompt;
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
