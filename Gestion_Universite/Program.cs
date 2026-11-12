using DataAccess;
using DAO;

namespace Gestion_Universite;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 1. Initialisation de la base de données & Injection des données initiales (Seeds)
        DataSeeder.Initialize(JsonDataStore.Instance);

        // 2. Génération automatique du script SQL de création de tables pour l'utilisateur
        var scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "script_creation_bdd.sql");
        DbHelper.GenerateScriptFile(scriptPath);

        // 3. Démarrage sur le Menu Principal (Dashboard & Formulaires)
        Application.Run(new FormMenuPrincipal());
    }
}
