using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class EnseignantDAO
{
    private readonly IDataStore _store;

    public EnseignantDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Enseignant> GetAll()
    {
        return _store.GetCollection<Enseignant>();
    }

    public Enseignant? GetById(int id)
    {
        return GetAll().FirstOrDefault(e => e.Id == id);
    }

    public void Add(Enseignant enseignant)
    {
        var list = _store.GetCollection<Enseignant>();
        enseignant.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(enseignant);
        _store.SaveCollection(list);
    }

    public void Update(Enseignant enseignant)
    {
        var list = _store.GetCollection<Enseignant>();
        var idx = list.FindIndex(x => x.Id == enseignant.Id);
        if (idx >= 0)
        {
            list[idx] = enseignant;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Enseignant>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}

public class CoursDAO
{
    private readonly IDataStore _store;

    public CoursDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Cours> GetAll()
    {
        return _store.GetCollection<Cours>();
    }

    public Cours? GetById(int id)
    {
        return GetAll().FirstOrDefault(c => c.Id == id);
    }

    public void Add(Cours cours)
    {
        var list = _store.GetCollection<Cours>();
        cours.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(cours);
        _store.SaveCollection(list);
    }

    public void Update(Cours cours)
    {
        var list = _store.GetCollection<Cours>();
        var idx = list.FindIndex(x => x.Id == cours.Id);
        if (idx >= 0)
        {
            list[idx] = cours;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Cours>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}

public class SalleDAO
{
    private readonly IDataStore _store;

    public SalleDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Salle> GetAll()
    {
        return _store.GetCollection<Salle>();
    }

    public Salle? GetById(int id)
    {
        return GetAll().FirstOrDefault(s => s.Id == id);
    }

    public void Add(Salle salle)
    {
        var list = _store.GetCollection<Salle>();
        salle.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(salle);
        _store.SaveCollection(list);
    }

    public void Update(Salle salle)
    {
        var list = _store.GetCollection<Salle>();
        var idx = list.FindIndex(x => x.Id == salle.Id);
        if (idx >= 0)
        {
            list[idx] = salle;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Salle>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}
