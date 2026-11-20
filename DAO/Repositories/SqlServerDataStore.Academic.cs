using DAO.Models;
using Microsoft.Data.SqlClient;

namespace DAO.Repositories;

public sealed partial class SqlServerDataStore
{
    private static List<Semestre> GetSemestres() => ReadList(
        "SELECT id_semestre, libelle, id_annee, date_debut, date_fin FROM dbo.SEMESTRE ORDER BY date_debut;",
        r => new Semestre
        {
            Id = Number(r, "id_semestre"), Nom = Value(r, "libelle"), AnneeAcademiqueId = Number(r, "id_annee"),
            DateDebut = Date(r, "date_debut"), DateFin = Date(r, "date_fin")
        });

    private static List<AnneeAcademique> GetAnnees()
    {
        var annees = ReadList(
            "SELECT id_annee, libelle, date_debut, date_fin, statut FROM dbo.ANNEE_ACADEMIQUE ORDER BY date_debut DESC;",
            r => new AnneeAcademique
            {
                Id = Number(r, "id_annee"), Code = Value(r, "libelle"), DateDebut = Date(r, "date_debut"),
                DateFin = Date(r, "date_fin"), Statut = DisplayStatus(Value(r, "statut"))
            });
        var semestres = GetSemestres();
        foreach (var annee in annees)
            annee.Semestres = semestres.Where(s => s.AnneeAcademiqueId == annee.Id).OrderBy(s => s.DateDebut).ToList();
        return annees;
    }

    private static List<Filiere> GetFilieres() => ReadList(
        "SELECT id_filiere, code_filiere, nom_filiere, description, statut FROM dbo.FILIERE ORDER BY nom_filiere;",
        r => new Filiere
        {
            Id = Number(r, "id_filiere"), Code = Value(r, "code_filiere"), Nom = Value(r, "nom_filiere"),
            Description = Value(r, "description"), Statut = DisplayStatus(Value(r, "statut"))
        });

    private static List<Niveau> GetNiveaux() => ReadList(
        "SELECT id_niveau, code_niveau, cycle FROM dbo.NIVEAU ORDER BY code_niveau;",
        r => new Niveau
        {
            Id = Number(r, "id_niveau"), Code = Value(r, "code_niveau"), Cycle = DisplayStatus(Value(r, "cycle"))
        });

    private static List<Classe> GetClasses() => ReadList(
        "SELECT id_classe, nom_classe, id_filiere, id_niveau, id_annee, groupe FROM dbo.CLASSE ORDER BY nom_classe;",
        r => new Classe
        {
            Id = Number(r, "id_classe"), Code = Value(r, "nom_classe"), FiliereId = Number(r, "id_filiere"),
            NiveauId = Number(r, "id_niveau"), AnneeAcademiqueId = Number(r, "id_annee"), Groupe = Value(r, "groupe", "A")
        });

    private static void SaveAnnees(SqlConnection connection, SqlTransaction transaction, List<AnneeAcademique> items)
    {
        var existing = ExistingIds(connection, transaction, "ANNEE_ACADEMIQUE", "id_annee");
        var desired = new HashSet<int>();
        foreach (var item in items)
        {
            var id = SaveAnnee(connection, transaction, item, item.Id > 0 && existing.Contains(item.Id) ? item.Id : 0);
            item.Id = id;
            desired.Add(id);
            foreach (var semestre in item.Semestres)
                semestre.AnneeAcademiqueId = id;
        }

        SaveSemestres(connection, transaction, items.SelectMany(a => a.Semestres).ToList());
        foreach (var removedId in existing.Except(desired))
        {
            using var command = new SqlCommand("EXEC dbo.usp_AnneeAcademique_Delete @id_annee;", connection, transaction);
            Add(command, "@id_annee", removedId);
            command.ExecuteNonQuery();
        }
    }

    private static int SaveAnnee(SqlConnection connection, SqlTransaction transaction, AnneeAcademique item, int id)
    {
        if (id == 0)
        {
            using var command = new SqlCommand(
                "INSERT dbo.ANNEE_ACADEMIQUE (libelle, date_debut, date_fin, statut) OUTPUT INSERTED.id_annee VALUES (@libelle,@debut,@fin,@statut);",
                connection, transaction);
            Add(command, "@libelle", item.Code); Add(command, "@debut", item.DateDebut.Date); Add(command, "@fin", item.DateFin.Date);
            Add(command, "@statut", SqlStatus(item.Statut));
            return SaveIdentity(command);
        }
        using (var command = new SqlCommand("UPDATE dbo.ANNEE_ACADEMIQUE SET libelle=@libelle,date_debut=@debut,date_fin=@fin,statut=@statut WHERE id_annee=@id;", connection, transaction))
        {
            Add(command, "@id", id); Add(command, "@libelle", item.Code); Add(command, "@debut", item.DateDebut.Date);
            Add(command, "@fin", item.DateFin.Date); Add(command, "@statut", SqlStatus(item.Statut)); command.ExecuteNonQuery();
        }
        return id;
    }

