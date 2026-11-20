using DataAccess;
using DAO.Repositories;

namespace Gestion_Universite;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) =>
        {
            var detail = e.Exception.GetBaseException().Message;
            MessageBox.Show(
                $"L'opération n'a pas pu être terminée.\n\nDétail : {detail}",
                "Erreur",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        };

        try
        {
            SqlServerDatabase.VerifyConnection();
            DatabaseMigrationRunner.ApplyPendingMigrations();
            DataStoreProvider.Current = SqlServerDataStore.Instance;
            Application.Run(new FormMenuPrincipal());
        }
        catch (Exception ex)
        {
            var detail = ex.GetBaseException().Message;
            MessageBox.Show(
                $"L'application n'a pas pu démarrer.\n\nDétail : {detail}",
                "Erreur au démarrage",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
