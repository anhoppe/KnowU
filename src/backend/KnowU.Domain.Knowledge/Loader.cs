using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace KnowU.Domain.Knowledge;

internal class Loader
{
    public Ontology Load()
    {
        var path = "C:/repo/KnowU/data/onto.json";
        var jsonText = File.ReadAllText(path);
        var ontology = JsonSerializer.Deserialize<Ontology>(jsonText);

        if (ontology == null)
        {
            throw new InvalidOperationException($"Could not load knowledge graph from {path}");
        }
        
        return ontology;
    }
}
