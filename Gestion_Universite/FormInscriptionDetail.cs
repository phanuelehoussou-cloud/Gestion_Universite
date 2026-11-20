using DAO.Models;
using DAO.Repositories;

namespace Gestion_Universite;

public partial class FormInscriptionDetail : Form
{
    private readonly InscriptionDAO _inscriptionDao = new();
    private readonly EtudiantDAO _etudiantDao = new();
    private readonly ClasseDAO _classeDao = new();
    private readonly AnneeAcademiqueDAO _anneeDao = new();
    private List<Classe> _classesDisponibles = new();


    private int _inscriptionId = 0;

    public event EventHandler? DataSaved;

    public FormInscriptionDetail() : this(0) { }

    public FormInscriptionDetail(int inscriptionId)
    {
        _inscriptionId = inscriptionId;
        InitializeComponent();
        Load += (_, _) =>
        {
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) return;
            LoadLookups();
            if (_inscriptionId > 0)
                LoadInscriptionData();
        };
    }



    private void LoadLookups()
    {
        _classesDisponibles = _classeDao.GetAll();

        _cboEtudiant.ValueMember = nameof(Etudiant.Id);
        _cboEtudiant.DataSource = _etudiantDao.GetAll();
        _cboClasse.DisplayMember = nameof(Classe.Code);
        _cboClasse.ValueMember = nameof(Classe.Id);
        _cboAnnee.DisplayMember = nameof(AnneeAcademique.Code);
        _cboAnnee.ValueMember = nameof(AnneeAcademique.Id);
        _cboAnnee.DataSource = _anneeDao.GetAll();
        LoadClassesForSelectedYear();
    }

    private void CboAnnee_SelectedIndexChanged(object? sender, EventArgs e) => LoadClassesForSelectedYear();

    private void LoadClassesForSelectedYear()
    {
        var anneeId = _cboAnnee.SelectedValue is int value ? value : 0;
        var classes = anneeId > 0
            ? _classesDisponibles.Where(classe => classe.AnneeAcademiqueId == anneeId).ToList()
            : new List<Classe>();

        _cboClasse.DataSource = null;
        _cboClasse.DataSource = classes;
    }

    private void LoadInscriptionData()
    {
        var ins = _inscriptionDao.GetById(_inscriptionId);
        if (ins != null)
        {
            _cboAnnee.SelectedValue = ins.AnneeAcademiqueId;
            LoadClassesForSelectedYear();
            _cboEtudiant.SelectedValue = ins.EtudiantId;
            _cboClasse.SelectedValue = ins.ClasseId;

            UpdateSoldeUI(ins);
        }
    }

    private void UpdateSoldeUI(Inscription ins)
    {
        _lblSoldeMontant.Text = $"{ins.SoldeActuel:N0} FCFA";

        _dgvPaiements.Rows.Clear();
        if (ins.Paiements != null)
        {
            foreach (var p in ins.Paiements)
            {
                _dgvPaiements.Rows.Add(p.DatePaiement.ToString("dd/MM/yyyy"), $"{p.Montant:N0} FCFA", p.ModePaiement, p.Reference);
            }
        }
    }

    private void BtnAddPaiement_Click(object? sender, EventArgs e)
    {
        if (_inscriptionId == 0)
        {
            MessageBox.Show("Veuillez d'abord enregistrer l'inscription avant d'ajouter des paiements.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!decimal.TryParse(_txtMontantPaiement.Text.Trim(), out var montant) || montant <= 0)
        {
            MessageBox.Show("Veuillez saisir un montant valide.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var refPaiement = _txtRefPaiement.Text.Trim();
        if (string.IsNullOrEmpty(refPaiement))
            refPaiement = $"REF{DateTime.Now:yyyyMMddHHmmss}";

        var p = new Paiement
        {
            DatePaiement = DateTime.Now,
            Montant = montant,
            ModePaiement = _cboModePaiement.SelectedItem?.ToString() ?? "Virement",
            Reference = refPaiement,
            Remarque = "Paiement de scolarité"
        };

        if (_inscriptionDao.AjouterPaiement(_inscriptionId, p, out var error))
        {
            MessageBox.Show("Paiement enregistré avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _txtMontantPaiement.Text = "";
            _txtRefPaiement.Text = "";
            LoadInscriptionData();
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            MessageBox.Show(error, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (_cboEtudiant.SelectedItem is not Etudiant etudiant ||
            _cboClasse.SelectedItem is not Classe classe ||
            _cboAnnee.SelectedItem is not AnneeAcademique annee)
        {
            MessageBox.Show("Veuillez sélectionner un étudiant, une classe et une année académique.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var ins = _inscriptionId > 0 ? _inscriptionDao.GetById(_inscriptionId) ?? new Inscription() : new Inscription();
        ins.EtudiantId = etudiant.Id;
        ins.ClasseId = classe.Id;
        ins.AnneeAcademiqueId = annee.Id;
        ins.DateInscription = DateTime.Now;
        ins.Statut = "Active";

        if (_inscriptionDao.Inscrire(ins, out var error))
        {
            _inscriptionId = ins.Id;
            MessageBox.Show("Inscription enregistrée avec succès !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateSoldeUI(ins);
            DataSaved?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // Message de blocage de double inscription active
            MessageBox.Show(error, "Règle de Gestion : Inscription impossible", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        if (_inscriptionId == 0)
        {
            MessageBox.Show("Aucune inscription sélectionnée.", "Impression", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var ins = _inscriptionDao.GetById(_inscriptionId);
        if (ins != null)
        {
            var msg = $"========================================\n" +
                      $"    IIT - REÇU DE PAIEMENT SCOLARITÉ    \n" +
                      $"========================================\n" +
                      $"Étudiant : {ins.EtudiantNomComplet}\n" +
                      $"Matricule : {ins.EtudiantMatricule}\n" +
                      $"Classe   : {ins.ClasseCode}\n" +
                      $"Année    : {ins.AnneeCode}\n" +
                      $"Total scolarité : {ins.MontantTotal:N0} FCFA\n" +
                      $"Total réglé     : {ins.MontantPaye:N0} FCFA\n" +
                      $"SOLDE RESTANT   : {ins.SoldeActuel:N0} FCFA\n" +
                      $"========================================\n" +
                      $"Généré le {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            MessageBox.Show(msg, "Reçu d'inscription officiel IIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
