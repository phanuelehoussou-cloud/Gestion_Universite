namespace DAO.Models;

public class Inscription
{
    public int Id { get; set; }
    public int EtudiantId { get; set; }
    public int ClasseId { get; set; }
    public int AnneeAcademiqueId { get; set; }
    public DateTime DateInscription { get; set; } = DateTime.Now;
    public decimal MontantTotal { get; set; } = 300000m;
    public string Statut { get; set; } = "Active"; // "Active", "Annulée"

    // Calculated / Navigation
    public string? EtudiantMatricule { get; set; }
    public string? EtudiantNomComplet { get; set; }
    public string? ClasseCode { get; set; }
    public string? AnneeCode { get; set; }

    public decimal MontantPaye { get; set; }
    public decimal SoldeActuel => MontantTotal - MontantPaye;

    public List<Paiement> Paiements { get; set; } = new();

    public override string ToString() => $"{EtudiantNomComplet} - {ClasseCode}";
}

public class Paiement
{
    public int Id { get; set; }
    public int InscriptionId { get; set; }
    public DateTime DatePaiement { get; set; } = DateTime.Now;
    public decimal Montant { get; set; }
    public string ModePaiement { get; set; } = "Virement"; // "Virement", "Espèces", "Mobile Money", "Chèque"
    public string Reference { get; set; } = string.Empty;
    public string Remarque { get; set; } = string.Empty;

    public override string ToString() => $"{DatePaiement:dd/MM/yyyy} - {Montant:N0} FCFA ({ModePaiement})";
}
