using System;
using System.Collections.Generic;
using System.Text;

namespace KnowU.Domain.Storage.Contract;

/// <summary>
/// Access to storage functionalities
/// </summary>
public interface IStorage
{
    /// <summary>
    /// Access to all documents that are available
    /// </summary>
    IList<IDocument> Documents { get; }
    
    /// <summary>
    /// Stores a document in the storage
    /// </summary>
    /// <param name="content">Content of the document to be stored</param>
    void StoreDocument(string content);
}
