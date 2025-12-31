using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KnowU.Domain.Knowledge;

internal class GraphItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    // Only used for entity classes
    [JsonPropertyName("subClassOf")]
    public string SubClassOf { get; set; }

    // Only used for predicates
    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    [JsonPropertyName("range")]
    public string Range { get; set; }

    [JsonPropertyName("allowedModality")]
    public List<string> AllowedModality { get; set; }
}
