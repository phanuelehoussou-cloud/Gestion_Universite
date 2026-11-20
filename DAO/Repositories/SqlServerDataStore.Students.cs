using DAO.Models;
using Microsoft.Data.SqlClient;

namespace DAO.Repositories;

public sealed partial class SqlServerDataStore
{
    private static List<Etudiant> GetEtudiants()
    {
        var students = ReadList(
            "SELECT e.id_etudiant,e.matricule,e.nom,e.prenom,e.date_naissance,e.sexe,e.nationalite,e.telephone,e.email,e.adresse, " +
            "(SELECT TOP (1) c.nom_classe FROM dbo.INSCRIPTION i JOIN dbo.CLASSE c ON c.id_classe=i.id_classe WHERE i.id_etudiant=e.id_etudiant AND i.statut='ACTIVE' ORDER BY i.date_inscription DESC) AS classe_actuelle " +
            "FROM dbo.ETUDIANT e ORDER BY e.nom,e.prenom;",
            r => new Etudiant
            {
                Id = Number(r, "id_etudiant"), Matricule = Value(r, "matricule"), Nom = Value(r, "nom"), Prenoms = Value(r, "prenom"),
                DateNaissance = Date(r, "date_naissance"), Sexe = Value(r, "sexe"), Nationalite = Value(r, "nationalite"),
                Telephone = Value(r, "telephone"), Email = Value(r, "email"), Adresse = Value(r, "adresse"),
                ClasseActuelle = Value(r, "classe_actuelle")
            });
        var pieces = ReadList(
            "SELECT id_piece,id_etudiant,type_piece,chemin_fichier,date_depot,statut_validation FROM dbo.PIECE_ADMINISTRATIVE ORDER BY date_depot DESC;",
            r => new PieceAdministrative
            {
                Id = Number(r, "id_piece"), EtudiantId = Number(r, "id_etudiant"), TypePiece = Value(r, "type_piece"),
                FichierPath = Value(r, "chemin_fichier"), DateDepot = Date(r, "date_depot"),
                EstValidee = Value(r, "statut_validation").Equals("VALIDEE", StringComparison.OrdinalIgnoreCase)
            });
        foreach (var student in students) student.Pieces = pieces.Where(p => p.EtudiantId == student.Id).ToList();
        return students;
    }

    private static List<Inscription> GetInscriptions()
    {
        var registrations = ReadList(
            "SELECT id_inscription,id_etudiant,id_classe,id_annee,date_inscription,type_inscription,statut,montant_total FROM dbo.INSCRIPTION ORDER BY date_inscription DESC;",
            r => new Inscription
            {
                Id = Number(r, "id_inscription"), EtudiantId = Number(r, "id_etudiant"), ClasseId = Number(r, "id_classe"),
                AnneeAcademiqueId = Number(r, "id_annee"), DateInscription = Date(r, "date_inscription"),
                TypeInscription = Value(r, "type_inscription"), Statut = DisplayStatus(Value(r, "statut")),
                MontantTotal = Convert.ToDecimal(r["montant_total"])
            });
        var payments = ReadList(
            "SELECT id_paiement,id_inscription,montant,date_paiement,mode_paiement,reference_paiement,statut FROM dbo.PAIEMENT ORDER BY date_paiement;",
            r => new Paiement
            {
                Id = Number(r, "id_paiement"), InscriptionId = Number(r, "id_inscription"), Montant = Convert.ToDecimal(r["montant"]),
                DatePaiement = Date(r, "date_paiement"), ModePaiement = DisplayPaymentMode(Value(r, "mode_paiement")),
                Reference = Value(r, "reference_paiement"), Statut = DisplayStatus(Value(r, "statut"))
            });
        foreach (var registration in registrations)
            registration.Paiements = payments.Where(p => p.InscriptionId == registration.Id).ToList();
        return registrations;
    }

    private static string DisplayPaymentMode(string value) => value.ToUpperInvariant() switch
    {
        "ESPECES" => "Espèces",
        "MOBILE_MONEY" => "Mobile Money",
        "VIREMENT" => "Virement",
        "CHEQUE" => "Chèque",
        _ => value
    };

    private static string SqlPaymentMode(string value) => value.Trim().ToUpperInvariant() switch
    {
        "ESPÈCES" or "ESPECES" => "ESPECES",
        "MOBILE MONEY" or "MOBILE_MONEY" => "MOBILE_MONEY",
        "CHÈQUE" or "CHEQUE" => "CHEQUE",
        _ => "VIREMENT"
    };

