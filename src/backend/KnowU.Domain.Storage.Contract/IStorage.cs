using System;
using System.Collections.Generic;
using System.Text;
using KnowU.Domain.Knowledge.Contract;

namespace KnowU.Domain.Storage.Contract;

/// <summary>
/// Access to storage functionalities
/// </summary>
public interface IStorage
{
    /// <summary>
    /// Access to all documents that are available
    /// </summary>
    IList<Document> Documents { get; }
    
    /// <summary>
    /// Access to all claims that are stored
    /// </summary>
    IList<Claim> Claims { get; }
    
    /// <summary>
    /// Stores a document in the storage
    /// </summary>
    /// <param name="document">The document to be stored</param>
    /// <returns>The ID of the stored document</returns>
    string StoreDocument(Document document);
    
    /// <summary>
    /// Stores a claim in the storage
    /// </summary>
    /// <param name="claim">The claim to store</param>
    void StoreClaim(Claim claim);
    
    /// <summary>
    /// Stores a claim in the storage with a reference to a document
    /// </summary>
    /// <param name="claim">The claim to store</param>
    /// <param name="documentId">The ID of the source document</param>
    void StoreClaim(Claim claim, string documentId);
    
    /// <summary>
    /// Retrieves claims by source document ID
    /// </summary>
    /// <param name="documentId">The document ID to filter by</param>
    /// <returns>List of claims from the specified document</returns>
    IList<Claim> GetClaimsByDocument(string documentId);
}

