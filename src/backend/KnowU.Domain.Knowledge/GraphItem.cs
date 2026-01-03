using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KnowU.Domain.Knowledge;

internal class GraphItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    // Only used for entity classes
    [JsonPropertyName("subClassOf")]
    public string SubClassOf { get; set; } = string.Empty;

    // Only used for predicates
    [JsonPropertyName("domain")]
    public string Domain { get; set; } = string.Empty;

    [JsonPropertyName("range")]
    public string Range { get; set; } = string.Empty;

    [JsonPropertyName("allowedModality")]
    public List<string> AllowedModality { get; set; } = new();
}
