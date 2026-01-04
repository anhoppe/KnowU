using System.Text;

namespace KnowU.Domain.Knowledge;

/// <summary>
/// Encapsulates the JSON response from an AI agent
/// </summary>
internal class AgentRespondJson
{
    private readonly StringBuilder _stringBuilder = new();

    /// <summary>
    /// Appends text to the response buffer
    /// </summary>
    /// <param name="text">Text chunk to append</param>
    public void AppendText(string text)
    {
        _stringBuilder.Append(text);
    }

    /// <summary>
    /// Extracts clean JSON from the response, removing markdown fences and preamble
    /// </summary>
    /// <returns>Clean JSON string</returns>
    public string ExtractJson()
    {
        var fullText = _stringBuilder.ToString();

        // Find opening fence
        var openFenceIndex = fullText.IndexOf("```json", StringComparison.Ordinal);
        if (openFenceIndex == -1)
        {
            openFenceIndex = fullText.IndexOf("```", StringComparison.Ordinal);
        }

        // Find closing fence
        var closeFenceIndex = fullText.LastIndexOf("```", StringComparison.Ordinal);

        if (openFenceIndex >= 0 && closeFenceIndex > openFenceIndex)
        {
            // Extract content between fences
            var startIndex = fullText.IndexOf('\n', openFenceIndex) + 1;
            return fullText.Substring(startIndex, closeFenceIndex - startIndex).Trim();
        }

        // No fences found, return the full text
        return fullText.Trim();
    }
}
