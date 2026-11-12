namespace DataAccess;

public interface IDataStore
{
    List<T> GetCollection<T>() where T : class;
    void SaveCollection<T>(List<T> list) where T : class;
    void Commit();
    void ResetToDefault();
}
