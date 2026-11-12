namespace DAO.Models;

public class Filiere
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty; // "Informatique", "Gestion commerciale", etc.
    public string Description { get; set; } = string.Empty;
    public string Statut { get; set; } = "Active"; // "Active", "Inactive"

    public override string ToString() => Nom;
}

public class Niveau
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty; // "L1", "L2", "L3", "M1", "M2"
    public string Cycle { get; set; } = "Licence"; // "Licence", "Master", "Doctorat"

    public override string ToString() => Code;
}
