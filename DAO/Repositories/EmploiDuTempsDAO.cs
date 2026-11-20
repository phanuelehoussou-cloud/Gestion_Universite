using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class EmploiDuTempsDAO
{
    private readonly IDataStore _store;

    public EmploiDuTempsDAO(IDataStore? store = null)
    {
        _store = store ?? DataStoreProvider.Current;
    }

    public List<SeanceEmploiDuTemps> GetAll()
    {
        var seances = _store.GetCollection<SeanceEmploiDuTemps>();
        var classes = _store.GetCollection<Classe>();
        var cours = _store.GetCollection<Cours>();
        var enseignants = _store.GetCollection<Enseignant>();
        var salles = _store.GetCollection<Salle>();

        foreach (var s in seances)
        {
            s.ClasseCode ??= classes.FirstOrDefault(c => c.Id == s.ClasseId)?.Code ?? "N/A";
            s.CoursIntitule ??= cours.FirstOrDefault(c => c.Id == s.CoursId)?.Intitule ?? "N/A";
            s.EnseignantNom ??= enseignants.FirstOrDefault(e => e.Id == s.EnseignantId)?.NomComplet ?? "N/A";
            s.SalleNom ??= salles.FirstOrDefault(sa => sa.Id == s.SalleId)?.Nom ?? "N/A";
        }

        return seances;
    }

    public List<SeanceEmploiDuTemps> GetByClasseAndSemestre(int classeId, int semestreId)
    {
        return GetAll()
            .Where(s => s.ClasseId == classeId && (semestreId == 0 || s.SemestreId == semestreId))
            .OrderBy(s => GetJourIndex(s.Jour))
            .ThenBy(s => s.HeureDebut)
            .ToList();
    }

    private static int GetJourIndex(string jour) => jour.ToLowerInvariant() switch
    {
        "lundi" => 1,
        "mardi" => 2,
        "mercredi" => 3,
        "jeudi" => 4,
        "vendredi" => 5,
        "samedi" => 6,
        _ => 7
    };

    public bool VerifierEtEnregistrer(SeanceEmploiDuTemps seance, out string conflitMessage)
    {
        conflitMessage = string.Empty;
        var all = GetAll();

        // Vérification des 3 conflits stricts :
        foreach (var s in all)
        {
            if (s.Id == seance.Id)
                continue; // On s'ignore soi-même lors d'une modification

            if (s.DateSeance.Date == seance.DateSeance.Date &&
                TryParseTime(s.HeureDebut, out var existingStart) && TryParseTime(s.HeureFin, out var existingEnd) &&
                TryParseTime(seance.HeureDebut, out var requestedStart) && TryParseTime(seance.HeureFin, out var requestedEnd) &&
                existingStart < requestedEnd && existingEnd > requestedStart)
            {
                // 1. Conflit de salle
                if (s.SalleId == seance.SalleId)
                {
                    conflitMessage = $"Conflit de Salle : La salle '{s.SalleNom}' est déjà occupée le {s.Jour} de {s.HeureDebut} par la classe '{s.ClasseCode}' ({s.CoursIntitule}).";
                    return false;
                }

                // 2. Conflit d'enseignant
                if (s.EnseignantId == seance.EnseignantId)
                {
                    conflitMessage = $"Conflit d'Enseignant : L'enseignant '{s.EnseignantNom}' dispense déjà un cours le {s.Jour} de {s.HeureDebut} pour la classe '{s.ClasseCode}'.";
                    return false;
                }

                // 3. Conflit de classe
                if (s.ClasseId == seance.ClasseId)
                {
                    conflitMessage = $"Conflit de Classe : La classe '{s.ClasseCode}' a déjà le cours '{s.CoursIntitule}' programmé le {s.Jour} de {s.HeureDebut}.";
                    return false;
                }
            }
        }

        // Aucun conflit -> enregistrement
        var list = _store.GetCollection<SeanceEmploiDuTemps>();
        if (seance.Id == 0)
        {
            seance.Id = IdentityHelper.NextId(_store, list.Select(x => x.Id));
            list.Add(seance);
        }
        else
        {
            var idx = list.FindIndex(x => x.Id == seance.Id);
            if (idx >= 0)
                list[idx] = seance;
            else
                list.Add(seance);
        }

        _store.SaveCollection(list);
        return true;
    }

    private static bool TryParseTime(string value, out TimeSpan time)
    {
        var normalized = value.Trim().Replace('h', ':').Replace('H', ':');
        if (normalized.EndsWith(':')) normalized += "00";
        return TimeSpan.TryParse(normalized, out time);
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<SeanceEmploiDuTemps>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}
