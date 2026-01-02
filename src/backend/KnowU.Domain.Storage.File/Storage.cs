using KnowU.Domain.Storage.Contract;
using KnowU.Infrastructure.Io.Contract;

namespace KnowU.Domain.Storage.File;

internal class Storage(IJsonSerializer _jsonSerializer) : IStorage
{
    public IList<IDocument> Documents { get; } = new List<IDocument>();

    public void StoreDocument(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Content is empty");
        }

        var document = new Document
        {
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        _jsonSerializer.Serialize(document);
        
        Documents.Add(document);
    }
}