namespace DataAccess;

public static class DataStoreProvider
{
    public static IDataStore Current { get; set; } = JsonDataStore.Instance;
}