    private static void SaveEtudiants(SqlConnection connection, SqlTransaction transaction, List<Etudiant> items)
    {
        var existing = ExistingIds(connection, transaction, "ETUDIANT", "id_etudiant");
        var desired = new HashSet<int>();
        var pieces = new List<PieceAdministrative>();
        foreach (var item in items)
        {
            if (string.IsNullOrWhiteSpace(item.Matricule)) item.Matricule = $"IIT{DateTime.Today.Year}{Guid.NewGuid():N}"[..19].ToUpperInvariant();
            item.DateNaissance = item.DateNaissance == DateTime.MinValue ? DateTime.Today.AddYears(-18) : item.DateNaissance;
            var id = item.Id > 0 && existing.Contains(item.Id) ? item.Id : 0;
            item.Id = id == 0 ? InsertStudent(connection, transaction, item) : UpdateStudent(connection, transaction, item, id);
            desired.Add(item.Id);
            foreach (var piece in item.Pieces)
            {
                piece.EtudiantId = item.Id;
                pieces.Add(piece);
            }
        }

        // Synchronize child rows once across all students; per-student Sync would delete other students' documents.
        SavePieces(connection, transaction, pieces);
        foreach (var removedId in existing.Except(desired))
        {
            using var command = new SqlCommand("EXEC dbo.usp_Etudiant_Delete @id_etudiant;", connection, transaction);
            Add(command, "@id_etudiant", removedId);
            command.ExecuteNonQuery();
        }
    }

    private static int InsertStudent(SqlConnection connection, SqlTransaction transaction, Etudiant x)
    {
        using var command = new SqlCommand(
            "INSERT dbo.ETUDIANT (matricule,nom,prenom,date_naissance,sexe,nationalite,telephone,email,adresse) " +
            "OUTPUT INSERTED.id_etudiant VALUES (@matricule,@nom,@prenom,@naissance,@sexe,@nationalite,@telephone,@email,@adresse);", connection, transaction);
        AddStudentParameters(command, x);
        return SaveIdentity(command);
    }

    private static int UpdateStudent(SqlConnection connection, SqlTransaction transaction, Etudiant x, int id)
    {
        using var command = new SqlCommand(
            "UPDATE dbo.ETUDIANT SET matricule=@matricule,nom=@nom,prenom=@prenom,date_naissance=@naissance,sexe=@sexe,nationalite=@nationalite,telephone=@telephone,email=@email,adresse=@adresse WHERE id_etudiant=@id;", connection, transaction);
        AddStudentParameters(command, x); Add(command, "@id", id); command.ExecuteNonQuery();
        return id;
    }

    private static void AddStudentParameters(SqlCommand command, Etudiant x)
    {
        Add(command, "@matricule", x.Matricule); Add(command, "@nom", x.Nom); Add(command, "@prenom", x.Prenoms);
        Add(command, "@naissance", x.DateNaissance.Date); Add(command, "@sexe", x.Sexe.ToUpperInvariant());
        Add(command, "@nationalite", x.Nationalite); Add(command, "@telephone", EmptyToNull(x.Telephone));
        Add(command, "@email", EmptyToNull(x.Email)); Add(command, "@adresse", EmptyToNull(x.Adresse));
    }

