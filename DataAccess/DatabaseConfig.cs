namespace DataAccess;

public static class DatabaseConfig
{
    private static string? _customDataDirectory;

    public static string DataDirectory
    {
        get
        {
            if (!string.IsNullOrEmpty(_customDataDirectory))
                return _customDataDirectory;

            // AppData or current application directory
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "IIT_Gestion_Universite");
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    dir = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
            return dir;
        }
        set => _customDataDirectory = value;
    }

    public static string DatabaseFilePath => Path.Combine(DataDirectory, "gestion_universite_db.json");

    // Can be used for SQL Server or MySQL when connected
    public static string ConnectionString { get; set; } =
        "Server=(localdb)\\MSSQLLocalDB;Database=IIT_Gestion_Universite;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

    public static bool UseSqlServer { get; set; } = false;
}
