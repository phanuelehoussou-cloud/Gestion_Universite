using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class FiliereDAO
{
    private readonly IDataStore _store;

    public FiliereDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Filiere> GetAll()
    {
        return _store.GetCollection<Filiere>();
    }

    public Filiere? GetById(int id)
    {
        return GetAll().FirstOrDefault(f => f.Id == id);
    }

    public void Add(Filiere filiere)
    {
        var list = _store.GetCollection<Filiere>();
        filiere.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(filiere);
        _store.SaveCollection(list);
    }

    public void Update(Filiere filiere)
    {
        var list = _store.GetCollection<Filiere>();
        var idx = list.FindIndex(x => x.Id == filiere.Id);
        if (idx >= 0)
        {
            list[idx] = filiere;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Filiere>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}

public class NiveauDAO
{
    private readonly IDataStore _store;

    public NiveauDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Niveau> GetAll()
    {
        return _store.GetCollection<Niveau>();
    }

    public Niveau? GetById(int id)
    {
        return GetAll().FirstOrDefault(n => n.Id == id);
    }

    public void Add(Niveau niveau)
    {
        var list = _store.GetCollection<Niveau>();
        niveau.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(niveau);
        _store.SaveCollection(list);
    }

    public void Update(Niveau niveau)
    {
        var list = _store.GetCollection<Niveau>();
        var idx = list.FindIndex(x => x.Id == niveau.Id);
        if (idx >= 0)
        {
            list[idx] = niveau;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Niveau>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}
