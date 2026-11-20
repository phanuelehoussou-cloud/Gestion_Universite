using DAO.Models;
using Microsoft.Data.SqlClient;

namespace DAO.Repositories;

public sealed partial class SqlServerDataStore
{
    private static List<SeanceEmploiDuTemps> GetSeances() => ReadList(
        "SELECT s.id_seance,e.id_classe,e.id_semestre,s.id_cours,s.id_enseignant,s.id_salle,s.jour,s.heure_debut,s.heure_fin,s.date_seance " +
        "FROM dbo.SEANCE s JOIN dbo.EMPLOI_DU_TEMPS e ON e.id_emploi=s.id_emploi ORDER BY s.date_seance,s.heure_debut;",
        r => new SeanceEmploiDuTemps
        {
            Id = Number(r, "id_seance"), ClasseId = Number(r, "id_classe"), SemestreId = Number(r, "id_semestre"),
            CoursId = Number(r, "id_cours"), EnseignantId = Number(r, "id_enseignant"), SalleId = Number(r, "id_salle"),
            Jour = UpperFirst(Value(r, "jour")), HeureDebut = FormatTime(r["heure_debut"]), HeureFin = FormatTime(r["heure_fin"]),
            DateSeance = Date(r, "date_seance")
        });

    private static List<Absence> GetAbsences() => ReadList(
        "SELECT id_absence,id_etudiant,id_seance,date_absence,statut_justification,motif FROM dbo.ABSENCE ORDER BY date_absence DESC;",
        r => new Absence
        {
            Id = Number(r, "id_absence"), EtudiantId = Number(r, "id_etudiant"), SeanceId = Number(r, "id_seance"),
            DateAbsence = Date(r, "date_absence"), Justifiee = Value(r, "statut_justification").Equals("JUSTIFIEE", StringComparison.OrdinalIgnoreCase),
            Motif = Value(r, "motif")
        });

