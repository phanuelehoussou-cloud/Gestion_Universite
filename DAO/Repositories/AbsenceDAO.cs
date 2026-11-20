using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class AbsenceDAO
{
    private readonly IDataStore _store;

    public AbsenceDAO(IDataStore? store = null)
    {
        _store = store ?? DataStoreProvider.Current;
    }

    public List<Absence> GetAll()
    {
        var absences = _store.GetCollection<Absence>();
        var etudiants = _store.GetCollection<Etudiant>();
        var seances = _store.GetCollection<SeanceEmploiDuTemps>();
        var classes = _store.GetCollection<Classe>();
        var cours = _store.GetCollection<Cours>();

        foreach (var absence in absences)
        {
            var etudiant = etudiants.FirstOrDefault(e => e.Id == absence.EtudiantId);
            var seance = seances.FirstOrDefault(s => s.Id == absence.SeanceId);
            absence.EtudiantNom = etudiant?.NomComplet ?? "Étudiant inconnu";
            absence.ClasseCode = classes.FirstOrDefault(c => c.Id == seance?.ClasseId)?.Code ?? "Classe inconnue";
            absence.CoursIntitule = cours.FirstOrDefault(c => c.Id == seance?.CoursId)?.Intitule ?? "Cours inconnu";
        }

        return absences.OrderByDescending(a => a.DateAbsence).ThenBy(a => a.EtudiantNom).ToList();
    }

    public Absence? GetById(int id) => GetAll().FirstOrDefault(a => a.Id == id);

    public bool Save(Absence absence, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (!_store.GetCollection<Etudiant>().Any(e => e.Id == absence.EtudiantId))
        {
            errorMessage = "Sélectionnez un étudiant valide.";
            return false;
        }

        var seance = _store.GetCollection<SeanceEmploiDuTemps>().FirstOrDefault(s => s.Id == absence.SeanceId);
        if (seance == null)
        {
            errorMessage = "Sélectionnez une séance valide.";
            return false;
        }

        var isEnrolled = _store.GetCollection<Inscription>().Any(i =>
            i.EtudiantId == absence.EtudiantId && i.ClasseId == seance.ClasseId &&
            string.Equals(i.Statut, "Active", StringComparison.OrdinalIgnoreCase));
        if (!isEnrolled)
        {
            errorMessage = "L’étudiant n’a pas d’inscription active dans la classe de cette séance.";
            return false;
        }

        if (absence.DateAbsence.Date != seance.DateSeance.Date)
        {
            errorMessage = "La date de l’absence doit correspondre à la date de la séance sélectionnée.";
            return false;
        }

        var records = _store.GetCollection<Absence>();
        if (records.Any(a => a.EtudiantId == absence.EtudiantId && a.SeanceId == absence.SeanceId && a.Id != absence.Id))
        {
            errorMessage = "Une absence est déjà enregistrée pour cet étudiant et cette séance.";
            return false;
        }

        if (absence.Id == 0)
        {
            absence.Id = IdentityHelper.NextId(_store, records.Select(a => a.Id));
            records.Add(absence);
        }
        else
        {
            var index = records.FindIndex(a => a.Id == absence.Id);
            if (index < 0)
            {
                errorMessage = "L'absence à modifier n'existe plus.";
                return false;
            }
            records[index] = absence;
        }

        _store.SaveCollection(records);
        return true;
    }

    public void Delete(int id)
    {
        var records = _store.GetCollection<Absence>();
        records.RemoveAll(a => a.Id == id);
        _store.SaveCollection(records);
    }
}
