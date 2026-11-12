using DataAccess;
using DAO.Models;

namespace DAO;

public static class DataSeeder
{
    public static void Initialize(IDataStore store)
    {
        var annees = store.GetCollection<AnneeAcademique>();
        if (annees.Count > 0)
            return; // Already initialized

        // 1. Années Académiques et Semestres
        var a1 = new AnneeAcademique
        {
            Id = 1,
            Code = "2026 - 2027",
            DateDebut = new DateTime(2026, 10, 1),
            DateFin = new DateTime(2027, 9, 30),
            Statut = "Ouverte",
            Semestres = new List<Semestre>
            {
                new() { Id = 1, AnneeAcademiqueId = 1, Nom = "Semestre 1", DateDebut = new DateTime(2026, 10, 1), DateFin = new DateTime(2027, 2, 28) },
                new() { Id = 2, AnneeAcademiqueId = 1, Nom = "Semestre 2", DateDebut = new DateTime(2027, 3, 1), DateFin = new DateTime(2027, 7, 31) }
            }
        };

        var a2 = new AnneeAcademique
        {
            Id = 2,
            Code = "2025 - 2026",
            DateDebut = new DateTime(2025, 10, 1),
            DateFin = new DateTime(2026, 9, 30),
            Statut = "Clôturée"
        };

        var a3 = new AnneeAcademique
        {
            Id = 3,
            Code = "2024 - 2025",
            DateDebut = new DateTime(2024, 10, 1),
            DateFin = new DateTime(2025, 9, 30),
            Statut = "Clôturée"
        };

        store.SaveCollection(new List<AnneeAcademique> { a1, a2, a3 });

        // 2. Filières
        var filieres = new List<Filiere>
        {
            new() { Id = 1, Nom = "Informatique", Description = "Développement logiciel, réseaux et systèmes...", Statut = "Active" },
            new() { Id = 2, Nom = "Gestion commerciale", Description = "Marketing, commerce international, vente...", Statut = "Active" },
            new() { Id = 3, Nom = "Finance comptabilité", Description = "Audit, fiscalité, gestion financière...", Statut = "Active" },
            new() { Id = 4, Nom = "Droit", Description = "Droit des affaires, droit privé et public...", Statut = "Active" }
        };
        store.SaveCollection(filieres);

        // 3. Niveaux
        var niveaux = new List<Niveau>
        {
            new() { Id = 1, Code = "L1", Cycle = "Licence" },
            new() { Id = 2, Code = "L2", Cycle = "Licence" },
            new() { Id = 3, Code = "L3", Cycle = "Licence" },
            new() { Id = 4, Code = "M1", Cycle = "Master" },
            new() { Id = 5, Code = "M2", Cycle = "Master" }
        };
        store.SaveCollection(niveaux);

        // 4. Classes
        var classes = new List<Classe>
        {
            new() { Id = 1, Code = "Info L2 A", FiliereId = 1, NiveauId = 2, AnneeAcademiqueId = 1, Groupe = "A", Details = "Informatique - L2 - A", FiliereNom = "Informatique", NiveauCode = "L2", AnneeCode = "2026 - 2027" },
            new() { Id = 2, Code = "Info L2 B", FiliereId = 1, NiveauId = 2, AnneeAcademiqueId = 1, Groupe = "B", Details = "Informatique - L2 - B", FiliereNom = "Informatique", NiveauCode = "L2", AnneeCode = "2026 - 2027" },
            new() { Id = 3, Code = "GC L1 A", FiliereId = 2, NiveauId = 1, AnneeAcademiqueId = 1, Groupe = "A", Details = "Gestion - L1 - A", FiliereNom = "Gestion commerciale", NiveauCode = "L1", AnneeCode = "2026 - 2027" },
            new() { Id = 4, Code = "FC L3 A", FiliereId = 3, NiveauId = 3, AnneeAcademiqueId = 1, Groupe = "A", Details = "Finance - L3 - A", FiliereNom = "Finance comptabilité", NiveauCode = "L3", AnneeCode = "2026 - 2027" },
            new() { Id = 5, Code = "DRT M1 A", FiliereId = 4, NiveauId = 4, AnneeAcademiqueId = 1, Groupe = "A", Details = "Droit - M1 - A", FiliereNom = "Droit", NiveauCode = "M1", AnneeCode = "2026 - 2027" },
            new() { Id = 6, Code = "Info L1 A", FiliereId = 1, NiveauId = 1, AnneeAcademiqueId = 1, Groupe = "A", Details = "Informatique - L1 - A", FiliereNom = "Informatique", NiveauCode = "L1", AnneeCode = "2026 - 2027" },
            new() { Id = 7, Code = "Info L3 A", FiliereId = 1, NiveauId = 3, AnneeAcademiqueId = 1, Groupe = "A", Details = "Informatique - L3 - A", FiliereNom = "Informatique", NiveauCode = "L3", AnneeCode = "2026 - 2027" },
            new() { Id = 8, Code = "Info M1 A", FiliereId = 1, NiveauId = 4, AnneeAcademiqueId = 1, Groupe = "A", Details = "Informatique - M1 - A", FiliereNom = "Informatique", NiveauCode = "M1", AnneeCode = "2026 - 2027" },
            new() { Id = 9, Code = "Info M2 A", FiliereId = 1, NiveauId = 5, AnneeAcademiqueId = 1, Groupe = "A", Details = "Informatique - M2 - A", FiliereNom = "Informatique", NiveauCode = "M2", AnneeCode = "2026 - 2027" },
            new() { Id = 10, Code = "GC L2 A", FiliereId = 2, NiveauId = 2, AnneeAcademiqueId = 1, Groupe = "A", Details = "Gestion - L2 - A", FiliereNom = "Gestion commerciale", NiveauCode = "L2", AnneeCode = "2026 - 2027" },
            new() { Id = 11, Code = "FC L1 A", FiliereId = 3, NiveauId = 1, AnneeAcademiqueId = 1, Groupe = "A", Details = "Finance - L1 - A", FiliereNom = "Finance comptabilité", NiveauCode = "L1", AnneeCode = "2026 - 2027" },
            new() { Id = 12, Code = "DRT L1 A", FiliereId = 4, NiveauId = 1, AnneeAcademiqueId = 1, Groupe = "A", Details = "Droit - L1 - A", FiliereNom = "Droit", NiveauCode = "L1", AnneeCode = "2026 - 2027" }
        };
        store.SaveCollection(classes);

        // 5. Étudiants
        var etudiants = new List<Etudiant>
        {
            new()
            {
                Id = 1,
                Matricule = "IIT2026001",
                Nom = "AHIBO",
                Prenoms = "Phanuel",
                Telephone = "0707070707",
                Email = "phanuel.ahibo@iit.ci",
                Adresse = "Abidjan, Côte d'Ivoire",
                ClasseActuelle = "Info L2 A",
                Statut = "Actif",
                Pieces = new List<PieceAdministrative>
                {
                    new() { Id = 1, EtudiantId = 1, TypePiece = "Extrait de naissance", EstValidee = true },
                    new() { Id = 2, EtudiantId = 1, TypePiece = "Diplôme du Baccalauréat", EstValidee = true },
                    new() { Id = 3, EtudiantId = 1, TypePiece = "Photo d'identité", EstValidee = true },
                    new() { Id = 4, EtudiantId = 1, TypePiece = "Certificat de nationalité", EstValidee = false }
                }
            },
            new()
            {
                Id = 2,
                Matricule = "IIT2026002",
                Nom = "KOFFI",
                Prenoms = "Marie",
                Telephone = "0505050505",
                Email = "marie.koffi@iit.ci",
                Adresse = "Cocody, Abidjan",
                ClasseActuelle = "Info L2 A",
                Statut = "Actif"
            },
            new()
            {
                Id = 3,
                Matricule = "IIT2026003",
                Nom = "DIALLO",
                Prenoms = "Moussa",
                Telephone = "0101010101",
                Email = "moussa.diallo@iit.ci",
                Adresse = "Marcory, Abidjan",
                ClasseActuelle = "GC L1 A",
                Statut = "Actif"
            }
        };
        store.SaveCollection(etudiants);

        // 6. Inscriptions & Paiements
        var ins1 = new Inscription
        {
            Id = 1,
            EtudiantId = 1,
            ClasseId = 1,
            AnneeAcademiqueId = 1,
            DateInscription = new DateTime(2026, 10, 10),
            MontantTotal = 300000m,
            MontantPaye = 250000m,
            Statut = "Active",
            EtudiantMatricule = "IIT2026001",
            EtudiantNomComplet = "AHIBO Phanuel",
            ClasseCode = "Info L2 A",
            AnneeCode = "2026 - 2027",
            Paiements = new List<Paiement>
            {
                new()
                {
                    Id = 1,
                    InscriptionId = 1,
                    DatePaiement = new DateTime(2026, 10, 10),
                    Montant = 250000m,
                    ModePaiement = "Virement",
                    Reference = "REF2026001",
                    Remarque = "Acompte d'inscription"
                }
            }
        };

        var ins2 = new Inscription
        {
            Id = 2,
            EtudiantId = 2,
            ClasseId = 1,
            AnneeAcademiqueId = 1,
            DateInscription = new DateTime(2026, 10, 12),
            MontantTotal = 300000m,
            MontantPaye = 300000m,
            Statut = "Active",
            EtudiantMatricule = "IIT2026002",
            EtudiantNomComplet = "KOFFI Marie",
            ClasseCode = "Info L2 A",
            AnneeCode = "2026 - 2027",
            Paiements = new List<Paiement>
            {
                new()
                {
                    Id = 2,
                    InscriptionId = 2,
                    DatePaiement = new DateTime(2026, 10, 12),
                    Montant = 300000m,
                    ModePaiement = "Chèque",
                    Reference = "CHQ2026012",
                    Remarque = "Paiement intégral"
                }
            }
        };

        store.SaveCollection(new List<Inscription> { ins1, ins2 });

        // 7. Enseignants
        var enseignants = new List<Enseignant>
        {
            new() { Id = 1, Nom = "KOUASSI", Prenoms = "Jean-Pierre", Specialite = "Info", TypeEnseignant = "Permanent", Statut = "Actif", Email = "jp.kouassi@iit.ci", Telephone = "0701020304" },
            new() { Id = 2, Nom = "TRAORÉ", Prenoms = "Fatoumata", Specialite = "Maths", TypeEnseignant = "Permanent", Statut = "Actif", Email = "f.traore@iit.ci", Telephone = "0501020304" },
            new() { Id = 3, Nom = "DIOP", Prenoms = "Samuel", Specialite = "Physique", TypeEnseignant = "Vacataire", Statut = "Actif", Email = "s.diop@iit.ci", Telephone = "0101020304" }
        };
        store.SaveCollection(enseignants);

        // 8. Cours
        var cours = new List<Cours>
        {
            new() { Id = 1, Code = "INF101", Intitule = "Programmation", Description = "Bases de la programmation C# et POO", VolumeHoraire = 45 },
            new() { Id = 2, Code = "INF102", Intitule = "Réseaux", Description = "Architecture réseau et routage IP", VolumeHoraire = 45 },
            new() { Id = 3, Code = "MTH101", Intitule = "Analyse", Description = "Calcul d'intégrales et algèbre linéaire", VolumeHoraire = 60 },
            new() { Id = 4, Code = "INF103", Intitule = "Bases de données", Description = "Modélisation SQL et SGBD relationnels", VolumeHoraire = 45 }
        };
        store.SaveCollection(cours);

        // 9. Salles
        var salles = new List<Salle>
        {
            new() { Id = 1, Nom = "S101", Capacite = 80, Localisation = "Bâtiment A", Statut = "Disponible" },
            new() { Id = 2, Nom = "S102", Capacite = 70, Localisation = "Bâtiment A", Statut = "Disponible" },
            new() { Id = 3, Nom = "S201", Capacite = 100, Localisation = "Bâtiment B", Statut = "En maintenance" }
        };
        store.SaveCollection(salles);

        // 10. Emplois du temps
        var seances = new List<SeanceEmploiDuTemps>
        {
            new()
            {
                Id = 1,
                ClasseId = 1,
                SemestreId = 1,
                CoursId = 1,
                EnseignantId = 1,
                SalleId = 1,
                Jour = "Lundi",
                HeureDebut = "08h-10h",
                HeureFin = "10h",
                ClasseCode = "Info L2 A",
                SemestreNom = "Semestre 1",
                CoursIntitule = "Programmation",
                EnseignantNom = "KOUASSI Jean-Pierre",
                SalleNom = "S101"
            },
            new()
            {
                Id = 2,
                ClasseId = 1,
                SemestreId = 1,
                CoursId = 2,
                EnseignantId = 3,
                SalleId = 2,
                Jour = "Lundi",
                HeureDebut = "10h-12h",
                HeureFin = "12h",
                ClasseCode = "Info L2 A",
                SemestreNom = "Semestre 1",
                CoursIntitule = "Réseaux",
                EnseignantNom = "DIOP Samuel",
                SalleNom = "S102"
            },
            new()
            {
                Id = 3,
                ClasseId = 1,
                SemestreId = 1,
                CoursId = 4,
                EnseignantId = 2,
                SalleId = 3,
                Jour = "Mardi",
                HeureDebut = "08h-10h",
                HeureFin = "10h",
                ClasseCode = "Info L2 A",
                SemestreNom = "Semestre 1",
                CoursIntitule = "Bases de données",
                EnseignantNom = "TRAORÉ Fatoumata",
                SalleNom = "S201"
            }
        };
        store.SaveCollection(seances);
    }
}