    private static void SavePieces(SqlConnection connection, SqlTransaction transaction, List<PieceAdministrative> items) =>
        Sync(connection, transaction, "PIECE_ADMINISTRATIVE", "id_piece", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                var status = x.EstValidee ? "VALIDEE" : "EN_ATTENTE";
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.PIECE_ADMINISTRATIVE (id_etudiant,type_piece,chemin_fichier,statut_validation) OUTPUT INSERTED.id_piece VALUES (@etudiant,@type,@path,@status);", connection, transaction);
                    Add(c, "@etudiant", x.EtudiantId); Add(c, "@type", x.TypePiece); Add(c, "@path", x.FichierPath); Add(c, "@status", status); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.PIECE_ADMINISTRATIVE SET id_etudiant=@etudiant,type_piece=@type,chemin_fichier=@path,statut_validation=@status WHERE id_piece=@id;", connection, transaction))
                {
                    Add(c, "@id", id); Add(c, "@etudiant", x.EtudiantId); Add(c, "@type", x.TypePiece); Add(c, "@path", x.FichierPath); Add(c, "@status", status); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_PieceAdministrative_Delete @id_piece;", connection, transaction); Add(c, "@id_piece", id); c.ExecuteNonQuery(); });

    private static void SaveInscriptions(SqlConnection connection, SqlTransaction transaction, List<Inscription> items)
    {
        var existing = ExistingIds(connection, transaction, "INSCRIPTION", "id_inscription");
        var desired = new HashSet<int>();
        var payments = new List<Paiement>();
        foreach (var item in items)
        {
            var id = item.Id > 0 && existing.Contains(item.Id) ? item.Id : 0;
            item.Id = id == 0 ? InsertInscription(connection, transaction, item) : UpdateInscription(connection, transaction, item, id);
            desired.Add(item.Id);
            foreach (var payment in item.Paiements)
            {
                payment.InscriptionId = item.Id;
                payments.Add(payment);
            }
        }
        SavePayments(connection, transaction, payments);
        foreach (var removedId in existing.Except(desired))
        {
            using var command = new SqlCommand("EXEC dbo.usp_Inscription_Delete @id_inscription;", connection, transaction);
            Add(command, "@id_inscription", removedId);
            command.ExecuteNonQuery();
        }
    }

    private static int InsertInscription(SqlConnection connection, SqlTransaction transaction, Inscription x)
    {
        using var command = new SqlCommand(
            "INSERT dbo.INSCRIPTION (id_etudiant,id_classe,id_annee,date_inscription,type_inscription,statut,montant_total) " +
            "OUTPUT INSERTED.id_inscription VALUES (@etudiant,@classe,@annee,@date,@type,@statut,@montant);", connection, transaction);
        AddInscriptionParameters(command, x);
        return SaveIdentity(command);
    }

    private static int UpdateInscription(SqlConnection connection, SqlTransaction transaction, Inscription x, int id)
    {
        using var command = new SqlCommand(
            "UPDATE dbo.INSCRIPTION SET id_etudiant=@etudiant,id_classe=@classe,id_annee=@annee,date_inscription=@date,type_inscription=@type,statut=@statut,montant_total=@montant WHERE id_inscription=@id;", connection, transaction);
        AddInscriptionParameters(command, x); Add(command, "@id", id); command.ExecuteNonQuery();
        return id;
    }

    private static void AddInscriptionParameters(SqlCommand command, Inscription x)
    {
        Add(command, "@etudiant", x.EtudiantId); Add(command, "@classe", x.ClasseId); Add(command, "@annee", x.AnneeAcademiqueId);
        Add(command, "@date", x.DateInscription); Add(command, "@type", string.IsNullOrWhiteSpace(x.TypeInscription) ? "PREMIERE" : x.TypeInscription.ToUpperInvariant());
        Add(command, "@statut", SqlStatus(x.Statut)); Add(command, "@montant", x.MontantTotal);
    }

    private static void SavePayments(SqlConnection connection, SqlTransaction transaction, List<Paiement> items) =>
        Sync(connection, transaction, "PAIEMENT", "id_paiement", items, x => x.Id, (x, id) => x.Id = id,
            (x, id) =>
            {
                if (id == 0)
                {
                    using var c = new SqlCommand("INSERT dbo.PAIEMENT (id_inscription,montant,date_paiement,mode_paiement,reference_paiement,statut) OUTPUT INSERTED.id_paiement VALUES (@inscription,@montant,@date,@mode,@reference,@statut);", connection, transaction);
                    Add(c, "@inscription", x.InscriptionId); Add(c, "@montant", x.Montant); Add(c, "@date", x.DatePaiement); Add(c, "@mode", SqlPaymentMode(x.ModePaiement)); Add(c, "@reference", EmptyToNull(x.Reference)); Add(c, "@statut", SqlPaymentStatus(x.Statut)); return SaveIdentity(c);
                }
                using (var c = new SqlCommand("UPDATE dbo.PAIEMENT SET id_inscription=@inscription,montant=@montant,date_paiement=@date,mode_paiement=@mode,reference_paiement=@reference,statut=@statut WHERE id_paiement=@id;", connection, transaction))
                {
                    Add(c, "@id", id); Add(c, "@inscription", x.InscriptionId); Add(c, "@montant", x.Montant); Add(c, "@date", x.DatePaiement); Add(c, "@mode", SqlPaymentMode(x.ModePaiement)); Add(c, "@reference", EmptyToNull(x.Reference)); Add(c, "@statut", SqlPaymentStatus(x.Statut)); c.ExecuteNonQuery();
                }
                return id;
            },
            id => { using var c = new SqlCommand("EXEC dbo.usp_Paiement_Delete @id_paiement;", connection, transaction); Add(c, "@id_paiement", id); c.ExecuteNonQuery(); });

    private static object EmptyToNull(string? value) => string.IsNullOrWhiteSpace(value) ? DBNull.Value : value;

    private static string SqlPaymentStatus(string value) => value.Trim().ToUpperInvariant() switch
    {
        "EN ATTENTE" or "EN_ATTENTE" => "EN_ATTENTE",
        "ANNULÉ" or "ANNULE" or "ANNULÉE" or "ANNULEE" => "ANNULE",
        _ => "EFFECTUE"
    };
}
