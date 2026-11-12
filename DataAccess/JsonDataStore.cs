using System.Collections.Concurrent;
using System.Text.Json;

namespace DataAccess;

public class JsonDataStore : IDataStore
{
    private static readonly object _lock = new();
    private static JsonDataStore? _instance;
    private readonly ConcurrentDictionary<string, string> _store = new();
    private readonly string _filePath;

    public static JsonDataStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    _instance ??= new JsonDataStore(DatabaseConfig.DatabaseFilePath);
                }
            }
            return _instance;
        }
    }

    public JsonDataStore(string filePath)
    {
        _filePath = filePath;
        LoadFromFile();
    }

    private void LoadFromFile()
    {
        lock (_lock)
        {
            if (!File.Exists(_filePath))
                return;

            try
            {
                var json = File.ReadAllText(_filePath);
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict != null)
                {
                    _store.Clear();
                    foreach (var kvp in dict)
                    {
                        _store[kvp.Key] = kvp.Value;
                    }
                }
            }
            catch
            {
                // In case of corrupt file, fallback gracefully
            }
        }
    }

    public List<T> GetCollection<T>() where T : class
    {
        var key = typeof(T).FullName ?? typeof(T).Name;
        if (_store.TryGetValue(key, out var json) && !string.IsNullOrWhiteSpace(json))
        {
            try
            {
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }
        return new List<T>();
    }

    public void SaveCollection<T>(List<T> list) where T : class
    {
        var key = typeof(T).FullName ?? typeof(T).Name;
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        _store[key] = json;
        Commit();
    }

    public void Commit()
    {
        lock (_lock)
        {
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var fullDict = new Dictionary<string, string>(_store);
                var json = JsonSerializer.Serialize(fullDict, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_filePath, json);
            }
            catch
            {
                // Silently handle if file writing is temporarily locked
            }
        }
    }

    public void ResetToDefault()
    {
        lock (_lock)
        {
            _store.Clear();
            if (File.Exists(_filePath))
            {
                try { File.Delete(_filePath); } catch { }
            }
        }
    }
}
