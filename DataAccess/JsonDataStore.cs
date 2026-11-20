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
                if (dict == null)
                    throw new InvalidDataException("Le fichier de données ne contient pas de collections valides.");

                _store.Clear();
                foreach (var kvp in dict)
                    _store[kvp.Key] = kvp.Value;
            }
            catch (Exception ex) when (ex is not InvalidDataException)
            {
                throw new InvalidDataException(
                    $"Le fichier de données locales '{Path.GetFileName(_filePath)}' est illisible. Il n'a pas été remplacé.",
                    ex);
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
                return JsonSerializer.Deserialize<List<T>>(json)
                    ?? throw new InvalidDataException($"La collection {typeof(T).Name} est vide ou invalide.");
            }
            catch (Exception ex) when (ex is not InvalidDataException)
            {
                throw new InvalidDataException($"La collection {typeof(T).Name} est illisible.", ex);
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
            var tempPath = _filePath + ".tmp";
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var fullDict = new Dictionary<string, string>(_store);
                var json = JsonSerializer.Serialize(fullDict, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(tempPath, json);
                File.Move(tempPath, _filePath, true);
            }
            catch (Exception ex)
            {
                try
                {
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch
                {
                    // Preserve the original persistence error.
                }

                throw new IOException("Impossible d'enregistrer les données locales.", ex);
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
