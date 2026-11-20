using DAO.Models;
using Microsoft.Data.SqlClient;

namespace DAO.Repositories;

public sealed partial class SqlServerDataStore
{
    private static List<Enseignant> GetEnseignants() => ReadList(
        "SELECT id_enseignant,matricule_enseignant,nom,prenom,specialite,type_enseignant,telephone,email,statut FROM dbo.ENSEIGNANT ORDER BY nom,prenom;",
        r => new Enseignant
        {
            Id = Number(r, "id_enseignant"), Matricule = Value(r, "matricule_enseignant"), Nom = Value(r, "nom"),
            Prenoms = Value(r, "prenom"), Specialite = Value(r, "specialite"), TypeEnseignant = DisplayStatus(Value(r, "type_enseignant")),
            Telephone = Value(r, "telephone"), Email = Value(r, "email"), Statut = DisplayStatus(Value(r, "statut"))
        });

    private static List<Cours> GetCours() => ReadList(
        "SELECT id_cours,code_cours,nom_cours,description,volume_horaire FROM dbo.COURS ORDER BY nom_cours;",
        r => new Cours
        {
            Id = Number(r, "id_cours"), Code = Value(r, "code_cours"), Intitule = Value(r, "nom_cours"),
            Description = Value(r, "description"), VolumeHoraire = Number(r, "volume_horaire")
        });

    private static List<Salle> GetSalles() => ReadList(
        "SELECT id_salle,nom_salle,capacite,localisation,statut FROM dbo.SALLE ORDER BY nom_salle;",
        r => new Salle
        {
            Id = Number(r, "id_salle"), Nom = Value(r, "nom_salle"), Capacite = Number(r, "capacite"),
            Localisation = Value(r, "localisation"), Statut = DisplayStatus(Value(r, "statut"))
        });

    private static void SaveEnseignants(SqlConnection connection, SqlTransaction transaction, List<Enseignant> items) =>
        Sync(connection, transaction, "ENSEIGNANT", "id_enseignant", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (string.IsNullOrWhiteSpace(x.Matricule)) x.Matricule = "ENS-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.ENSEIGNANT (matricule_enseignant,nom,prenom,specialite,telephone,email,type_enseignant,statut) OUTPUT INSERTED.id_enseignant VALUES (@matricule,@nom,@prenom,@specialite,@telephone,@email,@type,@statut);", connection, transaction);
                    AddTeacherParameters(c, x); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.ENSEIGNANT SET matricule_enseignant=@matricule,nom=@nom,prenom=@prenom,specialite=@specialite,telephone=@telephone,email=@email,type_enseignant=@type,statut=@statut WHERE id_enseignant=@id;", connection, transaction))
                {
                    AddTeacherParameters(c, x); Add(c, "@id", id); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Enseignant_Delete @id_enseignant;", connection, transaction); Add(c, "@id_enseignant", id); c.ExecuteNonQuery(); });

    private static void AddTeacherParameters(SqlCommand command, Enseignant x)
    {
        Add(command, "@matricule", x.Matricule); Add(command, "@nom", x.Nom); Add(command, "@prenom", x.Prenoms);
        Add(command, "@specialite", EmptyToNull(x.Specialite)); Add(command, "@telephone", EmptyToNull(x.Telephone));
        Add(command, "@email", EmptyToNull(x.Email)); Add(command, "@type", SqlStatus(x.TypeEnseignant)); Add(command, "@statut", SqlStatus(x.Statut));
    }

    private static void SaveCours(SqlConnection connection, SqlTransaction transaction, List<Cours> items) =>
        Sync(connection, transaction, "COURS", "id_cours", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.COURS (code_cours,nom_cours,description,volume_horaire) OUTPUT INSERTED.id_cours VALUES (@code,@nom,@description,@volume);", connection, transaction);
                    AddCourseParameters(c, x); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.COURS SET code_cours=@code,nom_cours=@nom,description=@description,volume_horaire=@volume WHERE id_cours=@id;", connection, transaction))
                { AddCourseParameters(c, x); Add(c, "@id", id); c.ExecuteNonQuery(); }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Cours_Delete @id_cours;", connection, transaction); Add(c, "@id_cours", id); c.ExecuteNonQuery(); });

    private static void AddCourseParameters(SqlCommand command, Cours x)
    {
        Add(command, "@code", x.Code); Add(command, "@nom", x.Intitule); Add(command, "@description", EmptyToNull(x.Description)); Add(command, "@volume", x.VolumeHoraire);
    }

    private static void SaveSalles(SqlConnection connection, SqlTransaction transaction, List<Salle> items) =>
        Sync(connection, transaction, "SALLE", "id_salle", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.SALLE (nom_salle,capacite,localisation,statut) OUTPUT INSERTED.id_salle VALUES (@nom,@capacite,@localisation,@statut);", connection, transaction);
                    AddRoomParameters(c, x); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.SALLE SET nom_salle=@nom,capacite=@capacite,localisation=@localisation,statut=@statut WHERE id_salle=@id;", connection, transaction))
                { AddRoomParameters(c, x); Add(c, "@id", id); c.ExecuteNonQuery(); }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Salle_Delete @id_salle;", connection, transaction); Add(c, "@id_salle", id); c.ExecuteNonQuery(); });

    private static void AddRoomParameters(SqlCommand command, Salle x)
    {
        Add(command, "@nom", x.Nom); Add(command, "@capacite", x.Capacite); Add(command, "@localisation", EmptyToNull(x.Localisation));
        Add(command, "@statut", string.Equals(x.Statut, "Disponible", StringComparison.OrdinalIgnoreCase) ? "DISPONIBLE" : "INDISPONIBLE");
    }
}
