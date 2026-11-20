namespace DataAccess;

public static class IdentityHelper
{
    public static int NextId(IDataStore store, IEnumerable<int> existingIds) =>
        store is SqlServerDataStoreMarker ? 0 : existingIds.DefaultIfEmpty(0).Max() + 1;
}

// Marker implemented by stores that use database-generated identity columns.
public interface SqlServerDataStoreMarker { }
