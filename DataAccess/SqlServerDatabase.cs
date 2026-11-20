using Microsoft.Data.SqlClient;

namespace DataAccess;

public static class SqlServerDatabase
{
    public static SqlConnection OpenConnection()
    {
        var connection = new SqlConnection(DatabaseConfig.ConnectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (SqlException ex)
        {
            connection.Dispose();
            throw new InvalidOperationException(
                $"Connexion impossible à SQL Server ({DatabaseConfig.ConnectionString.Split(';')[0]}). Vérifiez l'instance, la base et l'accès Windows. Détail : {ex.Message}",
                ex);
        }
    }

    public static string VerifyConnection()
    {
        using var connection = OpenConnection();
        using var command = new SqlCommand("SELECT DB_NAME();", connection);
        var databaseName = Convert.ToString(command.ExecuteScalar()) ?? string.Empty;
        if (!string.Equals(databaseName, "dbGest_universite", StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"La connexion pointe sur '{databaseName}', pas sur dbGest_universite.");
        return databaseName;
    }
}
