using KnowU.Domain.Storage.Contract;
using System.Text.Json;

namespace KnowU.Domain.Storage.File;

public class Storage : IStorage
{
    private readonly string _storageDirectory;
    
    IList<IDocument> Dcuments { get; } = new List<IDocument>();

    public Storage()
    {
        // Get the local app data folder: C:\Users\[username]\AppData\Local
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storageDirectory = Path.Combine(appDataPath, "KnowU", "Notes");
        
        // Ensure the directory exists
        Directory.CreateDirectory(_storageDirectory);
    }

    public void StoreNote(string note)
    {
        var noteToSerialize = new NoteMeta()
        {
            Content = note,
            CreatedAt = DateTime.UtcNow,
            Version = 1
        };

        // Generate a unique filename using timestamp and GUID
        var fileName = $"{Guid.NewGuid():N}.json";
        var filePath = Path.Combine(_storageDirectory, fileName);

        // Serialize to JSON and write to file
        var json = JsonSerializer.Serialize(noteToSerialize, new JsonSerializerOptions 
        {
            WriteIndented = true 
        });

        System.IO.File.WriteAllText(filePath, json);
    }
}
