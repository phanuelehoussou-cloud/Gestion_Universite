using DataAccess;
using DAO.Models;

namespace DAO.Repositories;

public class InscriptionDAO
{
    private readonly IDataStore _store;

    public InscriptionDAO(IDataStore? store = null)
    {
        _store = store ?? JsonDataStore.Instance;
    }

    public List<Inscription> GetAll()
    {
        var inscriptions = _store.GetCollection<Inscription>();
        var etudiants = _store.GetCollection<Etudiant>();
        var classes = _store.GetCollection<Classe>();
        var annees = _store.GetCollection<AnneeAcademique>();

        foreach (var ins in inscriptions)
        {
            var etu = etudiants.FirstOrDefault(e => e.Id == ins.EtudiantId);
            if (etu != null)
            {
                ins.EtudiantMatricule = etu.Matricule;
                ins.EtudiantNomComplet = etu.NomComplet;
            }

            var cls = classes.FirstOrDefault(c => c.Id == ins.ClasseId);
            if (cls != null)
            {
                ins.ClasseCode = cls.Code;
            }

            var an = annees.FirstOrDefault(a => a.Id == ins.AnneeAcademiqueId);
            if (an != null)
            {
                ins.AnneeCode = an.Code;
            }

            ins.MontantPaye = ins.Paiements?.Sum(p => p.Montant) ?? 0;
        }

        return inscriptions.OrderByDescending(i => i.DateInscription).ToList();
    }

    public Inscription? GetById(int id)
    {
        return GetAll().FirstOrDefault(i => i.Id == id);
    }

    public Inscription? GetActiveByEtudiantAndYear(int etudiantId, int anneeId)
    {
        return GetAll().FirstOrDefault(i =>
            i.EtudiantId == etudiantId &&
            i.AnneeAcademiqueId == anneeId &&
            i.Statut.Equals("Active", StringComparison.OrdinalIgnoreCase)
        );
    }

    public bool Inscrire(Inscription ins, out string errorMessage)
    {
        errorMessage = string.Empty;

        // RÈGLE : Bloquer la double inscription active dans une même année
        var existante = GetActiveByEtudiantAndYear(ins.EtudiantId, ins.AnneeAcademiqueId);
        if (existante != null && existante.Id != ins.Id)
        {
            errorMessage = $"Impossible d'enregistrer : L'étudiant {existante.EtudiantNomComplet} ({existante.EtudiantMatricule}) a déjà une inscription active pour cette année académique ({existante.ClasseCode}).";
            return false;
        }

        var list = _store.GetCollection<Inscription>();
        if (ins.Id == 0)
        {
            ins.Id = list.Count > 0 ? list.Max(x => x.Id) + 1 : 1;
            list.Add(ins);
        }
        else
        {
            var idx = list.FindIndex(x => x.Id == ins.Id);
            if (idx >= 0)
                list[idx] = ins;
            else
                list.Add(ins);
        }

        _store.SaveCollection(list);

        // Mettre à jour la classe actuelle de l'étudiant
        var etuList = _store.GetCollection<Etudiant>();
        var etudiant = etuList.FirstOrDefault(e => e.Id == ins.EtudiantId);
        var cls = _store.GetCollection<Classe>().FirstOrDefault(c => c.Id == ins.ClasseId);
        if (etudiant != null && cls != null)
        {
            etudiant.ClasseActuelle = cls.Code;
            _store.SaveCollection(etuList);
        }

        return true;
    }

    public bool AjouterPaiement(int inscriptionId, Paiement paiement, out string errorMessage)
    {
        errorMessage = string.Empty;
        var list = _store.GetCollection<Inscription>();
        var ins = list.FirstOrDefault(i => i.Id == inscriptionId);
        if (ins == null)
        {
            errorMessage = "Inscription introuvable.";
            return false;
        }

        paiement.Id = ins.Paiements.Count > 0 ? ins.Paiements.Max(p => p.Id) + 1 : 1;
        paiement.InscriptionId = inscriptionId;
        ins.Paiements.Add(paiement);
        ins.MontantPaye = ins.Paiements.Sum(p => p.Montant);

        _store.SaveCollection(list);
        return true;
    }

    public void Delete(int id)
    {
        var list = _store.GetCollection<Inscription>();
        list.RemoveAll(x => x.Id == id);
        _store.SaveCollection(list);
    }
}
