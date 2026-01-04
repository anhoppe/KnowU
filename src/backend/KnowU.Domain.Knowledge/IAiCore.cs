using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Knowledge;

/// <summary>
/// Encapsulates the AI/LLM chat session for processing documents
/// </summary>
internal interface IAiCore
{
    /// <summary>
    /// Initializes the AI core with a system prompt
    /// </summary>
    /// <param name="systemPrompt">The complete system prompt including ontology and schema</param>
    void Initialize(string systemPrompt);
    
    /// <summary>
    /// Processes a document through the AI model and returns the response
    /// </summary>
    /// <param name="document">The document to process</param>
    /// <returns>The AI response containing extracted JSON data</returns>
    Task<AgentRespondJson> ProcessAsync(Document document);
}
