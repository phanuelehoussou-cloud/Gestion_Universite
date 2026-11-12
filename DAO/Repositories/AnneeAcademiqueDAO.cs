using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class AnneeAcademiqueDAO
{
    private readonly IDataStore _store;

    public AnneeAcademiqueDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<AnneeAcademique> GetAll()
    {
        return _store.GetCollection<AnneeAcademique>().OrderByDescending(a => a.DateDebut).ToList();
    }

    public AnneeAcademique? GetById(int id)
    {
        return GetAll().FirstOrDefault(a => a.Id == id);
    }

    public AnneeAcademique? GetCurrentOpenYear()
    {
        return GetAll().FirstOrDefault(a => a.Statut.Equals("Ouverte", StringComparison.OrdinalIgnoreCase)) ?? GetAll().FirstOrDefault();
    }

    public void Add(AnneeAcademique annee)
    {
        var list = _store.GetCollection<AnneeAcademique>();
        annee.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(annee);
        _store.SaveCollection(list);
    }

    public void Update(AnneeAcademique annee)
    {
        var list = _store.GetCollection<AnneeAcademique>();
        var idx = list.FindIndex(x => x.Id == annee.Id);
        if (idx >= 0)
        {
            list[idx] = annee;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<AnneeAcademique>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }

    public void AddSemestre(int anneeId, Semestre sem)
    {
        var annee = GetById(anneeId);
        if (annee != null)
        {
            sem.Id = annee.Semestres.Count > 0 ? annee.Semestres.Max(s => s.Id) + 1 : 1;
            sem.AnneeAcademiqueId = anneeId;
            annee.Semestres.Add(sem);
            Update(annee);
        }
    }
}
