using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class ClasseDAO
{
    private readonly IDataStore _store;

    public ClasseDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Classe> GetAll()
    {
        var classes = _store.GetCollection<Classe>();
        var filieres = _store.GetCollection<Filiere>();
        var niveaux = _store.GetCollection<Niveau>();
        var annees = _store.GetCollection<AnneeAcademique>();

        foreach (var c in classes)
        {
            c.FiliereNom ??= filieres.FirstOrDefault(f => f.Id == c.FiliereId)?.Nom ?? "Inconnue";
            c.NiveauCode ??= niveaux.FirstOrDefault(n => n.Id == c.NiveauId)?.Code ?? "N/A";
            c.AnneeCode ??= annees.FirstOrDefault(a => a.Id == c.AnneeAcademiqueId)?.Code ?? "N/A";
            if (string.IsNullOrWhiteSpace(c.Details))
            {
                c.Details = $"{c.FiliereNom} - {c.NiveauCode} - {c.Groupe}";
            }
        }
        return classes;
    }

    public Classe? GetById(int id)
    {
        return GetAll().FirstOrDefault(c => c.Id == id);
    }

    public void Add(Classe classe)
    {
        var list = _store.GetCollection<Classe>();
        classe.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        list.Add(classe);
        _store.SaveCollection(list);
    }

    public void Update(Classe classe)
    {
        var list = _store.GetCollection<Classe>();
        var idx = list.FindIndex(x => x.Id == classe.Id);
        if (idx >= 0)
        {
            list[idx] = classe;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Classe>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}
