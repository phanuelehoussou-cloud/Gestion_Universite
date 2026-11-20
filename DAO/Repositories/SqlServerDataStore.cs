using System.Data;
using DAO.Models;
using DataAccess;
using Microsoft.Data.SqlClient;

namespace DAO.Repositories;

public sealed partial class SqlServerDataStore : IDataStore, SqlServerDataStoreMarker
{
    public static SqlServerDataStore Instance { get; } = new();

    private SqlServerDataStore() { }

    public List<T> GetCollection<T>() where T : class
    {
        object rows = typeof(T) switch
        {
            var type when type == typeof(AnneeAcademique) => GetAnnees(),
            var type when type == typeof(Semestre) => GetSemestres(),
            var type when type == typeof(Filiere) => GetFilieres(),
            var type when type == typeof(Niveau) => GetNiveaux(),
            var type when type == typeof(Classe) => GetClasses(),
            var type when type == typeof(Etudiant) => GetEtudiants(),
            var type when type == typeof(Inscription) => GetInscriptions(),
            var type when type == typeof(Enseignant) => GetEnseignants(),
            var type when type == typeof(Cours) => GetCours(),
            var type when type == typeof(Salle) => GetSalles(),
            var type when type == typeof(SeanceEmploiDuTemps) => GetSeances(),
            var type when type == typeof(Absence) => GetAbsences(),
            _ => throw new NotSupportedException($"Aucune table SQL n'est associée au modèle {typeof(T).Name}.")
        };
        return (List<T>)rows;
    }