    private static void SaveSeances(SqlConnection connection, SqlTransaction transaction, List<SeanceEmploiDuTemps> items) =>
        Sync(connection, transaction, "SEANCE", "id_seance", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) => SaveSeance(connection, transaction, x, id),
            id => { using var c = new SqlCommand("EXEC dbo.usp_Seance_Delete @id_seance;", connection, transaction); Add(c, "@id_seance", id); c.ExecuteNonQuery(); });

    private static int SaveSeance(SqlConnection connection, SqlTransaction transaction, SeanceEmploiDuTemps item, int id)
    {
        var emploiId = GetOrCreateEmploi(connection, transaction, item.ClasseId, item.SemestreId);
        var day = item.Jour.Trim().ToUpperInvariant();
        var start = ParseTime(item.HeureDebut);
        var end = ParseTime(item.HeureFin);
        if (end <= start) throw new InvalidOperationException("L'heure de fin doit être postérieure à l'heure de début.");
        var date = item.DateSeance == DateTime.MinValue ? DateTime.Today : item.DateSeance.Date;

        if (id == 0)
        {
            using var command = new SqlCommand("dbo.usp_Seance_Insert", connection, transaction) { CommandType = System.Data.CommandType.StoredProcedure };
            Add(command, "@id_emploi", emploiId); Add(command, "@id_cours", item.CoursId); Add(command, "@id_enseignant", item.EnseignantId);
            Add(command, "@id_salle", item.SalleId); Add(command, "@jour", day); Add(command, "@heure_debut", start); Add(command, "@heure_fin", end); Add(command, "@date_seance", date);
            var output = command.Parameters.Add("@id_seance", System.Data.SqlDbType.Int); output.Direction = System.Data.ParameterDirection.Output;
            command.ExecuteNonQuery();
            return Convert.ToInt32(output.Value);
        }

        using (var classConflict = new SqlCommand(
            "SELECT COUNT(*) FROM dbo.SEANCE s JOIN dbo.EMPLOI_DU_TEMPS e ON e.id_emploi=s.id_emploi " +
            "WHERE e.id_classe=@classe AND s.date_seance=@date AND s.heure_debut < @fin AND s.heure_fin > @debut AND s.id_seance<>@id;",
            connection, transaction))
        {
            Add(classConflict, "@classe", item.ClasseId); Add(classConflict, "@date", date);
            Add(classConflict, "@debut", start); Add(classConflict, "@fin", end); Add(classConflict, "@id", id);
            if (Convert.ToInt32(classConflict.ExecuteScalar()) > 0)
                throw new InvalidOperationException("Cette classe a déjà une séance sur cette plage horaire.");
        }

        using (var command = new SqlCommand("dbo.usp_Seance_Update", connection, transaction) { CommandType = System.Data.CommandType.StoredProcedure })
        {
            Add(command, "@id_seance", id); Add(command, "@id_cours", item.CoursId); Add(command, "@id_enseignant", item.EnseignantId);
            Add(command, "@id_salle", item.SalleId); Add(command, "@jour", day); Add(command, "@heure_debut", start); Add(command, "@heure_fin", end); Add(command, "@date_seance", date);
            command.ExecuteNonQuery();
        }
        using (var command = new SqlCommand("UPDATE dbo.SEANCE SET id_emploi=@emploi WHERE id_seance=@id;", connection, transaction))
        { Add(command, "@emploi", emploiId); Add(command, "@id", id); command.ExecuteNonQuery(); }
        return id;
    }

    private static int GetOrCreateEmploi(SqlConnection connection, SqlTransaction transaction, int classeId, int semestreId)
    {
        using (var query = new SqlCommand("SELECT TOP (1) id_emploi FROM dbo.EMPLOI_DU_TEMPS WHERE id_classe=@classe AND id_semestre=@semestre ORDER BY id_emploi DESC;", connection, transaction))
        {
            Add(query, "@classe", classeId); Add(query, "@semestre", semestreId);
            var existing = query.ExecuteScalar();
            if (existing != null && existing is not DBNull) return Convert.ToInt32(existing);
        }
        using var create = new SqlCommand("dbo.usp_EmploiDuTemps_Insert", connection, transaction) { CommandType = System.Data.CommandType.StoredProcedure };
        Add(create, "@id_classe", classeId); Add(create, "@id_semestre", semestreId);
        var output = create.Parameters.Add("@id_emploi", System.Data.SqlDbType.Int); output.Direction = System.Data.ParameterDirection.Output;
        create.ExecuteNonQuery();
        return Convert.ToInt32(output.Value);
    }

    private static void SaveAbsences(SqlConnection connection, SqlTransaction transaction, List<Absence> items) =>
        Sync(connection, transaction, "ABSENCE", "id_absence", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                var status = x.Justifiee ? "JUSTIFIEE" : "NON_JUSTIFIEE";
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.ABSENCE (id_etudiant,id_seance,date_absence,statut_justification,motif) OUTPUT INSERTED.id_absence VALUES (@etudiant,@seance,@date,@statut,@motif);", connection, transaction);
                    Add(c, "@etudiant", x.EtudiantId); Add(c, "@seance", x.SeanceId); Add(c, "@date", x.DateAbsence.Date); Add(c, "@statut", status); Add(c, "@motif", EmptyToNull(x.Motif)); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.ABSENCE SET id_etudiant=@etudiant,id_seance=@seance,date_absence=@date,statut_justification=@statut,motif=@motif WHERE id_absence=@id;", connection, transaction))
                { Add(c, "@id", id); Add(c, "@etudiant", x.EtudiantId); Add(c, "@seance", x.SeanceId); Add(c, "@date", x.DateAbsence.Date); Add(c, "@statut", status); Add(c, "@motif", EmptyToNull(x.Motif)); c.ExecuteNonQuery(); }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Absence_Delete @id_absence;", connection, transaction); Add(c, "@id_absence", id); c.ExecuteNonQuery(); });
}
