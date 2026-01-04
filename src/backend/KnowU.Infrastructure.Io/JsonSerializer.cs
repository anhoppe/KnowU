using System.Text.Json;
using KnowU.Infrastructure.Io.Contract;

namespace KnowU.Infrastructure.Io;

internal class JsonSerializer : IJsonSerializer
{
    private readonly string _storageDirectory;

    public JsonSerializer()
    {
        // Get the local app data folder: C:\Users\[username]\AppData\Local
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        _storageDirectory = Path.Combine(appDataPath, "KnowU", "Notes");

        // Ensure the directory exists
        Directory.CreateDirectory(_storageDirectory);
    }

    public void Serialize<T>(T obj)
    {
        // Generate a unique GUID for the filename
        var guid = Guid.NewGuid();
        Serialize(obj, guid);
    }
    
    public void Serialize<T>(T obj, Guid id)
    {
        var fileName = $"{id.ToString("N")}.json";
        var filePath = Path.Combine(_storageDirectory, fileName);

        // Serialize to JSON and write to file
        var json = System.Text.Json.JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }
}