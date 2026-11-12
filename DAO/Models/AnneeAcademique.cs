namespace DAO.Models;

public class AnneeAcademique
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // e.g. "2026 - 2027"
    public DateTime DateDebut { get; set; } = DateTime.Now;
    public DateTime DateFin { get; set; } = DateTime.Now.AddYears(1);
    public string Statut { get; set; } = "Ouverte"; // "Ouverte", "Clôturée"
    public List<Semestre> Semestres { get; set; } = new();

    public override string ToString() => Code;
}

public class Semestre
{
    public int Id { get; set; }
    public int AnneeAcademiqueId { get; set; }
    public string Nom { get; set; } = string.Empty; // "Semestre 1", "Semestre 2"
    public DateTime DateDebut { get; set; } = DateTime.Now;
    public DateTime DateFin { get; set; } = DateTime.Now.AddMonths(5);

    public override string ToString() => Nom;
}
