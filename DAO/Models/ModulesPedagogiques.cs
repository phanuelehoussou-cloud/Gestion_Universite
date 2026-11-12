namespace DAO.Models;

public class Enseignant
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenoms { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty; // "Informatique", "Maths", "Physique", etc.
    public string TypeEnseignant { get; set; } = "Permanent"; // "Permanent", "Vacataire"
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Statut { get; set; } = "Actif"; // "Actif", "Inactif"

    public string NomComplet => $"{Nom} {Prenoms}".Trim();

    public override string ToString() => NomComplet;
}

public class Cours
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // "INF101", "INF102", "MTH101"
    public string Intitule { get; set; } = string.Empty; // "Programmation", "Réseaux", "Analyse"
    public string Description { get; set; } = string.Empty;
    public int VolumeHoraire { get; set; } = 45; // in hours, e.g. 45 h

    public override string ToString() => $"{Code} - {Intitule}";
}

public class Salle
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty; // "S101", "S102", "S201"
    public int Capacite { get; set; } = 80;
    public string Localisation { get; set; } = "Bâtiment A"; // "Bâtiment A", "Bâtiment B"
    public string Statut { get; set; } = "Disponible"; // "Disponible", "En maintenance"

    public override string ToString() => $"{Nom} ({Capacite} places - {Localisation})";
}

public class SeanceEmploiDuTemps
{
    public int Id { get; set; }
    public int ClasseId { get; set; }
    public int SemestreId { get; set; }
    public int CoursId { get; set; }
    public int EnseignantId { get; set; }
    public int SalleId { get; set; }

    public string Jour { get; set; } = "Lundi"; // "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"
    public string HeureDebut { get; set; } = "08h";
    public string HeureFin { get; set; } = "10h";

    // Navigation properties for display
    public string? ClasseCode { get; set; }
    public string? SemestreNom { get; set; }
    public string? CoursIntitule { get; set; }
    public string? EnseignantNom { get; set; }
    public string? SalleNom { get; set; }

    public string Creneau => $"{HeureDebut}-{HeureFin}";

    public override string ToString() => $"{Jour} {Creneau} - {CoursIntitule} ({SalleNom})";
}
