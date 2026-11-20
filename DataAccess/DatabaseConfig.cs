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

    // Override with IIT_SQLSERVER_CONNECTION_STRING when running on another workstation.
    public static string ConnectionString { get; set; } =
        Environment.GetEnvironmentVariable("IIT_SQLSERVER_CONNECTION_STRING") ??
        "Server=YHPC\\SQLEXPRESS03;Database=dbGest_universite;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Application Name=IIT Gestion Universitaire";
}
