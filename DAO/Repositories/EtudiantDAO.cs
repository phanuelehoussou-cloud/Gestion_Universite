using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class EtudiantDAO
{
    private readonly IDataStore _store;

    public EtudiantDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Etudiant> GetAll()
    {
        return _store.GetCollection<Etudiant>();
    }

    public Etudiant? GetById(int id)
    {
        return GetAll().FirstOrDefault(e => e.Id == id);
    }

    public Etudiant? GetByMatricule(string matricule)
    {
        return GetAll().FirstOrDefault(e => e.Matricule.Equals(matricule.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public List<Etudiant> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        var q = query.Trim().ToLowerInvariant();
        return GetAll().Where(e =>
            e.Matricule.ToLowerInvariant().Contains(q) ||
            e.Nom.ToLowerInvariant().Contains(q) ||
            e.Prenoms.ToLowerInvariant().Contains(q) ||
            (e.ClasseActuelle != null && e.ClasseActuelle.ToLowerInvariant().Contains(q))
        ).ToList();
    }

    public void Add(Etudiant etudiant)
    {
        var list = _store.GetCollection<Etudiant>();
        etudiant.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
        if (string.IsNullOrWhiteSpace(etudiant.Matricule))
        {
            var year = DateTime.Now.Year;
            etudiant.Matricule = $"IIT{year}{etudiant.Id:D3}";
        }
        list.Add(etudiant);
        _store.SaveCollection(list);
    }

    public void Update(Etudiant etudiant)
    {
        var list = _store.GetCollection<Etudiant>();
        var idx = list.FindIndex(x => x.Id == etudiant.Id);
        if (idx >= 0)
        {
            list[idx] = etudiant;
            _store.SaveCollection(list);
        }
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Etudiant>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }

    public void AddOrUpdatePiece(int etudiantId, PieceAdministrative piece)
    {
        var etudiant = GetById(etudiantId);
        if (etudiant != null)
        {
            if (piece.Id == 0)
            {
                piece.Id = etudiant.Pieces.Count > 0 ? etudiant.Pieces.Max(p => p.Id) + 1 : 1;
                piece.EtudiantId = etudiantId;
                etudiant.Pieces.Add(piece);
            }
            else
            {
                var idx = etudiant.Pieces.FindIndex(p => p.Id == piece.Id);
                if (idx >= 0)
                    etudiant.Pieces[idx] = piece;
            }
            Update(etudiant);
        }
    }
}
