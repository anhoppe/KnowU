using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;


namespace KnowU.Domain.Knowledge;

public static class KnowledgeFacade
{
    public static IServiceCollection AddKnowledge(this IServiceCollection services)
    {
        var jsonText = File.ReadAllText("ontology.json");
        var ontology = JsonSerializer.Deserialize<Ontology>(jsonText);

        return services;
    }
}