    private static void SaveSemestres(SqlConnection connection, SqlTransaction transaction, List<Semestre> items) =>
        Sync(connection, transaction, "SEMESTRE", "id_semestre", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) => SaveSemestre(connection, transaction, x, id),
            id => { using var c = new SqlCommand("EXEC dbo.usp_Semestre_Delete @id_semestre;", connection, transaction); Add(c, "@id_semestre", id); c.ExecuteNonQuery(); });

    private static int SaveSemestre(SqlConnection connection, SqlTransaction transaction, Semestre item, int id)
    {
        if (id == 0)
        {
            using var command = new SqlCommand("INSERT dbo.SEMESTRE (libelle,id_annee,date_debut,date_fin) OUTPUT INSERTED.id_semestre VALUES (@libelle,@annee,@debut,@fin);", connection, transaction);
            Add(command, "@libelle", item.Nom); Add(command, "@annee", item.AnneeAcademiqueId); Add(command, "@debut", item.DateDebut.Date); Add(command, "@fin", item.DateFin.Date);
            return SaveIdentity(command);
        }
        using (var command = new SqlCommand("UPDATE dbo.SEMESTRE SET libelle=@libelle,id_annee=@annee,date_debut=@debut,date_fin=@fin WHERE id_semestre=@id;", connection, transaction))
        {
            Add(command, "@id", id); Add(command, "@libelle", item.Nom); Add(command, "@annee", item.AnneeAcademiqueId);
            Add(command, "@debut", item.DateDebut.Date); Add(command, "@fin", item.DateFin.Date); command.ExecuteNonQuery();
        }
        return id;
    }

    private static void SaveFilieres(SqlConnection connection, SqlTransaction transaction, List<Filiere> items) =>
        Sync(connection, transaction, "FILIERE", "id_filiere", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                var code = string.IsNullOrWhiteSpace(x.Code) ? "FIL-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant() : x.Code;
                x.Code = code;
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.FILIERE (code_filiere,nom_filiere,description,statut) OUTPUT INSERTED.id_filiere VALUES (@code,@nom,@description,@statut);", connection, transaction);
                    Add(c, "@code", code); Add(c, "@nom", x.Nom); Add(c, "@description", x.Description); Add(c, "@statut", SqlStatus(x.Statut)); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.FILIERE SET code_filiere=@code,nom_filiere=@nom,description=@description,statut=@statut WHERE id_filiere=@id;", connection, transaction))
                {
                    Add(c, "@id", id); Add(c, "@code", code); Add(c, "@nom", x.Nom); Add(c, "@description", x.Description); Add(c, "@statut", SqlStatus(x.Statut)); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Filiere_Delete @id_filiere;", connection, transaction); Add(c, "@id_filiere", id); c.ExecuteNonQuery(); });

    private static void SaveNiveaux(SqlConnection connection, SqlTransaction transaction, List<Niveau> items) =>
        Sync(connection, transaction, "NIVEAU", "id_niveau", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.NIVEAU (code_niveau,libelle,cycle) OUTPUT INSERTED.id_niveau VALUES (@code,@libelle,@cycle);", connection, transaction);
                    Add(c, "@code", x.Code); Add(c, "@libelle", x.Code); Add(c, "@cycle", SqlStatus(x.Cycle)); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.NIVEAU SET code_niveau=@code,libelle=@libelle,cycle=@cycle WHERE id_niveau=@id;", connection, transaction))
                {
                    Add(c, "@id", id); Add(c, "@code", x.Code); Add(c, "@libelle", x.Code); Add(c, "@cycle", SqlStatus(x.Cycle)); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Niveau_Delete @id_niveau;", connection, transaction); Add(c, "@id_niveau", id); c.ExecuteNonQuery(); });

    private static void SaveClasses(SqlConnection connection, SqlTransaction transaction, List<Classe> items) =>
        Sync(connection, transaction, "CLASSE", "id_classe", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.CLASSE (nom_classe,id_filiere,id_niveau,id_annee,groupe) OUTPUT INSERTED.id_classe VALUES (@nom,@filiere,@niveau,@annee,@groupe);", connection, transaction);
                    Add(c, "@nom", x.Code); Add(c, "@filiere", x.FiliereId); Add(c, "@niveau", x.NiveauId); Add(c, "@annee", x.AnneeAcademiqueId); Add(c, "@groupe", x.Groupe); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.CLASSE SET nom_classe=@nom,id_filiere=@filiere,id_niveau=@niveau,id_annee=@annee,groupe=@groupe WHERE id_classe=@id;", connection, transaction))
                {
                    Add(c, "@id", id); Add(c, "@nom", x.Code); Add(c, "@filiere", x.FiliereId); Add(c, "@niveau", x.NiveauId); Add(c, "@annee", x.AnneeAcademiqueId); Add(c, "@groupe", x.Groupe); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Classe_Delete @id_classe;", connection, transaction); Add(c, "@id_classe", id); c.ExecuteNonQuery(); });
}
