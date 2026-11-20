namespace DAO.Models;

public class Absence
{
    public int Id { get; set; }
    public int EtudiantId { get; set; }
    public int SeanceId { get; set; }
    public DateTime DateAbsence { get; set; } = DateTime.Today;
    public bool Justifiee { get; set; }
    public string Motif { get; set; } = string.Empty;

    public string? EtudiantNom { get; set; }
    public string? ClasseCode { get; set; }
    public string? CoursIntitule { get; set; }

    public string Statut => Justifiee ? "Justifiée" : "À justifier";
}