    public void SaveCollection<T>(List<T> list) where T : class
    {
        using var connection = SqlServerDatabase.OpenConnection();
        using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
        try
        {
            if (typeof(T) == typeof(AnneeAcademique)) SaveAnnees(connection, transaction, list.Cast<AnneeAcademique>().ToList());
            else if (typeof(T) == typeof(Semestre)) SaveSemestres(connection, transaction, list.Cast<Semestre>().ToList());
            else if (typeof(T) == typeof(Filiere)) SaveFilieres(connection, transaction, list.Cast<Filiere>().ToList());
            else if (typeof(T) == typeof(Niveau)) SaveNiveaux(connection, transaction, list.Cast<Niveau>().ToList());
            else if (typeof(T) == typeof(Classe)) SaveClasses(connection, transaction, list.Cast<Classe>().ToList());
            else if (typeof(T) == typeof(Etudiant)) SaveEtudiants(connection, transaction, list.Cast<Etudiant>().ToList());
            else if (typeof(T) == typeof(Inscription)) SaveInscriptions(connection, transaction, list.Cast<Inscription>().ToList());
            else if (typeof(T) == typeof(Enseignant)) SaveEnseignants(connection, transaction, list.Cast<Enseignant>().ToList());
            else if (typeof(T) == typeof(Cours)) SaveCours(connection, transaction, list.Cast<Cours>().ToList());
            else if (typeof(T) == typeof(Salle)) SaveSalles(connection, transaction, list.Cast<Salle>().ToList());
            else if (typeof(T) == typeof(SeanceEmploiDuTemps)) SaveSeances(connection, transaction, list.Cast<SeanceEmploiDuTemps>().ToList());
            else if (typeof(T) == typeof(Absence)) SaveAbsences(connection, transaction, list.Cast<Absence>().ToList());
            else throw new NotSupportedException($"Aucune table SQL n'est associée au modèle {typeof(T).Name}.");

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Commit() { }

    public void ResetToDefault() => throw new NotSupportedException("La base SQL Server ne peut pas être réinitialisée depuis l'application.");

    private static List<T> ReadList<T>(string sql, Func<SqlDataReader, T> map)
    {
        using var connection = SqlServerDatabase.OpenConnection();
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        var result = new List<T>();
        while (reader.Read()) result.Add(map(reader));
        return result;
    }

    private static HashSet<int> ExistingIds(SqlConnection connection, SqlTransaction transaction, string table, string idColumn)
    {
        using var command = new SqlCommand($"SELECT [{idColumn}] FROM dbo.[{table}];", connection, transaction);
        using var reader = command.ExecuteReader();
        var ids = new HashSet<int>();
        while (reader.Read()) ids.Add(reader.GetInt32(0));
        return ids;
    }

    private static void Sync<T>(
        SqlConnection connection,
        SqlTransaction transaction,
        string table,
        string idColumn,
        IReadOnlyCollection<T> items,
        Func<T, int> getId,
        Action<T, int> setId,
        Func<T, int, int> upsert,
        Action<int> delete)
    {
        var existing = ExistingIds(connection, transaction, table, idColumn);
        var desired = new HashSet<int>();
        foreach (var item in items)
        {
            var id = getId(item);
            var savedId = upsert(item, id > 0 && existing.Contains(id) ? id : 0);
            setId(item, savedId);
            desired.Add(savedId);
        }
        foreach (var removedId in existing.Except(desired)) delete(removedId);
    }

    private static int SaveIdentity(SqlCommand command)
    {
        var value = command.ExecuteScalar();
        return Convert.ToInt32(value);
    }

    private static void Add(SqlCommand command, string name, object? value) =>
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);

    private static string Value(SqlDataReader reader, string name, string fallback = "") =>
        reader[name] is DBNull ? fallback : Convert.ToString(reader[name]) ?? fallback;

    private static DateTime Date(SqlDataReader reader, string name) =>
        reader[name] is DBNull ? DateTime.MinValue : Convert.ToDateTime(reader[name]);

    private static int Number(SqlDataReader reader, string name) =>
        reader[name] is DBNull ? 0 : Convert.ToInt32(reader[name]);

    private static string DisplayStatus(string value) => value.ToUpperInvariant() switch
    {
        "OUVERTE" => "Ouverte",
        "CLOTUREE" => "Clôturée",
        "ARCHIVEE" => "Archivée",
        "ACTIVE" => "Active",
        "ANNULEE" => "Annulée",
        "SUSPENDUE" => "Suspendue",
        "ACTIF" => "Actif",
        "INACTIF" => "Inactif",
        "DISPONIBLE" => "Disponible",
        "INDISPONIBLE" => "Indisponible",
        "PUBLIE" => "Publié",
        "BROUILLON" => "Brouillon",
        "ARCHIVE" => "Archivé",
        "PERMANENT" => "Permanent",
        "VACATAIRE" => "Vacataire",
        "EFFECTUE" => "Effectué",
        "EN_ATTENTE" => "En attente",
        "ANNULE" => "Annulé",
        _ => value
    };

    private static string SqlStatus(string value) => value.Trim().ToUpperInvariant() switch
    {
        "CLÔTURÉE" or "CLOTUREE" => "CLOTUREE",
        "ARCHIVÉE" or "ARCHIVEE" => "ARCHIVEE",
        "ANNULÉE" or "ANNULEE" => "ANNULEE",
        "INACTIVE" or "INACTIF" => "INACTIF",
        "INDISPONIBLE" or "EN MAINTENANCE" or "OCCUPÉE" or "OCCUPEE" => "INDISPONIBLE",
        "VACATAIRE" => "VACATAIRE",
        _ => value.Trim().ToUpperInvariant()
    };

    private static TimeSpan ParseTime(string value)
    {
        var cleaned = value.Trim().Replace('h', ':').Replace('H', ':');
        if (cleaned.EndsWith(':')) cleaned += "00";
        if (TimeSpan.TryParse(cleaned, out var time)) return time;
        throw new InvalidOperationException($"Heure invalide : '{value}'. Utilisez le format HH:mm.");
    }

    private static string FormatTime(object value) => value is TimeSpan time ? time.ToString(@"hh\:mm") : Convert.ToString(value) ?? "08:00";

    private static string UpperFirst(string value) => string.IsNullOrWhiteSpace(value) ? value : char.ToUpperInvariant(value[0]) + value[1..].ToLowerInvariant();
}
