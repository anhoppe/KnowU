using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using KnowU.Infrastructure.Io.Contract;

namespace KnowU.Domain.Storage.File;

internal class Storage(IJsonSerializer _jsonSerializer) : IStorage
{
    // Map to track which document each claim belongs to
    private readonly Dictionary<Claim, string> _claimDocuments = new();
    
    public IList<IDocument> Documents { get; } = new List<IDocument>();

    public IList<Claim> Claims { get; } = new List<Claim>();

    public string StoreDocument(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            throw new ArgumentException("Content is empty");
        }

        // Generate ID first
        var guid = Guid.NewGuid();
        
        // Create document with the ID
        var document = new Document
        {
            Id = guid.ToString("N"),
            Content = content,
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        // Serialize with the specified ID
        _jsonSerializer.Serialize(document, guid);
        
        Documents.Add(document);
        
        return document.Id;
    }

    public void StoreClaim(Claim claim)
    {
        StoreClaim(claim, string.Empty);
    }
    
    public void StoreClaim(Claim claim, string documentId)
    {
        if (claim == null)
        {
            throw new ArgumentNullException(nameof(claim));
        }
        
        _jsonSerializer.Serialize(claim);
        
        Claims.Add(claim);
        
        if (!string.IsNullOrEmpty(documentId))
        {
            _claimDocuments[claim] = documentId;
        }
    }

    public IList<Claim> GetClaimsByDocument(string documentId)
    {
        if (string.IsNullOrEmpty(documentId))
        {
            throw new ArgumentException("Document ID is empty", nameof(documentId));
        }
        
        return Claims.Where(c => _claimDocuments.TryGetValue(c, out var docId) && docId == documentId).ToList();
    }
}
