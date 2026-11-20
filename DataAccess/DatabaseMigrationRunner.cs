using Microsoft.Data.SqlClient;

namespace DataAccess;

public static class DatabaseMigrationRunner
{
    private static readonly string[] MigrationFiles =
    [
        "001_inscription_amount.sql",
        "002_absences.sql"
    ];

    public static void ApplyPendingMigrations()
    {
        using var connection = SqlServerDatabase.OpenConnection();
        foreach (var file in MigrationFiles)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Database", file);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Migration SQL introuvable : {path}", path);

            var script = File.ReadAllText(path);
            foreach (var batch in SplitBatches(script))
            {
                using var command = new SqlCommand(batch, connection) { CommandTimeout = 120 };
                command.ExecuteNonQuery();
            }
        }
    }

    private static IEnumerable<string> SplitBatches(string script)
    {
        var batch = new List<string>();
        foreach (var line in script.Replace("\r\n", "\n").Split('\n'))
        {
            if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
            {
                var text = string.Join(Environment.NewLine, batch).Trim();
                if (text.Length > 0) yield return text;
                batch.Clear();
            }
            else batch.Add(line);
        }

        var finalBatch = string.Join(Environment.NewLine, batch).Trim();
        if (finalBatch.Length > 0) yield return finalBatch;
    }
}
