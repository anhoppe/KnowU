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
    /// Stores a note in the storage
    /// </summary>
    /// <param name="note">Note to be stored</param>
    void StoreNote(string note);
}
