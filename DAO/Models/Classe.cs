namespace DAO.Models;

public class Classe
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // "Info L2 A", "Info L2 B", "GC L1 A"
    public int FiliereId { get; set; }
    public int NiveauId { get; set; }
    public int AnneeAcademiqueId { get; set; }
    public string Groupe { get; set; } = "A"; // "A", "B", "C"
    public string Details { get; set; } = string.Empty; // "Informatique - L2 - A"

    // Navigation / Display properties
    public string? FiliereNom { get; set; }
    public string? NiveauCode { get; set; }
    public string? AnneeCode { get; set; }

    public override string ToString() => Code;
}
