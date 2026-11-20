using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class DashboardDAO
{
    private readonly IDataStore _store;

    public DashboardDAO(IDataStore? store = null)
    {
        _store = store ?? DataStoreProvider.Current;
    }

    public DashboardStatistiques GetStatistiques()
    {
        var inscriptions = _store.GetCollection<Inscription>();
        var classes = _store.GetCollection<Classe>();
        var annees = _store.GetCollection<AnneeAcademique>();
        var enseignants = _store.GetCollection<Enseignant>();
        var salles = _store.GetCollection<Salle>();

        var inscriptionsActives = inscriptions
            .Where(i => string.Equals(i.Statut, "Active", StringComparison.OrdinalIgnoreCase))
            .ToList();

        var anneesOuvertes = annees
            .Where(a => string.Equals(a.Statut, "Ouverte", StringComparison.OrdinalIgnoreCase))
            .Select(a => a.Id)
            .ToHashSet();

        var recettesDuJour = inscriptions
            .SelectMany(i => i.Paiements ?? new List<Paiement>())
            .Where(p => p.DatePaiement.Date == DateTime.Today && string.Equals(p.Statut, "Effectué", StringComparison.OrdinalIgnoreCase))
            .Sum(p => p.Montant);

        return new DashboardStatistiques
        {
            TotalEtudiantsInscrits = inscriptionsActives
                .Select(i => i.EtudiantId)
                .Distinct()
                .Count(),
            TotalClassesActives = classes.Count(c => anneesOuvertes.Contains(c.AnneeAcademiqueId)),
            RecettesDuJour = recettesDuJour,
            TotalEnseignants = enseignants.Count(e => string.Equals(e.Statut, "Actif", StringComparison.OrdinalIgnoreCase)),
            TotalSalles = salles.Count(s => string.Equals(s.Statut, "Disponible", StringComparison.OrdinalIgnoreCase))
        };
    }
}
