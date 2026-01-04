using KnowU.Domain.Knowledge.Contract;
using KnowU.Domain.Storage.Contract;
using KnowU.Infrastructure.Io.Contract;

namespace KnowU.Domain.Storage.File;

internal class Storage(IJsonSerializer _jsonSerializer) : IStorage
{
    // Map to track which document each claim belongs to
    private readonly Dictionary<Claim, string> _claimDocuments = new();
    
    public IList<Document> Documents { get; } = new List<Document>();

    public IList<Claim> Claims { get; } = new List<Claim>();

    public string StoreDocument(Document document)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        if (string.IsNullOrEmpty(document.Content))
        {
            throw new ArgumentException("Document content is empty");
        }

        // Generate ID if not provided
        var guid = string.IsNullOrEmpty(document.Id) ? Guid.NewGuid() : Guid.Parse(document.Id);
        
        // Update document with generated ID if needed
        var documentToStore = string.IsNullOrEmpty(document.Id) 
            ? new Document { Id = guid.ToString("N"), Content = document.Content, Tags = document.Tags }
            : document;

        // Serialize the document with the specified ID
        _jsonSerializer.Serialize(documentToStore, guid);
        
        Documents.Add(documentToStore);
        
        return documentToStore.Id;
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
