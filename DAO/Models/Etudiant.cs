namespace DAO.Models;

public class Etudiant
{
    public int Id { get; set; }
    public string Matricule { get; set; } = string.Empty; // e.g. "IIT2026001"
    public string Nom { get; set; } = string.Empty;
    public string Prenoms { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string? PhotoPath { get; set; }
    public string Statut { get; set; } = "Actif"; // "Actif", "Inactif"

    // Related data
    public string? ClasseActuelle { get; set; }
    public List<PieceAdministrative> Pieces { get; set; } = new();

    public string NomComplet => $"{Nom} {Prenoms}".Trim();

    public override string ToString() => $"{Matricule} - {NomComplet}";
}

public class PieceAdministrative
{
    public int Id { get; set; }
    public int EtudiantId { get; set; }
    public string TypePiece { get; set; } = string.Empty; // "Extrait de naissance", "Baccalauréat / Diplôme", "Photo d'identité", "Certificat de nationalité"
    public string FichierPath { get; set; } = string.Empty;
    public bool EstValidee { get; set; } = false;
    public DateTime DateDepot { get; set; } = DateTime.Now;

    public override string ToString() => $"{TypePiece} ({(EstValidee ? "Validée" : "En attente")})";
}
