namespace KnowU.Domain.Knowledge;

internal class AgentRespondJson
{
    private readonly System.Text.StringBuilder _responseBuilder = new();

    public void AppendText(string text)
    {
        _responseBuilder.Append(text);
    }

    public string ExtractJson()
    {
        var llmResponse = _responseBuilder.ToString();
        
        // Strip markdown code fences if present (LLMs sometimes add them despite instructions)
        // Also remove any text that appears before the opening fence
        var jsonContent = llmResponse.Trim();
        
        // Find the opening fence (could be ```json or just ```)
        var startFenceIndex = jsonContent.IndexOf("```json", StringComparison.OrdinalIgnoreCase);
        if (startFenceIndex >= 0)
        {
            // Remove everything before and including ```json
            jsonContent = jsonContent.Substring(startFenceIndex + 7);
        }
        else
        {
            startFenceIndex = jsonContent.IndexOf("```", StringComparison.OrdinalIgnoreCase);
            if (startFenceIndex >= 0)
            {
                // Remove everything before and including ```
                jsonContent = jsonContent.Substring(startFenceIndex + 3);
            }
        }
        
        // Remove trailing fence if present
        if (jsonContent.EndsWith("```"))
        {
            jsonContent = jsonContent.Substring(0, jsonContent.Length - 3);
        }
        
        return jsonContent.Trim();
    }
}
